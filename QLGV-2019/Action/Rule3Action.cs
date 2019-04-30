using Neo4jClient;
using Newtonsoft.Json.Serialization;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class Rule3Action
    {
        /* CREATE */
        public static void Add_Rule_3(int Id, int Max_Subject)
        {
            var client = ConnectNeo4J.Connection();
            var rule = new Rule3 {id = Id, max_subject = Max_Subject, isDelete = false };
            client.Cypher.Create("(:Rule_3 {rule})").WithParam("rule", rule).ExecuteWithoutResultsAsync().Wait();

        }

        /* UPDATE */
        public static void Update_Rule_3(int Id, int Max_Subject)
        {
            var client = ConnectNeo4J.Connection();
            var term = new Rule3 { id = Id, max_subject = Max_Subject};
            client.Cypher.Match("(a:Rule_3)")
                .Where((Rule3 item) => item.id == Id)
                .Set("a = {tmp}")
                .WithParam("tmp", term)
                .ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        public static Rule3 Find(int Id)
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Rule_3)").Where("a.id = {ID}").WithParam("ID", Id).Return<Rule3>("a").Results.SingleOrDefault();
            return term;
        }

        /* GET ALL */
        public static List<Rule3> ShowAll()
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Rule_3)").Return<Rule3>("a").Results.ToList();
            return term;
        }



    }
}