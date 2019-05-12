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
            //ViewBag.MH = SubjectAction.ShowAll();
            if ((string)Session["role"] == "Student")
            {
                var tmp = SubjectAction.ShowAll();
                List<Tuple<RegisterSubject, Student>> subject = StudentAction.GetAllSubject();
                List<Tuple<Subject, YearTerm, Class, Teacher>> lst = (List<Tuple<Subject, YearTerm, Class, Teacher>>)Session["student_subject"];
                if (lst != null)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        tmp.RemoveAll(a => a.Item1.id == lst[i].Item1.id);
                    }
                }
                if(subject != null)
                {
                    for(int i = 0; i < subject.Count; i++)
                    {
                        tmp.RemoveAll(a => a.Item1.id == subject[i].Item1.id_subject);
                    }
                }
                ViewBag.DK = lst;
                ViewBag.Mh = tmp;
                return View();
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }

        [HttpPost]
        public ActionResult RegisterSubject(string Id_Class, string Id_Subject, string Id_Teacher, int Year_Term)
        {
            if(Session["student_subject"] == null)
            {
                var gv = TeacherAction.GetAll();
                var mh = SubjectAction.GetAll();
                var lop = ClassAction.GetAll();
                var nam = YearTermAction.GetAll();
                var teacher_info = gv.Find(a => a.id == Id_Teacher);
                var subject_infor = mh.Find(a => a.id == Id_Subject);
                var class_info = lop.Find(a => a.id == Id_Class);
                var year_info = nam.Find(a => a.year_term == Year_Term);
                List<Tuple<Subject, YearTerm, Class, Teacher>> lst = new List<Tuple<Subject, YearTerm, Class, Teacher>>();
                Tuple<Subject, YearTerm, Class, Teacher> tup = new Tuple<Subject, YearTerm, Class, Teacher>(subject_infor, year_info, class_info, teacher_info);
                lst.Add(tup);
                Session["student_subject"] = lst;
            }
            else
            {
                List<Tuple<Subject, YearTerm, Class, Teacher>> lst =  (List<Tuple<Subject, YearTerm, Class, Teacher>>)Session["student_subject"];
                var gv = TeacherAction.GetAll();
                var mh = SubjectAction.GetAll();
                var lop = ClassAction.GetAll();
                var nam = YearTermAction.GetAll();
                var teacher_info = gv.Find(a => a.id == Id_Teacher);
                var subject_infor = mh.Find(a => a.id == Id_Subject);
                var class_info = lop.Find(a => a.id == Id_Class);
                var year_info = nam.Find(a => a.year_term == Year_Term);
                Tuple<Subject, YearTerm, Class, Teacher> tup = new Tuple<Subject, YearTerm, Class, Teacher>(subject_infor, year_info, class_info, teacher_info);
                lst.Add(tup);
                Session["student_subject"] = lst;
            }
            
            return Redirect("~/Student/RegisterSubject");
        }

        /* REMOVE SPECIFIC SUBJECT */
        public ActionResult RemoveRegisterSubject(string Id_Class, string Id_Subject, string Id_Teacher)
        {
            
                List<Tuple<Subject, YearTerm, Class, Teacher>> lst = (List<Tuple<Subject, YearTerm, Class, Teacher>>)Session["student_subject"];
                var tmp = lst.Find(a => a.Item1.id == Id_Subject && a.Item4.id == Id_Teacher && a.Item3.id == Id_Class);
                lst.Remove(tmp);
                if (lst.Count > 0)
                    Session["student_subject"] = lst;
                else
                    Session["student_subject"] = null;

                return Redirect("~/Student/RegisterSubject");
        }

        /* REMOVE SUBJECT REGISTATION */
        public ActionResult RemoveAll()
        {
            List<Tuple<Subject, YearTerm, Class, Teacher>> lst = (List<Tuple<Subject, YearTerm, Class, Teacher>>)Session["student_subject"];
            lst.Clear();
            Session["student_subject"] = null;
            return Redirect("~/Student/RegisterSubject");


        }


        /* SAVE SUBJECT REGISTATION */
        public ActionResult SaveSubject()
        {
            List<Tuple<Subject, YearTerm, Class, Teacher>> lst = (List<Tuple<Subject, YearTerm, Class, Teacher>>)Session["student_subject"];
            for (int i = 0; i < lst.Count; i++)
            {
                RegisterSubjectAction.CreateRelate(lst[i].Item1.id, lst[i].Item3.id, (string)Session["id"]);
            }
            lst.Clear();
            Session["student_subject"] = null;
            return Redirect("~/Student/RegisterSubject");
        }

        public JsonResult CheckDuplicateSubject(string Id_Class, string Id_Subject)
        {
            List<Tuple<Class, Teacher, Subject>> lst = (List<Tuple<Class, Teacher, Subject>>)Session["student_subject"];
            var result = lst.Find(a => a.Item1.id == Id_Class && a.Item3.id == Id_Subject);
            return Json(result , JsonRequestBehavior.AllowGet);
        }



    }
}