   M S B A M L   `   `   `  ����J  FNxServerMonitor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:NxServerMonitorNxServerMonitor  5   	   !    NxServerMonitor.ServersList   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:NxServerMonitor     5   	     N� Title$  ServersList��5   	   $
��auto��6   $
��auto��6+    N� 
Background �	#FFFEFEFE.��5	      � -   5        $��mainGrid��5	       J� 	AllowDrop . .+�5
   
    � -   5          �Name$ 	mainFrame��5
      �� 5   
   -   5         ��Name$ 	groupName��5      $��24(�6'   $��nazwa��65   $
��Left=�6C   $	��Top=�6^   $��20q�6v   �� 5   
   -   5         ��Name$ serversList��5      $��Disabled=�5      $��Visible=�5      $��	0,100,0,0q�5   <   ��5      �� *list+��5      Z� ��6&   Z� ��5      �� 5      .R�5      � 5      ��5      �� 5       �� DataType$ local:ServerViewModel��6   .R�5      � $	��332��6   $	��150��6'   ���	#FFECECEC � ColumnDefinitions 5      �� 5      $
��0.3* �6/   �� 5      $
��0.7* �6/    � RowDefinitions 5      .+�5      �� 	 ��Name$
	 icon��6    $��0��6F   $
��None=�6V   ��6.   �� *Icon+6V   �� 5       $��1��6@   $��Center=�6P   $��Center=�6m   $��24(�6�   
���   ���6$   �� *
DomainName+6�   5*      5+      ��5-      �� 5.      
 �� 
TargetType"
 ��6   .�5/      ��  �� Property � �� �� Value$ 0q�60   �� 50       � ��$ 6q�6/   51      53      57      58      