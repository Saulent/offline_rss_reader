using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ClosedXML.Excel;
using Dapper;
using Microsoft.Win32;
using System.IO.Compression;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using Ionic.Zip;
using SaveOptions = ClosedXML.Excel.SaveOptions;


namespace second_course
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            TextBlockSettingsHeader.Text = this.Title;
            
            var t = DataBase.LoadNewsSources();
            ListBoxSources.ItemsSource = t;
        }

        private void HeaderSettingsRectangle_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void ButtonSettingsClose_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            DataBase.SetReadNewsSources(ListBoxSources.ItemsSource as List<NewsSource>);
            
            this.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            QueryConstructorWindow qcw = new QueryConstructorWindow();
            qcw.ShowInTaskbar = true;
            qcw.ShowDialog();
        }
        private void SaveExcelReport_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            string sql = $"select count(*) from {TableNames.Newspapers}";
            int count = connection.QueryFirst<int>(sql);

            if (count < 1)
            {
                MessageBox.Show("В базе нет новостей.");
                return;
            }
            String path = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Выберите место для сохранения отчёта";
            saveFileDialog.FileName = "report.xlsx";
            saveFileDialog.Filter = "Excel table (*.xlsx)|*.xlsx";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
            {
                path = saveFileDialog.FileName;
            }
            else
            {
                return;
            }

            CreateReport(path);
            MessageBox.Show($"Отчёт сохранён в: {path}");
        }
        private void ButtonSaveFullDB_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            string sql = $"select count(*) from {TableNames.Newspapers}";
            int count = connection.QueryFirst<int>(sql);

            if (count < 1)
            {
                MessageBox.Show("В базе нет новостей.");
                return;
            }

            String path = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Выберите место для сохранения резервной копии";
            saveFileDialog.FileName = $"{DateTime.Now.ToShortDateString()}.ofrebak";
            saveFileDialog.Filter = "Резервная копия базы данных (*.ofrebak)|*.ofrebak";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
            {
                path = saveFileDialog.FileName;
            }
            else
            {
                return;
            }

            CreateAndSaveArchive(path);
            MessageBox.Show($"Данные сохранены в: {saveFileDialog.FileName}");
        }
        private void ButtonAddSource_OnClick(object sender, RoutedEventArgs e)
        {
            NewsSource ns = new NewsSource();

            ns.s_rss_link = TextBoxAddSource.Text;

            // проверка на наличия в БД
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            string sql = $"select count(*) from {TableNames.NetworkAddress} where s_url='{ns.s_rss_link}';";
            if (connection.QueryFirst<int>(sql) != 0)
            {
                MessageBox.Show("Данный источник уже содержится в таблице");
                return;
            }

            try
            {
                TryLoadLink(ns);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления " + ex.Message);
                return;
            }

            DataBase.SaveNewsSource(ns);
            MessageBox.Show($"Источник {ns.s_name} успешно добавлен.");
            ListBoxSources.ItemsSource = DataBase.LoadNewsSources();
        }
        private void ButtonLoadFullDB_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите файл резервной копии";
            openFileDialog.Filter = "Резервная копия базы данных (*.ofrebak)|*.ofrebak";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == false)
            {
                return;
            }
            
            using (Ionic.Zip.ZipFile zip1 = Ionic.Zip.ZipFile.Read(openFileDialog.FileName))
            {
                foreach (Ionic.Zip.ZipEntry entry in zip1)
                {
                    entry.Extract(Directory.GetCurrentDirectory(), ExtractExistingFileAction.DoNotOverwrite);
                }
            }

            SQLiteConnection externalConnection =
                DatabaseConnectionHandler.InvokeWithPath($"{Directory.GetCurrentDirectory()}\\{Names.TempBDFile}");
            SQLiteConnection localConnection = DatabaseConnectionHandler.Invoke();

            List<Newspaper> externalNews = DataBase.LoadNewspapers(externalConnection);

            string sql;
            foreach (Newspaper np in externalNews) // заполнение новостей данными
            {
                sql = $"select i_is_parsed from {TableNames.NewspaperFullText} where id={np.id}";
                np.i_is_parsed = externalConnection.QueryFirst<bool>(sql);

                sql = $"select s_text from {TableNames.NewspaperFullText} where id={np.id}";
                np.s_full_text = externalConnection.QueryFirst<string>(sql);

                sql = $"select s_name from {TableNames.Thematic} where id={np.i_thematic_id}";
                string thematic = externalConnection.QueryFirst<string>(sql);

                np.i_thematic_id = DataBase.GetThematicID(thematic);
            }

            List<NewsSource> sources = DataBase.LoadNewsSources(externalConnection);
            
            sql = $"select image.id id, i_newspaper_id, s_path," +
                  $" s_url from {TableNames.Images} " +
                  $"INNER JOIN {TableNames.LocalFileAddress} " +
                  $"ON image.i_local_address_id = local_address.id " +
                  $"INNER JOIN {TableNames.NetworkAddress} ON " +
                  $"network_address.id = image.i_network_address_id;";

            List<ImageDB> images = externalConnection.Query<ImageDB>(sql).ToList();

            foreach (NewsSource source in sources)
            {
                sql = $"select count(*) from {TableNames.NetworkAddress} " +
                      $"where s_url='{source.s_rss_link}';";
                int cnt = localConnection.QueryFirst<int>(sql);
                if (cnt == 0)
                {
                    DataBase.SaveNewsSource(source);
                }
            }

            foreach (Newspaper np in externalNews) // статьи получают актуальные локальные айди и сохраняются в базе
            {
                List<ImageDB> newsImages = GetNewspaperImages(np.id, images);

                DataBase.SaveNewspaper(np);
                
                foreach (ImageDB image in newsImages)
                {
                    sql = $"select count(*) from {TableNames.LocalFileAddress} " +
                          $"where s_path = '{image.s_path}';";
                    int count = localConnection.QuerySingle<int>(sql);
                    if (count != 0)
                    {
                        continue;
                    }

                    DataBase.SaveImage(image);
                }
            }
            
            File.Delete(Names.TempBDFile);
            MessageBox.Show("Данные успешно загружены");
        }
        private void ButtonRemoveAllData_OnClick(object sender, RoutedEventArgs e)
        {
            File.Delete(Names.Database);
            
            if (Directory.Exists("temp"))
                Directory.Delete("temp", true);

            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            connection.Execute(Queries.FullDBCreationQuery);

            DataBase.SaveDefaultNewsSources();
            MainWindow.LastDBIdent = 0;

            this.Close();
        }

        private void CreateReport(string filePath)
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            var wb = new XLWorkbook();

            var ws = wb.Worksheets.Add("Новости");

            ws.Cell("A1").Value = "Дата";
            ws.Cell("B1").Value = "Название";
            ws.Cell("C1").Value = "Краткое описание";
            ws.Cell("D1").Value = "Полный текст (формат HTML)";
            ws.Cell("E1").Value = "Название источника";

            List<Newspaper> news = DataBase.LoadNewspapers(); 

            for (int i = 0; i < news.Count; i++) // сдвиг на 2 вверх для работы с ячейками экселя
            {
                ws.Cell($"A{i + 2}").Value = news[i].s_date;
                ws.Cell($"B{i + 2}").Value = news[i].s_header;
                ws.Cell($"C{i + 2}").Value = news[i].s_description;
                ws.Cell($"D{i + 2}").Value = connection.QueryFirst<String>($"select s_text from {TableNames.NewspaperFullText} where id={news[i].id}");
                ws.Cell($"E{i + 2}").Value = news[i].SourceName;
            }
            
            var rngTable = ws.Range("A1:E1");
            rngTable.Style.Font.Bold = true;

            var content = ws.Range($"A1:E{news.Count + 1}");
            content.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            content.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

            ws.Columns(1, 5).AdjustToContents();

            File.Delete(filePath);
            wb.SaveAs(filePath);
        }
        private void CreateAndSaveArchive(String path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            System.IO.Compression.ZipFile.CreateFromDirectory("temp", path, CompressionLevel.Optimal, true);

            using (ZipArchive arc = System.IO.Compression.ZipFile.Open(path, ZipArchiveMode.Update))
            {
                ZipFileExtensions.CreateEntryFromFile(arc, Names.Database, Names.TempBDFile, CompressionLevel.Optimal);
            }
        }
        private List<ImageDB> GetNewspaperImages(int id, List<ImageDB> images)
        {
            List<ImageDB> res = new List<ImageDB>();
            foreach (ImageDB image in images)
            {
                if (image.i_newspaper_id == id)
                    res.Add(image);
            }

            return res;
        }
        private void TryLoadLink(NewsSource source)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(source.s_rss_link);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            
            Encoding enc; // некоторые сайты не возвращают кодировку
            try
            {
                enc = Encoding.GetEncoding(resp.CharacterSet);
            }
            catch (System.ArgumentException)
            {
                Console.WriteLine("enc");
                enc = Encoding.UTF8;
            }

            StreamReader sr = new StreamReader(resp.GetResponseStream(), enc);
            using (XmlReader reader = XmlReader.Create(new StringReader(sr.ReadToEnd())))
            {
                var formatter = new Rss20FeedFormatter();
                formatter.ReadFrom(reader);

                string tempName = formatter.Feed.Title.Text;
                source.s_name = tempName.Substring(0, tempName.Length < 18 ? tempName.Length : 18);
                
                foreach (SyndicationItem syndItem in formatter.Feed.Items)
                {
                    Newspaper np = new Newspaper();
                    try
                    {
                        np.i_thematic_id =
                            DataBase.GetThematicID(syndItem.Categories.Count != 0
                                ? syndItem.Categories.First().Name
                                : "-");
                        np.s_header = syndItem.Title.Text.Replace("'", "");
                        np.s_description = syndItem.Summary?.Text.Replace("'", "");
                        np.s_date = syndItem.PublishDate.DateTime.ToString();
                        np.i_source_id = source.id;
                        np.s_link = syndItem.Links[0].Uri.ToString();
                        np.i_is_read = false;
                        DataBase.SaveNewspaper(np);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("np (rss adding) parsing error ");
                    }
                }
            }
        }
    }
}
