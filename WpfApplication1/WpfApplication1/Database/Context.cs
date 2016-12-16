namespace WpfApplication1.Database
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;

    public partial class Context : DbContext
    {
        public Context() : base("NetTough_Cache")
        { }
        public virtual DbSet<Detail> Details { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Detail>()
                .Property(e => e.KeyData)
                .IsFixedLength();
            modelBuilder.Entity<Session>()
                .Property(e => e.IP_in)
                .IsUnicode(false);
            modelBuilder.Entity<Session>()
                .Property(e => e.IP_out)
                .IsUnicode(false);
            modelBuilder.Entity<Session>()
                .HasMany(e => e.Details)
                .WithRequired(e => e.Session)
                .WillCascadeOnDelete(false);
        }
        /*
        public virtual async System.Threading.Tasks.Task<long> NT_add_session(string iP_in, string iP_out, string mAC_in, DateTime? started, DateTime? ended, int? port_in, Nullable<int> port_out)
        {
            var iP_inParameter = iP_in != null ?
                new SqlParameter("IP_in", iP_in) :
                new SqlParameter("IP_in", typeof(string));

            var iP_outParameter = iP_out != null ?
                new SqlParameter("IP_out", iP_out) :
                new SqlParameter("IP_out", typeof(string));

            var mAC_inParameter = mAC_in != null ?
                new SqlParameter("MAC_in", mAC_in) :
                new SqlParameter("MAC_in", typeof(string));

            var startedParameter = started.HasValue ?
                new SqlParameter("Started", started) :
                new SqlParameter("Started", typeof(DateTime));

            var endedParameter = ended.HasValue ?
                new SqlParameter("Ended", ended) :
                new SqlParameter("Ended", typeof(DateTime));

            var port_inParameter = port_in.HasValue ?
                new SqlParameter("Port_in", port_in) :
                new SqlParameter("Port_in", typeof(int));

            var port_outParameter = port_out.HasValue ?
                new SqlParameter("Port_out", port_out) :
                new SqlParameter("Port_out", typeof(int));

            var new_id = new SqlParameter("NewID", typeof(long));

            //new_id.Direction = System.Data.ParameterDirection.Output;

            var data = Database.SqlQuery<long>("exec NT_add_session @IP_in, @IP_out, @MAC_in, @Started, @Ended, @Port_in, @Port_out, @NewID", iP_inParameter, iP_outParameter, mAC_inParameter, startedParameter, endedParameter, port_inParameter, port_outParameter, new_id);
            var returnid = data.FirstOrDefaultAsync();
            returnid.
            //return this.Database.ExecuteSqlCommand("exec NT_add_session @IP_in, @IP_out, @MAC_in, @Started, @Ended, @Port_in, @Port_out, @New_id", iP_inParameter, iP_outParameter, mAC_inParameter, startedParameter, endedParameter, port_inParameter, port_outParameter, new_id);
        }
        public virtual long NT_add_detail(long? sessID, DateTime? updateTime, string keydata, string textdata)
        {
            var sessIDParameter = sessID.HasValue ?
                new SqlParameter("SessID", sessID) :
                new SqlParameter("SessID", typeof(long));

            var updateTimeParameter = updateTime.HasValue ?
                new SqlParameter("UpdateTime", updateTime) :
                new SqlParameter("UpdateTime", typeof(System.DateTime));

            var keydataParameter = keydata != null ?
                new SqlParameter("Keydata", keydata) :
                new SqlParameter("Keydata", typeof(string));

            var textdataParameter = textdata != null ?
                new SqlParameter("Textdata", textdata) :
                new SqlParameter("Textdata", typeof(string));
            var newid = new SqlParameter("New_id", typeof(long));

       //     var bindataParameter = bindata != null ?
         //       new SqlParameter("Bindata", bindata) :
           //     new SqlParameter("Bindata", typeof(byte[]));
            
            return this.Database.ExecuteSqlCommand("exec NT_add_detail @SessID, @UpdateTime, @Keydata, @Textdata, @New_id", sessIDParameter, updateTimeParameter, keydataParameter, textdataParameter, newid);
        }
        public virtual int NT_clear_all_data()
        {
            return this.Database.ExecuteSqlCommand("NT_clear_all_data");
        }

        public virtual int NT_clear_old_data(DateTime? current_date, int? max_age)
        {
            var current_dateParameter = current_date.HasValue ?
                new SqlParameter("current_date", current_date) :
                new SqlParameter("current_date", typeof(DateTime));

            var max_ageParameter = max_age.HasValue ?
                new SqlParameter("max_age", max_age) :
                new SqlParameter("max_age", typeof(int));

            return this.Database.ExecuteSqlCommand("exec NT_clear_old_data @current_date, @max_age", current_dateParameter, max_ageParameter);
        }

        public virtual int NT_delete_detail(long? detID)
        {
            var detIDParameter = detID.HasValue ?
                new SqlParameter("DetID", detID) :
                new SqlParameter("DetID", typeof(long));

            return this.Database.ExecuteSqlCommand("NT_delete_detail", detIDParameter);
        }
        */
    }
}
