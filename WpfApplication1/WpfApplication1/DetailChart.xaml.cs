using System;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using WpfApplication1.Database;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for DetailChart.xaml
    /// </summary>
    public partial class DetailChart : Window
    {
        public DetailChart(List<string> ip_list, List<Detail> packet)
        {
            InitializeComponent();
            List<string> protocol_list = new List<string> { "Web", "DNS", "FTP", "Mail" };
            Column_Series = new SeriesCollection { };
            foreach (string i in protocol_list)
            {
                Column_Series.Add(new ColumnSeries
                {
                    Title = i,
                    Values = GetValuePacket(i, ip_list, packet), 
            });
            }
            
            Labels = ip_list;
            Formatter = value => value.ToString("N");

            DataContext = this;
        }

        public SeriesCollection Column_Series { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        internal ChartValues<int> GetValuePacket(string protocol, List<string> ip_list, List<Detail> packet_list)
        {
            ChartValues<int> n = new ChartValues<int>();
            foreach (var ip in ip_list)
            {
                int count = 0;
                switch (protocol)
                {
                    case "Web":
                        for (int i = 0; i < packet_list.Count; ++i)
                            if (MainWindow.web.Contains(packet_list[i].PluginID) && packet_list[i].Session.IP_in == ip)
                                ++count;
                        break;

                    case "DNS":
                        for (int i = 0; i < packet_list.Count; ++i)
                            if (MainWindow.dns.Contains(packet_list[i].PluginID) && packet_list[i].Session.IP_in == ip)
                                ++count;
                        break;

                    case "FTP":
                        for (int i = 0; i < packet_list.Count; ++i)
                            if (MainWindow.ftp.Contains(packet_list[i].PluginID) && packet_list[i].Session.IP_in == ip)
                                ++count;
                        break;

                    case "Mail":
                        for (int i = 0; i < packet_list.Count; ++i)
                            if (MainWindow.mail.Contains(packet_list[i].PluginID) && packet_list[i].Session.IP_in == ip)
                                ++count;
                        break;
                }
                n.Add(count);
            }
            return n;
        }


    }
}
