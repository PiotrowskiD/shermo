   M S B A M L   `   `   `  ����J  FNxServerMonitor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:NxServerMonitorNxServerMonitor  5   	       NxServerMonitor.ULS   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:NxServerMonitor     5   	     N� Title$	  ULS��5   	   $
��auto��6   $
��auto��6#    N� 
Background �	#FFFEFEFE.��5	      � -   5        $��MyGrid��5	      .+�5
   
   �� $��36(�6   $��Segoe UI)�6"    �� TextAlignment$
 Left=�68   $	��ULS��6M   
������ $��30,0,0,0q�6q   ,� 5   
   $��0,75,0,0q�6#   $��Segoe UI Light)�65   " ,� VerticalScrollBarVisibility$ Visible=�6Q   .��5      /� .T�5      �� -   5         ��Name$ ULSTable��5       �� Columns 5      �� 5       �� Width$	 120 �6&   �� 5      $	 100 �6&   �� 5      $	 100 �6&   �� 5      $	 100 �6&   �� 5      $	 100 �6&   �� 5      $	 100 �6&   �� 5      .�5      �� .�5      �� .�5      �� .�5   "   J�  �� TextAlignment$ Center=�6,   .*�6C   .*�5   &   �� $
��Bold"�6*   $��18(�6<   $��Segoe UI Light)�6J   $
�Time��6f   5   #   5      �� 5      .�5   "   J� $ Center=�6,   .*�6C   .*�5    &   �� $
��Bold"�6*   $��18(�6<   $��Segoe UI Light)�6J   $�Process��6f   5!   #   5"      �� 5#      .�5$   "   J� $ Center=�6,   .*�6C   .*�5%   &   �� $
��Bold"�6*   $��18(�6<   $��Segoe UI Light)�6J   $�Thread��6f   5&   #   5'      �� 5(      .�5)   "   J� $ Center=�6,   .*�6C   .*�5*   &   �� $
��Bold"�6*   $��18(�6<   $��Segoe UI Light)�6J   $�Category��6f   5+   #   5,      �� 5-      .�5.   "   J� $ Center=�6,   .*�6C   .*�5/   &   �� $
��Bold"�6*   $��18(�6<   $��Segoe UI Light)�6J   $�Event ID��6f   50   #   51      �� 52      .�53   "   J� $ Center=�6,   .*�6C   .*�54   &   �� $
��Bold"�6*   $��18(�6<   $��Segoe UI Light)�6J   $�Level��6f   55   #   56      �� 57      .�58   "   J� $ Center=�6,   .*�6C   .*�59   &   �� $
��Bold"�6*   $��18(�6<   $��Segoe UI Light)�6J   $�Message��6f   5:   #   5;      5<      5=      5>      5?      5@      5A      5B      