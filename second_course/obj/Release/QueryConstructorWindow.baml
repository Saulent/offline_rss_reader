   M S B A M L   `   `   `  ���� H  Dsecond_course, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:second_coursesecond_course  *    $second_course.QueryConstructorWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:second_course       � WindowStyle$
  None=� � 
ResizeMode$ NoResize=� � ShowInTaskbar .   � Title$- 'Конструктор запросов��$��694.667��$��743.5��.��� ���#33393f � RowDefinitions �� $��25 ��� $
��177* ��� $
��161* ��� $
��332* � � ColumnDefinitions �� $��* ��� $��* �.+��� -   $��ListBoxJoins����.  $��0q�
�������$
��Left=�$��	10,45,0,5q�$��1��$	��724��$��2������ .R�� $��0q� �� $	��150 ��� $	��150 ��� $	��150 ��� $	��150 �.+��� $��0������  �� Path$ Storage.FirstTable� �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=�	 �� SelectedItem	 �� $ Element.FirstTable�$ PropertyChanged=�$ TwoWay=��� $��1������ $ Storage.FirstField�$ PropertyChanged=�$ TwoWay=�	 �� $ Element.FirstField�$ PropertyChanged=�$ TwoWay=��� $��2������ $ Storage.SecondField�$ PropertyChanged=�$ TwoWay=�	 �� $ Element.SecondField�$ PropertyChanged=�$ TwoWay=��� $��3������ $ Storage.SecondTable�$ PropertyChanged=�$ TwoWay=�	 �� $ Element.SecondTable�$ PropertyChanged=�$ TwoWay=��� -   
 ��Name$
 HeaderQueryRectangle�����#373e45$��2���� -   $��TextBlockQueryHeader��$��16(�
������ ���#f5f5f5$��Header text��$��2��$��	0,-5,83,0q�$	��Top=� �� Padding$ 7q�$��36��$��2���� $��1��$��	347,0,0,0q�._��� -    ��Name$ ButtonQueryClose��$��resources/close.png�� J� RenderTransformOrigin$ 0.5,0.5&�$��Right=�$��25��$��1��$��0,0,0.333,0.333q� � 	Resources  �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value��%o   '��       �� " �����  � ��
 ����  P�  �@J�IsMouseOver . . ���  � ��
 �  ���� ���#f5f5f5$
��Left=�$��14(�$��10,6,0,0q�$��1��$
��Wrap=�$	��Top=�$��23��$	��226���� $�Объединение���� $� таблиц��&   System.Windows.Controls.DataGrid  -   $��DataGridResult��   SelectionMode$ Single=�   
IsReadOnly . $��3��$��
0,24,10,10q�$��2��$��Right=�$	��724���� -   $��ListBoxColumns����.  $��0q�
�������$
��Left=�$��
10,33,0,10q�$��2��$	��352������ .R�� $��0q� �� $��20 ��� $	��200 �.+��� $��0�� d� 	IsChecked �� $ 
IsSelected��� $��1������ $
 name��� -   $��ListBoxConditions����.  $��0q�
�������$
��Left=�$��
10,33,0,10q�$��2��$	��352��$��1������ .R�� $��0q� �� $	��150 ��� $��50 ��� $	��120 �.+��� $��0������ $ 
ColumnName�$ PropertyChanged=�$ TwoWay=��� $��1��	 �� $ Operator�$ PropertyChanged=�$ TwoWay=����� $ OperatorsList��� $��2������ $ ComparisonValue�$ PropertyChanged=�$ TwoWay=��� $��14(����#f5f5f5$
��Left=�$��10,7,0,0q�$��2��$
��Wrap=�$'��!Выводимые столбцы��$	��Top=�$��21��$	��196���� $��14(����#f5f5f5$
��Left=�$��10,7,0,0q�$��2��$
��Wrap=�$��Условия��$	��Top=�$��21��$	��113��$��1���� $��14(����#f5f5f5$
��Left=�$��10,0,0,0q�$��3��$
��Wrap=�$��Вывод запроса��$	��Top=�$��21��$	��125���� -   $%��Выполнить запрос��$
��Left=�$��	164,0,0,0q�$��3��$	��Top=�$	��136��$��20���� -	   $��ButtonAddJoinsRow��$#��Добавить строку��$
��Left=�$��	226,6,0,0q�$��1��$	��Top=�$	��136��$��1��$��20���� ���#f5f5f5$
��Left=�$��	10,29,0,0q�$��1��$
��Wrap=�$	��Top=�$	��125���� $	�1. ���� $�Первая т���� $�абли���� $
�ца���� ���#f5f5f5$
��Left=�$��	90,29,0,0q�$��1��$
��Wrap=�$$��2. Вторая таблица��$	��Top=�$��1���� ���#f5f5f5$
��Left=�$��
314,29,0,0q�$��1��$
��Wrap=�$-��'4. Поле второй таблицы��$	��Top=�$��2���� ���#f5f5f5$
��Left=�$��
164,29,0,0q�$��1��$
��Wrap=�$-��'3. Поле первой таблицы��$	��Top=��� ���#45d2bd$��1q�$
��Left=�$	��695��$	��Top=�$	��744��$��4��$��2���� 
�������$
��Left=�$��1��$	��Top=�$	��744��$��1��$��2��