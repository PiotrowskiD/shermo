   M S B A M L   `   `   `  ����J  FNxServerMonitor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:NxServerMonitorNxServerMonitor  5          NxServerMonitor.Clients   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:NxServerMonitor     5        N� Title$  Clients��5      $
��auto��6   $
��auto��6%    N� 
Background �	#FFFEFEFE.��5	      � .+�5
   
    � -   5        $��	mainFrame��5
      � 5   
    � ColumnDefinitions 5      �� 5      $��0.15* �6#   �� 5      $��0.85* �6#    � RowDefinitions 5      �� 5      $
��0.1* �6    �� 5      $
��0.9* �6    .+�5       � -   5        $��serversFrame��5      $��1��6(   $��1��68   �� 5      $��0��6   $��5��6(   ��5      �� 5       �� EndPoint$
 1, 1&�6*    �� 
StartPoint$	 0,0&�6:   .��5      �  � Color$ #d6d6d6��6'    � Offset$ 1Q�67   � 5      $ #4B4B4B��6'   $ 0Q�67   5      5!      �� 5#      $��0��6   $��4��6%   ��5$      �� 5%      $
 1, 1&�6*   $	 0,0&�6:   .��5&      � $ #4B4B4B��6'   $ 1Q�67   5'      5)      �� 5+      $'��!assets/sharemo-logo-header@2x.png��6   $��0��6?   $��0��6O   $��2��6\   $��50,0,0,0q�6p   $
��Left=�6�   $
��None=�6�   �� 5-      -   5         ��Name$ clientsList��5-      $��Disabled=�5.      $��Hidden=�5/      $��1��50   ,   $��0��69   ��6   �� *list+��51   1   Z� ��6H   Z� ��54      �� 55      	 �� DataType$	 local:ClientViewModel��6#   .R�56      �  57      �� 58   "   $
��0.2* �63   �� 59   "   $
��0.8* �63   .+�5;      �� 
 ��Name$
 chosen��6$   $��assets/group-empty.png��64   $��Center=�6T   $��0��6q   $
��None=�6�   �� 5<      $��22(�6C   $��Center=�6Q   $
��Left=�6l   $��1��6�   $��Segoe UI Light)�6�   ��6(   �� *	GroupName+6�   5>      5?      ��5A      �� 5B       �� 
TargetType" ��6   .�5C      ��  �� Property � �� �� Value$ 0q�64   �� 5D       � ��$ 0q�63   �� 5E       � ��$ Stretch=�6G   5F      5H      �� 5I   
   -   5        $��manageGroups��5I      $��0��6%   $��1��65   $��Center=�5J      $��Bottom=�6.   $��20,20, 20, 20q�6I   ��6{   Z� ��6�   Z� .��5K      �� .+�5L      �� $'��!assets/button-settings-indash.png��6   $
��None=�6C   5M      5N      5O      5P      5Q      