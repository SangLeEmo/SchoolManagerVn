using QLGV_2019.Action;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLGV_2019.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RegisterSubject()
        {
            ViewBag.MH = SubjectAction.ShowAll();
            var dk = RegisterSubjectAction.ShowAll();
            var gv = TeacherAction.GetAll();
            var mh = SubjectAction.GetAll();
            List<Tuple<Student, RegisterSubject, string, string>> item = new List<Tuple<Student, Models.RegisterSubject, string, string>>();
            Tuple<Student, RegisterSubject, string, string> tup;
            for (int i = 0; i < dk.Count; i++)
            {
                string name_teacher = null;
                string name_subject = null;
                for(int j = 0; j < gv.Count; j++)
                {
                    if (dk[i].Item2.id_teacher == gv[j].id)
                    {
                        name_teacher = gv[j].last_name + " " + gv[j].first_name;
                        break;
                    }
                        
                }

                for(int k = 0; k < mh.Count; k++)
                {
                    if(dk[i].Item2.id_subject == mh[k].id)
                    {
                        name_subject = mh[k].sub_name;
                        break;
                    }
                }
                tup = new Tuple<Student, Models.RegisterSubject, string, string>(dk[i].Item1, dk[i].Item2, name_teacher, name_subject);
                item.Add(tup);
            }
            ViewBag.DK = item;
            return View();
        }

        [HttpPost]
        public ActionResult RegisterSubject(string Id_Class, string Id_Subject, string Id_Teacher)
        {
            if(RegisterSubjectAction.Find(Id_Subject, Id_Class) == null)
            {
                var Id_Student = Session["id"].ToString();
                RegisterSubjectAction.Regist_Subject(Id_Subject, Id_Class, Id_Teacher, Id_Student);
            }
            else
            {
                var Id_Student = Session["id"].ToString();
                RegisterSubjectAction.CreateRelate(Id_Subject, Id_Class, Id_Student);
            }
            return Redirect("~/Student/RegisterSubject");
        }

    }
}