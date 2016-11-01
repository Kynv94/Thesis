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
        IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
        public MainWindow()
        {
            InitializeComponent();
            
            // Retrieve the device list from the local machine

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
            //PacketDevice selectedDevice = (e.AddedItems[0] as ComboBoxItem).Content as LivePacketDevice;
            String text = e.AddedItems[0] as String;
            int deviceIndex;
            for (deviceIndex = 0; deviceIndex != allDevices.Count; ++deviceIndex)
            {
                LivePacketDevice device = allDevices[deviceIndex];
                if (text == (device.Name + " " + device.Description))
                    break;
            }
            selectedDevice = allDevices[deviceIndex];

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            //worker.ProgressChanged += worker_ProgressChanged;
            //worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync(10000);
        }
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //BackgroundWorker worker1 = sender as BackgroundWorker;
            // Open the device
            using (PacketCommunicator communicator =
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
        // Callback function invoked by libpcap for every incoming packet
        private void PacketHandler(Packet packet)
        {
            this.Dispatcher.Invoke(() =>
            {
                // print timestamp and length of the packet
                PacketList.Items.Add(packet);
                //string Timestamp = packet.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff");
                //IpV4Datagram ip = packet.Ethernet.IpV4;
                //TcpDatagram tcp = ip.Tcp;
                //IpV4Protocol Protocol = ip.Protocol;
                //IpV4Address IpSrc = ip.Source, 
                //            IpDes = ip.Destination;
                //ushort PortSrc = tcp.SourcePort,
                //       PortDes = tcp.DestinationPort;

                //// print ip addresses and udp ports
                //Console.WriteLine(ip.Source + ":" + udp.SourcePort + " -> " + ip.Destination + ":" + udp.DestinationPort);
            });
        }
    
    }

    
}

