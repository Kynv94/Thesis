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
using System.Windows.Media;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        IList<LivePacketDevice> allDevices;
        List<Packet> list_new_packet = new List<Packet>();
        Database.HandleDatabase db = new Database.HandleDatabase();
        public MainWindow()
        {
            InitializeComponent();
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
                cbDevice.Items.Add(name_device);
            }


            //collview_packet.Filter += Filter_FTP;
        }

        private bool Filter_Web(object item)
        {
            if (cb_web.IsChecked == false)
                return true;
            Packet packet = item as Packet;
            int port_src = packet.Ethernet.IpV4.Tcp.SourcePort;
            int port_des = packet.Ethernet.IpV4.Tcp.DestinationPort;
            return (port_src == 443 || port_src == 80 || port_des == 443 || port_des == 80);
        }
        public PacketDevice selectedDevice;
        private void cbDevice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            String text = e.AddedItems[0] as String;
            int deviceIndex;
            for (deviceIndex = 0; deviceIndex != allDevices.Count; ++deviceIndex)
            {
                LivePacketDevice device = allDevices[deviceIndex];
                if (text == (device.Name + " " + device.Description))
                    break;
            }
            selectedDevice = allDevices[deviceIndex];
            BasicLine();
        }

        public PacketCommunicator communicator;
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

                    if (list_new_packet.Count >= 300)
                    {
                        Packet[] list_newpacket2 = new Packet[list_new_packet.Count];
                        list_new_packet.CopyTo(list_newpacket2);
                        list_new_packet.Clear();
                        foreach (var item in list_new_packet)
                            HandlePacket.add_informationv2(item);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // Callback function invoked by libpcap for every incoming packet
        private void PacketHandler(Packet packet)
        {

            lv_packet.Dispatcher.Invoke(() =>
            {
                if (!packet.Ethernet.Ip.IsValid || !packet.Ethernet.Arp.IsValid)
                {
                }
                else
                {
                    list_new_packet.Add(packet);
                }
            });

        }
        private void add_information(Database.Session _session, Database.Detail _detail, Packet _packet)
        {
            if (_packet.Ethernet.IpV4.Source != null)
                _session.IP_in = _packet.IpV4.Source.ToString();
            else
                _session.IP_in = "0.0.0.0";
            if (_packet.Ethernet.IpV4.Destination != null)
                _session.IP_out = _packet.IpV4.Destination.ToString();
            else
                _session.IP_out = "0.0.0.0";
            _session.MAC_in = _packet.Ethernet.Source.ToString();
            _session.Started = _packet.Timestamp;
            _session.Ended = _packet.Timestamp;
            _detail.UpdateTime = _packet.Timestamp;
            //Phaan biet theo port
            try
            {
                _detail.KeyData = Dns.GetHostEntry(_session.IP_out).HostName;
            }
            catch (Exception)
            {
                _detail.KeyData = "";
            }
            if (_packet.IpV4.Tcp != null)
            {
                _session.Port_in = _packet.IpV4.Tcp.SourcePort;
                _session.Port_out = _packet.IpV4.Tcp.DestinationPort;
                if (_packet.Ethernet.IpV4.Tcp.Payload != null)
                {
                    _detail.TextData = _packet.Ethernet.IpV4.Tcp.Payload.Decode(Encoding.UTF8);
                    _detail.BinData = _packet.Ethernet.IpV4.Tcp.Payload.ToHexadecimalString().ToUpper();
                }
                else
                {
                    _detail.TextData = "";
                    _detail.BinData = "";
                }
            }
            else
            {
                _session.Port_in = 0;
                _session.Port_out = 0;
                _detail.TextData = "";
                _detail.BinData = "";
            }
        }

        private void Start_Btn(object sender, RoutedEventArgs e)
        {
            lv_packet.Items.Clear();
            lv_packet.Items.Refresh();
            cbDevice.IsEnabled = false;
            Start_Button.IsEnabled = false;
            Stop_Button.IsEnabled = true;


            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerAsync(1000);
        }

        private void Stop_Btn(object sender, RoutedEventArgs e)
        {
            cbDevice.IsEnabled = true;
            Start_Button.IsEnabled = true;
            Stop_Button.IsEnabled = false;

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
            Packet packet = e.AddedItems[0] as Packet;
            //Stack_Info.Children.Clear();

            //TextBlock tbFrame = new TextBlock();
            //tbFrame.Text = "Frame:" + packet.Count + " byte(s)";

            //StackPanel_Add(Stack_Info, new List<UIElement> { tbFrame });
            // Expander Ethernet
            //Expander exEthernet = new Expander();
            //exEthernet.Header = "Ethernet";

            //StackPanel stack_Ethernet = new StackPanel();
            //TextBlock tbMacSrc = new TextBlock(), 
            //          tbMacDes = new TextBlock(),
            //          tbType = new TextBlock();
            //tbMacSrc.Text = "Mac Address Source: " + (packet.Ethernet.Source.ToString() == "ff:ff:ff:ff" ? "Broadcast" : packet.Ethernet.Source.ToString());
            //tbMacDes.Text = "Mac Address Destination: " + (packet.Ethernet.Destination.ToString() == "ff:ff:ff:ff" ? "Broadcast" : packet.Ethernet.Destination.ToString());
            //tbType.Text = "Type: " + packet.Ethernet.EtherType.ToString();
            //StackPanel_Add(stack_Ethernet, new List<UIElement> { tbMacSrc, tbMacDes, tbType });
            //exEthernet.Content = stack_Ethernet;

            try
            {

                // Expander Http Header
                tab_httpheader.Content = null;
                if (packet.Ethernet.IpV4.Tcp.Http.Header != null)
                {
                    //Expander exHTTPHeader = new Expander();
                    //exHTTPHeader.Header = "HTTP Header";

                    TextBlock tbHTTPHeader = new TextBlock();

                    tbHTTPHeader.Text = packet.Ethernet.IpV4.Tcp.Http.Header.ToString();

                    //exHTTPHeader.Content = tbHTTPHeader;

                    //StackPanel_Add(Stack_Info, new List<UIElement> { tbHTTPHeader });
                    tab_httpheader.Content = tbHTTPHeader;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("------" + ex);
                throw ex;
            }
        }

        private void cbweb_changed(object sender, RoutedEventArgs e)
        {
            // CollectionViewSource.GetDefaultView(lv_packet.ItemsSource).Refresh();
        }

        private void cbftp_changed(object sender, RoutedEventArgs e)
        {

        }

        public void BasicLine()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 },
                    Fill = new BrushConverter().ConvertFromString("#00000000") as Brush,
                    LineSmoothness = 0,

                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                    Fill = new BrushConverter().ConvertFromString("#00000000") as Brush,
                    LineSmoothness = 0,
                    // PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 3",
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                    Fill = new BrushConverter().ConvertFromString("#00000000") as Brush,
                    LineSmoothness = 0,
                    // PointGeometry = DefaultGeometries.Square,
                    // PointGeometrySize = 15
                }
            };

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            SeriesCollection.Add(new LineSeries
            {
                Title = "Series 4",
                Values = new ChartValues<double> { 5, 3, 2, 4 },
                Fill = new BrushConverter().ConvertFromString("#00000000") as Brush,
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                // PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                // PointGeometrySize = 50,
                // PointForeround = Brushes.Gray
            });

            //modifying any series values will also animate and update the chart
            SeriesCollection[3].Values.Add(5d);

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }

    


}

