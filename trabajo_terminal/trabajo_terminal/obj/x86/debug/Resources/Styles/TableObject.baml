   M S B A M L   `   `   `  ����G  CGraphicNotes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:GraphicNotes.Views.ObjectsGraphicNotes.Views.Objects  5      I)clr-namespace:GraphicNotes.Views.AdornersGraphicNotes.Views.Adorners  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089a ]DevExpress.Xpf.Core.v12.2, Version=12.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a+   %GraphicNotes.Views.Objects.CellBorder  �� 
TargetType   CellBorder_Vertical �� Property �� Value �� Path �� RelativeSource  CellBorder_Horizontal  GridSplitter_Column �� AncestorType  GridSplitter_Row*  $GraphicNotes.Views.Objects.TableCell  TableObject_CellStyle%  DevExpress.Xpf.Editors.BaseEdit �� 
TargetType J� SnapsToDevicePixels �� CornerRadius)  #DevExpress.Xpf.Editors.TextEditBase	 �� Triggers
 P� Property P� Value,  &GraphicNotes.Views.Objects.TableObject  BaseObjectStyle �� BasedOn � DataContext+   %GraphicNotes.Views.Adorners.MoveThumb  Name � Cursor  MoveThumbTemplate � RowDefinitions*  $GraphicNotes.Views.Objects.ObjectBar  Name J� Opacity  ObjectBarStyle �Name J� IsHitTestVisible1  +GraphicNotes.Views.Adorners.ResizeDecorator  Name �� Triggers+   %GraphicNotes.Views.Objects.BaseObject �� 
TargetName�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .v(clr-namespace:GraphicNotes.Views.Objects  5      /a)clr-namespace:GraphicNotes.Views.Adorners  5      ?dxe5http://schemas.devexpress.com/winfx/2008/xaml/editors  5      %f  &	        &	 �    &	     &	 �    &	 E    '  �    7  7  7  �� 5      "    6   .�5	   
   ��  � ��
 �   �6!   �� 5
   
    �@J�IsHitTestVisible .  6-   �� 5   
    � ��$ Right=�60   �� 5   
    � ��$ Stretch=�6.   �� 5   
    � �� 6"   �� $
 Size� �� *Self+�� 5   
    �@  Size$ 1Q�6!   �� 5   
    � y�$ Visible=�6'   �� 5   
    � ��$ 3��6)   5      �� 5      "    6   .�5   
   ��  � ��
 �   �6!   �� 5   
    �@J�IsHitTestVisible .  6-   �� 5   
    � ��$ Stretch=�60   �� 5   
    � ��$	 Top=�6.   �� 5   
    � �� 6#   �� $
 Size� �� *Self+�� 5   
    �@  Size$ 1Q�6!   �� 5   
    � y�$ Visible=�6'   �� 5   
    � ��$ 3��6,   5      �� 5      "  ��6   .�5    
   ��  � ��$ 0q�6#   �� 5!   
    � ��$ 0q�6$   �� 5"   
    � ��$ Right=�60   �� 5#   
    � ��$ Stretch=�6.   �� 5$   
    �@��ResizeBehavior$ PreviousAndNext=�6+   �� 5%   
    � ��$ 1��6"   �� 5&   
    � ��
 ���� 6'   �� 5'   
    � �� 6)   �� $ RowDefinitions.Count� �� *FindAncestor+" �5)      �� 5+      "  ��6   .�5,   
   ��  � ��$ 0q�6#   �� 5-   
    � ��$ 0q�6$   �� 5.   
    � ��$ Stretch=�60   �� 5/   
    � ��$	 Top=�6.   �� 50   
    �@��ResizeBehavior$ PreviousAndNext=�6+   �� 51   
    � ��$ 1��6#   �� 52   
    � ��
 ���� 6'   �� 53   
    � �� 6,   �� $ ColumnDefinitions.Count� �� *FindAncestor+" �55      �� 57      "   6   .�59   
   ��  �@ BorderTemplate 5:      �� 5;      " ��6"   .R�5=      �� $��0q�6   
������  . $ 0��5>      ._�5?      �� 5@      5A      5C      �� 5D   
    �@ TextWrapping$
 Wrap=�6)   	 5E   
   P� 5F      
 �@ 
IsSelected . . �5G      ��  �@ BorderTemplate 5H      �� 5I      " ��6*   .R�5J      �� $��0q�6%   
������ 
����ح�$ 0��5K      ._�5L   "   �� 5M      5N      5P      5Q      P� 5R      
 �@J�IsMouseOver . . �5S      ��  �@ 
IsSelected . 6/   �� 5T       �@�Cursor$ IBeam��6+   5U      5W      �� 5Z      "   6   8   .�5[   
   ��  � �� 5\      �� 5]      "  6"   .R�5^      �  6   ��  �� *TemplatedParent+$ .�.+�5_        $ PART_MoveThumb��6&   $ SizeAll��5`   &   8�� 5a   &   � 5b       5c      �� 5d   "   $
��Auto �60   6@   �� 5e   "   $��* �60   6=   .+�5g        $ PART_ObjectBar��67   $ 0Q�6*   $��0��6O   8�� 6�   � 5i      $��1��6#   .+�5j   "   � $ 	PART_Grid��6'    . 6T   5k      5l        5m      $ PART_BaseObjectDecorator��6,   5n       5o      P� 5p      
 �@ State$ Unlocked=�5q   "   . �5r      �� $ PART_BaseObjectDecorator��6%    �@ ShowDecorator . 5t   %   �� 5u      $ PART_ObjectBar��6%    �@J�Opacity$ 1Q�5w   %   5x      5z      5|      5}      5~      