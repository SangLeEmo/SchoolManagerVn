using Neo4jClient;
using Newtonsoft.Json.Serialization;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class SectorSubjectAction
    {
        /* CREATE */
        public static void Add_SectorSubject(int Numb_Student)
        {
            var client = ConnectNeo4J.Connection();
            var sector = new SectorSubject { numb_student = Numb_Student, isDelete = false };
            client.Cypher.Create("(:Sector_Subject {sector})").WithParam("sector", sector).ExecuteWithoutResultsAsync().Wait();

        }

        /* UPDATE */
        public static void Update_SectorSubject( int Numb_Student)
        {
            var client = ConnectNeo4J.Connection();
            var term = new SectorSubject { numb_student = Numb_Student };
            client.Cypher.Match("(a:Sector_Subject)")
                .Where((SectorSubject item) => item.numb_student == Numb_Student)
                .Set("a = {tmp}")
                .WithParam("tmp", term)
                .ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        public static SectorSubject Find(int Id)
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Sector_Subject)").Where("a.numb_student = {ID}").WithParam("ID", Id).Return<SectorSubject>("a").Results.SingleOrDefault();
            return term;
        }

        /* GET ALL */
        public static List<SectorSubject> ShowAll()
        {
            var client = ConnectNeo4J.Connection();
            var term = client.Cypher.Match("(a:Sector_Subject)").Return<SectorSubject>("a").Results.ToList();
            return term;
        }

    }
}