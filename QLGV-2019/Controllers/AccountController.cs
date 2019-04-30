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
        public static string MD5Hash(string text)//Ma hoa MD5
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(string Id_login, string Last_Name, string First_Name, string Password, string Role)
        {
            string passMD5 = MD5Hash(Password);
            UserAction.Add_User(Last_Name, First_Name, Role, passMD5, );
            if (Role == "Admin")
            {
                { Id_login, Password, true };
                client.Cypher.Create("(:Admin {admin})").WithParam("admin", admin).ExecuteWithoutResultsAsync().Wait();
            }
            else if (Role == "Teacher")
            {
                var teacher = new Teacher { id_login = Id_login, password = Password, active = true };
                client.Cypher.Create("(:Teacher {teacher})").WithParam("teacher", user).ExecuteWithoutResultsAsync().Wait();
            }
            else if (Role == "Student")
            {
                var student = new Student { id_login = Id_login, password = Password, active = true };
                client.Cypher.Create("(:Student {user})").WithParam("student", user).ExecuteWithoutResultsAsync().Wait();
            }
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
                //Response.Write("<script>alert('Sai tài khoản hoặc mật khẩu !')</script>"); // thông báo lên html
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

    }
}