using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Xml;
using ClosedXML.Report.Utils;
using Dapper;
using HtmlAgilityPack;
using Brushes = System.Windows.Media.Brushes;

namespace second_course
{
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker _updateRSSWorker = new BackgroundWorker();
        private readonly BackgroundWorker _loadFullNewsWorker = new BackgroundWorker();
        public static Int32 LastDBIdent;
        private Boolean _searchFieldWaterMarkActive = true;
        private List<Newspaper> _newspapers;
        
        public MainWindow()
        {
            if (!File.Exists(Consts.DBname)) // Запись таблиц в бд при первом запуске
            {
                SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
                connection.Execute(Consts.FullDBCreationQuery);
                connection.Execute(Consts.DefaultNewsSourcesQuery);
            }

            InitializeComponent();
            TextBlockHeader.Text = this.Title;
            LastDBIdent = GetLastDBIdent();

            
            TextBoxSearch.GotFocus += (source, e) =>
            {
                if (this._searchFieldWaterMarkActive)
                {
                    this._searchFieldWaterMarkActive = false;
                    this.TextBoxSearch.Text = "";
                    this.TextBoxSearch.Foreground = Brushes.Black;
                }
            };

            TextBoxSearch.LostFocus += (source, e) =>
            {
                if (!this._searchFieldWaterMarkActive && string.IsNullOrEmpty(TextBoxSearch.Text))
                {
                    this._searchFieldWaterMarkActive = true;
                    this.TextBoxSearch.Text = "Введите искомые слова";
                    this.TextBoxSearch.Foreground = Brushes.Gray;
                }
            };
            

            // ==========================================================================

            _updateRSSWorker.DoWork += Worker_RSSUpdateDoWork;
            _updateRSSWorker.RunWorkerCompleted += Worker_RSSUpdateFinished;
            _updateRSSWorker.WorkerSupportsCancellation = true;

            _loadFullNewsWorker.DoWork += Worker_ParseFullNewsDoWork;
            _loadFullNewsWorker.RunWorkerCompleted += Worker_ParseFullNewsFinished;
            _loadFullNewsWorker.WorkerSupportsCancellation = true;
            _loadFullNewsWorker.WorkerReportsProgress = true;
            _loadFullNewsWorker.ProgressChanged += Worker_ParseFullNewsProgressChanged;

            _newspapers = DataBase.LoadNewspapers();
            SetHeadersSourceList(_newspapers);

            // доделать парсинг новости на вывод в прогу. ++ придумать как сохранять изображения в БД <img>666</img>
            // определение начала новости по тегу div в классом. Изображения выводятся там, где должны быть
            // загрузка картинок в БД

            // TODO залить на гит, прописать .gitignore

            // TODO попробовать сделать поиск через БД

            // TODO нормализовать БД

            // todo вывод тематики

            // TODO сделать что-то с цветами, чтобы они не сливались (глянуть как это сделали в телеге). Мб использовать другую цветовую гамму.
            
            // попытка повтора после завершения парсинга?

            // после текста курсача: выделить наиболее актуальные источники новостей для их обработки
            

            // запихать вызов коннекшн в юзинги
            
            // проверить метод слияния текущих новостей и новых 
        }

        /// <summary>
        /// Получение максимального индекса в БД чтобы локально их присваивать
        /// </summary>
        /// <returns></returns>
        private static Int32 GetLastDBIdent()
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            int maxIdent = 0;
            try
            {
                maxIdent = connection.QueryFirst<Int32>($"select max(id) from {Consts.NewsTableName}");
            }
            catch
            {}

            maxIdent++; // Чтобы следующий элемент имел отличный от найденного id

