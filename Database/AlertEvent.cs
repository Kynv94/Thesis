﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Database
{
    public class AlertEvent
    {
        [Key]
        public int EventID { get; set; }
        public int AlertID { get; set; }
        public string Address { get; set; }
        public virtual Alert Alert { get; set; }
    }
}
