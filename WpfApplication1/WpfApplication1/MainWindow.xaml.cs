using System;
using System.Text;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using System.ComponentModel;
using System.Net;
using System.Windows.Data;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Windows.Media;
using WpfApplication1.Database;
using System.Linq;
using System.Timers;
using System.Security.AccessControl;
using System.Security;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        IList<LivePacketDevice> allDevices;
        List<Packet> list_new_packet = new List<Packet>();
        HandleDatabase db = new HandleDatabase();
        List<string> keys = new List<string>();
        private BackgroundWorker update_state;
        public static string password_hash = string.Empty;

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<int, string> YFormatter { get; set; }

        private List<Detail> list_packet_filter;
        private List<string> ip_src;
        private DateTime date_from;
        private DateTime date_to;
        private List<int> protocol;
        internal static List <int> web = new List<int> { 443, 80 };
        internal static List<int> ftp = new List<int> { 20, 21 };
        internal static List<int> dns = new List<int> { 53, 137, 5355 };
        internal static List<int> mail = new List<int> { 25, 109, 110, 143, 158, 209, 587, 5108, 5109, 7052 };
        internal static List<AlertWeb> list_web = new List<AlertWeb>();

        public PacketCommunicator communicator;
        private List<Packet> List_2;

        public PacketDevice selectedDevice_online;

        LoginForm lf;
        internal static bool check_password;

        internal static List<Alert> list_alert = new List<Alert>();

        public MainWindow()
        {
            InitializeComponent();

            // list_web = db.get_alert_web();
            string file_password = @".\password.txt";
            if (File.Exists(file_password) == false)
            {
                // create a new password file
                FileSecurity securityrules = new FileSecurity();
                securityrules.AddAccessRule(new FileSystemAccessRule("Administrators", FileSystemRights.FullControl, AccessControlType.Allow));
                securityrules.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.Read, AccessControlType.Allow));
                password_hash = "e19d5cd5af0378da05f63f891c7467af";
                
                using (FileStream fs = File.Create(file_password, 100, FileOptions.Encrypted, securityrules))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(password_hash);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }

                // add default password in hash form
            }
            else
            {
                using (StreamReader sr = File.OpenText(file_password))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        password_hash += s;
                    }
                }
            }

            try
            {
                // create ip list
                keys = db.databyPartyA().Select(s => s.Key).ToList();
                cb_ipadd_input.ItemsSource = keys;
            }
            catch (Exception) { }

            // Retrieve the device list from the local machine
            allDevices = LivePacketDevice.AllLocalMachine;
            if (allDevices.Count == 0)
            {
                string message = "No interfaces found! Make sure WinPcap is installed.";
                string caption = "Warning";
                MessageBoxButton buttons = MessageBoxButton.OK;
                MessageBoxResult result;

                result = MessageBox.Show(message, caption, buttons);
                return;
            }
            //Database.Session ss = new Database.Session
            // Print the list
            for (int i = 0; i != allDevices.Count; ++i)
            {
                LivePacketDevice device = allDevices[i];
                string name_device = device.Name + " ";
                if (device.Description != null)
                    name_device += device.Description;
                cb_device.Items.Add(name_device);
            }
            update_state = new BackgroundWorker();
            update_state.DoWork += db.Do_time_out;
            Timer timer = new Timer(8 * 60 * 1000);
            timer.Elapsed += timer_Elapsed;
            timer.Start();

        }


        void timer_Elapsed(object sender, ElapsedEventArgs e)

        {
            if (!update_state.IsBusy)
                update_state.RunWorkerAsync();
        }

        private void update_ip_list_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker update_iplist = new BackgroundWorker();
            update_iplist.WorkerReportsProgress = true;
            update_iplist.DoWork += update_iplist_bw;
            update_iplist.RunWorkerAsync();
            
        }
        
        private void update_iplist_bw(object sender, DoWorkEventArgs e)
        {
            update_ip_list.Dispatcher.Invoke(()
            =>
            {
                string message;
                try
                {
                    keys = db.databyPartyA().Select(s => s.Key).ToList();
                    cb_ipadd_input.ItemsSource = keys;

                    message = "Updated";

                }
                catch (Exception)
                {
                    message = "Update Fail";
                }
                string caption = "Anoucement";
                MessageBoxButton buttons = MessageBoxButton.OK;
                MessageBoxResult result;
                result = MessageBox.Show(message, caption, buttons, MessageBoxImage.Information);
            });
        }

        private void cb_device_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_start.IsEnabled = true;
            String text = e.AddedItems[0] as String;
            int deviceIndex;
            for (deviceIndex = 0; deviceIndex != allDevices.Count; ++deviceIndex)
            {
                LivePacketDevice device = allDevices[deviceIndex];
                if (text == (device.Name + " " + device.Description))
                    break;
            }
            selectedDevice_online = allDevices[deviceIndex];
            
        }

        
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var selectedDevice = selectedDevice_online;
                if (filename_import != null)
                {
                    selectedDevice = selectedDevice_offline;
                }
                
                // Open the device
                using (communicator =
                    selectedDevice.Open(65536,                                  // portion of the packet to capture
                                                                                // 65536 guarantees that the whole packet will be captured on all the link layers
                                        PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
                                        1000))                                  // read timeout
                {
                    
                    // Check the link layer. We support only Ethernet for simplicity.
                    if (communicator.DataLink.Kind != DataLinkKind.Ethernet)
                    {
                        string message = "This program works only on Ethernet networks.";
                        string caption = "Warning";
                        MessageBoxButton buttons = MessageBoxButton.OK;
                        MessageBoxResult result;
                        result = MessageBox.Show(message, caption, buttons);
                        return;
                    }

                    
                    // start the capture
                    communicator.ReceivePackets(0, PacketHandler);

                    List_2 = list_new_packet;
                    list_new_packet = new List<Packet>();

                    BackgroundWorker handle_packet = new BackgroundWorker();
                    handle_packet.WorkerReportsProgress = true;
                    handle_packet.DoWork += Add_Database;
                    if (filename_import != null)
                    {
                        handle_packet.RunWorkerCompleted += PacketHandler_Completed;
                    }
                    handle_packet.RunWorkerAsync();
                }
            }
            catch (Exception) { }
        }

        void Add_Database(object sender, DoWorkEventArgs e)
        {
            foreach (var item in List_2)
                HandlePacket.add_informationv2(item);
        }
        // Callback function invoked by libpcap for every incoming packet
        private void PacketHandler(Packet packet)
        {

            if (packet.Ethernet.Ip == null || packet.Ethernet.Arp == null)
            {
            }
            else
            {
                list_new_packet.Add(packet);

                if (list_new_packet.Count >= 500)
                {
                    List_2 = list_new_packet;
                    list_new_packet = new List<Packet>();

                    BackgroundWorker handle_packet = new BackgroundWorker();
                    handle_packet.WorkerReportsProgress = true;
                    handle_packet.DoWork += Add_Database;
                    handle_packet.RunWorkerAsync();

                    BackgroundWorker handle_alert = new BackgroundWorker();
                    handle_alert.WorkerReportsProgress = true;
                    handle_alert.DoWork += Show_Alert;
                    handle_alert.RunWorkerAsync();
                }

            }


        }

        private void Show_Alert (object sender, DoWorkEventArgs e)
        {
           // list
        }

        private void PacketHandler_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            filename_import = null;
            btn_import.Dispatcher.Invoke(()
                =>
            {
                IsEnabled = true;
                btn_start.IsEnabled = true;
                tb_total.Text = null;
                MessageBox.Show("Done", "Import", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private void Start_Btn(object sender, RoutedEventArgs e)
        {
            if (lf == null || lf.IsActive == false)
            {
                lf = new LoginForm();
            }
            check_password = false;
            lf.ShowDialog();

            if (check_password == true)
            {
                cb_device.IsEnabled = false;
                btn_start.IsEnabled = false;
                btn_stop.IsEnabled = true;
                btn_import.IsEnabled = false;


                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.RunWorkerAsync();
            }
        }

        private void Stop_Btn(object sender, RoutedEventArgs e)
        {
           
            cb_device.IsEnabled = true;
            btn_start.IsEnabled = true;
            btn_stop.IsEnabled = false;
            btn_import.IsEnabled = true;

            communicator.Break();

        }

        private string filename_import = null;
        private OfflinePacketDevice selectedDevice_offline;
        private void btn_import_Click(object sender, RoutedEventArgs e)
        {    
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pcap file (*.pcap)|*.pcap";
            if (openFileDialog.ShowDialog() == true)
            {
                filename_import = openFileDialog.FileName;

                selectedDevice_offline = new OfflinePacketDevice(filename_import);
                btn_import.IsEnabled = false;
                btn_start.IsEnabled = false;
                tb_total.Text = "IMPORTING";
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.RunWorkerAsync();

            }
        }

        public void StackPanel_Add(StackPanel sp, List<System.Windows.UIElement> list)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                sp.Children.Add(list[i]);
            }
        }

        private void lv_packet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.AddedItems.Count == 0)
                return;
            Detail packet = e.AddedItems[0] as Detail;
            sv_discription.Content = null;
            try
            {
                if (packet.TextData != null)
                {

                    sv_discription.Content = packet.TextData;
                }
            }
            catch (Exception) { }
        }

        private ChartValues<ObservableValue> Number_Packet(string ip, DateTime date_from, DateTime date_to)
        {
            ChartValues<ObservableValue> np = new ChartValues<ObservableValue>();
            for (DateTime time = date_from; time <= date_to; time = time.AddDays(1))
            {
                np.Add(new ObservableValue(list_packet_filter.Count(i => i.Session.IP_in == ip && i.UpdateTime >= time && i.UpdateTime <= time.AddDays(1))));
            }
            return np;
        }
        // Chart
        public void BasicLine(List<string> ip_src, DateTime date_from, DateTime date_to)
        {
            InitializeComponent();

            SeriesCollection = null;
            Labels = null;
            YFormatter = null;

            SeriesCollection = new SeriesCollection();
            foreach (string ip in ip_src)
            {
                SeriesCollection.Add(
                new LineSeries
                {
                    Title = ip,
                    Values = Number_Packet(ip, date_from, date_to),
                    Fill = new BrushConverter().ConvertFromString("#00000000") as Brush,
                    LineSmoothness = 0,
                }
                );
            }
            Labels = new List<string>();
            for (DateTime time = date_from; time <= date_to; time = time.AddDays(1))
            {
                Labels.Add(time.Date.ToString("d"));
            }

            YFormatter = value => value.ToString();
            // modifying the series collection will animate and update the chart
            //SeriesCollection.Add(new LineSeries
            //{
            //    Title = "Series 4",
            //    Values = new ChartValues<double> { 5, 3, 2, 4 },
            //    Fill = new BrushConverter().ConvertFromString("#00000000") as Brush,
            //    LineSmoothness = 0, //0: straight lines, 1: really smooth lines
            //});

            //modifying any series values will also animate and update the chart
            // SeriesCollection[3].Values.Add(5d);

            DataContext = null;
            DataContext = this;
        }

        private void check_filter_info ()
        {
            while (lv_ip.Items == null)
            {
                MessageBox.Show("Select IP Source", "Caution", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            ip_src = lv_ip.Items.OfType<string>().ToList();
            date_from = dp_from.SelectedDate.Value;
            date_to = DateTime.Now;
            if (dp_to.SelectedDate.Value != DateTime.Today)
            {
                date_to = dp_to.SelectedDate.Value;
            }
            protocol = new List<int>();
            
            if (cb_web.IsChecked == true)
                protocol.InsertRange(protocol.Count, web);
            if (cb_ftp.IsChecked == true)
                protocol.InsertRange(protocol.Count, ftp);
            if (cb_dns.IsChecked == true)
                protocol.InsertRange(protocol.Count, dns);
            if (cb_mail.IsChecked == true)
                protocol.InsertRange(protocol.Count, mail);

        }

        private void btn_filter_apply_Click(object sender, RoutedEventArgs e)
        {
            check_filter_info();

            list_packet_filter = db.getdata(ip_src, date_from, date_to, protocol);
            lv_packet.ClearValue(ItemsControl.ItemsSourceProperty);
            lv_packet.Items.Clear();
            lv_packet.Items.Refresh();
            lv_packet.ItemsSource = list_packet_filter;
            long total_packet = list_packet_filter.Count;
            tb_total.Text = "TOTAL: " + total_packet;

            BasicLine(ip_src, date_from, date_to);
            

        }

        private void btn_filter_delete_Click(object sender, RoutedEventArgs e)
        {
            check_filter_info();
            if (ip_src != null)
            {
                if (MessageBox.Show("Are you sure?", "Delete Data", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    string message = "Total Delete: " + db.delete_details(ip_src, date_from, date_to, protocol);
                    MessageBox.Show(message, "Anoucement", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void btn_add_alert_Click(object sender, RoutedEventArgs e)
        {
            HandleDatabase a = new HandleDatabase();
            list_alert.Add(a.get_alert(6));
            Alert_Design new_alert = new Alert_Design();
            new_alert.ShowDialog();
            lv_alert.ItemsSource = list_alert;
        }

        private void btn_add_ip_Click(object sender, RoutedEventArgs e)
        {
            while (cb_ipadd_input.SelectedItem == null)
            {
                MessageBox.Show("Select IP Source", "Caution", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            lv_ip.Items.Add(cb_ipadd_input.SelectedItem.ToString());
            cb_ipadd_input.SelectedItem = null;
        }

        private void btn_del_ip_Click(object sender, RoutedEventArgs e)
        {
            lv_ip.Items.Remove(lv_ip.SelectedItem);
        }

        private void lv_ip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_del_ip.IsEnabled = true;
        }

        private void line_chart_DataClick(object sender, ChartPoint chartPoint)
        {
            DetailChart dc = new DetailChart(ip_src, list_packet_filter);
            dc.Show();
          
        }
    }


}

