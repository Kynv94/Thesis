using System.Collections.Generic;
using System.Linq;

namespace WpfApplication1
{
    class DatabaseHandle
    {
        public void Add_Packet(SimplePacket NewPacket)
        {
            using (var _SimpleData = new SimpleContext())
            {
                _SimpleData.SimplePackets.Add(NewPacket);
                _SimpleData.SaveChanges();
            }
        }
        public void Edit_Packet(SimplePacket _Packet)
        {
            using (var _SimpleData = new SimpleContext())
            {
                _SimpleData.Entry(_Packet).State = System.Data.Entity.EntityState.Modified;
                _SimpleData.SaveChanges();
            }
        }
        public void Delete_Packet(SimplePacket _Packet)
        {
            using (var _SimpleData = new SimpleContext())
            {
                _SimpleData.Entry(_Packet).State = System.Data.Entity.EntityState.Deleted;
                _SimpleData.SaveChanges();
            }
        }
        public List<SimplePacket> ListPacket()
        {
            using (var _SimpleData = new SimpleContext())
            {
                return _SimpleData.SimplePackets.ToList();
            }
        }
    }
}
