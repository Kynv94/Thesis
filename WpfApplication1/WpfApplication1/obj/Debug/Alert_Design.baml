   M S B A M L   `   `   `  ����J  FWpfApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WpfApplication1WpfApplication1  5   	   "    WpfApplication1.Alert_Design   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:WpfApplication1     5   	     � Title$  Alert��5   	   $	��320��6   $��511.125��6$    � WindowStartupLocation$ CenterScreen=�5	   	    � 
ResizeMode$ NoResize=�6.   .��5
      � .+�5   
   � $
��Left=�6   $	��112��6*   $��	10,10,0,0q�67   $	��Top=�6J   $	��235��6b   .+�5      �� -   5         ��Name$ lb_main��5      $
��Main��6%   $
��Left=�64   $	��Top=�6O   �� 5      -   5        $ lb_name_alert��5      $
��Name��6+   $
��Left=�6:   $��	10,26,0,0q�6U   $	��Top=�6h   �� 5      -   5         ��Name$ tb_name_alert��5      $
��Left=�6-   $��23��6H   $��	10,52,0,0q�6T    �� TextWrapping$
 Wrap=�6g   $	��Top=�6{   $	��215��6�   �� 5      -   5         ��Name$ 
cb_enabled��5      $��Enabled��6+   $
��Left=�6=   $��	10,80,0,0q�6X   $	��Top=�6k    d� 	IsChecked$
 TrueY�6�   �� 5      ���	#FF88B04B$��1q�6-   $
��Left=�6A   $	��112��6\   $	��Top=�6i   $	��235��6�   5      � 5   
   $
��Left=�6   $	��230��6*   $��
250,10,0,0q�67   $	��Top=�6K   $	��235��6c   .+�5      �� -   5        $ lb_filter_alert��5      $��Filter��6-   $
��Left=�6>   $	��Top=�6Y   �� 5      -   5        $ lb_filter_web��5      $	��Web��6+   $
��Left=�69   $��	10,26,0,0q�6T   $	��Top=�6g   �� 5      -   5        $ tb_filter_web��5      $
��Left=�6-   $��23��6H   $��	10,52,0,0q�6T   $ NoWrap=�6g   $	��Top=�6}   $	��215��6�   �� 5      -   5         ��Name$ lv_web��5      $
��Left=�6'   $	��140��6B   $��	10,80,0,0q�6O   $	��Top=�6b   $	��215��6z   	 �� View	 5      �� 5      .P�5      �� $��Address��6)   
 �� Width$	
 210��6:   5      5      �� 5      -	   5         ��Name$ btn_add_web��5      $��+��6*   $
��Left=�66   $��
180,27,0,0q�6Q   $	��Top=�6e   $��20��6}   $��20��6�   $
��Bold"�6�    �� 	IsDefault . 6�   �� 5      -
   5        $ btn_del_web��5      $��x��6*   $
��Left=�66   $��
205,27,0,0q�6Q   $	��Top=�6e   $��20��6}   $
��Bold"�6�   $��20��6�   {�. 6�   �� 5      ���	#FF88B04B$��1q�6-   $
��Left=�6A   $	��230��6\   $	��Top=�6i   $	��235��6�   5!      � 5"   
   $
��Left=�6   $	��100��6*   $��
10,140,0,0q�67   $	��Top=�6K   $	��235��6c   .+�5#      �� -   5        $ label1��5#      $��Alarms��6$   $
��Left=�65   $	��Top=�6P   �� 5$      -   5        $ cb_announce��5$      $��Announcement��6,   $
��Left=�6C   $��	30,31,0,0q�6^   $	��Top=�6q   $
 TrueY�6�   �� 5%      -   5        $ cb_popup��5%      $��Message Box��6)   $
��Left=�6?   $��	30,65,0,0q�6Z   $	��Top=�6m   $
 TrueY�6�   �� 5&      ���	#FF88B04B$��1q�6-   $
��Left=�6A   $	��100��6\   $	��Top=�6i   $	��235��6�   5,      �� 5-   
   -   5        $ btn_alert_ok��5-      $��OK��6'   $
��Left=�64   $��310,254,0,0q�6O   $	��Top=�6d   $��76��6|   6�   �� 5.   
   -   5        $ btn_alert_cancel��5.      $��Cancel��6+   $
��Left=�6<   $��410,254,0,0q�6W   $	��Top=�6l   $��75��6�   6�   5/      50      