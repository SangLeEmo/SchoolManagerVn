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
        public static void Add_Subject(string Id,string Sub_name, int Numb_Credit, int Numb_Theory, int Numb_Practice, double Theory_Percent, int Numb_Student, string Specialized_Name,string Class_Name, string Teacher_Name)
        {
            var client = ConnectNeo4J.Connection();
            var subject = new Subject {id = Id, sub_name = Sub_name, numb_credit = Numb_Credit, numb_theory = Numb_Theory, numb_practice = Numb_Practice, theory_percent = Theory_Percent, numb_student = Numb_Student ,isDelete = false };
            client.Cypher.Create("(:Subject {subject})").WithParam("subject", subject).ExecuteWithoutResultsAsync().Wait();
            client.Cypher.Match("(a:Subject)", "(b:Specialized)", "(c:Class)").
                Where((Subject a) => a.id == Id).
                AndWhere((Specialized b) => b.name == Specialized_Name).
                AndWhere((Class c) => c.id == Class_Name).
                Create("(b)-[:Subject_Specialized]->(a)<-[:Subject_Class]-(c)").ExecuteWithoutResults();
            client.Cypher.Match("(a:Subject)", "(b:Teacher)").
                Where((Subject a) => a.id == Id).
                AndWhere((Teacher b) => b.id == Teacher_Name).
                Create("(a)-[:Subject_Teacher]->(b)").ExecuteWithoutResults();
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


        public static List<Tuple<Class, Subject, Specialized, Major, Teacher>> ShowAll()
        {
            List<Tuple<Class, Subject, Specialized, Major, Teacher>> lst = new List<Tuple<Class, Subject, Specialized, Major, Teacher>>();
            Tuple<Class, Subject, Specialized, Major, Teacher> tup;
            var client = ConnectNeo4J.Connection();
            var tmp = client.Cypher.
                Match("(a:Teacher)<-[:Subject_Teacher]-(b:Subject)<-[:Subject_Class]-(c:Class)<-[:Class_In_Specialized]-(d:Specialized)<-[:Belong_Specialized]-(e:Major)").
                Return((a, b, c, d, e) => new {
                    Class = c.As<Class>(),
                    Subject = b.As<Subject>(),
                    Specialized = d.As<Specialized>(),
                    Major = e.As<Major>(),
                    Teacher = a.As<Teacher>()
                }).Results;
            foreach (var item in tmp)
            {
                tup = new Tuple<Class, Subject, Specialized, Major, Teacher>(item.Class, item.Subject, item.Specialized, item.Major, item.Teacher);
                lst.Add(tup);
            }
            return lst;

        }


        public static void CreateRelate(string Id,string Class_Name, string Teacher_Name)
        {
            var client = ConnectNeo4J.Connection();
            client.Cypher.Match("(a:Subject)", "(b:Class)").
                Where((Subject a) => a.id == Id).
                AndWhere((Class b) => b.id == Class_Name).
                Create("(a)<-[:Subject_Class]-(c)").ExecuteWithoutResults();
            client.Cypher.Match("(a:Subject)", "(b:Teacher)").
               Where((Subject a) => a.id == Id).
               AndWhere((Teacher b) => b.id == Teacher_Name).
               Create("(a)-[:Subject_Teacher]->(b)").ExecuteWithoutResults();
        }


    }

}