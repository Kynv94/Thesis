namespace WpfApplication1.Database
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public partial class Detail
    {
        [Key]
        public long Det_ID { get; set; }
        public long SessionID { get; set; }
        public DateTime UpdateTime { get; set; }
        [StringLength(1000)]
        public string KeyData { get; set; }
        public string TextData { get; set; }
        //public byte[] BinData { get; set; }
        public virtual Session Session { get; set; }
    }
}
