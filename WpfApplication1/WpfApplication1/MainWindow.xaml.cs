﻿using System;
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
        public MainWindow()
        {
            InitializeComponent();

            keys = db.databyPartyA().Select(s => s.Key).ToList();
            cb_ipadd_input.ItemsSource = keys;

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

        }


        public PacketDevice selectedDevice;
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
            selectedDevice = allDevices[deviceIndex];
        }

        public PacketCommunicator communicator;
        List<Packet> List_2;
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
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
                    //kiểm tra nếu List có đủ hơn 300 gói đã bắt

                }
            }
            catch (Exception) { }
        }

        void Add_Database(object sender, DoWorkEventArgs e)
        {
            foreach (var item in List_2)            
                HandlePacket.add_informationv2(item);
            keys = db.databyPartyA().Select(s => s.Key).ToList();
            cb_ipadd_input.ItemsSource = keys;
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

                    Console.WriteLine("======================");
                    List_2 = list_new_packet;
                    list_new_packet = new List<Packet>();

                    BackgroundWorker handle_packet = new BackgroundWorker();
                    handle_packet.WorkerReportsProgress = true;
                    handle_packet.DoWork += Add_Database;
                    handle_packet.RunWorkerAsync();

                }
            }
  

        }
        LoginForm lf = new LoginForm();

        private void Start_Btn(object sender, RoutedEventArgs e)
        {
            lf.Show();
            lf.Activate();
            lf.Topmost = true;
            lf.Topmost = false;
            lf.Focus();
            if (lf.IsActive != true)
            {
                cb_device.IsEnabled = false;
                btn_start.IsEnabled = false;
                btn_stop.IsEnabled = true;


                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.RunWorkerAsync(1000);
            }
        }

        private void Stop_Btn(object sender, RoutedEventArgs e)
        {
            cb_device.IsEnabled = true;
            btn_start.IsEnabled = true;
            btn_stop.IsEnabled = false;

            communicator.Break();
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
            Detail packet = e.AddedItems[0] as Detail ;
            try
            {
                if (packet.TextData != null)
                {
                    sv_discription.Content = packet.TextData;
                }
            }
            catch (Exception) { }
        }

        private ChartValues<int> Number_Packet(DateTime date_from, DateTime date_to)
        {
            ChartValues<int> np = new ChartValues<int>();
            for (DateTime time = date_from; time <= date_to; time = time.AddDays(1))
            {
                np.Add(list_packet_filter.Count(i => i.UpdateTime >= time && i.UpdateTime <= time.AddDays(1)));
            }
            return np;
        }
        // Chart
        public void BasicLine(string ip_src, DateTime date_from, DateTime date_to)
        {
            InitializeComponent();
            

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = ip_src,
                    Values = Number_Packet(date_from, date_to),
                    // Fill = new BrushConverter().ConvertFromString("#00000000") as Brush,
                    // LineSmoothness = 0,
                },
                
            };
            Labels = new List<string>();
            for (DateTime time = date_from; time <= date_to; time = time.AddDays(1))
            {
                Labels.Add(time.Date.ToString("d"));
            }

            YFormatter = value => value.ToString() ;
            // modifying the series collection will animate and update the chart
            //SeriesCollection.Add(new LineSeries
            //{
            //    Title = "Series 4",
            //    Values = new ChartValues<double> { 5, 3, 2, 4 },
            //    Fill = new BrushConverter().ConvertFromString("#00000000") as Brush,
            //    LineSmoothness = 0, //0: straight lines, 1: really smooth lines
            //    // PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
            //    // PointGeometrySize = 50,
            //    // PointForeround = Brushes.Gray
            //});

            //modifying any series values will also animate and update the chart
            // SeriesCollection[3].Values.Add(5d);

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<int, string> YFormatter { get; set; }
        private List<Detail> list_packet_filter;

        private void btn_filter_apply_Click(object sender, RoutedEventArgs e)
        {
            while (cb_ipadd_input.SelectedItem == null)
            {
                MessageBox.Show("Select IP Source", "Caution", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            string ip_src = cb_ipadd_input.SelectedItem.ToString();
            DateTime date_from = dp_from.SelectedDate.Value;
            DateTime date_to = DateTime.Now;
            if (dp_to.SelectedDate.Value != DateTime.Today)
            {
                date_to = dp_to.SelectedDate.Value;
            }
            List<long> protocol = new List<long>();
            if (cb_web.IsChecked == true)
                protocol.InsertRange(protocol.Count, new long[] { 443, 80 });
            if (cb_ftp.IsChecked == true)
                protocol.InsertRange(protocol.Count, new long[] { 20, 21 });
            if (cb_dns.IsChecked == true)
                protocol.InsertRange(protocol.Count, new long[] { 53, 137, 5355 });
            if (cb_mail.IsChecked == true)
                protocol.InsertRange(protocol.Count, new long[] { 25, 109, 110, 143, 158, 209, 587, 5108, 5109, 7052 });

            list_packet_filter = db.getdata(ip_src, date_from, date_to, protocol);
            lv_packet.ClearValue(ItemsControl.ItemsSourceProperty);
            lv_packet.Items.Clear();
            lv_packet.Items.Refresh();
            lv_packet.ItemsSource = list_packet_filter;
            long total_packet = list_packet_filter.Count;
            tb_total.Text = "TOTAL: " + total_packet;

            BasicLine(ip_src, date_from, date_to);
            // line_chart.Update();

        }



        //private IEnumerable<IGrouping<string, Detail>> databy()
        //{
        //    using (var _data = new Context())
        //    {
        //        var datadetail = HandleDatabase.getdata();
        //        var groupofPartyA = datadetail.GroupBy(s => s.Session.IP_in);
        //        return groupofPartyA;
        //    }
        //}
    }

    


}

