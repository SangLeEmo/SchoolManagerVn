using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Models
{
    public class RegSectorSubject
    {
        public string id { get; set; }
        public double mark_theory { get; set; }
        public double mark_practice { get; set; }
        public double mark_average { get; set; }
        public bool isDelete { get; set; }
    }
}