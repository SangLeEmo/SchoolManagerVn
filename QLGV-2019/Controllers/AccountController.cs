using QLGV_2019.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;

namespace QLGV_2019.Controllers
{    
    public class AccountController : Controller
    {
      
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Register()
        {
        //    if (Session["id"] != null && (string)Session["role"] == "Admin")
                return View();
            //else
            //    return Redirect("~/Home/Index");
        }

        [HttpPost]
        public ActionResult RegisterUser(string Id_Number, string Last_Name, string First_Name, string Password, string Role)
        {
            UserAction.Add_User(Id_Number, Last_Name, First_Name, Password, Role);
            return Redirect("~/");
        }

        [HttpPost]
        public ActionResult Login(string Id_Number, string Password)
        {
            
            if (UserAction.CheckLogin(Id_Number, Password) != null)
            {
                var user = UserAction.Find(Id_Number);
                Session["id"] = user.id_number;
                Session["role"] = user.role;
                Session["name"] = user.sub_name + " " + user.name;
                Session.Timeout = 24;
                return Redirect("~/Home/Index");
            }
            else
            {
                return Redirect("~/");
            }
        }

        public ActionResult Logout()
        {
            if (Session["id"] != null)
            {
                Session.Clear();
                Session["id"] = null;
            }
            return Redirect("~/");
        }

        [HttpGet]
        public ActionResult UpdateInfo()
        {
            if (Session["id"] != null)
            {
                ViewBag.Info = UserAction.Find((string)Session["id"]);
                return View();
            }
            else
                return Redirect("~/Home/Index");
        }

        [HttpPost]
        public ActionResult UpdateInfo(string Password)
        {
            UserAction.Edit_User((string)Session["id"], Password);
            return Redirect("~/Account/UpdateInfo");
        }

    }
}