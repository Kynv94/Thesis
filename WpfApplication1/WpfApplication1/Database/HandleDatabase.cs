using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApplication1.Database
{
    class HandleDatabase
    {
        //for v2
        internal void Add_dataV2(Session new_session, Detail new_detail)
        {
            if (new_session != null) //Kiểm tra nếu gói tin mới hợp lệ
            {
                Session oldsession = get_old_session(new_session);
                if (oldsession != null && oldsession.State != 4 && new_detail != null && oldsession.SessionID !=0) // Kiểm tra nếu gói tin mới thuộc session cũ 
                {
                    oldsession.State = new_session.State; //cập nhật state
                    oldsession.Ended = new_session.Ended; // cập nhật thời gian cuối
                    oldsession.Details.Add(new_detail);
                    Int16 flag = 0;
                    foreach (var detail in oldsession.Details)
                    {
                        if (detail.SessionID != oldsession.SessionID)
                            continue;
                        flag += 1;
                        if (flag == oldsession.Details.Count)
                            add_new_detail(oldsession);
                    }
                }
                else //Gói tin không thuộc session cũ
                {
                    if (new_detail != null)
                    {
                        new_session.Details.Add(new_detail);
                        add_new_session(new_session);
                    }
                    else
                        add_new_session(new_session);
                }
            }
        }

        //Method thêm Session vào database
        private static void add_new_session(Session newsession)
        {
            using (var _data = new Context())
            {
                try
                {
                    _data.Sessions.Add(newsession);
                    _data.SaveChanges();
                }
                catch (Exception)
                { }
            }
        }
        //Method thêm Detail vào database
       // private static void add_new_detail(Session _session)
        private static void add_new_detail(Session _session)
        {
            using (var _data = new Context())
            {
                try
                {
                    _data.Entry(_session).State = System.Data.Entity.EntityState.Modified;
                    _data.SaveChanges();
                }
                catch (Exception)
                { }
            }
        }

        //Lấy session đã lưu
        internal Session get_old_session(Session newsession)
        {
            using (var _data = new Context())
            {
                var old_session = (from s in _data.Sessions
                                   where ((s.IP_in == newsession.IP_in && s.IP_out == newsession.IP_out && s.Port_in == newsession.Port_in && s.Port_out == newsession.Port_out)
                                   || (s.IP_in == newsession.IP_out && s.IP_out == newsession.IP_in && s.Port_in == newsession.Port_out && s.Port_out == newsession.Port_in))
                                   orderby s.SessionID descending
                                   select s).FirstOrDefault();
                return old_session;
            }
        }

        /////get data
        internal void getdata(ListView lv_packet, string ip_src, DateTime date_from, DateTime date_to, List<long> protocol)
        {
            using (var _data = new Context())
            {
                PortService ps = new PortService();
                var sessions = _data.Sessions.ToList();
                var details = _data.Details.ToList();
                var result = from d in details
                             join s in sessions
                             on d.SessionID equals s.SessionID
                             select new
                             {
                                 Id = d.Det_ID,
                                 Date = s.Started,
                                 UpdateTime = d.UpdateTime,
                                 PluginID = d.PluginID,
                                 Protocol = ps.GetServiceName(d.PluginID),                
                                 PartyA = s.IP_in,
                                 PartyB = s.IP_out,
                                 PortA = s.Port_in,
                                 PortB = s.Port_out,
                                 Info = d.KeyData,
                                 Discription = d.TextData
                             };
                
                var result2 = result.Where(s => s.PartyA == ip_src && s.UpdateTime >= date_from && s.UpdateTime <= date_to && protocol.Contains(s.PluginID)).ToList();
                lv_packet.ClearValue(ItemsControl.ItemsSourceProperty);
                lv_packet.Items.Clear();
                lv_packet.Items.Refresh();
                lv_packet.ItemsSource = result2;
            }
        }
        //Group by date
        private IEnumerable<IGrouping<string, Detail>> databydate(List<Detail> list)
        {
            var groupofdate = list.GroupBy(s => s.UpdateTime.ToShortDateString());
            return groupofdate;
        }
        //Group by Party A
        internal IEnumerable<IGrouping<string, Detail>> databyPartyA()
        {
            using (var _data = new Context())
            {
                var datadetail = _data.Details;
                var groupofPartyA = datadetail.GroupBy(s => s.Session.IP_in).ToList();
                return groupofPartyA;
            }
        }

        


        //Group by Party B
        //private IEnumerable<IGrouping<string, Detail>> databyPartyB()
        //{
        //    using (var _data = new Context())
        //    {
        //        var datadetail = getdata();
        //        var groupofPartyB = datadetail.GroupBy(s => s.Session.IP_out);
        //        return groupofPartyB;
        //    }
        //}
        ////Group by State
        //private IEnumerable<IGrouping<string, Detail>> databyState()
        //{
        //    using (var _data = new Context())
        //    {
        //        var datadetail = getdata();
        //        var groupofState = datadetail.GroupBy(s =>
        //        {
        //            if (s.Session.State == 4)
        //                return "Finished";
        //            return "Inprogress";
        //        });
        //        return groupofState;
        //    }
        //}
        ////Group by Application Protocol
        //private IEnumerable<IGrouping<string, Detail>> databyProtocol()
        //{
        //    PortService ps = new PortService();
        //    using (var _data = new Context())
        //    {
        //        var datadetail = getdata();
        //        var groupofProtocol = datadetail.GroupBy(s =>
        //        {
        //            return ps.GetServiceName(s.PluginID);
        //        });
        //        // var x = groupofProtocol.Where(s => s.Key== "http").SelectMany(d => d).ToList();
        //        // foreach(var items in x)
        //        //    foreach(var item in items) 

        //        return groupofProtocol;
        //    }
        //}
        ////Xoa details
        //private bool delete_detail(IEnumerable<Detail> _details)
        //{
        //    try
        //    {
        //        using (var _data = new Context())
        //        {
        //            _data.Details.RemoveRange(_details);
        //            _data.SaveChanges();
        //        }
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //v2


        //v1


    }
}
