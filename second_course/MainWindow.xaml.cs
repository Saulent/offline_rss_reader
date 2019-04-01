using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml;
using Dapper;
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
        private Boolean _isFullNewsUpdatingNow = false;

        public MainWindow()
        {
            if (!File.Exists(second_course.Names.Database)) // Запись таблиц в бд при первом запуске
            {
                SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
                connection.Execute(Queries.FullDBCreationQuery);
                DataBase.SaveDefaultNewsSources();
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
                    this.TextBoxSearch.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f5f5f5")); 
                }
            };
            TextBoxSearch.LostFocus += (source, e) =>
            {
                if (!this._searchFieldWaterMarkActive && string.IsNullOrEmpty(TextBoxSearch.Text))
                {
                    this._searchFieldWaterMarkActive = true;
                    this.TextBoxSearch.Text = "Введите искомые слова";
                    this.TextBoxSearch.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#A1A1A1")); 
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

            _newspapers = DataBase.LoadSelectedNewspapers();
            SetHeadersSourceList(_newspapers);
            
            //QueryConstructorWindow qw = new QueryConstructorWindow();
            //qw.ShowInTaskbar = true;
            //qw.ShowDialog();

            //Application.Current.Shutdown();
        }

        /// <summary>
        /// Получение максимального индекса в БД чтобы локально их присваивать
        /// </summary>
        /// <returns></returns>
        private static Int32 GetLastDBIdent()
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            int maxIdent;
            try
            {
                maxIdent = connection.QueryFirst<Int32>($"select max(id) from {TableNames.Newspapers}");
            }
            catch (Exception)
            {
                Console.WriteLine("set max id = 0");
                maxIdent = 0;
            }

            maxIdent++; // Чтобы следующий элемент имел отличный от найденного id

            return maxIdent;
        }
        public void Worker_ParseFullNewsProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ButtonUpdate.Dispatcher.BeginInvoke(new Action(delegate ()
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
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            
            string sql = $"select np.id id, np.s_link s_link from " +
                         $"{TableNames.NewspaperFullText} as ft " +
                         $"INNER JOIN {TableNames.Newspapers} as np on ft.id = np.id " +
                         $"where ft.i_is_parsed = 0";

            List<Newspaper> news = connection.Query<Newspaper>(sql).ToList();
            
            int threads = 16;
            List<List<Newspaper>> parallelWorks = new List<List<Newspaper>>();

            int step = news.Count / (threads - 1);
            if (step < 1)
                step = 1;

            for (int i = 0; i < news.Count; i += step) // деление на части для параллельного парсинга
            {
                List<Newspaper> works = new List<Newspaper>();
                for (int j = i; j < i + step && j < news.Count; j++)
                {
                    works.Add(news[j]);
                }
                parallelWorks.Add(works);
            }

            bool isServerDown = false;
            int parsed = 0;
            Parallel.For(0, threads, (i, state) =>
            {
                if (i < parallelWorks.Count)
                {
                    foreach (Newspaper np in parallelWorks[i])
                    {
                        if (_loadFullNewsWorker.CancellationPending)
                        {
                            break;
                        }
                        
                        string fullText;
                        try
                        {
                            fullText = ParseFullNewspaper.GetFullNewspaperText(np.s_link, np.id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"error in parsing newspaper id={np.id}, {ex.Message}, s_link={np.s_link}");
                            if (ex.Message == "Невозможно соединиться с удаленным сервером")
                            {
                                isServerDown = true;
                                break;
                            }
                            continue;
                        }
                        DataBase.UpdateNewspaperFullText(np.id, fullText);

                        parsed++;
                        if (_loadFullNewsWorker.WorkerReportsProgress)
                        {
                            _loadFullNewsWorker.ReportProgress((int) (((double) parsed / (double) news.Count) * 100));
                        }
                        //Console.WriteLine(parsed + " / " + news.Count + " np id: " + np.id);
                    }
                }
            });

            if (isServerDown)
            {
                MessageBox.Show("Невозможно соединиться с удаленным сервером, возможно сервер выключен или отсутствует подключение к сети интернет.");
                return;
            }

            e.Result = news;
        }
        public void Worker_ParseFullNewsFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            List<Newspaper> items = e.Result as List<Newspaper>;
            //SetHeadersSourceList(items);
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            String sql = $"select count(*) from {TableNames.NewspaperFullText}" +
                         $" where i_is_parsed=1;";
            Int32 parsed = connection.QueryFirst<Int32>(sql);

            sql = $"select count(*) from {TableNames.NewspaperFullText};";
            Int32 allCount = connection.QueryFirst<Int32>(sql);
            
            MessageBox.Show($"Обновление завершено. Загружены полные версии для {parsed} из {allCount}.");
            ProgressBarParsingNews.Value = 0;
            _isFullNewsUpdatingNow = false;
            ButtonUpdate.IsEnabled = true;
            ButtonUpdate.Content = "Обновить";
            ButtonSettings.IsEnabled = true;
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
                    Console.WriteLine( "rss get error: " + source.s_rss_link + " ## " + ex.Message);
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
                    try
                    {
                        formatter.ReadFrom(reader);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("error in rss parsing: " + ex.Message);
                        continue;
                    }
                    foreach (SyndicationItem syndItem in formatter.Feed.Items)
                    {
                        Newspaper np = new Newspaper();

                        try
                        {
                            np.i_thematic_id = DataBase.GetThematicID(syndItem.Categories.Count != 0 ?
                                syndItem.Categories.First().Name : "-");
                            np.s_header = syndItem.Title.Text.Replace("'", "");
                            np.s_description = syndItem.Summary?.Text.Replace("'", "");
                            np.s_date = syndItem.PublishDate.DateTime.ToString();
                            np.i_source_id = source.id;
                            np.s_link = syndItem.Links[0].Uri.ToString();
                            np.i_is_read = false;
                            news.Add(np);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("np (rss updating) parsing error ");
                        }
                    }
                }

                Console.WriteLine(source.s_name + " done");
            }
            
            return news;
        }
        public void Worker_RSSUpdateDoWork(object sender, DoWorkEventArgs e) // выполняется загрузка и слияние текущего и нового списков новостей
        {
            List<NewsSource> newsSources = DataBase.LoadNewsSources();
            List<Newspaper> loadedNews = DownloadAndParseRSS(newsSources);

            e.Result = loadedNews;
        }
        public void Worker_RSSUpdateFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            List<Newspaper> loadedNews = e.Result as List<Newspaper>;

            if (loadedNews.Count < 1)
            {
                MessageBox.Show("Отсутствует интернет-соединение.");
            }
            else
            {
                foreach (Newspaper np in loadedNews)
                {
                    DataBase.SaveNewspaper(np);
                }
                
                List<Newspaper> newsToDisplay = DataBase.LoadSelectedNewspapers();
                DataBase.FillSourceNames(newsToDisplay);
                
                SetHeadersSourceList(newsToDisplay);
                _loadFullNewsWorker.RunWorkerAsync();
                _isFullNewsUpdatingNow = true;
            }

            ButtonUpdate.IsEnabled = true;
        }
        private void BuildFullTextView(Newspaper np)
        {
            GridScrollView.Children.Clear();
            
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            String sql = $"select s_text from {TableNames.NewspaperFullText} " +
                         $"where id={np.id}";
            string fullText = "";
            try
            {
                fullText = connection.QueryFirst<String>(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine("full text loading error, np id=" + np.id);    
            }

            String thematic = connection.QueryFirst<String>(
                    $"select s_name from {TableNames.Thematic} where id={np.i_thematic_id}");
           
            GridScrollView.RowDefinitions.Add(new RowDefinition());
            GridScrollView.VerticalAlignment = VerticalAlignment.Top;
            var tb = new TextBlock();
            
            tb.Inlines.Clear();
            Hyperlink hyperLink = new Hyperlink()
            {
                NavigateUri = new Uri(np.s_link)
            };
            hyperLink.RequestNavigate += Hyperlink_RequestNavigate;
            hyperLink.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#d9d9d9"));
            hyperLink.Inlines.Add(new Run("Открыть в браузере") { FontWeight = FontWeights.Normal });
            tb.Inlines.Add(hyperLink);
            tb.FontSize = 20;
            tb.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#d9d9d9"));
            GridScrollView.Children.Add(tb);
            Grid.SetRow(tb, GridScrollView.RowDefinitions.Count - 1);
            Grid.SetColumnSpan(tb,2);
            
            if (thematic != "-")
            {
                var tb6 = new TextBlock();
                tb6.Text = thematic;
                tb6.TextAlignment = TextAlignment.Right;
                tb6.FontSize = 20;
                tb6.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#d9d9d9"));
                GridScrollView.Children.Add(tb6);
                Grid.SetRow(tb6, GridScrollView.RowDefinitions.Count - 1);
                Grid.SetColumn(tb6, 1);
            }
            
            GridScrollView.RowDefinitions.Add(new RowDefinition());
            var rect = new System.Windows.Shapes.Rectangle();
            rect.Height = 1;
            rect.Fill = Brushes.Gray;
            GridScrollView.Children.Add(rect);
            Grid.SetRow(rect, GridScrollView.RowDefinitions.Count - 1);
            Grid.SetColumnSpan(rect, 2);


            GridScrollView.RowDefinitions.Add(new RowDefinition());
            var tb3 = new TextBlock();
            tb3.Text = "\n" + np.s_header + "\n";
            tb3.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f5f5f5"));
            tb3.TextWrapping = TextWrapping.Wrap;
            tb3.FontWeight = FontWeights.Bold;
            tb3.FontSize = 28;
            GridScrollView.Children.Add(tb3);
            Grid.SetRow(tb3, GridScrollView.RowDefinitions.Count - 1);
            Grid.SetColumnSpan(tb3, 2);


            GridScrollView.RowDefinitions.Add(new RowDefinition());
            var tb2 = new TextBlock();
            tb2.Text = fullText.Length == 0 ? np.s_description : fullText;
            tb2.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f5f5f5"));
            tb2.TextWrapping = TextWrapping.Wrap;
            tb2.FontSize = 20;
            GridScrollView.Children.Add(tb2);
            Grid.SetRow(tb2, GridScrollView.RowDefinitions.Count - 1);
            Grid.SetColumnSpan(tb2,2);


            SQLiteConnection conn = DatabaseConnectionHandler.Invoke();

            sql = $"select i_local_address_id from " +
                  $"{TableNames.Images} where i_newspaper_id={np.id}";
            List<int> localAddressIds = conn.Query<int>(sql).ToList();
            
            foreach (int localAddressId in localAddressIds) // загрузка всех картинок 
            {
                sql = $"select s_path from {TableNames.LocalFileAddress} where id={localAddressId};";
                string localPath = conn.QueryFirst<string>(sql);
                var img = new System.Windows.Controls.Image();

                try
                {
                    img.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + localPath));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"error in image loading: {localPath} // {ex.Message}");
                    continue;
                }
                
                GridScrollView.RowDefinitions.Add(new RowDefinition());
                GridScrollView.Children.Add(img);
                Grid.SetRow(img, GridScrollView.RowDefinitions.Count - 1);
                Grid.SetColumnSpan(img, 2);
            }
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
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
                SQLiteConnection connection = DatabaseConnectionHandler.Invoke();

                BuildFullTextView(selectedItem);
                
                selectedItem.i_is_read = true;
                string sql = $"update {TableNames.Newspapers} set " +
                             $"i_is_read=1 where id={selectedItem.id}";
                connection.Execute(sql);
            }
        }
        private void TextBoxSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (ListBoxNewHeaders.ItemsSource != null && !_searchFieldWaterMarkActive)
            {
                SetHeadersSourceList(Search.SearchByQuery(_newspapers, TextBoxSearch.Text.Trim()));
            }
        }
        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_isFullNewsUpdatingNow)
            {
                ButtonSettings.IsEnabled = false;
                ButtonUpdate.IsEnabled = false;
                _loadFullNewsWorker.CancelAsync(); // останавливаем выполнение фонового парсинга
                _updateRSSWorker.RunWorkerAsync();
                ButtonUpdate.Content = "Остановить обновление";
            }
            else
            {
                _loadFullNewsWorker.CancelAsync();
                ButtonUpdate.IsEnabled = false;
                ButtonUpdate.Content = "Выполняется остановка...";
            }
        }

        private void ButtonMinimize_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonSettings_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_isFullNewsUpdatingNow)
            {
            SettingsWindow sw = new SettingsWindow();
            sw.ShowInTaskbar = true;
            sw.ShowDialog();

            _newspapers = DataBase.LoadSelectedNewspapers();
            SetHeadersSourceList(_newspapers);
            
            this._searchFieldWaterMarkActive = true;
            this.TextBoxSearch.Text = "Введите искомые слова";
            this.TextBoxSearch.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#A1A1A1"));
            }
            else
            {
                MessageBox.Show("Пожалуйста, дождитесь завершения обновления или прервите его.");
            }
        }
    }
}