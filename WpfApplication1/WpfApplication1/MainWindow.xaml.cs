using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using System.ComponentModel;
using System.Windows.Data;
<<<<<<< HEAD
using System.Data.Entity;
=======
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using PcapDotNet.Packets.Http;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Text;
>>>>>>> origin/vers-1

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
                    // collview_packet.CollectionViewType = typeof(ListCollectionView);
                    communicator.ReceivePackets(0, PacketHandler);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private CollectionViewSource collview_packet = new CollectionViewSource();

        private void Filter_Web(object sender, FilterEventArgs e)
        {
            if (cb_web.IsChecked == false)
                e.Accepted &= true;
            Packet packet = e.Item as Packet;
            int port_src = packet.Ethernet.IpV4.Tcp.SourcePort;
            int port_des = packet.Ethernet.IpV4.Tcp.DestinationPort;
            e.Accepted &= (port_src == 443 || port_src == 80 || port_des == 443 || port_des == 80);
        }

        private void Filter_FTP(object sender, FilterEventArgs e)
        {
            if (cb_ftp.IsChecked == false)
                e.Accepted &= true;
            Packet packet = e.Item as Packet;
            int port_src = packet.Ethernet.IpV4.Tcp.SourcePort;
            int port_des = packet.Ethernet.IpV4.Tcp.DestinationPort;
            e.Accepted &= (port_src == 21 || port_des == 21);
        }

        // Callback function invoked by libpcap for every incoming packet
        private void PacketHandler(Packet packet)
        {
           
            lv_packet.Dispatcher.Invoke(() =>
            {
                // Add packet to the gridview
                
                lv_packet.Items.Add(packet);
                // lv_packet.ItemsSource = lv_packet.Items;
                //Create collection packet
                //collview_packet.Source = lv_packet.Items as ICollectionView;
                //collview_packet.Filter += new FilterEventHandler(Filter_Web);
                //collview_packet.Filter += new FilterEventHandler(Filter_FTP);

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

            try
            {

                // Expander Http Header
                
                sv_body.Content = packet.Ethernet.IpV4.Tcp.Http;
                if (packet.Ethernet.IpV4.Tcp.Http.Header != null)
                {
                    //Expander exHTTPHeader = new Expander();
                    //exHTTPHeader.Header = "HTTP Header";


                    sv_httpheader.Content = packet.Ethernet.IpV4.Tcp.Http.Header;
                    
                    //exHTTPHeader.Content = tbHTTPHeader;

                    //StackPanel_Add(Stack_Info, new List<UIElement> { tbHTTPHeader });

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("------" + ex);
                throw ex;
            }


            //IpV4Datagram ip = packet.Ethernet.IpV4;
            //TcpDatagram tcp = ip.Tcp;
            //HttpDatagram http = tcp.Http;
            //string httpBody = "";
            //string httpHeader = "";

            //try
            //{
            //    // parse packet
            //    if (tcp.IsValid && tcp.PayloadLength > 0)
            //    {
            //        // pull the payload 
            //        Datagram dg = tcp.Payload;
            //        MemoryStream ms = dg.ToMemoryStream();
            //        StreamReader sr = new StreamReader(ms);
            //        string content = sr.ReadToEnd();

            //        // skip if encrypted / non parsable
            //        if (content.IndexOf("HTTP") == -1) { continue; }

            //        // parse out header
            //        int endHeader = content.IndexOf("\r\n\r\n");
            //        if (endHeader == -1) { throw new System.Exception("Cant discern header breakpoint."); }
            //        httpHeader = content.Substring(0, endHeader);

            //        // parse out body
            //        // but make sure it isn't just composed of only the CRLF CRLF breaks
            //        if (http.Body != null && (content.Length - endHeader > 4))
            //        {
            //            // we have some body content
            //            // parse out and decompress if necessary
            //            Stream bodyStream = new MemoryStream(http.Body.ToArray());
            //            if (http.Header.ToString().ToLower().Contains("content-encoding: gzip"))
            //            {
            //                bodyStream = new GZipStream(bodyStream, CompressionMode.Decompress);
            //            }
            //            if (http.Header.ToString().ToLower().Contains("content-encoding: deflate"))
            //            {
            //                bodyStream = new DeflateStream(bodyStream, CompressionMode.Decompress);
            //            }

            //            // ERROR: for gzip streams, getting:
            //            // "The magic number in GZip header is not correct. Make sure you are passing in a GZip stream."
            //            // works fine for non-encrypted streams
            //            byte[] bodyBytes = StreamReader(bodyStream);
            //            httpBody = Encoding.UTF8.GetString(bodyBytes);
            //            sv_body.Content = httpBody;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            
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

