using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApplication1.Database;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Alert.xaml
    /// </summary>
    public partial class Alert_Design : Window
    {
        private HandleDatabase db = new HandleDatabase();
        public Alert_Design()
        {
            InitializeComponent();
        }

        private void btn_add_web_Click(object sender, RoutedEventArgs e)
        {
            lv_web.Items.Add(tb_filter_web.Text);
            tb_filter_web.Text = null;
        }

        private void btn_del_web_Click(object sender, RoutedEventArgs e)
        {
            lv_web.Items.Remove(lv_web.SelectedItem);
        }

        internal void btn_alert_ok_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.alert_click == 1)
            {
                Alert new_alert = HandleAlert.add_alert(tb_name_alert.Text, (bool)cb_enabled.IsChecked, (bool)cb_announce.IsChecked, (bool)cb_popup.IsChecked);
                foreach (string i in lv_web.Items)
                    HandleAlert.add_web(i, new_alert);

                MainWindow.list_alert.Add(new_alert);
            }
            else
                if (MainWindow.alert_click == 2)
            {
                HandleAlert.modify_alert(MainWindow.alert_id, tb_name_alert.Text, (bool)cb_enabled.IsChecked, (bool)cb_announce.IsChecked, (bool)cb_popup.IsChecked);
                foreach (string i in lv_web.Items)
                    HandleAlert.modify_web_entry(MainWindow.alert_id, i);

                MainWindow.list_alert = db.get_all_alert();
            }

            this.Close();
        }

        private void lv_web_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_del_web.IsEnabled = true;
        }

        private void btn_alert_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
