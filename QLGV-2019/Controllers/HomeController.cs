using Neo4jClient;
using Newtonsoft.Json.Serialization;
using QLGV_2019.Action;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLGV_2019.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpGet]
       public ActionResult CreateSchoolYear()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateSchoolYear( int School_Year)
        {
            YearAction.Add_Year(School_Year);
            return View();
        }

        [HttpGet]
        public ActionResult UpdateSchoolYear(int school_year)
        {
            //int tmp = School_Year;
            ViewBag.Year = YearAction.Find(school_year);
            return View();
        }

        [HttpPost]
        public ActionResult UpdateSchooLYear(int Id, int nam_hoc)
        {
            int tmp = nam_hoc;
            YearAction.Edit_Year(nam_hoc);
            return Redirect("~/Home/AllSchoolYear");
        }

        [HttpGet]
        public ActionResult AllSchoolYear()
        {
            ViewBag.Year = YearAction.ShowAll();
            return View();
        }

        [HttpGet]
        public ActionResult DanhSachHocPhan()
        {
            
            return View();
        }

        [HttpGet]
        public ActionResult Marks()
        {

            return View();
        }
        [HttpGet]
        public ActionResult Phancong()
        {

            return View();
        }
        [HttpGet]
        public ActionResult Student()
        {

            return View();
        }
        [HttpGet]
        public ActionResult Teacher()
        {

            return View();
        }
        [HttpGet]
        public ActionResult ThongTinCaNhan()
        {

            if ((string)Session["role"] == "Student")
            {
                var tmp = StudentAction.ShowAll();
                for(int i = 0; i < tmp.Count; i++)
                {
                    if(tmp[i].Item1.id == (string)Session["id"])
                    {
                        ViewBag.SV = tmp[i];
                        break;
                    }
                }
                return View();
            }
            return Redirect("~/Home/Index");
        }

        [HttpGet]
        public ActionResult ThongTinMonHoc()
        {
            ViewBag.ChuyenNganh = SpecializedAction.GetAll();
            return View();
        }

        [HttpGet]
        public ActionResult DangKyMonHoc()
        {

            return View();
        }
        

        public ActionResult GiangVienInfo()
        {
            if ((string)Session["role"] == "Teacher")
            {
                var tmp = TeacherAction.ShowAll();
                for (int i = 0; i < tmp.Count; i++)
                {
                    if (tmp[i].Item1.id == (string)Session["id"])
                    {
                        ViewBag.Gv = tmp[i];
                        break;
                    }
                }
                return View();
            }
            return Redirect("~/Home/Index");
        }


    }
}