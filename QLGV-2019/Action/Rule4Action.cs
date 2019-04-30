using Neo4jClient;
using Newtonsoft.Json.Serialization;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class Rule4Action
    {
        /* CREATE */
        public static void Add_Rule_4(int Id, int Min_Subject, int Max_Subject, double Old_Avg_Limit)
        {
            var client = ConnectNeo4J.Connection();
            var rule = new Rule4 {id = Id, min_subject = Min_Subject, max_subject = Max_Subject, old_avg_limit = Old_Avg_Limit, isDelete = false };
            client.Cypher.Create("(:Rule_4 {rule})").WithParam("rule", rule).ExecuteWithoutResultsAsync().Wait();
        }

        /* UPDATE */
        public static void Update_Rule_4(int Id, int Min_Subject, int Max_Subject, double Old_Avg_Limit)
        {
            var client = ConnectNeo4J.Connection();
            var term = new Rule4 { id = Id, min_subject = Min_Subject, max_subject = Max_Subject, old_avg_limit = Old_Avg_Limit };
            client.Cypher.Match("(a:Rule_4)")
                .Where((Rule4 item) => item.id == Id)
                .Set("a = {tmp}")
                .WithParam("tmp", term)
                .ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        public static Rule4 Find(int Id)
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Rule_4)").Where("a.id = {ID}").WithParam("ID", Id).Return<Rule4>("a").Results.SingleOrDefault();
            return term;
        }

        /* GET ALL */
        public static List<Rule4> ShowAll()
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Rule_4)").Return<Rule4>("a").Results.ToList();
            return term;
        }


    }
}