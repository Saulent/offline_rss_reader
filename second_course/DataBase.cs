using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using Dapper;

namespace second_course
{
    public static class DatabaseConnectionHandler
    {
        private static SQLiteConnection connection;
        private static String connectionString = "Data Source = " + Consts.DBname + "; Version = 3; ";

        public static SQLiteConnection Invoke()
        {
            if (connection == null)
                connection = new SQLiteConnection(connectionString);
            return connection;
        }

        public static SQLiteConnection InvokeMultiple()
        {
            return new SQLiteConnection(connectionString);
        }

        public static SQLiteConnection InvokeWithPath(String dbPath)
        {
            String connString = "Data Source = " + dbPath + "; Version = 3; ";
            return new SQLiteConnection(connString);
        }
}

    public class DataBase
    {
        public static List<NewsSource> LoadNewsSources()
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            return new List<NewsSource>(connection.Query<NewsSource>($"select * from {Consts.NewSourcesTableName}"));
        }

        public static void SaveNewsSources(List<NewsSource> sources)
        {
            foreach (NewsSource source in sources)
            {
                SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
                String sql = $"update {Consts.NewSourcesTableName} set i_is_chosen={source.i_is_chosen} where id={source.id}";
                connection.Execute(sql);
            }
        }

        /// <summary>
        /// Обновляет полный текст новости в БД. Новость отмечается как обработанная.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fullText"></param>
        public static void UpdateNewspaperFullText(Int32 id, String fullText)
        {
            SQLiteConnection connection = DatabaseConnectionHandler.InvokeMultiple();
            String sql = $"update {Consts.FullTextTableName} set s_text='{fullText}', i_is_parsed=1 where id={id}";
            connection.Execute(sql);
        }

        /// <summary>
        /// Сохраняет новость в БД. Предполагается, что новости ещё нет в БД.
        /// </summary>
        /// <param name="np">Новость для сохранения</param>
        /// <returns>Удачно ли прошло сохранение</returns>
        public static Boolean SaveNewspaper(Newspaper np) // предполагается, что новости ещё нет в БД 
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke(); // проверка на наличие новости в БД
            if (np.id == -1) // useless?
            {
                np.id = MainWindow.LastDBIdent;
                MainWindow.LastDBIdent++;
            }

            String sql = $"insert into {Consts.NewsTableName} (id, i_thematic_id, s_header, i_source_id," +
                         $" s_date, s_description, s_link, i_is_read) " +
                         $"values ({np.id}, {np.i_thematic_id}, '{np.s_header.Replace("'", "")}', {np.i_source_id}, '{np.s_date}', '{np.s_description.Replace("'", "")}', '{np.s_link.Replace("'", "")}', {Convert.ToInt32(np.i_is_read).ToString()})";
            try
            {
                connection.Execute(sql);
                //sql = String.Format("select id from {0} where s_link='{1}' limit 1", Consts.NewsTableName, np.s_link); // устанавливаем хранимой новости id
                //np.id = connection.QueryFirst<Int32>(sql);
                var sql2 = $"insert into {Consts.FullTextTableName} (id, s_text, i_is_parsed) values ({np.id}, '{np.s_full_text?.Replace("'", "")}', 0)";
                connection.Execute(sql2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in newspaper save: " + ex.Message);
                return false; // если ломается запрос в БД, то потом могут воникнуть проблемы с обработкой полной новости. 
            }

            return true;
        }

        public static List<Newspaper> LoadNewspapers(SQLiteConnection connection = null)
        {
            if (connection == null)
                connection = DatabaseConnectionHandler.Invoke();
            
            String sql = $"select * from {Consts.NewsTableName} where i_source_id in (select id from {Consts.NewSourcesTableName} where i_is_chosen = 1)";
            List<Newspaper> newspapers = connection.Query<Newspaper>(sql).ToList();
            
            FillSourceNames(newspapers);

            newspapers.Sort((np1, np2) => np2.s_date.CompareTo(np1.s_date));

            return newspapers;
        }

        public static void FillSourceNames(List<Newspaper> newspapers)
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();

            List<NewsSource> sources = connection.Query<NewsSource>("select * from " + Consts.NewSourcesTableName).ToList();

            foreach (Newspaper newspaper in newspapers)
            {
                foreach (NewsSource source in sources)
                {
                    if (source.id == newspaper.i_source_id)
                    {
                        newspaper.SourceName = source.s_name;
                        break;
                    }
                }
            }

        }
    }
}
