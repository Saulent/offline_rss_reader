   M S B A M L   `   `   `  ���� H  Dsecond_course, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:second_coursesecond_course      second_course.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:second_course       � WindowStyle$
  None=� � 
ResizeMode$ NoResize=� � Title$! Новости офлайн�� � Icon$ resources\rss2.png��$	��450��$	��800�� � 	Resources    StandardBorderBrush �� Color  StandardBackgroundBrush  HoverBorderBrush  SelectedBackgroundBrush  SelectedForegroundBrush  DisabledForegroundBrush  
GlyphBrush  NormalBrush  NormalBorderBrush 	 HorizontalNormalBrush 
 HorizontalNormalBorderBrush  ListBoxBackgroundBrush �� 
StartPoint �� EndPoint � Color	 � Offset  StandardBrush  PressedBrush  ScrollBarLineButton
 �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Path �� RelativeSource �� Triggers P� Property P� Value �� 
TargetName  ScrollBarPageButton  ScrollBarThumb  VerticalScrollBar � RowDefinitions ]� DecreaseRepeatButton ]� Thumb ]� IncreaseRepeatButton  HorizontalScrollBar � ColumnDefinitions �� Triggers  FavsScrollViewer �� VerticalOffset � Value �� ScrollableHeight � Maximum �� ViewportHeight  �� ViewportSize*! �� #ComputedVerticalScrollBarVisibility" �� Orientation# �� HorizontalOffset$ �� ScrollableWidth% �� ViewportWidth,& �� %ComputedHorizontalScrollBarVisibility  ButtonTemplate' �� TextAlignment��%�  &	        &	      &	 !     &	 1     &	 A     &	 R     &	 b     &	 r     &	 �     &		 �     &	
 �     &	 �     &	 0    &	 �    &	     7  7  7  7  7  &	 �    &	 |    &	 [    7  7  7  7 	 7 
 7  7  &	     7  7  7  7  7  7  7  '�� �    7  7  &	 

    &	 E    �� $
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
Horizontal=�$��1��$��1��# z# # z$ #  z% #y�z& �� " ��.R��� ���#45d2bd$��1q����#f5f5f5$��13(�$' Center=����#373e45._��� ��. .���  �� $	��200 ��� $��* � �� $��25 ��� $��* �.+��� -   ( ��Name$( HeaderRectangle�����#373e45$
��Left=�$��25��$	��Top=�$	��800��$��2���� -   $��TextBlockHeader��$��16(�
������ ���#f5f5f5$��Header text��$��2��$��0,0,83,0q�$��Center=�) �� Padding$) 7q��� $��1��$��	575,0,0,0q�._��� -   * ��Name$* ButtonClose��$��25��$��25��$��resources/close.png��$��Bottom=�$��1��+ J� RenderTransformOrigin$+ 0.5,0.5&�$��Right=� ��%o   '��       �� "
 �����  � ��
 ����  P�  �@J�IsMouseOver . . ���  � ��
 �  ���� $��1��$��
550,0,25,0q�._��� -   $* ButtonMinimize��$��25��$��25��$��resources/minimize.png��$��Bottom=�$��1�� ��%o   '��       �� "
 �����  � ��
 ����  P�  �@J�IsMouseOver . . ���  � ��
 �iii��� -   $��ButtonSettings��$��Настройки��$��0,401.667,0,0.667q�$��1������ " ��.R��� $��1q����#373e45���#45d2bd, J� SnapsToDevicePixels, . ._��� $��0q�$��Center=�$��Center=���. , .  P�  � {� .  . ���  � �� �#A8A8A8 ��%4   '��       �� "
 �����  � �� �#f5f5f5�� ���#33393f$��1��$
��Left=�$	��401��$	��Top=�$	��600��$��1��$��0,1,0,0q�.��� -   $��GridScrollView��$	��570��$
��auto�� �� $
��106* ��� $
��295* � �� �� .+��� -   $��TextBlockNewText��$��0��$��2��$
��Wrap=����#f5f5f5$��20(�$��	Helvetica)�$\��VВыберите новость из списка слева для просмотра��$��2���� -   $��ListBoxNewHeaders�����#33393f$��Stretch=�$��Disabled=�$��0q�$��0q�$
��Left=�$	��354��$��1��$	��Top=�$	��200����.  $��0,47.667,0,0q����� .R�� $��GridHeadersListBox��$��0,0,0,0q� �� $��12 ��� $��48 ��� $��12 ��� $��14 � �� $��* ��� $��50 �.+��� $��NoWrap=�$��0��$��0��$) 0q�$��10(�
������ ���#C7C7C7$	��Top=�$��0q����� $ 
SourceName��� $��NoWrap=�$��0��$��1��$) 0q�$��10(�
������ ���#C7C7C7$��0q����� $ s_date��� $��WrapWithOverflow=�$��1��$��0��$) 0q�$��2��$��12(�$
��Bold"�
������ ���#f5f5f5$��0q����� $ s_header��� $��NoWrap=�$��2��$��0��$��2��$) 0q�$��11(�
������ ���#C7C7C7$��0q����� $ s_description� - �� Binding. �� UpdateSourceTrigger/ �� Value��%�   '�       �� "
 � �� - �� $ 	i_is_read�$. PropertyChanged=�/ True.a���  � ��
 ���� �� - �� $ 	i_is_read�$. PropertyChanged=�	/ false.a���  � �� �#5E5E5E�� -	   $��ButtonUpdate��$��Обновить��$
��Left=�$��1��$	��200��$��0,24.667,0,375.667q����� " ��.R��� $��1q����#373e45���#45d2bd, . ._��� $��0q�$��Center=�$��Center=���. , .  P�  � {� .  . ���  � �� �#A8A8A8 ��%4   '��       �� "
 �����  � �� �#f5f5f5�� -
   $��TextBoxSearch��$��14(�$��Center=�$��0q�$.��(Введите искомые слова�����#A1A1A1���#3d444b$��0q�$
��Left=�$��25��$��1��0 �� TextWrapping$
0 Wrap=�$	��Top=�$	��199��$��1,0,0,0q�� -   $��ProgressBarParsingNews��
���iii�$��1��$
��Left=�$��23��$��	0,402,0,0q�$��1��$	��Top=�$	��600���� 
�������$
��Left=�$	��402��$��0,25,0,0q�$	��Top=�$��1��$��1��$��2���� 
�������$
��Left=�$��1��$	��Top=�$	��200��$��1���� 
�������$
��Left=�$��24��$	��Top=�$��1��$��1��$��0,1,0,0q��� 
�������$
��Left=�$��1��$	��Top=�$	��600��$��1��$��1���� 
������$��1q�$
��Left=�$	��450��$	��Top=�$	��800��$��2��$��2��