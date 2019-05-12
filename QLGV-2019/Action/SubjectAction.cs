using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo4jClient;
using Newtonsoft.Json.Serialization;
using QLGV_2019.Models;

namespace QLGV_2019.Action
{
    public class SubjectAction
    {
        /* CREATE */
        public static void Add_Subject(string Id,string Sub_name, int Numb_Credit, int Numb_Theory, int Numb_Practice, double Theory_Percent, int Numb_Student, string Class_Name, string Teacher_Name, int Year_Term)
        {
            var client = ConnectNeo4J.Connection();
            var subject = new Subject {id = Id, sub_name = Sub_name, numb_credit = Numb_Credit, numb_theory = Numb_Theory, numb_practice = Numb_Practice, theory_percent = Theory_Percent, numb_student = Numb_Student ,isDelete = false };
            client.Cypher.Create("(:Subject {subject})").WithParam("subject", subject).ExecuteWithoutResultsAsync().Wait();
            client.Cypher.Match("(a:Subject)", "(b:YearTerm)").Where((Subject a) => a.id == Id).
                AndWhere((YearTerm b) =>  b.year_term == Year_Term).
                Create("(a)<-[:Subject_YearTerm]-(b)").ExecuteWithoutResults();
            RegisterSubjectAction.Regist_Subject(Id, Class_Name, Teacher_Name);
        }


        /* UPDATE */
        public static void Update_Subject(string Id,string Sub_name, int Numb_Credit, int Numb_Theory, int Numb_Practice, double Theory_Percent)
        {
            var client = ConnectNeo4J.Connection();
            var term = new Subject {id = Id, sub_name = Sub_name, numb_credit = Numb_Credit, numb_theory = Numb_Theory, numb_practice = Numb_Practice, theory_percent = Theory_Percent };
            client.Cypher.Match("(a:Subject)")
                .Where((Subject item) => item.id == Id)
                .Set("a = {tmp}")
                .WithParam("tmp", term)
                .ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        public static Subject Find(string Id)
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Subject)").Where("a.id = {ID}").WithParam("ID", Id).Return<Subject>("a").Results.SingleOrDefault();
            return term;
        }

        /* GET ALL */
        public static List<Subject> GetAll()
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Subject)").Return<Subject>("a").Results.ToList();
            return term;
        }


        public static List<Tuple<Subject, YearTerm, RegisterSubject, Class, Teacher>> ShowAll()
        {

            List<Tuple<Subject, YearTerm, RegisterSubject, Class, Teacher>> lst_subject = new List<Tuple<Subject, YearTerm, RegisterSubject, Class, Teacher>>();
            Tuple<Subject, YearTerm, RegisterSubject, Class, Teacher> tup_subject;
            var client = ConnectNeo4J.Connection();
            var tmp = client.Cypher.Match("(a)<-[:Subject_YearTerm]-(b)", "(c)-[:Subject_Regist]->(d:RegisterSubject)").
                Return((a,b,d) => new {
                    Subject = a.As<Subject>(),
                    YearTerm = b.As<YearTerm>(),
                    RegisterSubject = d.As<RegisterSubject>()

                }).Results;
            foreach(var item in tmp)
            {
                tup_subject = new Tuple<Subject, YearTerm, RegisterSubject, Class, Teacher>(item.Subject, item.YearTerm, item.RegisterSubject, ClassAction.Find(item.RegisterSubject.id_class), TeacherAction.Find(item.RegisterSubject.id_teacher));
                lst_subject.Add(tup_subject);
            }

            return lst_subject;

        }


        public static void CreateRelate(string Id,string Class_Name, string Teacher_Name, int School_Year)
        {
            //var client = ConnectNeo4J.Connection();
            //client.Cypher.Match("(a:Subject)", "(b:Class)").
            //    Where((Subject a) => a.id == Id).
            //    AndWhere((Class b) => b.id == Class_Name).
            //    Create("(a)<-[:Subject_Class]-(b)").ExecuteWithoutResults();
            //client.Cypher.Match("(a:Class)", "(b:Teacher)").
            //   Where((Class a) => a.id == Class_Name).
            //   AndWhere((Teacher b) => b.id == Teacher_Name).
            //   Create("(a)<-[:Class_Teacher]-(b)").ExecuteWithoutResults();
            RegisterSubjectAction.Regist_Subject(Id, Class_Name, Teacher_Name);
        }


    }

}