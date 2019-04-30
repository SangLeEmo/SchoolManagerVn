using Neo4jClient;
using Newtonsoft.Json.Serialization;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class MajorAction
    {
        /* CREATE */
        public static void Add_Major( string Name)
        {
            var client = ConnectNeo4J.Connection();
            var major = new Major { name = Name, isDelete = false };
            client.Cypher.Create("(:Major {major})").WithParam("major", major).ExecuteWithoutResultsAsync().Wait();

        }


        /* UPDATE */
        public static void Update_Major( string Name)
        {
            var client = ConnectNeo4J.Connection();
            var term = new Major {  name = Name };
            client.Cypher.Match("(a:Major)")
                .Where((Major item) => item.name == Name)
                .Set("a = {tmp}")
                .WithParam("tmp", term)
                .ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        public static Major Find(string Id)
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Major)").Where("a.name = {ID}").WithParam("ID", Id).Return<Major>("a").Results.SingleOrDefault();
            return term;
        }



        /* GET ALL */
        public static List<Major> GetAll()
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Major)").Return<Major>("a").Results.ToList();
            return term;
        }

    }
}