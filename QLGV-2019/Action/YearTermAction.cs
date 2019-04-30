using Neo4jClient;
using Newtonsoft.Json.Serialization;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class YearTermAction
    {
        /* CREATE */
        public static void Add_Year_Term(int School_Year, int Year_Term, string Day_Start, string Day_End)
        {
            var client = ConnectNeo4J.Connection();
            var tmp = new YearTerm {year_term = Year_Term, day_start = Day_Start, day_end = Day_End, isDelete = false };
            client.Cypher.Create("(:Year_Term {year_term})").WithParam("year_term", tmp).ExecuteWithoutResultsAsync().Wait();
            client.Cypher.Match("(a:Year_Term)", "(b:School_Year)").
                Where((YearTerm a) => a.year_term == Year_Term).
                AndWhere((SchoolYear b) => b.school_year == School_Year).
                Create("(a)-[:In_Year]->(b)").ExecuteWithoutResults();
        }

        /* UPDATE */
        //public static void Update_Year_Term(int Year_Term, string Day_Start, string Day_End)
        //{
        //    var client = ConnectNeo4J.Connection();
        //    var term = new YearTerm {  day_start = Day_Start, day_end = Day_End };
        //    client.Cypher.Match("(a:Year_Term)")
        //        .Where((YearTerm item) => item.id == Id)
        //        .Set("a = {tmp}")
        //        .WithParam("tmp", term)
        //        .ExecuteWithoutResultsAsync();
        //}

        /* SEARCH */
        public static YearTerm Find(int Id)
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Year_Term)").Where("a.year_term = {ID}").WithParam("ID", Id).Return<YearTerm>("a").Results.SingleOrDefault();
            return term;
        }

        /* GET ALL */
        public static List<YearTerm> ShowAll()
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Year_Term)").Return<YearTerm>("a").Results.ToList();
            return term;
        }



    }
}