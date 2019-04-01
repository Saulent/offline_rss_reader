using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using Dapper;

namespace second_course
{
    public static class DatabaseConnectionHandler
    {
        private static SQLiteConnection connection;
        private static String connectionString = "Data Source = " + Names.Database + "; Version = 3; ";

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

    public static class DataBase
    {
        public static void SaveDefaultNewsSources()
        {
            List<NewsSource> defaultSources = new List<NewsSource>();
            defaultSources.Add(new NewsSource("Медуза", "https://meduza.io/rss/all", true));
            defaultSources.Add(new NewsSource("Про Пермь", "http://properm.ru/news/rss/", true));
            defaultSources.Add(new NewsSource("Новый компаньон", "https://www.newsko.ru/rss.php", true));
            defaultSources.Add(new NewsSource("Лента", "https://lenta.ru/rss/news", true));
            defaultSources.Add(new NewsSource("Рамблер общество", "https://news.rambler.ru/rss/community/", true));
            defaultSources.Add(new NewsSource("АИФ политика", "http://www.aif.ru/rss/politics.php", true));

            foreach (NewsSource ns in defaultSources)
            {
                SaveNewsSource(ns);
            }
        }
        public static void SaveImage(ImageDB image)
        {
            SQLiteConnection connection = DatabaseConnectionHandler.InvokeMultiple();

            string sql = $"insert into {TableNames.NetworkAddress} (s_url) values ('{image.s_url}')";
            connection.Execute(sql);

            int networkId = connection.QueryFirst<int>(
                $"select id from {TableNames.NetworkAddress} where s_url='{image.s_url}'");

            connection.Execute($"insert into {TableNames.LocalFileAddress} " +
                               $"(s_path) values ('{image.s_path}')");

            int localId = connection.QueryFirst<int>(
                $"select id from {TableNames.LocalFileAddress} where s_path='{image.s_path}'");

            connection.Execute($"insert into {TableNames.Images} (i_newspaper_id, " +
                               $"i_local_address_id, i_network_address_id) values " +
                               $"({image.i_newspaper_id}, {localId}, {networkId})");
        }
        public static List<NewsSource> LoadNewsSources(SQLiteConnection connection = null)
        {
            if (connection == null)
                connection = DatabaseConnectionHandler.Invoke();

            String sql = $"select src.id id, names.s_name s_name, address.s_url " +
                         $"s_rss_link, i_is_chosen from ({TableNames.NewsSource} " +
                         $"src INNER JOIN {TableNames.SourceSiteName} names ON " +
                         $"src.i_site_name_id = names.id) INNER JOIN " +
                         $"{TableNames.NetworkAddress} address ON src.i_rss_url_id = address.id";

            return new List<NewsSource>(connection.Query<NewsSource>(sql));
        }
        public static void SaveNewsSource(NewsSource source)
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();

            string sql = $"insert into {TableNames.SourceSiteName} (s_name) values ('{source.s_name}');";
            connection.Execute(sql);
            //sql = $"select id from {TableNames.SourceSiteName} where rowid=last_insert_rowid();";
            sql = $"select max(id) from {TableNames.SourceSiteName};";
            int siteNameId = connection.QueryFirst<int>(sql);

            sql = $"insert into {TableNames.NetworkAddress} (s_url) values ('{source.s_rss_link}');";
            connection.Execute(sql);
            //sql = $"select id from {TableNames.NetworkAddress} where rowid=last_insert_rowid();";
            sql = $"select max(id) from {TableNames.NetworkAddress};";
            int networkAddressId = connection.QueryFirst<int>(sql);

            sql = $"insert into {TableNames.NewsSource} (i_site_name_id, i_rss_url_id, i_is_chosen) " +
                  $"values ({siteNameId}, {networkAddressId}, 1);";
            connection.Execute(sql);
        }
        public static void SetReadNewsSources(List<NewsSource> sources)
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            foreach (NewsSource source in sources)
            {
                String sql = $"update {TableNames.NewsSource} set " +
                             $"i_is_chosen={source.i_is_chosen} where id={source.id}";
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
            String sql = $"update {TableNames.NewspaperFullText} set " +
                         $"s_text='{fullText}', i_is_parsed=1 where id={id}";
            connection.Execute(sql);

            //DatabaseConnectionHandler.SafeExecute(sql, connection);
        }
        public static Int32 GetThematicID(String thematicName)
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            String sql = $"select count(*) from {TableNames.Thematic} where s_name = '{thematicName}'";
            int thematicId = -1;

