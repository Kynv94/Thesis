   M S B A M L   `   `   `  ���� J  FWpfApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WpfApplication1WpfApplication1  U QLiveCharts.Wpf, Version=0.8.0.0, Culture=neutral, PublicKeyToken=3b585c2a5f1a92c1G4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem #clr-namespace:Wpf.UtilWpf.Util       WpfApplication1.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:WpfApplication1     Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     6sys&clr-namespace:System;assembly=mscorlib     'utilclr-namespace:Wpf.Util       � Title$  	NET TOUGH�� � WindowState$ 	Maximized=� � 	Resources  �� 
TargetType �� Property �� Value&   WpfApplication1.Convert_Protocol   GetServiceNameMethod��%A   '��       &	  (     �� " ��.���  � ��$ Stretch=�  .���  � RowDefinitions �� .+��� -    ��Name$ tabControl_level1��$��5q�
����������	#FF88B04B.@��� -    ��Name$ 	Tab_Event��$��EVENT��$��0q�$��75��$��20��.��� 
������� �� $	��100 ��� $
��259* ��� $	��83* ��� $��20 �.+��� -   	 ��Name$	 	lv_packet��$
 �� IsSynchronizedWithCurrentItem$

 TrueY�$��Stretch=�$��Stretch=���.  �� SelectionMode$ Single=�$��1��   Wpf.Util.GridViewSort   AutoSort .  ��%/   '��       �� " ��.���  � ��$
 Left=� �� View �� .P��� $��ID�� �� Width$ 50��   PropertyName$ Det_ID�� �� DisplayMemberBinding �� *Det_ID+�� $
��Date��$	 180��$ Session.Started�� �� *Session.Started+�� $��Last Update��$	 180��$ Session.Ended�� �� *Session.Ended+�� $��Protocol��$	 130��$ Protocol�� �� *
PluginID+ �� 	Converter# [  �� $��Party A��$	 150��$ Session.IP_in�� �� *Session.IP_in+�� $��Port A��$ 70��$ Session.Port_in�� �� *Session.Port_in+�� $��Party B��$	 150��$ Session.IP_out�� �� *Session.IP_out+�� $��Port B��$ 70��$ Session.Port_out�� �� *Session.Port_out+�� $
��Info��$	 300�� �� *	KeyData+.@��� $	��100��$	��100���� -    ��Name$ gridSplitter6��$��0��
�������$��Stretch=�$��2��$��5��$	��Top=��� -   $ tab_Info��$��0,5,0,0q�$��2��.@��� -   $ tab_discription��$��Discription��$��
-2,-1,1,-1q�.���� -    ��Name$ sv_discription���� -   $ tabControl_Event��$��0q�.@��� $
��View��$��	3,-2,-7,0q�.��� -	    �Name$ grid_filter�����	#FFE5E5E5$��0q� � ColumnDefinitions �� $	��101 ��� $	��101 ��� $	��101 ��� $	��197 ��� $	��235 �.+�d� -
    d�Name$ btn_filter_ipsrc��$��	IP Source��$��0q�
���   � J� OpacityMask# � �����	#FFE5E5E5��Z� � -    �Name$ popup_filter_ipsrc�� � 	StaysOpen .  ���� *	IsChecked+ �� ElementName$ btn_filter_ipsrc�� � PlacementTarget �� $ btn_filter_ipsrc��.��� 
�������$	��115��$	��Top=�$	��200��.+��� 
�������$��1q�
���   ��� -    ��Name$ lb_ipadd��$��IP Address:��$��10,10,106,45q��� -    ��Name$ cb_ipadd_input��$
��Left=�$��	10,45,0,0q�$	��Top=�$	��180���� -    ��Name$ update_ip_list��$��Update��$
��Left=�$��	61,88,0,0q�$	��Top=�$��75��d� -   $ btn_filter_time��$
��Time��$��0q����	#FFE5E5E5# � ��$��1����Z� � -   $ popup_filter_time�� .  ���� *	IsChecked+$ btn_filter_time�� �� $ btn_filter_time��.��� 
�������$��80��$	��Top=�$	��200��.+��� 
�������$��1q�
���   ��� $��From: ��$��
0,10,152,0q�$��28��$	��Top=��� $
��To: ��$��0,45,152,10q�(  "System.Windows.Controls.DatePicker  -   $��dp_from��
 �� Now    SelectedDate#  Z $
��Left=�$��	53,10,0,0q�$	��Top=�$	��137��  -   $��dp_to��#  Z $
��Left=�$��	53,47,0,0q�$	��Top=�$	��137��d� -   $ btn_filter_protocol��$��Protocol�����	#FFE5E5E5$��2����Z� � -   $ popup_filter_protocol�� .  ���� *	IsChecked+$ btn_filter_protocol�� �� $ btn_filter_protocol��.���� 
�������$��1q�
���   �$��2��$��2��._�� -   ! �Name$! wrap_filter_protocol��$��85��$	��170��
�������$��2��$��2��.+��� -   " ��Name$" cb_web��$	��Web��$
��Left=�$��	10,10,0,0q�$	��Top=�# d� 	IsChecked$
# TrueY��� -   $" cb_ftp��$	��FTP��$
��Left=�$��	50,10,0,0q�$	��Top=�$
# TrueY��� -   $" cb_dns��$	��DNS��$
��Left=�$��	10,10,0,0q�$	��Top=�$
# TrueY��� -   $" cb_mail��$
��Mail��$
��Left=�$��	50,10,0,0q�$	��Top=�$
# TrueY��� -   $" cb_proxy��$��Proxy��$
��Left=�$��	60,10,0,0q�$	��Top=�$
# TrueY��� -   $ btn_filter_apply��$��Apply��$��3��$��Center=�$	��2,0q�$��Right=�$	��101����Z� ��Z� �� -   $ btn_filter_delete��$��Delete��$��4��$��Center=�$	��101��$
��Left=���Z� ��Z� �� -   $ tab_control��$��Control��$��
8,-2,-12,0q�.��� ���	#FFE5E5E5 �� $	��405 ��� $	��333 �.+��� -   $ 
btn_import��$��Import��$��30,5,0,0q�$��1��$��60��$��60��$
��Left=�$	��Top=��� -   $ 	cb_device��$ �� Text$$ --Select device--��$��	60,24,0,0q�$	��Top=�$��26��$
��Left=�$	��174���� -    $ btn_stop��$
��Stop��{�.  $��
330,11,0,0q�$��50��$��50��$��Center=�$��Center=�% J� RenderTransformOrigin$% 1.033,0.775&�$
��Left=�$	��Top=����� �� -!   $ 	btn_start��$��Start��{�.  $��
256,11,0,0q�$��50��$
��Left=�$	��Top=�$% 0.5,0.5&�$��50��$��Center=�$��Center=����� �� -"   $ label_device��$��Device��$��	10,24,0,0q�$��26��$
��Left=�$	��Top=�$��45���� -#   $��sb_total��$��3�����	#FF88B04B.@��� -$   & ��Name$& tb_total��$
��Wrap=�$��25��$	��183��$��27,0,0,0q�' � UseLayoutRounding' .  $
��Bold"���Z� �� -%   $ 	Tab_Chart��$��0q�$��CHART��$��75��$��20��.��� 
������� �� .+�� $��	0,119,0,0q� �� #  LiveCharts.Wpf.CartesianChart  -&   (  Name$( 
line_chart��&   LiveCharts.Wpf.Charts.Base.Chart)   LegendLocation$) Right=�$��0q�*   Series* �� *SeriesCollection++   AxisY+   LiveCharts.Wpf.Axis  ,   FontSize$, 15Q�-   Title$- Packets��.   LabelFormatter. �� *
YFormatter+/   AxisX/   $, 15Q�$
- Date��0   Labels0 �� *Labels+