using System;
using System.Data.SQLite;
using System.Windows;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using second_course;

namespace second_course_testing
{
    // кидать в исходник актуальную версию БД

    [TestClass]
    public class UnitTests
    {
        private static Int32 _testingID = 666666;
        private static Newspaper _testingNewspaper;
        [TestMethod]
        public void DatabaseWriteNewspaper()
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke(); 
            connection.Execute("delete from newpaper where id=" + _testingID);

            Newspaper np = new Newspaper();
            np.id = _testingID;
            np.s_header = "test header";
            np.s_description = "test description";
            np.i_source_id = 1083;
            np.s_full_text = "test full text";
            np.s_link = "test link";
            np.s_date = "01.01.1999 20:21:34";
            
            _testingNewspaper = np;

            DataBase.SaveNewspaper(np);

            Assert.AreEqual(0, 0);
        }

        [TestMethod]
        public void DatabaseReadNewspaper()
        {
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            
            Newspaper np = connection.QueryFirst<Newspaper>("select * from newpaper where id=" + _testingID);

            Assert.AreEqual(np.id, _testingID);
            Assert.AreEqual(np.s_header, _testingNewspaper.s_header);
            Assert.AreEqual(np.s_description, _testingNewspaper.s_description);
            Assert.AreEqual(np.i_source_id, _testingNewspaper.i_source_id);
            Assert.AreEqual(np.s_full_text, _testingNewspaper.s_full_text);
            Assert.AreEqual(np.s_link, _testingNewspaper.s_link);
            Assert.AreEqual(np.s_date, _testingNewspaper.s_date);
            
            connection.Execute("delete from newpaper where id=" + _testingID);
        }
    }
}
