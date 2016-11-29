using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using System.ComponentModel;
using System.Windows.Data;
using System.Data.Entity;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
 
    public partial class MainWindow : Window
    {       
        IList<LivePacketDevice> allDevices;
        List<Packet> list_packet = new List<Packet>();
        public MainWindow()
        {
            InitializeComponent();
           /* using (var sc = new SimpleContext())
            {
                var _packet = new SimplePacket { PacketID = Guid.NewGuid(), DestIP = "123", DestMAC = "22", DestPort = "33" };
                sc.SimplePackets.Add(_packet);
                sc.SaveChanges();
            }*/
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

        private ICollectionView collview_packet;
        private bool Filter_Web(object item)
        {
            if (cb_web.IsChecked == false)
                return true;
            Packet packet = item as Packet;
            int port_src = packet.Ethernet.IpV4.Tcp.SourcePort;
            int port_des = packet.Ethernet.IpV4.Tcp.DestinationPort;
            return (port_src == 443 || port_src == 80  || port_des == 443 || port_des == 80);
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
                // Add packet to the gridview
                lv_packet.Items.Add(packet);

                //Create collection packet
                collview_packet = CollectionViewSource.GetDefaultView(lv_packet.Items);
                collview_packet.Filter = new Predicate<object>(Filter_Web);
            });
            
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

        public void StackPanel_Add(StackPanel sp, List<UIElement> list)
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
    }

    
}

