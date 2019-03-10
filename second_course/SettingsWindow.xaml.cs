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

namespace second_course
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            TextBlockSettingsHeader.Text = this.Title;

            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            
            ListBoxSources.ItemsSource = connection.Query<NewsSource>("select * from " + Consts.NewSourcesTableName);
        }

        private void HeaderSettingsRectangle_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ButtonSettingsClose_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            DataBase.SaveNewsSources(ListBoxSources.ItemsSource as List<NewsSource>);
            
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            QueryConstructorWindow qcw = new QueryConstructorWindow();
            qcw.ShowDialog();
        }

        private void ButtonResetNewspapers_OnClick(object sender, RoutedEventArgs e)
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            connection.Execute($"delete from {Consts.NewsTableName}");
            connection.Execute($"delete from {Consts.FullTextTableName}");
            MainWindow.LastDBIdent = 0;
            this.Close();
        }

        private void SaveExcelReport_Click(object sender, RoutedEventArgs e)
        {
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
            MessageBox.Show($"Отчёт сохранён в: {saveFileDialog.FileName}");
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
                ws.Cell($"D{i + 2}").Value = connection.QueryFirst<String>($"select s_text from {Consts.FullTextTableName} where id={news[i].id}");
                ws.Cell($"E{i + 2}").Value = news[i].SourceName;
            }
            
            var rngTable = ws.Range("A1:E1");
            rngTable.Style.Font.Bold = true;

            var content = ws.Range($"A1:E{news.Count + 1}");
            content.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            content.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

            ws.Columns(1, 5).AdjustToContents();

            wb.SaveAs(filePath);
        }

        private void ButtonSaveFullDB_Click(object sender, RoutedEventArgs e)
        {
            String path = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Выберите место для сохранения данных";
            saveFileDialog.FileName = $"db_dump_{DateTime.Now.ToShortDateString()}.zip";
            saveFileDialog.Filter = "Zip архив (*.zip)|*.zip";
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

        private void CreateAndSaveArchive(String path)
        {
            using (ZipArchive arc = ZipFile.Open(path, ZipArchiveMode.Create))
            {
                ZipFileExtensions.CreateEntryFromFile(arc, Consts.DBname, Consts.TempBDFileName, CompressionLevel.Optimal);
            }
        }

        private void ButtonLoadFullDB_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите архив с новостями";
            openFileDialog.Filter = "Zip архив (*.zip)|*.zip";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (!openFileDialog.ShowDialog() == true)
            {
                return;
            }

            ZipFile.ExtractToDirectory(openFileDialog.FileName, Directory.GetCurrentDirectory()); 

            SQLiteConnection tempConn =
                DatabaseConnectionHandler.InvokeWithPath($"{Directory.GetCurrentDirectory()}\\{Consts.TempBDFileName}");

            List<Newspaper> tempNews = DataBase.LoadNewspapers(tempConn);
            List<Newspaper> localNews = DataBase.LoadNewspapers();

            foreach (Newspaper tempNewspaper in tempNews)
            {
                Boolean isExists = false;
                foreach (Newspaper localNewspaper in localNews)
                {
                    if (localNewspaper.s_link == tempNewspaper.s_link)
                    {
                        isExists = true;
                    }
                }

                if (!isExists)
                {
                    tempNewspaper.s_full_text =
                        tempConn.QueryFirst<String>(
                            $"select s_text from {Consts.FullTextTableName} where id={tempNewspaper.id}");

                    // TODO если добавлю картинки, перегрузить их тоже.
                    // на тематику забиваю.
                    // TODO Добавить вывод тематики в полной новости

                    DataBase.SaveNewspaper(tempNewspaper);
                }
            }

            MessageBox.Show("Данные успешно загружены");
            File.Delete(Consts.TempBDFileName);
        }
    }
}
