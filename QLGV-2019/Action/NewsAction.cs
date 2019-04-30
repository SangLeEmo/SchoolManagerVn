using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class NewsAction
    {
        /* CREATE */
        public static void Add_News(string Title, string Content, string Public_Day)
        {
            var client = ConnectNeo4J.Connection();
            var lop = new News { title = Title, content = Content, public_day = Public_Day };
            client.Cypher.Create("(:News {n})").WithParam("n", lop).ExecuteWithoutResultsAsync().Wait();

        }

        /* UPDATE */
        public static void Update_News(string Title, string Content, string Public_Day)
        {
            var client = ConnectNeo4J.Connection();
            var tmp = new News { title = Title, content = Content, public_day = Public_Day };
            client.Cypher.Match("(n:News)").Where((News n) => n.title == Title).Set("n = {a}").WithParam("a", tmp).ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        public static News Find(string Title)
        {
            var client = ConnectNeo4J.Connection();
            var lop = client.Cypher.Match("(a:News)").Where("a.title = {ID}").WithParam("ID", Title).Return<News>("a").Results.SingleOrDefault();
            return lop;
        }

        /* GET ALL */
        public static List<News> ShowAll()
        {
            var client = ConnectNeo4J.Connection();
            var lop = client.Cypher.Match("(n:News)").Return<News>("n").Results.ToList();
            return lop;
        }
    }
}