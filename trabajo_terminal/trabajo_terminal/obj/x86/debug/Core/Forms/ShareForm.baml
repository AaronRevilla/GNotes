   M S B A M L   `   `   `  ����  GraphicNotes'    !GraphicNotes.Core.Forms.ShareForm   5      a ]DevExpress.Xpf.Core.v12.2, Version=12.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a?dxe5http://schemas.devexpress.com/winfx/2008/xaml/editors  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      ;dx2http://schemas.devexpress.com/winfx/2008/xaml/core  5        � Title$  Insert Table��5      $
��Auto��6   $	��300��6)    � ShowInTaskbar .   � Topmost .  � WindowStyle$ 
ToolWindow=�6Z    � WindowStartupLocation$ CenterScreen=�5       � 
ResizeMode$ NoResize=�6*    � SizeToContent$ Height=�6@   .��5	      � .+�5
   
   ]� .+�5      �� $��Bottom=�6   ._�5      �  � ColumnDefinitions 5      �� 5      $��* �6+   �� 5      $��* �6+   .+�5      �� -   5         ��Name$ acceptButton��5      $��0��63   $��Accept��6C   $��20(�6T   $
��Bold"�6b   $��5,5,5,5q�6t   $	��100��5      $��Right=�6*   6F   �� 5      -   5        $ cancelButton��5      $��1��63   $��Cancel��6C   $��20(�6T   $
��Bold"�6b   $��5,5,5,5q�6t   $	��100��5      $
��Left=�6*   6E   5      5      �� 5      ._�5      �  5      �� 5      $
��Auto �6+   �� 5      $��* �6+   �� 5      $
��Auto �6+   �� 5      $��* �6+   	 � RowDefinitions	 5       �� 5!      $
��Auto �6(   �� 5"      $
��Auto �6(   .+�5$      �� $��0��6   $��0��6,   $��	DataPort:��69   $
��Bold"�6M   $��0q�6_   $��
10,10,0,10q�6k   $��Right=�6   %  DevExpress.Xpf.Editors.SpinEdit  5%      -   5        
  Name$
 dataPortField��5%   '   $��1��6>   $��0��6N   $��10q�6[      MinValue$
 1025X�6g      MaxValue$ 65536X�6w      Value$
 9092~�6�      IsFloatValue .  6�   �� 5'      $��2��6   $��0��6,   $��FilePort��69   $
��Bold"�6L   $��0q�6^   $��
10,10,0,10q�6j   $��Right=�6~     5(      -   5        $
 filePortField��5(   #   $��3��6:   $��0��6J   $��10q�6W   $
 1025X�6c   $ 65536X�6s   $
 9093~�6�    .  6�   �� 5*      $��1��6   $��0��6)   $��	Password:��69   $
��Bold"�6M   $��0q�6_   $��
10,10,0,10q�6k   $��Right=�6   ,  &DevExpress.Xpf.Editors.PasswordBoxEdit  5+      -   5          Name$ passwordField��5+   *   $��1��6A   $��1��6N   $��3��6^   $��10q�6r   5,      5-      5.      5/      50      