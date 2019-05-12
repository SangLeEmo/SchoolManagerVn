using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class StudentAction
    {
        /* CREATE */
        public static void Add_Student(string Id, string First_Name, string Last_Name, string Email, string Phone, string Date_Of_Birth, string Address, string Major_Name, string Specialized_Name ,string Class_Name)
        {
            var client = ConnectNeo4J.Connection();
            var student = new Student { id = Id, first_name = First_Name, last_name = Last_Name, email = Email, phone = Phone, date_of_birth = Date_Of_Birth, address = Address};
            client.Cypher.Create("(:Student {n})").WithParam("n", student).ExecuteWithoutResultsAsync().Wait();
            client.Cypher.Match("(a:Student)", "(b:User)").
                Where((Student a) => a.id == Id).
                AndWhere((User b) => b.id_number == Id).
                Create("(a)-[:Was_Create]->(b)").ExecuteWithoutResults();
            client.Cypher.Match("(a:Student)", "(b:Major)", "(c:Class)").
                Where((Student a) => a.id == Id).
                AndWhere((Major b) => b.name == Major_Name).
                AndWhere((Class c) => c.id == Class_Name).
                Create("(c)-[:Student_Class]->(a)<-[:Student_In_Major]-(b)").ExecuteWithoutResults();
        }

        /* UPDATE */
        public static void Update_Student(string Id, string First_Name, string Last_Name, string Email, string Phone, string Date_Of_Birth, string Address, string Status)
        {
            var client = ConnectNeo4J.Connection();
            var tmp = new Student { id = Id, first_name = First_Name, last_name = Last_Name, email = Email, phone = Phone, date_of_birth = Date_Of_Birth, address = Address, status = Status };
            client.Cypher.Match("(n:Student)").Where((Student n) => n.id == Id).Set("n = {a}").WithParam("a", tmp).ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        public static Student Find(string Id)
        {
            var client = ConnectNeo4J.Connection();
            var lop = client.Cypher.Match("(a:Student)").Where("a.id = {ID}").WithParam("ID", Id).Return<Student>("a").Results.SingleOrDefault();
            return lop;
        }

        /* GET ALL */
        public static List<Student> GetAll()
        {
            var client = ConnectNeo4J.Connection();
            var lop = client.Cypher.Match("(n:Student)").Return<Student>("n").Results.ToList();
            return lop;
        }


        public static List<Tuple<Student, Major, Specialized, Class>> ShowAll()
        {
            List<Tuple<Student, Major, Specialized, Class>> lst = new List<Tuple<Student, Major, Specialized, Class>>();
            Tuple<Student, Major, Specialized, Class> tup;
            var client = ConnectNeo4J.Connection();
            var tmp1 = client.Cypher.Match("(a)-[:Student_In_Major]->(b)<-[:Student_Class]-(c)<-[:Class_In_Specialized]-(d)").
                Return((a,b,c,d) => new {
                    Major = a.As<Major>(),
                    Student = b.As<Student>(),
                    Class = c.As<Class>(),
                    Specialized = d.As<Specialized>()
                }).Results;

            foreach(var item in tmp1)
            {
                tup = new Tuple<Student, Major, Specialized, Class>(item.Student, item.Major, item.Specialized, item.Class);
                lst.Add(tup);
            }

            return lst;
        }


        public static void CreateRelate(string Id_Student, string Id_Subject)
        {
            var client = ConnectNeo4J.Connection();
            client.Cypher.Match("(a:Student)", "(b:Subject)").
                Where((Student a) => a.id == Id_Student).
                AndWhere((Subject b) => b.id == Id_Subject).
                Create("(a)<-[:Student_Subject]-(b)").ExecuteWithoutResults();
        }

        public static List<Tuple<RegisterSubject, Student>> GetAllSubject()
        {
            List<Tuple<RegisterSubject, Student>> lst = new List<Tuple<Models.RegisterSubject, Models.Student>>();
            Tuple<RegisterSubject, Student> tup;
            var client = ConnectNeo4J.Connection();
            var tmp = client.Cypher.Match("(a:Student)<-[:Student_Regist_Subject]-(b)").
                Return((a, b) => new {
                    Student = a.As<Student>(),
                    RegisterSubject = b.As<RegisterSubject>()
                }).Results;
            foreach(var item in tmp)
            {
                tup = new Tuple<RegisterSubject, Student>(item.RegisterSubject, item.Student);
                lst.Add(tup);
            }
            return lst;
        }


    }
}