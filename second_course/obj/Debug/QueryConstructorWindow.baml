   M S B A M L   `   `   `  ����H  Dsecond_course, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:second_coursesecond_course  5   	   *    $second_course.QueryConstructorWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:second_course     5   	     � WindowStyle$
  None=�5   	    � 
ResizeMode$ NoResize=�5	   	    � ShowInTaskbar .   � Title$- 'Конструктор запросов��5   	   $��694.667��5   	   $��743.5��5   	   .��5      � ���#33393f � RowDefinitions 5   
   �� 5      $��25 �6   �� 5      $
��177* �6   �� 5      $
��161* �6   �� 5      $
��332* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $��* �6   6+   .+�5   
   �� -   5        $��ListBoxJoins��5      ��.  $��0q�6L   
�������$
��Left=�6w   $��	10,45,0,5q�6�   $��1��6�   $	��724��6�   $��2��6�   ��5      �� 5      .R�5      � $��0q�6    5      �� 5      $	��150 �6/   6=   �� 5       $	��150 �6/   6=   �� 5!      $	��150 �6/   6=   �� 5"      $	��150 �6/   6=   .+�5$      �� $��0��6#   ��63   ��  �� Path$ Storage.FirstTable� �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=�	 �� SelectedItem	 6�   �� $ Element.FirstTable�$ PropertyChanged=�$ TwoWay=�6�   �� 5%      $��1��6#   ��63   �� $ Storage.FirstField�$ PropertyChanged=�$ TwoWay=�	 6�   �� $ Element.FirstField�$ PropertyChanged=�$ TwoWay=�6�   �� 5&      $��2��6#   ��63   �� $ Storage.SecondField�$ PropertyChanged=�$ TwoWay=�	 6�   �� $ Element.SecondField�$ PropertyChanged=�$ TwoWay=�6�   �� 5'      $��3��6#   ��63   �� $ Storage.SecondTable�$ PropertyChanged=�$ TwoWay=�	 6�   �� $ Element.SecondTable�$ PropertyChanged=�$ TwoWay=�6�   5(      5)      5+      �� 5,   
   -   5        
 ��Name$
 HeaderQueryRectangle��5,      ���#373e45$��2��6n   �� 5-   
   -   5        $��TextBlockQueryHeader��5-      $��16(�60   
������ ���#f5f5f5$��Header text��6l   $��2��6   $��	0,-5,83,0q�6�   $	��Top=�6�    �� Padding$ 7q�6�   $��36��6�   $��2��6  �� 5.   
   $��1��6   $��	347,0,0,0q�6!   ._�5/      �� -   5         ��Name$ ButtonQueryClose��5/      $��resources/close.png��6.    J� RenderTransformOrigin$ 0.5,0.5&�6p   $��Right=�6�   $��25��6�   $��1��6�   $��0,0,0.333,0.333q�6�    � 	Resources 50       �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value��%�   '��       �� 51      " ��6   �52      �� 53       � ��
 ���� 67    55      P� 56       �@J�IsMouseOver . . �57      ��  � ��
 �  ��6;   58      5:      5;      5<      �� 5=   
   ���#f5f5f5$
��Left=�6)   $��14(�6D   $��10,6,0,0q�6R   $��1��6d   $
��Wrap=�6q   $	��Top=�6�   $��23��6�   $	��226��6�   �� 6�   $�Объединение��6�   �� 6�   $� таблиц��6�   6�   &   System.Windows.Controls.DataGrid  5?   
   -   5        $��DataGridResult��5?   <      SelectionMode$ Single=�6      
IsReadOnly . $��3��6R   $��
0,24,10,10q�6_   $��2��6s   $��Right=�6�   $	��724��6�   �� 5@   
   -   5        $��ListBoxColumns��5@      ��.  $��0q�6N   
�������$
��Left=�6y   $��
10,33,0,10q�6�   $��2��6�   $	��352��6�   ��5A      �� 5B      .R�5C      � $��0q�6    5D      �� 5E      $��20 �6/   6<   �� 5F      $	��200 �6/   6=   .+�5H      �� $��0��6#    d� 	IsChecked 63   �� $ 
IsSelected�6[   �� 5I      $��1��6$   ��64   �� $
 name�6Q   5J      5K      5M      �� 5O   
   -   5        $��ListBoxConditions��5O      ��.  $��0q�6Q   
�������$
��Left=�6|   $��
10,33,0,10q�6�   $��2��6�   $	��352��6�   $��1��6�   ��5P      �� 5Q      .R�5R      � $��0q�6    5S      �� 5T      $	��150 �6/   6=   �� 5U      $��50 �6/   6<   �� 5V      $	��120 �6/   6=   .+�5X      �� $��0��6$   ��64   �� $ 
ColumnName�$ PropertyChanged=�$ TwoWay=�6�   �� 5Y      $��1��6#   	 63   �� $ Operator�$ PropertyChanged=�$ TwoWay=���6�   �� $ OperatorsList�6�   �� 5Z      $��2��6"   ��62   �� $ ComparisonValue�$ PropertyChanged=�$ TwoWay=�6�   5[      5\      5^      �� 5_   
   $��14(�6   ���#f5f5f5$
��Left=�67   $��10,7,0,0q�6R   $��2��6d   $
��Wrap=�6q   $'��!Выводимые столбцы��6�   $	��Top=�6�   $��21��6�   $	��196��6�   �� 5`   
   $��14(�6   ���#f5f5f5$
��Left=�67   $��10,7,0,0q�6R   $��2��6d   $
��Wrap=�6q   $��Условия��6�   $	��Top=�6�   $��21��6�   $	��113��6�   $��1��6�   �� 5a   
   $��14(�6   ���#f5f5f5$
��Left=�67   $��10,0,0,0q�6R   $��3��6d   $
��Wrap=�6q   $��Вывод запроса��6�   $	��Top=�6�   $��21��6�   $	��125��6�   �� 5b   
   -   5        $%��Выполнить запрос��5b      $
��Left=�6G   $��	164,0,0,0q�6b   $��3��6u   $	��Top=�6�   $	��136��6�   $��20��6�   �� 5c   
   -	   5        $��ButtonAddJoinsRow��5c   +   $#��Добавить строку��6   $
��Left=�6g   $��	226,6,0,0q�6�   $��1��6�   $	��Top=�6�   $	��136��6�   $��1��6�   $��20��6�   �� 5d   
   ���#f5f5f5$
��Left=�6)   $��	10,29,0,0q�6D   $��1��6W   $
��Wrap=�6d   $	��Top=�6x   $	��125��6�   �� 6�   $	�1. ��6�   �� 6�   $�Первая т��6�   �� 6�   $�абли��6�   �� 6�   $
�ца��6�   6�   �� 5e   
   ���#f5f5f5$
��Left=�6)   $��	90,29,0,0q�6D   $��1��6W   $
��Wrap=�6d   $$��2. Вторая таблица��6x   $	��Top=�6�   $��1��6�   �� 5f   
   ���#f5f5f5$
��Left=�6)   $��
314,29,0,0q�6D   $��1��6X   $
��Wrap=�6e   $-��'4. Поле второй таблицы��6y   $	��Top=�6�   $��2��6�   �� 5g   
   ���#f5f5f5$
��Left=�6)   $��
164,29,0,0q�6D   $��1��6X   $
��Wrap=�6e   $-��'3. Поле первой таблицы��6y   $	��Top=�6�   �� 5h   
   ���#45d2bd$��1q�6'   $
��Left=�6;   $	��695��6V   $	��Top=�6c   $	��744��6{   $��4��6�   $��2��6�   �� 5i   
   
�������$
��Left=�6    $��1��6;   $	��Top=�6F   $	��744��6^   $��1��6j   $��2��6w   5j      5k      