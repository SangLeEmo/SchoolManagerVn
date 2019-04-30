using Neo4jClient;
using Newtonsoft.Json.Serialization;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class Rule1Action
    {
        /* CREATE */
        public static void Add_Rule_1(int Id, int Major_Max, int Special_Major_Max)
        {
            var client = ConnectNeo4J.Connection();
            var rule = new Rule1 {id = Id, major_max = Major_Max, special_major_max = Special_Major_Max, isDelete = false };
            client.Cypher.Create("(:Rule_1 {rule})").WithParam("rule", rule).ExecuteWithoutResultsAsync().Wait();

        }

        /* UPDATE */
        public static void Update_Rule_1(int Id, int Major_Max, int Special_Major_Max)
        {
            var client = ConnectNeo4J.Connection();
            var term = new Rule1 { id = Id, major_max = Major_Max, special_major_max = Special_Major_Max };
            client.Cypher.Match("(a:Rule_1)")
                .Where((Rule1 item) => item.id == Id)
                .Set("a = {tmp}")
                .WithParam("tmp", term)
                .ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        public static Rule1 Find(int Id)
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Rule_1)").Where("a.id = {ID}").WithParam("ID", Id).Return<Rule1>("a").Results.SingleOrDefault();
            return term;
        }

        /* GET ALL */
        public static List<Rule1> ShowAll()
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Rule_1)").Return<Rule1>("a").Results.ToList();
            return term;
        }


    }
}