            return maxIdent;
        }
        public void Worker_ParseFullNewsProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Button666.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                ProgressBarParsingNews.Value = e.ProgressPercentage;
                
            }));
        }

        /// <summary>
        /// Загружает полную версию новости для каждого элемента. Часть backgroundWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Worker_ParseFullNewsDoWork(object sender, DoWorkEventArgs e)
        {
            var news = e.Argument as List<Newspaper>;
            
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            String sql = $"select id from {Consts.FullTextTableName} where i_is_parsed=0";
            var notParsed = connection.Query<Int32>(sql).ToList();
            
            int threads = 14;
            List<List<Newspaper>> parallelWorks = new List<List<Newspaper>>();

            int step = news.Count / (threads - 1);
            for (int i = 0; i < news.Count; i += step) // деление на части для параллельного парсинга
            {
                List<Newspaper> works = new List<Newspaper>();
                for (int j = i; j < i + step && j < news.Count; j++)
                {
                    works.Add(news[j]);
                }
                parallelWorks.Add(works);
            }
            
            int parsed = 0;
            Parallel.For(0, threads, (i, state) =>
            {
                //Console.WriteLine($"parallel {i} started");
                foreach (Newspaper np in parallelWorks[i])
                {
                    if (notParsed.Contains(np.id))
                    {
                        try
                        {
                            String fullText = ParseFullNewspaper.GetFullNewspaperText(np.s_link);
                            DataBase.UpdateNewspaperFullText(np.id, fullText);
                        }
                        catch
                        {
                            Console.WriteLine($"error in parsing newpaper id={np.id}");
                            continue;
                        }

                        parsed++;
                        if (_loadFullNewsWorker.WorkerReportsProgress)
                        {
                            _loadFullNewsWorker.ReportProgress((int) (((double) parsed / (double) notParsed.Count) * 100));
                        }
                        Console.WriteLine(parsed + " / " + notParsed.Count);
                    }
                }
                //Console.WriteLine($"parallel {i} finished");
            });

            e.Result = news;
        }
        public void Worker_ParseFullNewsFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            List<Newspaper> items = e.Result as List<Newspaper>;
            SetHeadersSourceList(items);
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            String sql = $"select count(*) from {Consts.FullTextTableName} where i_is_parsed=1;";
            Int32 parsed = connection.QueryFirst<Int32>(sql);

            sql = $"select count(*) from {Consts.FullTextTableName};";
            Int32 allCount = connection.QueryFirst<Int32>(sql);
            
            MessageBox.Show($"Обновление завершено. Загружены полные версии для {parsed} из {allCount}.");
            ProgressBarParsingNews.Value = 0;
        }
        public List<Newspaper> DownloadAndParseRSS(List<NewsSource> newsSources)
        {
            List<Newspaper> news = new List<Newspaper>();

            foreach (NewsSource source in newsSources)
            {
                if (_updateRSSWorker.CancellationPending)
                {
                    return new List<Newspaper>();
                }

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(source.s_rss_link);
                HttpWebResponse resp = null;
                try
                {
                    resp = (HttpWebResponse)req.GetResponse();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(source.s_rss_link + " ## " + ex.Message);
                    continue;
                }

                Encoding enc; // некоторые сайты не возвращают кодировку
                try
                {
                    enc = Encoding.GetEncoding(resp.CharacterSet);
                }
                catch (System.ArgumentException)
                {
                    //Console.WriteLine("set default encoding: " + resp.CharacterSet + " site: " + source.s_rss_link);
                    enc = Encoding.UTF8;
                }

                StreamReader sr = new StreamReader(resp.GetResponseStream(), enc);

                using (XmlReader reader = XmlReader.Create(new StringReader(sr.ReadToEnd())))
                {
                    var formatter = new Rss20FeedFormatter();
                    formatter.ReadFrom(reader);
                    foreach (SyndicationItem syndItem in formatter.Feed.Items)
                    {
                        Newspaper np = new Newspaper();
                        np.i_thematic_id = GetThematicID(syndItem.Categories.Count != 0 ? syndItem.Categories.First().Name : "-");
                        np.s_header = syndItem.Title.Text;
                        np.s_description = syndItem.Summary?.Text;
                        np.s_date = syndItem.PublishDate.DateTime.ToString();
                        np.i_source_id = source.id;
                        np.s_link = syndItem.Links[0].Uri.ToString();
                        np.i_is_read = false;
                        news.Add(np);
                    }
                }

                Console.WriteLine(source.s_name + " done");
            }
            
            return news;
        }
        private Int32 GetThematicID(String thematicName)
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            String sql = String.Format("select id from thematic where s_name = '{0}'", thematicName);
            Int32 res = -1;

            try
            {
                res = connection.QueryFirst<Int32>(sql);
            }
            catch
            {
                String sqlInsert = String.Format("insert into thematic (s_name) values ('{0}')", thematicName);
                connection.Execute(sqlInsert);
                res = connection.QueryFirst<Int32>(sql);

            }

            return res;
        }
        public void Worker_RSSUpdateDoWork(object sender, DoWorkEventArgs e) // выполняется загрузка и слияние текущего и нового списков новостей
        {
            List<NewsSource> newsSources = DataBase.LoadNewsSources();
            List<Newspaper> loadedNews = DownloadAndParseRSS(newsSources);
            
            e.Result = loadedNews;
        }
        public void Worker_RSSUpdateFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            var loadedNews = e.Result as List<Newspaper>;
            
            List<Newspaper> newspapers = GetHeadersSourceList();

            SetHeadersSourceList(null); // костыль. Надеюсь там не по ссылке присвоение идёт, чтобы массив не обнулять

            foreach (Newspaper loadedNew in loadedNews)
            {
                Boolean isExist = false;
                foreach (Newspaper newpaper in newspapers)
                {
                    if (loadedNew.s_link == newpaper.s_link) // ищем, есть ли этот элемент в текущем списке новостей
                    {
                        isExist = true;
                        break;
                    }
                }

                if (!isExist)
                {
                    if (DataBase.SaveNewspaper(loadedNew))
                    {
                        newspapers.Add(loadedNew);
                    }
                }
            }

            DataBase.FillSourceNames(newspapers);

            SetHeadersSourceList(newspapers);
            _loadFullNewsWorker.RunWorkerAsync(newspapers);
            Button666.IsEnabled = true;
        }
        private string BuildFullTextView(String fullText, String link)
        {
            GridTest.Children.Clear();
            
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(fullText);
            string res = "";
            try
            {
                res = doc.DocumentNode.InnerText.Trim();
                //res = doc.DocumentNode.SelectSingleNode("//div[@class='news-detail-text']").InnerText.Trim();
            }
            catch
            {}
            
            
            /*
            try
            {
                List<HtmlAttribute> tttt =
                    doc.DocumentNode.SelectSingleNode("//img").Attributes.AttributesWithName("src").ToList();

                foreach (HtmlAttribute attribute in tttt)
                {
                    //MessageBox.Show(attribute.Name + " " + attribute.Value);
                }
            }
            catch
            {

            }
            */
            
            // TODO в цикле выделение всех блоков. Если это блок текста, то выделяется текст и создаётся текстблок, если картинка -- картинка создаётся и загружается
            
            GridTest.RowDefinitions.Add(new RowDefinition());
            GridTest.VerticalAlignment = VerticalAlignment.Top;
            var tb = new TextBlock();
            
            tb.Inlines.Clear();
            Hyperlink hyperLink = new Hyperlink()
            {
                NavigateUri = new Uri(link)
            };
            hyperLink.RequestNavigate += Hyperlink_RequestNavigate;
            hyperLink.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#d9d9d9"));
            hyperLink.Inlines.Add(new Run("Открыть в браузере") { FontWeight = FontWeights.Normal });
            tb.Inlines.Add(hyperLink);
            tb.FontSize = 20;
            GridTest.Children.Add(tb);
            Grid.SetRow(tb, GridTest.RowDefinitions.Count - 1);

            GridTest.RowDefinitions.Add(new RowDefinition());
            var tb2 = new TextBlock();
            tb2.Text = res;
            tb2.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f5f5f5"));
            tb2.TextWrapping = TextWrapping.Wrap;
            tb2.FontSize = 20;

            GridTest.Children.Add(tb2);
            Grid.SetRow(tb2, GridTest.RowDefinitions.Count - 1);

            return "";
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
        public List<Newspaper> GetHeadersSourceList()
        {
            return ListBoxNewHeaders.ItemsSource as List<Newspaper>;
        }
        public void SetHeadersSourceList(List<Newspaper> news)
        {
            if (news == null)
                ListBoxNewHeaders.ItemsSource = null;
            else
            {
                news.Sort((t, r) => Convert.ToDateTime(r.s_date).CompareTo(Convert.ToDateTime(t.s_date)));
                ListBoxNewHeaders.ItemsSource = news;
            }
        }
        private void HeaderRectangle_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void ButtonClose_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void ListBoxNewHeaders_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Newspaper selectedItem = (Newspaper) e.AddedItems[0];
                //TextBlockNewText.Text = selectedItem.s_full_text;
                SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
                String sql = $"select s_text from {Consts.FullTextTableName} where id={selectedItem.id}";
                string fullText = "";
                try
                {
                    fullText = connection.QueryFirst<String>(sql);
                }
                catch (Exception ee)
                {}
                
                BuildFullTextView(fullText.Length == 0? selectedItem.s_description : fullText, selectedItem.s_link);
                
                selectedItem.i_is_read = true;
                sql = $"update {Consts.NewsTableName} set i_is_read=1 where id={selectedItem.id}";
                connection.Execute(sql);
            }
        }
        private void SettingsButton_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.ShowDialog();
            
            SetHeadersSourceList(DataBase.LoadNewspapers());
        }
        private void TextBoxSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (ListBoxNewHeaders.ItemsSource != null)
            {
                if (TextBoxSearch.Text.IsNullOrWhiteSpace())
                {
                    SetHeadersSourceList(_newspapers);
                }

                if (!_searchFieldWaterMarkActive)
                    SetHeadersSourceList(Search.SearchByQuery(GetHeadersSourceList(), TextBoxSearch.Text.Trim()));
            }
        }
        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            Button666.IsEnabled = false;
            _loadFullNewsWorker.CancelAsync(); // останавливаем выполнение фонового парсинга
            _updateRSSWorker.RunWorkerAsync();
        }
    }
}