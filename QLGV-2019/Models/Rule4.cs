using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Models
{
    public class Rule4
    {
        public int id { get; set; }
        public int min_subject { get; set; }
        public int max_subject { get; set; }
        public double old_avg_limit { get; set; }
        public bool isDelete { get; set; }
    }
}