using Neo4jClient;
using Newtonsoft.Json.Serialization;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class DegreeAction
    {

        /* CREATE */
        public static void Add_Degree(int Id, string Name)
        {
            var client = ConnectNeo4J.Connection();
            var degree = new Degree {id = Id, name = Name, isDelete = false };
            client.Cypher.Create("(:Degree {degree})").WithParam("degree", degree).ExecuteWithoutResultsAsync().Wait();

        }

        /* UPDATE */
        public static void Update_Degree(int Id, string Name)
        {
            var client = ConnectNeo4J.Connection();
            var degree = new Degree { id = Id, name = Name };
            client.Cypher.Match("(a:Degree)")
                .Where((Degree item) => item.id == Id)
                .Set("a = {tmp}")
                .WithParam("tmp", degree)
                .ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        public static Degree Find(int Id)
        {
            var client = ConnectNeo4J.Connection();
            var degree = client.Cypher.Match("(a:Degree)").Where("a.id = {ID}").WithParam("ID", Id).Return<Degree>("a").Results.SingleOrDefault();
            return degree;
        }

        /* GET ALL */
        public static List<Degree> ShowAll()
        {
            var client = ConnectNeo4J.Connection();
            var degree = client.Cypher.Match("(a:Degree)").Return<Degree>("a").Results.ToList();
            return degree;
        }


    }
}