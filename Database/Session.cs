namespace WpfApplication1.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class Session
    {
        public Session()
        {
            Details = new HashSet<Detail>();
        }
        [Key]
        public long SessionID { get; set; }
        [Required]
        [StringLength(16)]
        public string IP_in { get; set; }
        //[Required]
        //[StringLength(16)]
        //public byte[] IP_in_bin { get; set; }
        [Required]
        [StringLength(16)]
        public string IP_out { get; set; }
        //[Required]
        //[StringLength(16)]
        //public byte[] IP_out_bin { get; set; }
        [Required]
        [StringLength(32)]
        public string MAC_in { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Ended { get; set; }
        public int? Port_in { get; set; }
        public int? Port_out { get; set; }
        //public bool IsSSL { get; set; }
        public virtual ICollection<Detail> Details { get; set; }
    }
}
