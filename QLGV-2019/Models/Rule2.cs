using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Models
{
    public class Rule2
    {
        public int id { get; set; }
        public int term_in_year { get; set; }
        public int min_students { get; set; }
        public int max_students { get; set; }
        public bool isDelete { get; set; }

    }
}