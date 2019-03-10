using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace second_course
{
    public class Consts
    {
        public const String DBname = "data.db";
        public const String TempBDFileName = "temp_database.db";
        public const String NewSourcesTableName = "news_source";
        public const String NewsTableName = "newspaper";
        public const String FullTextTableName = "full_text";

        public const String FullDBCreationQuery = @"
-- Create tables section -------------------------------------------------

-- Table newspaper

CREATE TABLE newspaper
(
  i_thematic_id INTEGER NOT NULL,
  s_header TEXT,
  i_source_id INTEGER NOT NULL,
  s_date TEXT,
  i_is_read INTEGER,
  s_description TEXT,
  s_link TEXT,
  id INTEGER NOT NULL,
  CONSTRAINT Key3 PRIMARY KEY (id,i_source_id,i_thematic_id),
  CONSTRAINT id UNIQUE (id),
  CONSTRAINT R6 FOREIGN KEY (id) REFERENCES full_text (id),
  CONSTRAINT R7 FOREIGN KEY (i_thematic_id) REFERENCES thematic (id),
  CONSTRAINT R9 FOREIGN KEY (i_source_id) REFERENCES news_source (id)
);

-- Table news_source

CREATE TABLE news_source
(
  id INTEGER NOT NULL
        CONSTRAINT Key2 PRIMARY KEY AUTOINCREMENT,
  s_name TEXT,
  s_rss_link TEXT,
  i_is_chosen INTEGER,
  temp_s_full_rss TEXT
);

-- Table image

CREATE TABLE image
(
  id INTEGER NOT NULL,
  i_newpaper_id INTEGER NOT NULL,
  s_local_link TEXT,
  s_inet_link TEXT,
  CONSTRAINT Key5 PRIMARY KEY (i_newpaper_id,id),
  CONSTRAINT R12 FOREIGN KEY (i_newpaper_id) REFERENCES newspaper (id)
);

-- Table thematic

CREATE TABLE thematic
(
  id INTEGER NOT NULL,
  s_name TEXT,
  CONSTRAINT Key6 PRIMARY KEY (id)
);

-- Table full_text

CREATE TABLE full_text
(
  s_text TEXT,
  id INTEGER NOT NULL,
  i_is_parsed INTEGER,
  CONSTRAINT Key7 PRIMARY KEY (id)
);
";

        public const String DefaultNewsSourcesQuery = @"
insert into news_source (s_name, s_rss_link, i_is_chosen)
values
('Медуза', 'https://meduza.io/rss/all', 1),
('Про Пермь', 'http://properm.ru/news/rss/', 1),
('Новый компаньон', 'https://www.newsko.ru/rss.php', 1),
('В курсе', 'http://v-kurse.ru/rss_news.php', 1);
        ";
    }
}
