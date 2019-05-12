using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Models
{
    public class Student
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string date_of_birth { get; set; }
        public string email { get; set; }
        public string  phone { get; set; }
        public string address { get; set; }
        public string status{ get; set; }
    }
}