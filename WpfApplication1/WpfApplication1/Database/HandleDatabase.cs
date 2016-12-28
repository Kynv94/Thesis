using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                if (oldsession != null && oldsession.State != 4) // Kiểm tra nếu gói tin mới thuộc session cũ 
                {
                    oldsession.State = new_session.State; //cập nhật state
                    oldsession.Ended = new_session.Ended; // cập nhật thời gian cuối
                    if (new_detail != null) //Kiểm tra nếu gói tin có thông tin Detail
                        add_new_detail(new_detail, oldsession);
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
                _data.Sessions.Add(newsession);
                _data.SaveChanges();
            }
        }
        //Method thêm Detail vào database
        private static void add_new_detail(Detail newdetail, Session _session)
        {
            using (var _data = new Context())
            {
                _session.Details.Add(newdetail);
                _data.Entry(_session).State = System.Data.Entity.EntityState.Modified;
                _data.SaveChanges();
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
                                   select s).FirstOrDefault();
                return old_session;
            }
        }

        /////get data


        //get all data
        private List<Detail> getdata()
        {
            using (var _data = new Context())
            {
                return _data.Details.ToList();
            }
        }
        //Group by date
        private IEnumerable<IGrouping<string, Detail>> databydate(List<Detail> list)
        {
            var groupofdate = list.GroupBy(s => s.UpdateTime.ToShortDateString());
            return groupofdate;
        }
        //Group by Party A
        private IEnumerable<IGrouping<string, Detail>> databyPartyA()
        {
            using (var _data = new Context())
            {
                var datadetail = getdata();
                var groupofPartyA = datadetail.GroupBy(s => s.Session.IP_in);
                return groupofPartyA;
            }
        }
        //Group by Party B
        private IEnumerable<IGrouping<string, Detail>> databyPartyB()
        {
            using (var _data = new Context())
            {
                var datadetail = getdata();
                var groupofPartyB = datadetail.GroupBy(s => s.Session.IP_out);
                return groupofPartyB;
            }
        }
        //Group by State
        private IEnumerable<IGrouping<string, Detail>> databyState()
        {
            using (var _data = new Context())
            {
                var datadetail = getdata();
                var groupofState = datadetail.GroupBy(s =>
                {
                    if (s.Session.State == 4)
                        return "Finished";
                    return "Inprogress";
                });
                return groupofState;
            }
        }
        //Group by Application Protocol
        private IEnumerable<IGrouping<string, Detail>> databyProtocol()
        {
            PortService ps = new PortService();
            using (var _data = new Context())
            {
                var datadetail = getdata();
                var groupofProtocol = datadetail.GroupBy(s =>
                {
                    return ps.GetServiceName(s.PluginID);
                });
                return groupofProtocol;
            }
        }
        //Xoa details
        private bool delete_detail(IEnumerable<Detail> _details)
        {
            try
            {
                using (var _data = new Context())
                {
                    _data.Details.RemoveRange(_details);
                    _data.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //v2


        //v1


    }
}
