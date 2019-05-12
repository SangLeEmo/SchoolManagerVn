using Neo4jClient;
using Newtonsoft.Json.Serialization;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace QLGV_2019.Action
{
    public class UserAction
    {
        /* CREATE */
        public static void Add_User(string Id_Number, string Sub_Name, string Name, string Password,string Role)
        {
            var client = ConnectNeo4J.Connection();
            var user = new User {id_number = Id_Number, sub_name = Sub_Name, name = Name, password = MD5Hash(Password), status = "none", role = Role, isDelete = false };
            client.Cypher.Create("(:User {user})").WithParam("user", user).ExecuteWithoutResultsAsync().Wait();
        }

        /* UPDATE */
        public static void Edit_User(string Id_Number, string Password)
        {
            var client = ConnectNeo4J.Connection();
            client.Cypher
                .Match("(user:User)")
                .Where((User user) => user.id_number == Id_Number)
                .Set("user.password = {tmp}")
                .WithParam("tmp", MD5Hash(Password)).ExecuteWithoutResultsAsync();
        }

        /* SEARCH */
        public static User Find(string Id_Number)
        {
            var client = ConnectNeo4J.Connection();
            var tmp = client.Cypher.Match("(user:User)")
                .Where("user.id_number = {ID}").WithParam("ID", Id_Number).Return<User>("user").Results.SingleOrDefault();
            return tmp;

        }

        /* GET ALL */
        public static List<User> GetAll()
        {
            var client = ConnectNeo4J.Connection();
            List<User> tmp = client.Cypher.Match("(user:User)").Return<User>("user").Results.ToList();
            return tmp;
        }


        public static User CheckLogin(string Id_Number, string Password)
        {
            User user = null;
            var client = ConnectNeo4J.Connection();
            user = client.Cypher.Match("(a:User)").Where("a.id_number = {ID}").WithParam("ID", Id_Number).AndWhere("a.password = {Pass}").WithParam("Pass", MD5Hash(Password)).Return<User>("a").Results.SingleOrDefault();
            return user;
        }


        public static int CountUser()
        {
            var client = ConnectNeo4J.Connection();
            var cnt = client.Cypher.Match("(a:User)").Return<int>("count(a)").Results.FirstOrDefault<int>();
            return cnt;
        }


        public static string MD5Hash(string text)//Ma hoa MD5
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }


            return strBuilder.ToString();
        }

    }
}