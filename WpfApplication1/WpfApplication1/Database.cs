using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace WpfApplication1
{
    public class SimplePacket
    {
        [Key]
        public System.Guid PacketID { get; set; }
        public byte[] TimeStamp { get; set; }
        public long Length { get; set; }
        public string SourceIP { get; set; }
        public string DestIP { get; set; }
        public string SourcePort { get; set; }
        public string DestPort { get; set; }
        public string SourceMAC { get; set; }
        public string DestMAC { get; set; }
        public byte[] Info { get; set; }
    }
    public class SimpleContext : DbContext
    {
        public SimpleContext() : base("SimpleDatabase")
        {
            Database.SetInitializer<SimpleContext>(new CreateDatabaseIfNotExists<SimpleContext>());
            //Database.CreateIfNotExists();
        }

        public DbSet<SimplePacket> SimplePackets { get; set; }
    }
}
