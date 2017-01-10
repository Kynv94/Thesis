using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Data.Entity;
using System.Data.SqlClient;
using System.ComponentModel;

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
                if (oldsession != null && oldsession.State != 4 && new_detail != null && oldsession.SessionID != 0) // Kiểm tra nếu gói tin mới thuộc session cũ 
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
                    _data.Entry(_session).State = EntityState.Modified;
                    _data.SaveChanges();
                }
                catch (Exception)
                { }
            }
        }

        //Update Detail - Data
        internal void UpdateDetail(Detail new_detail)
        {
            if (new_detail.Session.Ended < new_detail.UpdateTime)
                new_detail.Session.Ended = new_detail.UpdateTime;
            try
            {
                using (var _data = new Context())
                {
                    _data.Entry(new_detail).State = EntityState.Modified;
                    _data.SaveChanges();
                }
            }
            catch (Exception) { }
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
                try
                {
                    _data.Entry(old_session).Collection(s => s.Details).Load();
                }
                catch (Exception) { }
                return old_session;
            }
        }

        /////get data
        internal List<Detail> getdata(string ip_src, DateTime date_from, DateTime date_to, List<int> protocol)
        {
            using (var _data = new Context())
            {
                var result = _data.Details.Include(s => s.Session).Where(s => s.Session.IP_in == ip_src
                                                    && s.UpdateTime >= date_from && s.UpdateTime <= date_to
                                                    && protocol.Contains(s.PluginID)).ToList();
                return result;
            }
        }
        
        //Update Detail - timeout
        internal int Do_time_out()
        {
            using (var _data = new Context())
            {
                int noOfRowUpdated = _data.Database.ExecuteSqlCommand("Update [dbo].[Sessions] Set [State] = 4  Where (ABS(DATEDIFF(MINUTE, [Ended], GETDATE())) > 8)");
                return noOfRowUpdated;
            }
        }

        //Delete Details

        internal int delete_details(DateTime? Date_from, DateTime? Date_to, List<int?> Protocols, List<string> Sources)
        {
            int NoofDeleted = 0;
            using (var _data = new Context())
            {
                var ListSessionIDs = _data.Details.Where(s => Sources.Contains(s.Session.IP_in)
                                                    && s.UpdateTime >= Date_from && s.UpdateTime <= Date_to
                                                    && Protocols.Contains(s.PluginID)).Select(s => s.SessionID).ToList<long>();
                foreach (var SessionID in ListSessionIDs)
                {
                    var SessionIdParameter = new SqlParameter("@Sessionid", SessionID);
                    NoofDeleted += _data.Database.ExecuteSqlCommand("DELETE FROM [dbo].[Details] WHERE [SessionID] = @Sessionid", SessionIdParameter);
                }
                return NoofDeleted;
            }
        }
        //Delete Detail
        internal int delete_detail(long? detID)
        {
            var DetIDParameter = detID.HasValue ?
                    new SqlParameter("@DetID", detID) :
                    new SqlParameter("@DetID", typeof(long));
            using (var _data = new Context())
            {
                int noOfRowDeleted = _data.Database.ExecuteSqlCommand("DELETE FROM [dbo].[Details] WHERE Det_ID = @DetID", DetIDParameter);
                return noOfRowDeleted;
            }
        }
        //Delte old detail
        internal int delete_old_data(int? max_age)
        {
            var MaxAgeParameter = max_age.HasValue ?
                    new SqlParameter("@Max_age", max_age) :
                    new SqlParameter("@Max_age", typeof(int));
            using (var _data = new Context())
            {
                int noOfRowDeleted = _data.Database.ExecuteSqlCommand("DELETE FROM [dbo].[Details] WHERE (ABS(DATEDIFF(DAY, [UpdateTime], GETDATE() )) > @MaxAge)", MaxAgeParameter);
                return noOfRowDeleted;
            }
        }
        ////Delete all
        internal int clear_all()
        {
            using (var _data = new Context())
            {
                long? a = _data.Details.Select(s => s.Det_ID).LastOrDefault();
                var DetIdParamether = a.HasValue ?
                    new SqlParameter("@ident", a) :
                    new SqlParameter("@ident", typeof(long));
                //int noOfDetailsRowDeleted = _data.Database.ExecuteSqlCommand("ALTER TABLE[Details] DROP CONSTRAINT[FK_dbo.Details_dbo.Sessions_SessionID] TRUNCATE TABLE[Details] TRUNCATE TABLE[Sessions] ALTER TABLE[Details] ADD CONSTRAINT[FK_dbo.Details_dbo.Sessions_SessionID] FOREIGN KEY([SessionID]) REFERENCES[Sessions]([SessionID])");
                int RemoveForeignKey = _data.Database.ExecuteSqlCommand("ALTER TABLE [Details] DROP CONSTRAINT [FK_dbo.Details_dbo.Sessions_SessionID]");
                int ClearDetails = _data.Database.ExecuteSqlCommand("TRUNCATE TABLE [Details]");
                int ClearSessions = _data.Database.ExecuteSqlCommand("TRUNCATE TABLE [Sessions]");
                int AddIdent = _data.Database.ExecuteSqlCommand("DBCC CHECKIDENT ([Details], RESEED, @ident) WITH NO_INFOMSGS", DetIdParamether);
                int AddForeignKey = _data.Database.ExecuteSqlCommand("ALTER TABLE [Details] ADD CONSTRAINT [FK_dbo.Details_dbo.Sessions_SessionID] FOREIGN KEY([SessionID]) REFERENCES [Sessions] ([SessionID])");
                return ClearSessions;
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

        public void Do_time_out(object sender, DoWorkEventArgs e)
        {
            using (var _data = new Context())
            {
                int noOfRowUpdated = _data.Database.ExecuteSqlCommand("Update [dbo].[Sessions] Set [State] = 4  Where (ABS(DATEDIFF(MINUTE, [Ended], GETDATE())) > 8)");
                // return noOfRowUpdated;
            }
        }

       

        

        ////Delete Detail
        //internal int delete_detail(long? detID)
        //{
        //    using (var _data = new Context())
        //    {
        //        var DetIDParameter = detID.HasValue ?
        //            new SqlParameter("@DetID", detID) :
        //            new SqlParameter("@DetID", typeof(long));
        //        int noOfRowDeleted = _data.Database.ExecuteSqlCommand("DELETE FROM [dbo].[Details] WHERE Det_ID = @DetID", DetIDParameter);
        //        return noOfRowDeleted;
        //    }
        //}

        ////Delte old detail
        //internal int delete_old_data(int? max_age)
        //{
        //    using (var _data = new Context())
        //    {
        //        var MaxAgeParameter = max_age.HasValue ?
        //            new SqlParameter("@Max_age", max_age) :
        //            new SqlParameter("@Max_age", typeof(int));
        //        int noOfRowDeleted = _data.Database.ExecuteSqlCommand("DELETE FROM [dbo].[Details] WHERE (ABS(DATEDIFF(DAY, [UpdateTime], GETDATE() )) > @MaxAge)", MaxAgeParameter);
        //        return noOfRowDeleted;
        //    }
        //}


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
