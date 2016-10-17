using System.Windows;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MenList.Items.Add(new Packet()
            {
                ID = 1,
                Src = "192.168.1.2",
                Des = "192.168.2.2",
                Proc = "TCP",
                Len = 135,
                Info = "Server Hello"
            });
            MenList.Items.Add(new Packet()
            {
                ID = 2,
                Src = "192.168.2.2",
                Des = "192.168.1.2",
                Proc = "TCP",
                Len = 149,
                Info = "Reply Server"
            });
        }

    }

    class Packet
    {
        public long ID { get; set; }
        public string Src { get; set; }
        public string Des { get; set; }
        public string Proc { get; set; }
        public long Len { get; set; }
        public string Info { get; set; }

    }
}

