using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Models
{
    public class Subject
    {
        public string id { get; set; }
        public string sub_name { get; set; }
        public int numb_credit { get; set; }
        public int numb_theory { get; set; }
        public int numb_practice { get; set; }
        public double theory_percent { get; set; }
        public int numb_student { get; set; }
        public bool isDelete { get; set; }

    }
}