            if (connection.QueryFirst<int>(sql) == 0)
            {
                String sqlInsert = $"insert into {TableNames.Thematic} (s_name) values ('{thematicName}')";
                connection.Execute(sqlInsert);
            }

            sql = $"select id from {TableNames.Thematic} where s_name = '{thematicName}'";
            thematicId = connection.QueryFirst<Int32>(sql);
            
            return thematicId;
        }
        public static Boolean SaveNewspaper(Newspaper np)
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();

            string sql = $"select count(*) from {TableNames.Newspapers} where s_link='{np.s_link}'";
            int t = connection.QueryFirst<int>(sql);
            if (t != 0) // проверка наличия новости в БД
            {
                return true;
            }
            
            np.id = MainWindow.LastDBIdent;
            MainWindow.LastDBIdent++;
            
            try
            {
                sql = $"insert into {TableNames.NewspaperHeader} (s_text) " +
                      $"values ('{np.s_header}');";
                connection.Execute(sql);

                int headerId = 
                    connection.QueryFirst<int>($"select id from " +
                                               $"{TableNames.NewspaperHeader} " +
                                               $"where s_text='{np.s_header}'");

                sql = $"insert into {TableNames.NewspaperDescription} " +
                      $"(s_text) values ('{np.s_description}');";
                connection.Execute(sql);

                int descriptionId =
                    connection.QueryFirst<int>($"select id from " +
                                               $"{TableNames.NewspaperDescription} " +
                                               $"where s_text='{np.s_description}'");

                sql = $"insert into {TableNames.NewspaperFullText} " +
                      $"(id, s_text, i_is_parsed) values ({np.id}, " +
                      $"'{np.s_full_text}', {np.i_is_parsed});";
                connection.Execute(sql);

                sql =$"insert into {TableNames.Newspapers} (i_thematic_id, " +
                     $"i_header_id, i_source_id, s_date, i_is_read, i_description_id, s_link) " +
                     $"values ({np.i_thematic_id}, {headerId}, {np.i_source_id}, " +
                     $"'{np.s_date}', {0}, {descriptionId}, '{np.s_link}');";
                connection.Execute(sql);
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
            
            String sql = $"select news.id id, news.s_date s_date, descs.s_text " +
                         $"s_description, heads.s_text s_header, i_thematic_id," +
                         $" i_source_id, i_is_read, s_link from (({TableNames.Newspapers} " +
                         $"news INNER JOIN {TableNames.NewspaperHeader} heads " +
                         $"ON news.i_header_id = heads.id) INNER JOIN {TableNames.NewspaperDescription} " +
                         $"descs ON news.i_description_id = descs.id)";

            List<Newspaper> newspapers = connection.Query<Newspaper>(sql).ToList();
            
            FillSourceNames(newspapers);
            newspapers.Sort((np1, np2) => np2.s_date.CompareTo(np1.s_date));

            return newspapers;
        }
        public static List<Newspaper> LoadSelectedNewspapers(SQLiteConnection connection = null)
        {
            if (connection == null)
                connection = DatabaseConnectionHandler.Invoke();

            String sql =
                $"select news.id id, news.s_date s_date, descs.s_text " +
                $"s_description, heads.s_text s_header, i_thematic_id, " +
                $"i_source_id, i_is_read, s_link from (({TableNames.Newspapers} " +
                $"news INNER JOIN {TableNames.NewspaperHeader} heads " +
                $"ON news.i_header_id = heads.id) INNER JOIN" +
                $" {TableNames.NewspaperDescription} descs ON " +
                $"news.i_description_id = descs.id) where i_source_id in " +
                $"(select id from {TableNames.NewsSource} where i_is_chosen = 1)";

            List<Newspaper> newspapers = connection.Query<Newspaper>(sql).ToList();

            FillSourceNames(newspapers);
            newspapers.Sort((np1, np2) => np2.s_date.CompareTo(np1.s_date));

            return newspapers;
        }
        public static void FillSourceNames(List<Newspaper> newspapers)
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();

            List<NewsSource> sources = connection.Query<NewsSource>($"select * from {TableNames.NewsSource} INNER JOIN {TableNames.SourceSiteName} names ON i_site_name_id = names.id").ToList();

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