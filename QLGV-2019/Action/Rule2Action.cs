using Neo4jClient;
using Newtonsoft.Json.Serialization;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class Rule2Action
    {
        /* CREATE */
        public static void Add_Rule_2(int Id, int Term_In_Year, int Min_Student, int Max_Student)
        {
            var client = ConnectNeo4J.Connection();
            var rule = new Rule2 { id = Id, term_in_year = Term_In_Year, min_students = Min_Student, max_students = Max_Student, isDelete = false};
            client.Cypher.Create("(:Rule_2 {rule})").WithParam("rule", rule).ExecuteWithoutResultsAsync().Wait();


        }

        /* UPDATE */
        public static void Update_Rule_2(int Id, int Term_In_Year, int Min_Student, int Max_Student)
        {
            var client = ConnectNeo4J.Connection();
            var term = new Rule2 { id = Id, term_in_year = Term_In_Year, min_students = Min_Student, max_students = Max_Student };
            client.Cypher.Match("(a:Rule_2)")
                .Where((Rule2 item) => item.id == Id)
                .Set("a = {tmp}")
                .WithParam("tmp", term)
                .ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        public static Rule2 Find(int Id)
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Rule_2)").Where("a.id = {ID}").WithParam("ID", Id).Return<Rule2>("a").Results.SingleOrDefault();
            return term;
        }

        /* GET ALL */
        public static List<Rule2> ShowAll()
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Rule_2)").Return<Rule2>("a").Results.ToList();
            return term;
        }


    }
}