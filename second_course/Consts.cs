using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace second_course
{
    public class TableNames
    {
        public const String LocalFileAddress = "local_address";
        public const String Images = "image";
        public const String Thematic = "thematic";
        public const String NewspaperHeader = "newspaper_header";
        public const String NewspaperDescription = "newspaper_description";
        public const String NetworkAddress = "network_address";
        public const String Newspapers = "newspaper";
        public const String NewspaperFullText = "np_full_text";
        public const String NewsSource = "news_source";
        public const String SourceSiteName = "site_name";
    }

    class Params
    {
        public static bool isLocalServerUsing = false;
    }

    public class Names
    {
        public const String Database = "data.db";
        public const String TempBDFile = "temp_database.db";
    }

    public class Queries
    {
        public const String FullDBCreationQuery = @"
CREATE TABLE newspaper
(
  i_thematic_id INTEGER NOT NULL,
  i_header_id INTEGER,
  i_source_id INTEGER NOT NULL,
  s_date TEXT,
  i_is_read INTEGER,
  i_description_id INTEGER,
  s_link TEXT,
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT
);

-- Table news_source

CREATE TABLE news_source
(
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  i_site_name_id INTEGER,
  i_rss_url_id INTEGER,
  i_is_chosen INTEGER
);

-- Table image

CREATE TABLE image
(
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  i_newspaper_id INTEGER NOT NULL,
  i_local_address_id INTEGER,
  i_network_address_id INTEGER
);

-- Table thematic

CREATE TABLE thematic
(
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  s_name TEXT
);

-- Table newspaper_header

CREATE TABLE newspaper_header
(
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  s_text TEXT
);

-- Table newspaper_description

CREATE TABLE newspaper_description
(
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  s_text TEXT
);

-- Table site_name

CREATE TABLE site_name
(
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  s_name TEXT
);

-- Table np_full_text

CREATE TABLE np_full_text
(
  s_text TEXT,
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  i_is_parsed INTEGER
);

-- Table network_address

CREATE TABLE network_address
(
  s_url TEXT,
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT
);

-- Table local_address

CREATE TABLE local_address
(
  s_path TEXT,
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT
);
";
    }
}