using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Models
{
    public class User
    {
        
        public string id_number { get; set; }
        public string sub_name { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string status { get; set; }
        public string role { get; set; }
        public bool isDelete { get; set; }

    }
}