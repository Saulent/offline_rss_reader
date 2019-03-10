using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dapper;

namespace second_course
{
    public partial class QueryConstructorWindow : Window
    {
        public QueryConstructorWindow()
        {
            InitializeComponent();
            TextBlockQueryHeader.Text = this.Title;

            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            ListViewTest.ItemsSource = connection.Query<Table>("SELECT name FROM sqlite_master WHERE type='table';").ToList();
        }

        private void HeaderQueryRectangle_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ButtonQueryClose_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        
        private String ComposeQuery(String tableName)
        {
            Boolean oneColumnExist = false;
            String sql = "select ";
            foreach (TableColumn column in DataGridColumns.ItemsSource)
            {
                if (column.selected)
                {
                    if (oneColumnExist)
                        sql += ", ";
                    sql += column.name + " ";
                    oneColumnExist = true;
                }
            }

            if (!oneColumnExist) // если не выделено ни одного столбца, то выводятся все
                sql += "* ";

            sql += String.Format("from {0}", tableName);
            
            return sql;
        }
        
        private void ListViewTest_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String tableName = ((Table)ListViewTest.SelectedItem).name;
            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();

            String sql = String.Format("PRAGMA table_info({0});", tableName);
            List<TableColumn> itemsSource = new List<TableColumn>(connection.Query<TableColumn>(sql));
            DataGridColumns.ItemsSource = itemsSource;

            DataGridColumns.Columns[0].IsReadOnly = true; // чтобы нельзя было редактировать имя столбца
        }

        private void DataGridColumns_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String tableName = ((Table)ListViewTest.SelectedItem).name;
            //String tableName = ((Table)DataGridTables.SelectedItem).name;
            String sql = ComposeQuery(tableName);
            DataTable table = new DataTable();
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(sql, DatabaseConnectionHandler.Invoke());
            dataAdapter.Fill(table);
            DataGridResult.ItemsSource = table.DefaultView;
        }
    }
}
