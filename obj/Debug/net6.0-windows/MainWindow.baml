   M S B A M L   `   `   `  ����  
MiniWriter    MiniWriter.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  
MiniWriter��5   	   $��40��5   	   $	��350��5   	    � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��5   	   
������  � Topmost$
 True��5
   	    � 
ResizeMode$ NoResize=�5   	    � ShowInTaskbar$ False��5   	   .��5      �� -   5         ��Name$ 
dragBorder��5      
������ ._�5   
   �� -   5        $ 
mainBorder��5      ���#CCCCCC$	��0.5q�5       J� Opacity$	 0.3Q�5      ._�5      � .+�5      �� -   5         ��Name$ backgroundRect��5      ���#F5F5F5$	 0.3Q�6C   �� 5      -   5        	 ��Name$	 inputTextBox��5      
������ ���#CCCCCC$��0q�5      $��Stretch=�5      $��40��5       $��14(�5!      $��5,0,25,0q�5"      $��0,2,0,2q�5#      
 �� AcceptsReturn$

 True��5$      " �� VerticalScrollBarVisibility$ Hidden=�5&      $
��True��5'       � 	Resources 5)       �� 
TargetType �� Property �� Value��%�   'J�       �� 5*      " J�6    .�5+      ��  �@��Margin$ 0q�67   �� 5,       �@��Padding$ 0q�68   �� 5-       �@��
LineHeight$ 18��6;   �� 5.       �@��LineStackingStrategy$ BlockLineHeight=�6E   5/      50       �51      /� 52       /� PagePadding$ 0q�6'    /� 
LineHeight$ 18��53   &    /� LineStackingStrategy$ BlockLineHeight=�54   &   .T�55      J�  �� Margin$ 0q�6(    �� TextAlignment$
 Left=�56   '    �� LineStackingStrategy$ BlockLineHeight=�57   '    �� 
LineHeight$ 18��58   '   59      5;      �� 5=       �� Orientation$ 
Horizontal=�6   $��Right=�5>      $	��Top=�5?      $��0,3,3,0q�5@      .+�5A      �� -   5        $��S��5A      $��20��5B      $��20��5C      $��12(�5D      $��2,0,0,0q�5E      
������ ���#CCCCCC���#CCCCCC   ButtonStyle#��[  5J      �� 5K      -   5        $��E��5K      $��20��5L      $��20��5M      $��12(�5N      $��2,0,0,0q�5O      
������ ���#CCCCCC���#CCCCCC#��[  5T      �� 5U      $ 
Horizontal=�6!   $��2,0,0,0q�5V      .+�5W      �� -   5        $ themeToggle��5W   !   $��8��5X   !   $��8��5Y   !   $��0,6,2,0q�5Z   !   
������ ���#CCCCCC$��1q�5]   !    �� CornerRadius$ 4��5^   !   ��5`      �� 5a   "   " ��6(    �� Triggers 5b   &   P� 5c   *    P� Property �@J�IsMouseOver P� Value$
 True��6I   . �5d   .   ��  � �� �	#1ACCCCCC6K   5e   +   5g   #   5i      �� 5j      -	   5        $ 
dragHandle��5j   !   $��12��5k   !   $��12��5l   !   $��0,4,0,0q�5m   !   
������ ���#CCCCCC$��1q�5p   !   $ 2��5q   !    � Cursor$ SizeAll��5s   !   ��5t      �� 5u   "   " ��6(    5v   &   P� 5w   *    �@J�IsMouseOver$
 True��6I   . �5x   .   ��  � �� �	#1ACCCCCC6K   5y   +   5{   #   5}      5~      5      5�      5�      5�      5�      