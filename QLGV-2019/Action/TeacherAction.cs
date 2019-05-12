using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class TeacherAction
    {
        /* CREATE */
        public static void Add_Teacher(string Id, string First_Name, string Last_Name, string Email, string Phone, string Date_Of_Birth, string Address, string Degree, string Major_Name)
        {
            var client = ConnectNeo4J.Connection();
            var student = new Teacher { id = Id, first_name = First_Name, last_name = Last_Name, email = Email, phone = Phone, date_of_birth = Date_Of_Birth, address = Address, degree = Degree };
            client.Cypher.Create("(:Teacher {n})").WithParam("n", student).ExecuteWithoutResultsAsync().Wait();
            client.Cypher.Match("(a:Teacher)", "(b:User)").
                Where((Teacher a) => a.id == Id).
                AndWhere((User b) => b.id_number == Id).
                Create("(a)-[:Owned]->(b)").ExecuteWithoutResults();
            client.Cypher.Match("(a:Teacher)", "(b:Major)").
                Where((Teacher a) => a.id == Id).
                AndWhere((Major b) => b.name == Major_Name).
                Create("(a)<-[:Teacher_In_Major]-(b)").ExecuteWithoutResults();
        }

        /* UPDATE */
        public static void Update_Teacher(string Id, string First_Name, string Last_Name, string Email, string Phone, string Date_Of_Birth, string Address, string Degree ,string Status)
        {
            var client = ConnectNeo4J.Connection();
            var tmp = new Teacher { id = Id, first_name = First_Name, last_name = Last_Name, email = Email, phone = Phone, date_of_birth = Date_Of_Birth, address = Address, degree = Degree ,status = Status };
            client.Cypher.Match("(n:Teacher)").Where((Teacher n) => n.id == Id).Set("n = {a}").WithParam("a", tmp).ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        public static Teacher Find(string Id)
        {
            var client = ConnectNeo4J.Connection();
            var lop = client.Cypher.Match("(a:Teacher)").Where("a.id = {ID}").WithParam("ID", Id).Return<Teacher>("a").Results.SingleOrDefault();
            return lop;
        }

        /* GET ALL */
        public static List<Teacher> GetAll()
        {
            var client = ConnectNeo4J.Connection();
            var lop = client.Cypher.Match("(n:Teacher)").Return<Teacher>("n").Results.ToList();
            return lop;
        }



        public static List<Tuple<Teacher, Major>> ShowAll()
        {
            List<Tuple<Teacher, Major>> lst = new List<Tuple<Teacher,  Major>>();
            Tuple<Teacher,  Major> tup;
            var client = ConnectNeo4J.Connection();
            var tmp = client.Cypher.Match("(a:Teacher)<-[:Teacher_In_Major]-(b:Major)").
                Return((a, b) => new {
                    Teacher = a.As<Teacher>(),
                    Major = b.As<Major>()
                }).Results;
            
            foreach(var item in tmp)
            {
                tup = new Tuple<Teacher, Major>(item.Teacher, item.Major);
                lst.Add(tup);
            }
            return lst;
        }



    }

}