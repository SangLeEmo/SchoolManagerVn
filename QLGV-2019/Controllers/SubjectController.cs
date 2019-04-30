using QLGV_2019.Action;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLGV_2019.Controllers
{
    public class SubjectController : Controller
    {
        // GET: Subject
        public ActionResult Index()
        {
            ViewBag.YearTerm = YearAction.ShowAll();
            return View();
        }

        [HttpGet]
        public ActionResult YearTerm()
        {
            var client = ConnectNeo4J.Connection();
            client.Cypher.Match("(a:YearTerm),(b:School_Year)").Where("a.year_term = {Year_Term}").WithParam("Year_Term", 2).AndWhere("b.school_year = {school_year}").WithParam("school_year", 2019).Create("(a)-[:BelongTo]->(b)").ExecuteWithoutResultsAsync().Wait();
            ViewBag.YearTerm = YearAction.ShowAll();
            return View();
        }

        [HttpPost]
        public ActionResult YearTerm(int School_Year, int Year_Term, string Day_Start, string Day_End)
        {
            YearTermAction.Add_Year_Term(School_Year, Year_Term, Day_Start, Day_End);
            return Redirect("~/Home/Index");
        }


        [HttpGet]
        public ActionResult CreateSector()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CreateSector(int Numb_Student)
        {
            SectorSubjectAction.Add_SectorSubject(Numb_Student);
            return View();
        }


        [HttpGet]
        public ActionResult CreateClass()
        {
            ViewBag.nganh = SpecializedAction.GetAll();
            ViewBag.Lop = ClassAction.ShowAll();
            return View();
        }

        [HttpPost]
        public ActionResult CreateClass(string Id, string Name, int Numb_Student, string Name_Specialized)
        {
            ClassAction.Add_Class(Id, Name, Numb_Student, Name_Specialized);
            return Redirect("~/Subject/CreateClass");
        }

        [HttpGet]
        public ActionResult CreateSubject()
        {
            //ViewBag.Numb = SectorSubjectAction.ShowAll();
            //ViewBag.Class = ClassAction.ShowAll();
            ViewBag.ChuyenNganh = SpecializedAction.GetAll();
            ViewBag.Nganh = MajorAction.GetAll();
            ViewBag.Lop = ClassAction.GetAll();
            ViewBag.MH = SubjectAction.ShowAll();
            ViewBag.GV = TeacherAction.GetAll();
            return View();
        }


        [HttpPost]
        public ActionResult CreateSubject(string Id,string Sub_name, int Numb_Credit, int Numb_Theory, int Numb_Practice, double Theory_Percent, int Numb_Student, string Specialized_Name, string Class_Name, string Teacher_Name)
        {
            if(SubjectAction.Find(Id) != null)
            {
                SubjectAction.CreateRelate(Id, Class_Name, Teacher_Name);
            }
            else
            {
                SubjectAction.Add_Subject(Id, Sub_name, Numb_Credit, Numb_Theory, Numb_Practice, Theory_Percent, Numb_Student, Specialized_Name, Class_Name, Teacher_Name);
            }
            return Redirect("~/Subject/CreateSubject");
        }

        [HttpGet]
        public ActionResult CreateSpecialized()
        {
            ViewBag.Nganh = MajorAction.GetAll();
            ViewBag.Node = SpecializedAction.ShowAll();
            return View();
        }

        [HttpPost]
        public ActionResult CreateSpecialized(string Id, string Specialized_Name, string Name_Class, string Subject_Name, string Major_Name)
        {
            SpecializedAction.Add_Specialized(Id, Specialized_Name, Name_Class, Subject_Name, Major_Name);
            return Redirect("~/Subject/CreateSpecialized");
        }

        [HttpGet]
        public ActionResult CreateMajor()
        {
            return View();
        }

        public ActionResult CreateMajor(string Major_Name)
        {
            MajorAction.Add_Major(Major_Name);
            return Redirect("~/Subject/CreateSpecialized");
        }


    }
}