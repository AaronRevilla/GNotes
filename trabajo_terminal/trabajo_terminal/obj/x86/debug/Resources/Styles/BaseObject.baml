   M S B A M L   `   `   `  ����G  CGraphicNotes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:GraphicNotes.Views.ObjectsGraphicNotes.Views.Objects  5      I)clr-namespace:GraphicNotes.Views.AdornersGraphicNotes.Views.Adorners  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source   MoveThumbTemplate �� 
TargetType+    %GraphicNotes.Views.Adorners.MoveThumb+   %GraphicNotes.Views.Objects.BaseObject �� 
TargetType  BaseObjectStyle �� Property �� Value � DataContext �� RelativeSource �� Path	   Name
 � Cursor � RowDefinitions*  $GraphicNotes.Views.Objects.ObjectBar  Name J� Opacity  ObjectBarStyle ��Name1  +GraphicNotes.Views.Adorners.ResizeDecorator  Name �� Triggers P� Property P� Value �� 
TargetName�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .v(clr-namespace:GraphicNotes.Views.Objects  5      /a)clr-namespace:GraphicNotes.Views.Adorners  5        5      �� 5   
   $ ResizeChrome.xaml?�6   �� 5   
   $ LockedChrome.xaml?�6   �� 5	   
   $ ObjectBar.xaml?�6   %,  &	        &	 F     7   7  �� 5      "   5      .R�5   
   �� 
������ 6   5      �� 5      "  6   .�5   
   ��  � ��$ 50��5      �� 5   
    � ��$ 50��5      �� 5   
    �@J�SnapsToDevicePixels . 5      �� 5   
    � �� 5      �� 5      "  6"   .R�5      �  6   ��  �� *TemplatedParent+$ .�.+�5         $	 PART_MoveThumb��6&   $
 SizeAll��5   &   8��  5   &   � 5       5       �� 5!   "   $
��Auto �60   6@   �� 5"   "   $��* �60   6=   .+�5$        $ PART_ObjectBar��67   $ 0Q�6*   $��0��6O   8�� 6�   �� 5%      $ PART_ContentPresenter��6<   $��1��6/   #��z��5&   +   #��z��5'   +   5(        5)      $ PART_BaseObjectDecorator��6,   5*       5+      P� 5,       �@ State$ Unlocked=�5-   "   . �5.      �� $ PART_BaseObjectDecorator��6%    �@ ShowDecorator . 50   %   �� 51      $ PART_ObjectBar��6%    �@J�Opacity$ 1Q�53   %   54      56      58      59      5;      