using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Models
{
    public class User
    {
        public int userID { get; set; }
        public string name { get; set; }
        public string sub_name { get; set; }
        public string bday { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
    }
}