using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Models
{
    public class YearTerm
    {
        public int year_term { get; set; }
        public string day_start { get; set; }
        public string day_end { get; set; }
        public bool isDelete { get; set; }
    }
}