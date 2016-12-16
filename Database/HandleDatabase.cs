using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Database
{
    struct Selected_sessions
    {
        public string IP_in;
        public string IP_out;
        public int? Port_in;
        public int? Port_out;
    };
    class HandleDatabase
    {
        //Thêm dữ liệu vào database
        public void Add_data(Session newsession, Detail newdetail)
        {
            using (var _data = new Context())
            {
                ObjectParameter new_detail_id = new ObjectParameter("Det_ID", typeof(long));

                //Thu thập IP, Port trong Database
                IEnumerable<Selected_sessions> Selected_session_ip_port = from s in _data.Sessions
                                                                          select new Selected_sessions { IP_in = s.IP_in, IP_out = s.IP_out, Port_in = s.Port_in, Port_out = s.Port_out };

                //Chọn riêng IP, Port trong new session để so sánh
                Selected_sessions New_session_ip_port = new Selected_sessions { IP_in = newsession.IP_in, IP_out = newsession.IP_out, Port_in = newsession.Port_in, Port_out = newsession.Port_out };

                //So sánh dữ liệu hiện tại có thuộc session nào không, nếu không thì tạo mới session, có thì chỉ add detail vào season
                if (!Selected_session_ip_port.Contains(New_session_ip_port, new Selected_session_Comparer()))
                {
                    //add new session
                    ObjectParameter new_session_id = new ObjectParameter("SessionID", typeof(long));
                    long sessionid = _data.NT_add_session(newsession.IP_in
                        , newsession.IP_out
                        , newsession.MAC_in
                        , newsession.Started
                        , newsession.Ended
                        , newsession.Port_in
                        , newsession.Port_out
                        , new_session_id);
                    sessionid = Convert.ToInt64(new_session_id.Value);
                    //add detail với sessionID được tạo mới từ add session
                    long detailid = _data.NT_add_detail(sessionid
                        , newdetail.UpdateTime
                        , newdetail.KeyData
                        , newdetail.TextData
                        //  , newdetail.BinData
                        , new_detail_id);
                }
                else
                {
                    //Lấy SessionID của session đã tồn tại.
                    var old_session_id = (from s in _data.Sessions
                                          where ((s.IP_in == newsession.IP_in && s.IP_out == newsession.IP_out && s.Port_in == newsession.Port_in && s.Port_out == newsession.Port_out)
                                          || (s.IP_in == newsession.IP_out && s.IP_out == newsession.IP_in && s.Port_in == newsession.Port_out && s.Port_out == newsession.Port_in))
                                          select s.SessionID).First();
                    //add detail vào session
                    long detailid = _data.NT_add_detail(old_session_id
                        , newdetail.UpdateTime
                        , newdetail.KeyData
                        , newdetail.TextData
                        //, newdetail.BinData
                        , new_detail_id);
                    newsession.Details.Add(newdetail);
                }
            }
        }
        //Xóa detail được chọn
        public bool delete_detail(long detid)
        {
            try
            {
                using (var _data = new Context())
                {
                    detid = _data.NT_delete_detail(detid);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        //Xóa dữ liệu trong khoảng thời gian bao nhiêu ngày
        public bool delete_old_data(DateTime Current_time, int max_age)
        {
            try
            {
                using (var _data = new Context())
                {
                    max_age = _data.NT_clear_old_data(Current_time, max_age);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        //Xóa tất cả dữ liệu
        public bool delete_all_data()
        {
            try
            {
                using (var _data = new Context())
                {
                    int delete = _data.NT_clear_all_data();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<Detail> GetData()
        {
            using (var _data = new Context())
            {
                return _data.Details.ToList();
            }
        }
    }
    class Selected_session_Comparer : IEqualityComparer<Selected_sessions>
    {
        public bool Equals(Selected_sessions x, Selected_sessions y)
        {
            if ((x.IP_in == y.IP_in && x.IP_out == y.IP_out && x.Port_in == y.Port_in && x.Port_out == y.Port_out)
                || (x.IP_in == y.IP_out && x.IP_out == y.IP_in && x.Port_in == y.Port_out && x.Port_out == y.Port_in))
                return true;
            return false;
        }
        public int GetHashCode(Selected_sessions obj)
        {
            return obj.GetHashCode();
        }
    }
}
