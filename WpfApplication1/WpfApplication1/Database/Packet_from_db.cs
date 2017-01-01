using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfApplication1.Database
{
    class Packet_from_db
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime UpdateTime { get; set; }
        public int PluginID { get; set; }
        public string Protocol { get; set; }
        public string PartyA { get; set; }
        public string PartyB { get; set; }
        public int PortA { get; set; }
        public int PortB { get; set; }
        public string Info { get; set; }
        public string Discription { get; set; }
    }
}
