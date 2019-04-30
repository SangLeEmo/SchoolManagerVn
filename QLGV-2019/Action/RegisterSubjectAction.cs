using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class RegisterSubjectAction
    {
        /* CREATE  */
        public static void Regist_Subject(string Id_Subject, string Id_Class, string Id_Teacher, string Id_Student)
        {
            var client = ConnectNeo4J.Connection();
            var mh = new RegisterSubject { id_subject = Id_Subject, id_class = Id_Class, id_teacher = Id_Teacher};
            client.Cypher.Create("(:RegisterSubject {mh})").WithParam("mh", mh).ExecuteWithoutResultsAsync().Wait();
            client.Cypher.Match("(a:Student)", "(b:RegisterSubject)").
                Where((Student a) => a.id == Id_Student).
                AndWhere((RegisterSubject b) => b.id_subject == Id_Subject).
                Create("(a)<-[:Student_Regist_Subject]-(b)").ExecuteWithoutResults();
            client.Dispose();
        }

        /* UPDATE */
        public static void Update_Regist(string Id_Subject, string Id_Class, string Id_Teacher, string Id_Student)
        {
            var client = ConnectNeo4J.Connection();
            var mh = new RegisterSubject { id_subject = Id_Subject, id_class = Id_Class, id_teacher = Id_Teacher, id_student = Id_Student};
            client.Cypher.Match("(a:RegisterSubject)").Where((RegisterSubject a) => a.id_subject == Id_Subject).Set("a = {mh}").WithParam("mh", mh).ExecuteWithoutResults();
            client.Dispose();
        }


        /* SEARCH*/
        public static RegisterSubject Find(string Id_subject, string Id_Class)
        {
            var client = ConnectNeo4J.Connection();
            var tmp = client.Cypher.Match("(a:Registersubject)").
                Where((RegisterSubject a) => a.id_subject == Id_subject && a.id_class == Id_Class).Return<RegisterSubject>("a").Results.FirstOrDefault();
            client.Dispose();
            return tmp;
        }


        /* GET ALL */
        public static List<Tuple<Student, RegisterSubject>> ShowAll()
        {
            List<Tuple<Student, RegisterSubject>> lst = new List<Tuple<Student, RegisterSubject>>();
            Tuple<Student,RegisterSubject> tup;
            var client = ConnectNeo4J.Connection();
            var tmp = client.Cypher.Match("(a:Student)<-[:Student_Regist_Subject]-(b:RegisterSubject)").
                Return((a, b) => new {
                    Student = a.As<Student>(),
                    RegisterSubject = b.As<RegisterSubject>()
                }).Results;

            foreach(var item in tmp)
            {
                tup = new Tuple<Student, RegisterSubject>(item.Student, item.RegisterSubject);
                lst.Add(tup);
            }
            client.Dispose();
            return lst;
        }



        /* MAKE RELATION */
        public static void CreateRelate(string Id_Subject, string Id_Class ,string Id_Student)
        {
            var client = ConnectNeo4J.Connection();
            client.Cypher.Match("(a:Student)", "(b:RegisterSubject)").
                Where((Student a) => a.id == Id_Student).
                AndWhere((RegisterSubject b) => b.id_subject == Id_Subject && b.id_class == Id_Class).
                Create("(a)<-[:Student_Regist_Subject]-(b)").ExecuteWithoutResults();
            client.Dispose();
        }

    }
}