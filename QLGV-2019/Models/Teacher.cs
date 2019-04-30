using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Models
{
    public class Teacher
    {
        public string id_login { get; set; }
        public string degree { get; set; }
        public string password { get; set; }
        public bool active { get; set; }
    }
}