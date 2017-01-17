using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Database
{
    public class Alert
    {
        public Alert()
        {
            AlertEvents = new HashSet<AlertEvent>();
        }
        [Key]
        public int AlertID { get; set; }
        [Required]
        [StringLength(1000)]
        public string AlertName { get; set; }
        public bool Enable { get; set; }
        public bool Anouncement { get; set; }
        public bool Popup { get; set; }
        public virtual ICollection<AlertEvent> AlertEvents { get; set; }
    }
}
