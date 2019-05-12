using QLGV_2019.Action;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLGV_2019.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult AllInfo(string Id)
        {
            return Json(UserAction.Find(Id), JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult CreateTeacher()
        {
            if ((string)Session["role"] == "Admin")
            {
                var teacher = UserAction.GetAll();
                List<User> lst_user = new List<User>();
                for (int i = 0; i < teacher.Count; i++)
                {
                    if (teacher[i].role == "Teacher" && TeacherAction.Find(teacher[i].id_number) == null)
                    {
                        lst_user.Add(teacher[i]);
                    }
                }
                ViewBag.Account = lst_user;
                ViewBag.Nganh = MajorAction.GetAll();
                ViewBag.Lop = ClassAction.GetAll();
                ViewBag.GV = TeacherAction.ShowAll();
                return View();
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }

        [HttpPost]
        public ActionResult CreateTeacher(string Id, string First_Name, string Last_Name, string Email, string Phone, string Address, string Date_Of_Birth, string Degree, string Major_Name)
        {
            TeacherAction.Add_Teacher(Id, First_Name, Last_Name, Email, Phone, Date_Of_Birth, Address, Degree, Major_Name);
            return Redirect("~/Member/CreateTeacher");
        }


        [HttpGet]
        public ActionResult CreateStudent()
        {
            if ((string)Session["role"] == "Admin")
            {
                var student = UserAction.GetAll();
                List<User> lst_student = new List<User>();
                for (int i = 0; i < student.Count; i++)
                {
                    if (student[i].role == "Student" && StudentAction.Find(student[i].id_number) == null)
                    {
                        lst_student.Add(student[i]);
                    }
                }
                ViewBag.Account = lst_student;
                ViewBag.Nganh = MajorAction.GetAll();
                ViewBag.ChuyenNganh = SpecializedAction.GetAll();
                ViewBag.Lop = ClassAction.GetAll();
                ViewBag.SV = StudentAction.ShowAll();
                return View();
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }

        [HttpPost]
        public ActionResult CreateStudent(string Id, string First_Name, string Last_Name, string Email, string Phone, string Address, string Date_Of_Birth, string Major_Name, string Specialized_Name ,string Class_Name)
        {
            StudentAction.Add_Student(Id, First_Name, Last_Name, Email, Phone, Date_Of_Birth, Address, Major_Name, Specialized_Name ,Class_Name);
            return Redirect("~/Member/CreateStudent");
        }

        [HttpGet]
        public ActionResult CreateGraduate(string Id, string Level, string Date_Sign, string Id_Student)
        {
            GraduateAction.Add_Graduate(Id, Level, Date_Sign, Id_Student);
            return View();
        }

        public JsonResult GetAllSubject(string Id)
        {
            return Json(SubjectAction.Find(Id), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreateTranscript()
        {
            if ((string)Session["role"] == "Admin")
            {
                ViewBag.SV = StudentAction.GetAll();
                ViewBag.Mh = SubjectAction.GetAll();
                ViewBag.Mark = RegSectorSubjectAction.ShowAll();
                return View();
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }

        [HttpPost]
        public ActionResult CreateTranscript(string Id, double Mark_Theory, double Mark_Practice, double Mark_Average, string Subject_Name, string Student_Id)
        {
            RegSectorSubjectAction.Add_RegSectorSubject(Id, Mark_Theory, Mark_Practice, Mark_Average, Subject_Name, Student_Id);
            return View();
        }

        public JsonResult GetUser(string Id)
        {
            return Json(UserAction.Find(Id), JsonRequestBehavior.AllowGet);
        }


    }
}