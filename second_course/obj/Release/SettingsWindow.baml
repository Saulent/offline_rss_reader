   M S B A M L   `   `   `  ���� H  Dsecond_course, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:second_coursesecond_course  "    second_course.SettingsWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:second_course       � WindowStyle$
  None=� � 
ResizeMode$ NoResize=� � Title$ Настройки�� � ShowInTaskbar .  $��500.667��$	��500�� � 	Resources    StandardBorderBrush �� Color  StandardBackgroundBrush  HoverBorderBrush  SelectedBackgroundBrush  SelectedForegroundBrush  DisabledForegroundBrush  
GlyphBrush  NormalBrush  NormalBorderBrush 	 HorizontalNormalBrush 
 HorizontalNormalBorderBrush  ListBoxBackgroundBrush �� 
StartPoint �� EndPoint � Color	 � Offset  StandardBrush  PressedBrush  ScrollBarLineButton
 �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Path �� RelativeSource �� Triggers P� Property P� Value �� 
TargetName  ScrollBarPageButton  ScrollBarThumb  VerticalScrollBar � RowDefinitions ]� DecreaseRepeatButton ]� Thumb ]� IncreaseRepeatButton  HorizontalScrollBar � ColumnDefinitions �� Triggers  FavsScrollViewer �� VerticalOffset � Value �� ScrollableHeight � Maximum �� ViewportHeight  �� ViewportSize*! �� #ComputedVerticalScrollBarVisibility" �� Orientation# �� HorizontalOffset$ �� ScrollableWidth% �� ViewportWidth,& �� %ComputedHorizontalScrollBarVisibility��%k  &	        &	      &	 !     &	 1     &	 A     &	 R     &	 b     &	 r     &	 �     &		 �     &	
 �     &	 �     &	 0    &	 �    &	     7  7  7  7  7  &	 �    &	 |    &	 [    7  7  7  7 	 7 
 7  7  &	     7  7  7  7  7  7  7  '�� �    7  7  &	 

    �� $
 #888���� $ Black���� $
 #DDD���� $
 Gray���� $ White���� $
 #888���� $
 #444���� $
 #888���� $
 #888���� $ 	#FF686868���� $
 #888���� $	 0,0&�$ 1,0.001&���� � $ White��$		 0.0Q�� $ White��$		 0.6Q�� $ #DDDDDD��$		 1.2Q��� $	 0,0&�$	 0,1&���� � $
 #FFF��$		 0.0Q�� $
 #CCC��$		 1.0Q��� $	 0,0&�$	 0,1&���� � $
 #BBB��$		 0.0Q�� $
 #EEE��$		 0.1Q�� $
 #EEE��$		 0.9Q�� $
 #FFF��$		 1.0Q��� "
 ��.���  � y�$ Hidden=���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � |� .  ��  � �� �� " ��.R��� $��Border��$��1q�$ 2��8��  8�� $��1q�._�F� $��Center=�$��Center=�8�� ���� $ Content� �� *TemplatedParent+ P�  � �� . . ��� $ Border��8   � ��P�  � {� .  . ��� 8   � ���� "
 ��.���  � y�$ Hidden=���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � �� .  ��  � |� .  ��  � �� �� " ��.R��� 
���   ��� "
 n�.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � �� .  ��  � |� .  ��  � �� �� " n�.R��� $ 4��#��z��#��z��$��0q�$��8��$��8,0,-2,0q��� " ��.R��  �� $��0���� $��0.00001* ��� $��0��.+��� $��3��$ 2��
������ �� $��0��8��  $��18��$��ScrollBar.LineUpCommand��$��M 0 4 L 8 4 L 4 0 Z��]� $��
PART_Track��$��1����.  �� 8�� $��ScrollBar.PageUpCommand�� n� 8�� $��1,0,1,0q�8�� 8��  �� 8�� $��ScrollBar.PageDownCommand���� $��3��8�� $��18��$��ScrollBar.LineDownCommand��$��M 0 0 L 4 4 L 8 0 Z���� " ��.R��  �� $��18���� $��0.00001* ��� $��18��.+��� $��3��$ 2�����#F0F0F0�� $��0��8��  $��18��$��ScrollBar.LineLeftCommand��$��M 4 0 L 4 8 L 0 4 Z��]� $��
PART_Track��$��1����.   �� 8�� $��ScrollBar.PageLeftCommand�� n� 8�� $��0,1,0,1q�8�� 8��  �� 8�� $ ��ScrollBar.PageRightCommand���� $��3��8�� $��18��$ ��ScrollBar.LineRightCommand��$��M 0 0 L 4 4 L 0 8 Z���� "
 ��.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle .  P�  �@��Orientation$ 
Horizontal=�. ���  � ��$
 Auto����  � ��$ 18����  � ��8   P�  �@��Orientation$ Vertical=�. ���  � ��$ 18����  � ��$
 Auto����  � ��8  �� "
 ��.���  �@�OverridesDefaultStyle . ��  � �� �� " ��.R��  �� $
��Auto ���  �� �� $
��Auto �.+��� $��1���� $��PART_VerticalScrollBar��# z # z #  z #y�z! �� $��PART_HorizontalScrollBar��$" 
Horizontal=�$��1��$��1��# z# # z$ #  z% #y�z& .��� ���#33393f �� $��25 ��� $	��330 ��� $	��44* ��� $	��29* �.+��� -   ' ��Name$' HeaderSettingsRectangle�����#373e45�� -   $��TextBlockSettingsHeader��$��16(�
������ ���#f5f5f5$��Header text��$��2��$��0,0,83,0q�$��Center=�( �� Padding$( 7q��� $��475,0,0,0.333q�) J� RenderTransformOrigin$) 0.5,0.5&�$��25��$��25��._��� -   * ��Name$* ButtonSettingsClose��$��resources/close.png��$��25��$��25��$��0,0,0.333,0.333q�$) 0.5,0.5&� ��%o   '��       �� "
 �����  � ��
 ����  P�  �@J�IsMouseOver . . ���  � ��
 �  ��z�V� .~��� + �� ScaleX$+ -1Q��� �� T� �� $
��Left=����#f5f5f5$��14(�$��10,5,0,0q�$��2��$
��Wrap=�$$��Работа с файлами��$	��Top=�$��18��$	��191���� $
��Left=����#f5f5f5$��14(�$��10,5,0,0q�$��3��$
��Wrap=�$��Отладка��$	��Top=�$��23��$	��191���� $
��Left=����#f5f5f5$��14(�$��10,5,0,0q�$��1��$
��Wrap=�$��Каналы��$	��Top=�$��18��$��94���� -   $��ListBoxSources����.  $��0q����#3d444b$
��Left=�$��	0,23,0,55q�$��1��$	��500������ .R�� $��0q� �� $��30 ��� $��* �.+��� $��0��$��Center=�$��Center=�, d� 	IsChecked, �� $ i_is_chosen��� $��1��$��WrapWithOverflow=�$��16(�
������ ���#f5f5f5$��0q����� $ s_name��� -   $��ButtonSaveFullDB��$5��/Создать резервную копию...��$
��Left=�$
��Left=�$��	10,28,0,0q�$��2��$	��Top=�$	��215��$��20������ " ��.R��� $��1q����#373e45���#45d2bd- J� SnapsToDevicePixels- . ._��� $��0q�$��Center=�$��Center=���. - .  P�  � {� .  . ���  � �� �#A8A8A8 ��%4   '��       �� "
 �����  � �� �#f5f5f5�� -   $��ButtonLoadFullDB��$9��3Загрузить резервную копию...��$
��Left=�$
��Left=�$��	10,58,0,0q�$��2��$	��Top=�$	��215��$��20������ " ��.R��� $��1q����#373e45���#45d2bd- . ._��� $��0q�$��Center=�$��Center=���. - .  P�  � {� .  . ���  � �� �#A8A8A8 ��%4   '��       �� "
 �����  � �� �#f5f5f5�� -   $<��6Открыть конструктор запросов��$
��Left=�$��	10,28,0,0q�$��3��$	��Top=�$	��215��$��20������ " ��.R��� $��1q����#373e45���#45d2bd- . ._��� $��0q�$��Center=�$��Center=���. - .  P�  � {� .  . ���  � �� �#A8A8A8 ��%4   '��       �� "
 �����  � �� �#f5f5f5�� -   $��SaveExcelReport��$Q��KСохранить отчёт о загруженных новостях...��$
��Left=�$
��Left=�$��
244,28,0,0q�$��2��$	��Top=�$	��246��$��20������ " ��.R��� $��1q����#373e45���#45d2bd- . ._��� $��0q�$��Center=�$��Center=���. - .  P�  � {� .  . ���  � �� �#A8A8A8 ��%4   '��       �� "
 �����  � �� �#f5f5f5�� -	   $��ButtonAddSource��$��Добавить��$
��Left=�$��415,305,0,0q�$��1��$	��Top=�$��75��$��20������ " ��.R��� $��1q����#373e45���#45d2bd- . ._��� $��0q�$��Center=�$��Center=���. - .  P�  � {� .  . ���  � �� �#A8A8A8 ��%4   '��       �� "
 �����  � �� �#f5f5f5�� -
   $��TextBoxAddSource�����#3d444b���#f5f5f5$
��Left=�$��20��$��
10,305,0,0q�$��1��. �� TextWrapping$
. Wrap=�$	��Top=�$	��400���� $
��Left=����#f5f5f5$��14(�$��
10,280,0,0q�$��1��$
��Wrap=�$H��BДобавление ссылки на новый источник��$	��Top=�$	��309���� 
�������$
��Left=�$��1��$	��Top=�$	��500��$��1���� 
�������$
��Left=�$��1��$	��Top=�$	��500��$��2���� 
�������$
��Left=�$��1��$	��Top=�$	��500��$��3���� ���#45d2bd$��1q�$
��Left=�$	��501��$	��Top=�$	��500��$��4���� -   / ��Name$/ ButtonRemoveAllData��$?��9Удалить все загруженные данные��$
��Left=�$
��Left=�$��
244,28,0,0q�$��3��$	��Top=�$	��246��$��20�� ��%3   '��       �� "
 ��.���  � �� �#f5f5f5���� " ��.R��� $��1q����#373e45���#45d2bd- . ._��� $��0q�$��Center=�$��Center=���. - .  P�  � {� .  . ���  � �� �#A8A8A8