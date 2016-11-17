using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using System.ComponentModel;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        IList<LivePacketDevice> allDevices;
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
           
            PacketList.Dispatcher.Invoke(() =>
            {
                // Add packet to the gridview
                PacketList.Items.Add(packet);
                try
                {
                    
                   Console.WriteLine("------" + packet);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("------" + ex);
                }
            });
        }

        private void Start_Btn(object sender, RoutedEventArgs e)
        {
            PacketList.Items.Clear();
            PacketList.Items.Refresh();
            cbDevice.IsEnabled = false;
            Start_Button.IsEnabled = false;
            Stop_Button.IsEnabled = true;

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerAsync(10000);
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

        private void PacketList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Packet packet = e.AddedItems[0] as Packet;
            Stack_Info.Children.Clear();

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

            // Expander Http Header
            if (packet.Ethernet.IpV4.Tcp.Http.Header != null)
            {
                Expander exHTTPHeader = new Expander();
                exHTTPHeader.Header = "HTTP Header";

                TextBlock tbHTTPHeader = new TextBlock();

                tbHTTPHeader.Text = packet.Ethernet.IpV4.Tcp.Http.Header.ToString();

                exHTTPHeader.Content = tbHTTPHeader;

                StackPanel_Add(Stack_Info, new List<UIElement> { exHTTPHeader });
            }


            
        }
    }

    
}

