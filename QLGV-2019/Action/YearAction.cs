using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLGV_2019.Models;
using Neo4jClient;
using Newtonsoft.Json.Serialization;

namespace QLGV_2019.Action
{
    public class YearAction
    {
        /* CREATE  */
        public static void Add_Year(int School_Year)
        {
            var client = ConnectNeo4J.Connection();

            var year = new SchoolYear {school_year = School_Year, isEnded = false , isDeleted = false};
            client.Cypher.Create("(:School_Year {year})").WithParam("year", year).ExecuteWithoutResultsAsync().Wait();
            
        }

        /* UPDATE */
        public static void Edit_Year(int School_Year)
        {
            var client = ConnectNeo4J.Connection();
            var tmp = new SchoolYear { school_year = School_Year };
            client.Cypher
                .Match("(year:School_Year)")
                .Where((SchoolYear year) => year.school_year == School_Year) // sửa lại rồi nhé , bỏ luôn id rồi
                .WithParam("tmp", tmp).ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        public static SchoolYear Find(int Id)
        {
            var client = ConnectNeo4J.Connection();
            var tmp = client.Cypher.Match("(year:School_Year)")
                .Where("year.school_year = {ID}").WithParam("ID", Id).Return<SchoolYear>("year").Results.SingleOrDefault();
            return tmp;

        }

        /* GET ALL */
        public static List<SchoolYear> ShowAll()
        {
            var client = ConnectNeo4J.Connection();
            List<SchoolYear> list_school_year = client.Cypher.Match("(year:School_Year)").Return<SchoolYear>("year").Results.ToList();
            return list_school_year;
        }







    }
}