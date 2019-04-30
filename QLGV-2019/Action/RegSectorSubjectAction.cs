using Neo4jClient;
using Newtonsoft.Json.Serialization;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class RegSectorSubjectAction
    {
        /* CREATE */
        public static void Add_RegSectorSubject(string Id, double Mark_Theory, double Mark_Practice, double Mark_Average, string Subject_Name, string Student_Id)
        {
            var client = ConnectNeo4J.Connection();
            var reg_suject = new RegSectorSubject {id = Id, mark_theory = Mark_Theory, mark_practice = Mark_Practice, mark_average = Mark_Average, isDelete = false };
            client.Cypher.Create("(:RegSectorSubject {reg_subject})").WithParam("reg_subject", reg_suject).ExecuteWithoutResultsAsync().Wait();
            client.Cypher.Match("(a:RegSectorSubject)", "(b:Subject)", "(c:Student)").
                Where((RegSectorSubject a) => a.id == Id).
                AndWhere((Subject b) => b.sub_name == Subject_Name).
                AndWhere((Student c) => c.id == Student_Id).
                Create("(b)-[:Subject_RegSectorSubject]->(a)<-[:RegSectorSubject_Student]-(c)").ExecuteWithoutResults();
        }

        /* UPDATE */
        public static void Update_RegSectorSubject(string Id, double Mark_Theory, double Mark_Practice, double Mark_Average)
        {
            var client = ConnectNeo4J.Connection();
            var term = new RegSectorSubject { id = Id, mark_theory = Mark_Theory, mark_practice = Mark_Practice, mark_average = Mark_Practice };
            client.Cypher.Match("(a:RegSectorSubject)")
                .Where((RegSectorSubject item) => item.id == Id)
                .Set("a = {tmp}")
                .WithParam("tmp", term)
                .ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        //public static RegSectorSubject Find(string Id)
        //{
        //    var client = ConnectNeo4J.Connection();
        //    var term = client.Cypher.Match("(a:RegSectorSubject)").Where("a.id = {ID}").WithParam("ID", Id).Return<RegSectorSubject>("a").Results.SingleOrDefault();
        //    return term;
        //}

        /* GET ALL */
        public static List<RegSectorSubject> GetAll()
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:RegSectorSubject)").Return<RegSectorSubject>("a").Results.ToList();
            return term;
        }



        public static List<Tuple<RegSectorSubject, Subject, Student>> ShowAll()
        {
            List<Tuple<RegSectorSubject, Subject, Student>> lst = new List<Tuple<RegSectorSubject, Subject, Student>>();
            Tuple<RegSectorSubject, Subject, Student> tup;
            var client = ConnectNeo4J.Connection();
            var tmp = client.Cypher.
                Match("(b)-[:Subject_RegSectorSubject]->(a)<-[:RegSectorSubject_Student]-(c)").
                Return((a, b, c) => new {
                    RegSectorSubject = a.As<RegSectorSubject>(),
                    Subject = b.As<Subject>(),
                    Student = c.As<Student>()
                }).Results;

            foreach(var item in tmp)
            {
                tup = new Tuple<RegSectorSubject, Subject, Student>(item.RegSectorSubject, item.Subject, item.Student);
                lst.Add(tup);
            }
            return lst;
        }



    }
}