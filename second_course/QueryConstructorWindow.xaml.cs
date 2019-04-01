using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Dapper;

namespace second_course
{
    public partial class QueryConstructorWindow : Window
    {
        private List<string> _allTablesString;
        private readonly List<string> _operators = new List<string> { "-", "=", "<", ">", "<>" };

        public QueryConstructorWindow()
        {
            InitializeComponent();

            SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
            TextBlockQueryHeader.Text = this.Title;
            //_allTables = connection.Query<Table>("SELECT name FROM sqlite_master WHERE type='table';").ToList();
            _allTablesString = connection.Query<string>("SELECT name FROM sqlite_master WHERE type='table';").ToList();
            _allTablesString.Insert(0, "-");


            JoinTableElement jte = new JoinTableElement();
            JoinTableElementStorage js = new JoinTableElementStorage();

            js.FirstTable = _allTablesString;
            js.SecondTable = _allTablesString;
            
            jte.OwnStorage = js;

            JoinTableElementPack jp = new JoinTableElementPack();
            jp.Element = jte;
            jp.Storage = js;
            
            var t = new List<JoinTableElementPack>();

            jp.Element.PropertyChanged += ElementChangeHandler;

            t.Add(jp);

            ListBoxJoins.ItemsSource = t;
        }

        private void ElementChangeHandler(object obj, PropertyChangedEventArgs e)
        {
            var element = obj as JoinTableElement;

            switch (e.PropertyName)
            {
                case "FirstTable":
                    element.OwnStorage.FirstField = GetListOfColumnNames(element.FirstTable);
                    element.FirstField = "-";
                    break;
                case "SecondTable":
                    element.OwnStorage.SecondField = GetListOfColumnNames(element.SecondTable);
                    element.SecondField = "-";
                    break;
            }

            // без этого костыля значения у списков полей не обновляются в ГУИ
            List<JoinTableElementPack> packs = ListBoxJoins.ItemsSource as List<JoinTableElementPack>;
            ListBoxJoins.ItemsSource = null;
            ListBoxJoins.ItemsSource = packs;

            UpdateColumnsListBox(GetSelectedTables());
        }
        private List<string> GetSelectedTables()
        {
            List<JoinTableElementPack> packs = ListBoxJoins.ItemsSource as List<JoinTableElementPack>;
            List<string> selectedTables = new List<string>();

            bool isJoinsFound = false;

            foreach (var pack in packs)
            {
                if (pack.Element.FirstTable != null && pack.Element.SecondTable != null &&
                    pack.Element.FirstField != null && pack.Element.SecondField != null &&
                    pack.Element.FirstTable != "-" && pack.Element.SecondTable != "-" &&
                    pack.Element.FirstField != "-" &&
                    pack.Element.SecondField != "-") // проверка на завершённость строки джойнов
                {
                    isJoinsFound = true;
                    selectedTables.Add(pack.Element.FirstTable);
                    selectedTables.Add(pack.Element.SecondTable);
                }
            }

            if (!isJoinsFound)
            {
                foreach (var pack in packs)
                {
                    if (pack.Element.FirstTable != null && pack.Element.FirstTable != "-")
                    {
                        selectedTables.Add(pack.Element.FirstTable);
                        break;
                    }

                    if (pack.Element.SecondTable != null && pack.Element.SecondTable != "-")
                    {
                        selectedTables.Add(pack.Element.SecondTable);
                        break;
                    }
                }
            }

            return selectedTables;
        }
        private void UpdateColumnsListBox(List<string> selectedTables)
        {
            List<string> columns = new List<string>();
            List<TableColumn> items = new List<TableColumn>();

            foreach (string table in selectedTables)
            {
                foreach (string columnName in GetListOfColumnNames(table))
                {
                    string cName = table + "." + columnName;
                    if (columnName != "-" && !columns.Contains(cName))
                        columns.Add(cName);
                }
            }

            foreach (string columnName in columns)
            {
                var temp = new TableColumn {IsSelected = true, name = columnName};
                temp.PropertyChanged += OnColumnSelectionChanged;
                items.Add(temp);
            }
            ListBoxColumns.ItemsSource = items;
            FillConditionTable();
        }
        private List<string> GetListOfColumnNames(string tableName)
        {
            if (tableName != "-")
            {
                SQLiteConnection connection = DatabaseConnectionHandler.Invoke();
                string sql = $"PRAGMA table_info({tableName});";
                List<TableColumn> columns = connection.Query<TableColumn>(sql).ToList();
                List<string> res = new List<string>();
                res.Add("-");
                columns.ForEach(c => res.Add(c.name));
                return res;
            }

            return null;
        }
        private void ButtonQueryClose_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void OnColumnSelectionChanged(object obj, PropertyChangedEventArgs e)
        {
            FillConditionTable();
        }
        private void FillConditionTable()
        {
            var lst = ListBoxColumns.ItemsSource as List<TableColumn>;
            List<ConditionsTableElement> cte = new List<ConditionsTableElement>();

            foreach (TableColumn tc in lst)
            {
                cte.Add(new ConditionsTableElement { ColumnName = tc.name, ComparisonValue = "", Operator = "-", OperatorsList = _operators });
            }

            ListBoxConditions.ItemsSource = cte;
            
            //List<ConditionsTableElement> cte = new List<ConditionsTableElement>();

            //foreach (string column in GetSelectedColumns())
            //{
            //    cte.Add(new ConditionsTableElement { ColumnName = column, ComparisonValue = "", Operator = "-", OperatorsList = _operators });
            //}

            //ListBoxConditions.ItemsSource = cte;
        }
        private string PerformConditions(string sql)
        {
            bool conditionsExists = false;

            List<ConditionsTableElement> cte = ListBoxConditions.ItemsSource as List<ConditionsTableElement>;

            foreach (var element in cte)
            {
                if (element.Operator != "-" && !string.IsNullOrEmpty(element.Operator))
                    conditionsExists = true;
            }

            if (conditionsExists)
            {
                sql += " WHERE";

                bool firstDone = false;

                foreach (var element in cte)
                {
                    if (element.Operator != "-" && !string.IsNullOrEmpty(element.Operator))
                    {
                        if (firstDone)
                        {
                            sql += " AND ";
                        }

                        if (element.ColumnName.Split('.')[1][0] == 's')
                        {
                            sql += $" {element.ColumnName} {element.Operator} '{element.ComparisonValue}'";
                        }
                        else
                        {
                            sql += $" {element.ColumnName} {element.Operator} {element.ComparisonValue}";
                        }
                        firstDone = true;
                    }
                }
            }
            
            return sql;
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            List<JoinTableElementPack> lst = ListBoxJoins.ItemsSource as List<JoinTableElementPack>;
            bool execauteSql = false;
            List<string> tablesInUse = new List<string>();
            
            string sql = $"SELECT # FROM";

            foreach (var pack in lst)
            {
                if (pack.Element.FirstTable != null && pack.Element.SecondTable != null &&
                    pack.Element.FirstField != null && pack.Element.SecondField != null &&
                    pack.Element.FirstTable != "-" && pack.Element.SecondTable != "-" &&
                    pack.Element.FirstField != "-" && pack.Element.SecondField != "-") // проверка на завершённость строки джойнов
                {
                    if (execauteSql)
                    {
                        if (!tablesInUse.Contains(pack.Element.FirstTable) &&
                            !tablesInUse.Contains(pack.Element.SecondTable))
                        {
                            MessageBox.Show("Попытка соединить две пары разных таблиц в одном запросе");
                            return;
                        }
                    }
                    if (!execauteSql)
                    {
                        sql += $" {pack.Element.FirstTable}";
                        execauteSql = true;
                    }

                    if (pack.Element.FirstTable == pack.Element.SecondTable)
                    {
                        MessageBox.Show("Попытка объединить таблицу с ней же.");
                        return;
                    }

                    if (tablesInUse.Contains(pack.Element.SecondTable))
                    {
                        sql += $" INNER JOIN {pack.Element.FirstTable} ON " +
                               $"{pack.Element.FirstTable}.{pack.Element.FirstField} = " +
                               $"{pack.Element.SecondTable}.{pack.Element.SecondField}";
                    }
                    else
                    {
                        sql += $" INNER JOIN {pack.Element.SecondTable} ON " +
                               $"{pack.Element.FirstTable}.{pack.Element.FirstField} = " +
                               $"{pack.Element.SecondTable}.{pack.Element.SecondField}";
                    }
                    tablesInUse.Add(pack.Element.FirstTable);
                    tablesInUse.Add(pack.Element.SecondTable);
                }
            }

            if (!execauteSql)
            {
                foreach (var pack in lst)
                {
                    if (pack.Element.FirstTable != null && pack.Element.FirstTable != "-")
                    {
                        sql = $"SELECT # FROM {pack.Element.FirstTable}";
                        execauteSql = true;
                        tablesInUse.Add(pack.Element.FirstTable);
                        break;
                    }
                    if (pack.Element.SecondTable != null && pack.Element.SecondTable != "-")
                    {
                        sql = $"SELECT # FROM {pack.Element.SecondTable}";
                        execauteSql = true;
                        tablesInUse.Add(pack.Element.SecondTable);
                        break;
                    }
                }
            }

            List<string> selectedColumns = GetSelectedColumns();
            if (selectedColumns.Count < 1)
            {
                MessageBox.Show("Выберите хотя бы один столбец для вывода.");
                return;
            }
            sql = PerformSelectedColumns(sql, selectedColumns);
            
            sql = PerformConditions(sql);
            
            if (execauteSql)
            {
                try
                {
                    System.Data.DataTable table = new System.Data.DataTable();
                    SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(sql, DatabaseConnectionHandler.Invoke());
                    dataAdapter.Fill(table);
                    DataGridResult.ItemsSource = table.DefaultView;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(sql, "Ошибка: " + exception.Message);
                }
            }
            else
            {
                MessageBox.Show(
                    "Для выполнения запроса выберите таблицу, либо установите значения для объединения таблиц.");
            }

            //MessageBox.Show(sql);
        }
        private string PerformSelectedColumns(string sql, List<string> selectedColumns)
        {
            var split = sql.Split('#');

            string res = split[0];
            string secondPart = split[1];
            string selectedColumnsString = "";

            bool isAdded = false;

            foreach (string column in selectedColumns)
            {
                if (isAdded)
                {
                    res += ", ";
                }
                res += $"{column}";
                isAdded = true;
            }

            res += " ";
            res += secondPart;
            return res;
        }
        private List<string> GetSelectedColumns()
        {
            var columns = ListBoxColumns.ItemsSource as List<TableColumn>;
            List<string> res = new List<string>();

            foreach (TableColumn column in columns)
            {
                if (column.IsSelected)
                {
                    res.Add(column.name);
                }
            }

            return res;
        }
        private void ButtonAddJoinsRow_OnClick(object sender, RoutedEventArgs e)
        {
            List<JoinTableElementPack> packs = ListBoxJoins.ItemsSource as List<JoinTableElementPack>;
            
            JoinTableElement jte = new JoinTableElement();
            JoinTableElementStorage js = new JoinTableElementStorage();
            jte.OwnStorage = js;

            //js.FirstTable = GetExistTables(packs);
            js.FirstTable = _allTablesString;
            js.SecondTable = _allTablesString;

            JoinTableElementPack joinTableElementPack = new JoinTableElementPack{Element = jte, Storage = js};

            joinTableElementPack.Element.PropertyChanged += ElementChangeHandler;
            
            packs.Add(joinTableElementPack);

            ListBoxJoins.ItemsSource = null;
            ListBoxJoins.ItemsSource = packs;
        }
        private void HeaderQueryRectangle_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
