using Neo4jClient;
using Newtonsoft.Json.Serialization;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class GraduateAction
    {
        /* CREATE */
        public static void Add_Graduate(string Id, string Level, string Date_Sign, string Id_Student)
        {
            var client = ConnectNeo4J.Connection();
            var graduate = new Graduate { id  = Id, level = Level, date_sign = Date_Sign, isDelete = false};
            client.Cypher.Create("(:Graduate {graduate})").WithParam("graduate", graduate).ExecuteWithoutResultsAsync().Wait();
            client.Cypher.Match("(a:Graduate)", "(b:Student)").
                Where((Graduate a) => a.id == Id).
                AndWhere((Student b) => b.id == Id_Student).
                Create("(a)-[:Pass]->(b)").ExecuteWithoutResults();
        }

        /* UPDATE */
        public static void Update_Graduate(string Id, string Level, string Date_Sign)
        {
            var client = ConnectNeo4J.Connection();
            var graduate = new Graduate { id = Id, level = Level, date_sign = Date_Sign };
            client.Cypher.Match("(a:Graduate)")
                .Where((Graduate item) => item.id == Id)
                .Set("a = {tmp}")
                .WithParam("tmp", graduate)
                .ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        public static Graduate Find(string Id)
        {
            var client = ConnectNeo4J.Connection();
            var graduate = client.Cypher.Match("(a:Graduate)").Where("a.id = {ID}").WithParam("ID", Id).Return<Graduate>("a").Results.SingleOrDefault();
            return graduate;
        }

        /* GET ALL */
        public static List<Graduate> ShowAll()
        {
            var client = ConnectNeo4J.Connection();
            var graduate = client.Cypher.Match("(a:Graduate)").Return<Graduate>("a").Results.ToList();
            return graduate;
        }



    }
}