using Neo4jClient;
using Newtonsoft.Json.Serialization;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class SpecializedAction
    {
        /* CREATE */
        public static void Add_Specialized(string Id, string Name, string Name_Class, string Subject_Name, string Major_Name)
        {
            var client = ConnectNeo4J.Connection();
            var specialized = new Specialized {id = Id, name = Name, isDelete = false };
            client.Cypher.Create("(:Specialized {specialized})").WithParam("specialized", specialized).ExecuteWithoutResultsAsync().Wait();
            //client.Cypher.Match("(a:Specialized)", "(b:Class)").
            //    Where((Specialized a) => a.name == Name).
            //    AndWhere((Class b) => b.name == Name_Class).
            //    Create("(a)<-[:In_Specialized]-(b)").ExecuteWithoutResults();
            client.Cypher.Match("(a:Specialized)", "(b:Major)").
                Where((Specialized a) => a.name == Name).
                AndWhere((Major b) => b.name == Major_Name).
                Create("(b)-[:Belong_Specialized]->(a)").
                ExecuteWithoutResults();
        }

        /* UPDATE */
        public static void Update_Specialized(string Id, string Name)
        {
            var client = ConnectNeo4J.Connection();
            var term = new Specialized { id = Id,name = Name };
            client.Cypher.Match("(a:Specialized)")
                .Where((Specialized item) => item.id == Id)
                .Set("a = {tmp}")
                .WithParam("tmp", term)
                .ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        public static Specialized Find(string Id)
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Specialized)").Where("a.id = {ID}").WithParam("ID", Id).Return<Specialized>("a").Results.SingleOrDefault();
            return term;
        }

        /* GET ALL */
        //public static List<Specialized> ShowAll()
        //{
        //    var client = ConnectNeo4J.Connection();
        //    var term = client.Cypher.Match("(a:Specialized)").Return<Specialized>("a").Results.ToList();
        //    return term;
        //}

        public static Dictionary<Specialized, Major> ShowAll()
        {
            var client = ConnectNeo4J.Connection();
            Dictionary<Specialized, Major> dict = new Dictionary<Specialized, Major>();
            var tmp = client.Cypher.Match("(a)-[:Belong_Specialized]->(b)").
                 Return((a, b) => new {
                     Major = a.As<Major>(),
                     Specialized = b.As<Specialized>()

                 });
            foreach (var item in tmp.Results)
            {
                dict.Add(item.Specialized, item.Major);
            }
            return dict;
        }


        public static List<Specialized> GetAll()
        {
            var client = ConnectNeo4J.Connection();
            var tmp = client.Cypher.Match("(a:Specialized)").Return<Specialized>("a").Results.ToList();
            return tmp;
        }


        public static List<Tuple<Major, Specialized>> Get_Major_Specialized()
        {
            List<Tuple<Major, Specialized>> lst = new List<Tuple<Major, Specialized>>();
            Tuple<Major, Specialized> tup;
            var client = ConnectNeo4J.Connection();
            var tmp = client.Cypher.Match("(a)-[:Belong_Specialized]->(b)").
                Return((a, b) => new {
                    Major = a.As<Major>(),
                    Specialized = b.As<Specialized>()
                }).Results;
            foreach(var item in tmp)
            {
                tup = new Tuple<Major, Specialized>(item.Major, item.Specialized);
                lst.Add(tup);
            }
            return lst;

        }



    }
}