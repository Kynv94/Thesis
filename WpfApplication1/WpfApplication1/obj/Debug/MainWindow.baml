   M S B A M L   `   `   `  ����J  FWpfApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WpfApplication1WpfApplication1  5   	   U QLiveCharts.Wpf, Version=0.8.0.0, Culture=neutral, PublicKeyToken=3b585c2a5f1a92c1G4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   #clr-namespace:Wpf.UtilWpf.Util  5	   	        WpfApplication1.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:WpfApplication1     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   'utilclr-namespace:Wpf.Util     5	   	     � Title$  	NET TOUGH��5   	    � WindowState$ 	Maximized=�5   	    � 	Resources 5       �� 
TargetType �� Property �� Value&   WpfApplication1.Convert_Protocol   GetServiceNameMethod��%t   '��       &	  M     �� 5   
   " ��6   .�5      ��  � ��$ Stretch=�6;   5        5   
   6!   5      .��5      �  � RowDefinitions 5   
   �� 5      .+�5   
   �� -   5         ��Name$ tabControl_level1��5      $��5q�60   
����������	#FF88B04B.@�5      �� -   5         ��Name$ 	Tab_Event��5      $��EVENT��6)   $��0q�68   $��75��6C   $��20��6N   .��5      � 
������� 5      �� 5      $	��100 �6(   �� 5       $
��259* �6(   �� 5!      $	��83* �6(   �� 5"      $��20 �6(   .+�5$      �� -   5        	 ��Name$	 	lv_packet��5$      $
 �� IsSynchronizedWithCurrentItem$

 TrueY�64   $��Stretch=�5%      $��Stretch=�6>   ��.  �� SelectionMode$ Single=�5&   >   $��1��5'   G      Wpf.Util.GridViewSort   AutoSort .  5(      ��%T   '��       �� 5)      " ��6$   .�5*   "   ��  � ��$
 Left=�6O   5+      5,       �� View 5-      �� 5/      .P�51   "   �� $��ID��61    �� Width$ 50��52   O      PropertyName$ Det_ID��53   1    �� DisplayMemberBinding 52   %   �� *Det_ID+53   1   �� 55   "   $
��Date��61   $	 180��56   X   $ Session.Started��57   1    56   %   �� *Session.Started+57   1   �� 59   "   $��Last Update��61   $	 180��5:   V   $ Session.Ended��5;   1    5:   %   �� *Session.Ended+5;   1   �� 5=   "   $��Protocol��61   $	 130��5>   �   $ Protocol��5?   1    5>   %   �� *
PluginID+ �� 	Converter# [  5?   1   �� 5A   "   $��Party A��61   $	 150��5B   V   $ Session.IP_in��5C   1    5B   %   �� *Session.IP_in+5C   1   �� 5E   "   $��Port A��61   $ 70��5F   X   $ Session.Port_in��5G   1    5F   %   �� *Session.Port_in+5G   1   �� 5I   "   $��Party B��61   $	 150��5J   W   $ Session.IP_out��5K   1    5J   %   �� *Session.IP_out+5K   1   �� 5M   "   $��Port B��61   $ 70��5N   Y   $ Session.Port_out��5O   1    5N   %   �� *Session.Port_out+5O   1   �� 5Q   "   $
��Info��61   $	 300��5R   P    6%   �� *	KeyData+6P   5T      .@�5W      �� $	��100��6!   $	��100��6.   5Y      �� 5Z      -   5         ��Name$ gridSplitter6��5Z   #   $��0��6:   
�������$��Stretch=�6a   $��2��6   $��5��6�   $	��Top=�6�   �� 5[      -   5        $ tab_Info��5[   !   $��0,5,0,0q�63   $��2��6D   .@�5\      �� -   5        $ tab_discription��5\   "   $��Discription��6;   $��
-2,-1,1,-1q�6P   .��5]      �� -   5         ��Name$ sv_discription��5]   +   5^      5_      �� 5`      -   5        $ tabControl_Event��5`   !   $��0q�6;   .@�5b      �� $
��View��6"   $��	3,-2,-7,0q�60   .��5c      � -	   5         �Name$ grid_filter��5c   #   ���	#FFE5E5E5$��0q�6O    � ColumnDefinitions 5d   "   �� 5e   &   $	��101 �67   �� 5f   &   $	��101 �67   �� 5g   &   $	��101 �67   �� 5h   &   $	��197 �67   �� 5i   &   $	��235 �67   .+�5l   "   d� -
   5         d�Name$ btn_filter_ipsrc��5l   /   $��	IP Source��6J   $��0q�6^   
���   � J� OpacityMask# � ��6�   ���	#FFE5E5E5��6i   Z� 6�   � 5m   "   -   5         �Name$ popup_filter_ipsrc��5m   (    � 	StaysOpen .  ��6D   �� *	IsChecked+ �� ElementName$ btn_filter_ipsrc�� � PlacementTarget 5n   %   �� $ btn_filter_ipsrc��.��5o   &   � 
�������$	��239��6>   $	��Top=�6K   $	��200��6c   .+�5p   *   �� 
�������$��1q�6D   
���   �6X   �� 5q   *   -   5         ��Name$ 
btn_add_ip��5q   1   $��+��6E   $��135,11,40,203q�6Q   $��25��6h   $��25��6s    �� 	IsDefault . 6�   �� 5r   *   -   5        $ 
btn_del_ip��5r   1   $��x��6E   $��165,11,10,203q�6Q   $��25��6h   $��25��6s   {�.  6�   �� 5s   *   -   5         ��Name$ lb_ipadd��5s   0   $��IP Address:��6B   $��10,10,106,45q�6X   �� 5t   *   -   5         ��Name$ cb_ipadd_input��5t   3   $
��Left=�6K   $��	10,45,0,0q�6f   $	��Top=�6y   $	��180��6�   �� 5u   *   -   5        $ update_ip_list��5u   1   $��Update��6I   $
��Left=�6Z   $��	61,82,0,0q�6u   $	��Top=�6�   $��75��6�   6�   �� 5v   *   -   5        $	 lv_ip��5v   3   $
��Left=�6B   $	��126��6]   $��	0,113,0,0q�6j   $	��Top=�6}   $	��200��6�    5w   .   �� 5x   2   .P�5y   6   �� $��List IP��6E   $	 190��6V   5z   3   5|   +   5�   '   5�   #   d� 5�   "   -   5        $ btn_filter_time��5�   /   $
��Time��6I   $��0q�6X   ���	#FFE5E5E5# � ��6�   $��1��6�   ��6z   Z� 6�   � 5�   "   -   5        $ popup_filter_time��5�   (    .  ��6C   �� *	IsChecked+$ btn_filter_time�� 5�   %   �� $ btn_filter_time��.��5�   &   � 
�������$��80��6>   $	��Top=�6J   $	��200��6b   .+�5�   *   �� 
�������$��1q�6D   
���   �6X   �� 5�   *   $��From: ��60   $��
0,10,152,0q�6A   $��28��6U   $	��Top=�6a   �� 5�   *   $
��To: ��60   $��0,45,152,10q�6?   (  "System.Windows.Controls.DatePicker  5�   *   -   5        $��dp_from��5�   5   
  �� Now!   SelectedDate#! Z  6D   $
��Left=�6o   $��	53,10,0,0q�6�   $	��Top=�6�   $	��137��6�     5�   *   -   5        $��dp_to��5�   5   #! Z  6B   $
��Left=�6m   $��	53,47,0,0q�6�   $	��Top=�6�   $	��137��6�   5�   '   5�   #   d� 5�   "   -   5        $ btn_filter_protocol��5�   /   $��Protocol��6M   ���	#FFE5E5E5$��2��6�   ��6w   Z� 6�   � 5�   "   -   5        $ popup_filter_protocol��5�   (    .  ��6G   �� *	IsChecked+$ btn_filter_protocol�� 5�   %   �� $ btn_filter_protocol��.��5�   &   �� 
�������$��1q�6@   
���   �$��2��6h   $��2��6x   ._�5�   *   � -   5        " �Name$" wrap_filter_protocol��5�   4   $��65��6R   $	��170��6^   
�������$��2��6}   $��2��6�   .+�5�   .   �� -   5        # ��Name$# cb_web��5�   7   $	��Web��6G   $
��Left=�6U   $��	10,10,0,0q�6p   $	��Top=�6�   $ d� 	IsChecked$
$ TrueY�6�   �� 5�   .   -   5        $# cb_ftp��5�   7   $	��FTP��6G   $
��Left=�6U   $��	50,10,0,0q�6p   $	��Top=�6�   $
$ TrueY�6�   �� 5�   .   -   5        $# cb_dns��5�   7   $	��DNS��6G   $
��Left=�6U   $��	10,10,0,0q�6p   $	��Top=�6�   $
$ TrueY�6�   �� 5�   .   -   5        $# cb_mail��5�   7   $
��Mail��6H   $
��Left=�6W   $��	50,10,0,0q�6r   $	��Top=�6�   $
$ TrueY�6�   5�   +   5�   '   5�   #   �� 5�   "   -   5        $ btn_filter_apply��5�   )   $��Apply��6C   $��3��6S   $��Center=�6�   $	��2,0q�6�   $��Right=�6�   $	��101��6�   ��6c   Z� ��6y   Z� 6�   �� 5�   "   -   5        $ btn_filter_delete��5�   )   $��Delete��6D   $��4��6U   $��Center=�6�   $	��101��6�   $
��Left=�6�   ��6e   Z� ��6{   Z� 6�   5�      5�      �� 5�      -   5        $ tab_control��5�   "   $��Control��67   $��
8,-2,-12,0q�6H   .��5�      � ���	#FFE5E5E5 5�   "   �� 5�   &   $	��405 �67   �� 5�   &   $	��333 �67   .+�5�   "   �� -    5        $ 
btn_import��5�   )   $��Import��6=   $��30,5,0,0q�6N   $��1��6`   $��60��6p   $��60��6{   $
��Left=�6�   $	��Top=�6�   6�   �� 5�   "   -!   5        $ 	cb_device��5�   +   % �� Text$% --Select device--��6>   $��	60,24,0,0q�6�   $	��Top=�6�   $��26��6�   $
��Left=�6�   $	��174��6�   �� 5�   "   -"   5        $ btn_stop��5�   )   $
��Stop��6;   {�.  $��
330,11,0,0q�6\   $��50��6p   $��50��6�   $��Center=�6�   $��Center=�6�   & J� RenderTransformOrigin$& 1.033,0.775&�6�   $
��Left=�6  $	��Top=�6  ��5�   &   �� 5�   *   5�   #   �� 5�   "   -#   5        $ 	btn_start��5�   )   $��Start��6<   {�.  $��
256,11,0,0q�6^   $��50��6r   $
��Left=�6�   $	��Top=�6�   $& 0.5,0.5&�6�   $��50��6�   $��Center=�6�   $��Center=�6  ��5�   &   �� 5�   *   5�   #   �� 5�   "   -$   5        $ label_device��5�   (   $��Device��6>   $��	10,24,0,0q�6O   $��26��6b   $
��Left=�6n   $	��Top=�6�   $��45��6�   5�      5�      5�      �� 5�      -%   5        $��sb_total��5�       $��3��60   ���	#FF88B04B.@�5�      �� -&   5        ' ��Name$' tb_total��5�   $   $
��Wrap=�66   $��25��6J   $	��183��6V   $��27,0,0,0q�6b   ( � UseLayoutRounding( .  $
��Bold"�6�   ��6�   Z� 6�   5�      5�      5�      �� 5�      -'   5        $ 	Tab_Chart��5�      $��0q�6)   $��CHART��64   $��75��6C   $��20��6N   .��5�      � 
������� 5�      �� 5�      .+�5�      � $��	0,119,0,0q�6    5�      �� 5�      5�      #  LiveCharts.Wpf.CartesianChart  5�      -(   5        )  Name$) 
line_chart��5�   )   &   LiveCharts.Wpf.Charts.Base.Chart*   LegendLocation$* Right=�6a   $��0q�6x   +   Series+ 6=   �� *SeriesCollection+,   AxisY, 5�        LiveCharts.Wpf.Axis  -   FontSize$- 15Q�6'   .   Title$. Packets��65   /   LabelFormatter/ 6E   �� *
YFormatter+6m   0   AxisX0 5�        $- 15Q�6'   $
. Date��65   1   Labels1 6B   �� *Labels+6^   5�      5�      5�      �� 5�      $��ALERT��6   $
��Left=�6%   $��20��6@   $	��Top=�6L   $��54��6d   $��-1,0,0,0q�6o   .��5�      � 
������� 5�      �� 5�      $��80 �6(   �� 5�      $	��67* �6(   �� 5�      $	��55* �6(   �� 5�      $��20 �6(   .+�5�      � -)   5        $ grid_alert_control��5�      
�������.+�5�      �� -*   5        $ btn_add_alert��5�   !   $
��Left=�68   $	��Top=�6S   $��80��6k   $��80��6v   ��6�   Z� ��6�   Z� ��6�   Z� .��5�      �� .+�5�   "   �� $��Image/add_bell.png��6(   $��50��6D   $��50��6O   5�      5�      �� 5�      -+   5        $ btn_modify_alert��5�   !   $
��Left=�6;   $��80,0,0,0q�6V   $	��Top=�6h   $��80��6�   $��80��6�   ��6�   Z� ��6�   Z� .��5�      �� .+�5�   "   �� $��Image/mod_bell.png��6(   $��50��6D   $��50��6O   5�      5�      �� 5�      -,   5        $ btn_delete_alert��5�   !   $
��Left=�6;   $��	160,0,0,0q�6V   $��80��6i   ��6t   Z� ��6�   Z� .��5�      �� .+�5�   "   �� $��Image/del_bell.png��6(   $��50��6D   $��50��6O   5�      5�      5�      �� 5�      --   5        $	 lv_alert��5�      $��1��61   $��Stretch=�5�      $��Stretch=�6D    5�      ��%T   '��       �� 5�      " ��6$   .�5�   "   ��  � ��$
 Left=�6O   5�      5�       5�      �� 5�      .P�5  "   �� $��Enabled��61   $	 100��5  O    6%   �� *Enable+6O   �� 5  "   $
��Name��61   $	 150��5  R    6%   �� *	AlertName+6R   �� 5  "   $��
Anoucement��61   $	 100��5  T    6%   �� *Anouncement+6T   �� 5
  "   $��Popup��61   $	 100��5  N    6%   �� *Popup+6N   5     5     �� 5     -.   5        $	 lv_listalert��5     $��Stretch=�65   $��Stretch=�6Z   $��2��6}    5     ��%T   '��       �� 5     " ��6$   .�5  "   ��  � ��$
 Left=�6O   5     5      5     �� 5     .P�5  "   �� $
��Date��61   $	 150��5  M    6%   �� *Date+6M   �� 5  "   $��
Alert Name��61   $	 150��5  M    6%   �� *Name+6M   �� 5   "   $��PacketID��61   $	 100��5!  P    6%   �� *	EventID+6P   �� 5#  "   $��	IP Source��61   $	 200��5$  R    6%   �� *	IP_Source+6R   5&     5(     �� 5*     $��0q�6    $��3��6+   ���	#FF88B04B68   5+     5,     5.     52     53     