/*
Created: 26.02.2019
Modified: 10.03.2019
Model: SQLite 3.7
Database: SQLite 3.7
*/


-- Create tables section -------------------------------------------------

-- Table newspaper

CREATE TABLE newspaper
(
  i_thematic_id INTEGER NOT NULL,
  i_header_id INTEGER,
  i_source_id INTEGER NOT NULL,
  s_date TEXT,
  i_is_read INTEGER,
  i_description_id INTEGER,
  s_link TEXT,
  id INTEGER NOT NULL,
  CONSTRAINT Key3 PRIMARY KEY (id,i_source_id,i_thematic_id),
  CONSTRAINT id UNIQUE (id),
  CONSTRAINT R6 FOREIGN KEY (id) REFERENCES np_full_text (id),
  CONSTRAINT R7 FOREIGN KEY (i_thematic_id) REFERENCES thematic (id),
  CONSTRAINT R9 FOREIGN KEY (i_source_id) REFERENCES news_source (id),
  CONSTRAINT Re6 FOREIGN KEY (id) REFERENCES newpaper_description (id),
  CONSTRAINT Re7 FOREIGN KEY (id) REFERENCES newspaper_header (id)
);

-- Table news_source

CREATE TABLE news_source
(
  id INTEGER NOT NULL
        CONSTRAINT Key2 PRIMARY KEY AUTOINCREMENT,
  i_site_name_id INTEGER,
  i_rss_url_id INTEGER,
  i_is_chosen INTEGER,
  CONSTRAINT R3 FOREIGN KEY (id) REFERENCES network_address (id)
);

-- Table image

CREATE TABLE image
(
  id INTEGER NOT NULL,
  i_newpaper_id INTEGER NOT NULL,
  i_local_address_id INTEGER,
  i_network_address_id INTEGER,
  CONSTRAINT Key5 PRIMARY KEY (i_newpaper_id,id),
  CONSTRAINT R12 FOREIGN KEY (i_newpaper_id) REFERENCES newspaper (id),
  CONSTRAINT R4 FOREIGN KEY (id) REFERENCES network_address (id),
  CONSTRAINT R5 FOREIGN KEY (id) REFERENCES local_address (id)
);

-- Table thematic

CREATE TABLE thematic
(
  id INTEGER NOT NULL
        CONSTRAINT Key6 PRIMARY KEY AUTOINCREMENT,
  s_name TEXT
);

-- Table newspaper_header

CREATE TABLE newspaper_header
(
  id INTEGER NOT NULL
        CONSTRAINT Key6 PRIMARY KEY AUTOINCREMENT,
  s_text TEXT
);

-- Table newpaper_description

CREATE TABLE newpaper_description
(
  id INTEGER NOT NULL
        CONSTRAINT Key6 PRIMARY KEY AUTOINCREMENT,
  s_text TEXT
);

-- Table site_name

CREATE TABLE site_name
(
  id INTEGER NOT NULL
        CONSTRAINT Key6 PRIMARY KEY AUTOINCREMENT,
  s_name TEXT,
  CONSTRAINT R1 FOREIGN KEY (id) REFERENCES news_source (id)
);

-- Table np_full_text

CREATE TABLE np_full_text
(
  s_text TEXT,
  id INTEGER NOT NULL
        CONSTRAINT Key7 PRIMARY KEY AUTOINCREMENT,
  i_is_parsed INTEGER
);

-- Table network_address

CREATE TABLE network_address
(
  s_url TEXT,
  id INTEGER NOT NULL
        CONSTRAINT Key8 PRIMARY KEY AUTOINCREMENT
);

-- Table local_address

CREATE TABLE local_address
(
  s_path TEXT,
  id INTEGER NOT NULL
        CONSTRAINT Key8 PRIMARY KEY AUTOINCREMENT
);

