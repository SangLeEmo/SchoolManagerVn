using Neo4jClient;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Models
{
    public class ConnectNeo4J
    {
        private static string Host_Name;
        private static string User_Name;
        private static string Password;
        public static BoltGraphClient Connection()
        {
            //Host_Name = "bolt://localhost:7687";
            //User_Name = "neo4j";
            ////Password = "qlgvneo4j"; // mật khẩu xài chung local
            //Password = "nghia7111998";

            //sanboxNeo4j
            Host_Name = "bolt://3.89.155.201:33737";
            User_Name = "neo4j";
            Password = "distribution-deserter-coordinate";

            var client = new BoltGraphClient(new Uri(Host_Name), User_Name, Password);
            {
                var JsonContractResolver = new CamelCasePropertyNamesContractResolver();
            };
            client.Connect();
            return client;
        }
    }
}