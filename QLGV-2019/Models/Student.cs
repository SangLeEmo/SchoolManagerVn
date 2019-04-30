using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Models
{
    public class Student
    {
        public string id_login { get; set; }
        public int yearSt { get; set; }
        public string specialize { get; set; }
        public string password { get; set; }
        public bool active { get; set; }
    }
}