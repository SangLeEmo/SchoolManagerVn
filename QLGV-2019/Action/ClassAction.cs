using Neo4jClient;
using Newtonsoft.Json.Serialization;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class ClassAction
    {
        /* CREATE */
        public static void Add_Class(string Id, string Name, int Numb_Student, string Specialized_Name)
        {
            var client = ConnectNeo4J.Connection();
            var lop = new Class { id = Id, name = Name, numb_student = Numb_Student ,isDelete = false };
            client.Cypher.Create("(:Class {lop})").WithParam("lop", lop).ExecuteWithoutResultsAsync().Wait();
            client.Cypher.Match("(a:Class)", "(b:Specialized)").
                Where((Class a) => a.id == Id).
                AndWhere((Specialized b) => b.name == Specialized_Name).
                Create("(b)-[:Class_In_Specialized]->(a)").ExecuteWithoutResults();

        }

        /* UPDATE */
        public static void Update_Class(string Id, string Name, int Numb_Student)
        {
            var client = ConnectNeo4J.Connection();
            var tmp = new Class { id = Id, name = Name, numb_student = Numb_Student };
            client.Cypher.Match("(lop:Class)").Where((Class lop) => lop.id == Id).Set("lop = {a}").WithParam("a", tmp).ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        public static Class Find(string Id)
        {
            var client = ConnectNeo4J.Connection();
            var lop = client.Cypher.Match("(a:Class)").Where("a.id = {ID}").WithParam("ID", Id).Return<Class>("a").Results.SingleOrDefault();
            return lop;
        }

        /* GET ALL */
        public static Dictionary<Class,Specialized> ShowAll()
        {
            Dictionary<Class, Specialized> dict = new Dictionary<Class, Specialized>();
            var client = ConnectNeo4J.Connection();
            var lop = client.Cypher.Match("(a:Class)<-[:Class_In_Specialized]-(b:Specialized)").
                Return((a, b) => new {
                    Class = a.As<Class>(),
                    Specialized = b.As<Specialized>()
                }).Results;
            foreach(var item in lop)
            {
                dict.Add(item.Class, item.Specialized);
            }
            return dict;
        }

        public static List<Class> GetAll()
        {
            var client = ConnectNeo4J.Connection();
            var tmp = client.Cypher.Match("(a:Class)").Return<Class>("a").Results.ToList();
            return tmp;
        }



    }
}