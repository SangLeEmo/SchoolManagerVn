    using Neo4jClient;
using Newtonsoft.Json.Serialization;
using QLGV_2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLGV_2019.Action
{
    public class UserAction
    {
        /* CREATE */
        public static void Add_User(string Sub_Name, string Name, string Role, string Password, string Bday, string Email, string Phone, string Address)
        {
            var client = ConnectNeo4J.Connection();
            int user_id = client.Cypher.Match("(usr:User)").Return<int>("count(usr)").Results.FirstOrDefault<int>();
            var user = new User {userID = user_id + 1,sub_name = Sub_Name, name = Name, bday = Bday, email = Email, phone = Phone, address = Address};
            client.Cypher.Create("(:User {user})").WithParam("user", user).ExecuteWithoutResultsAsync().Wait();
        }

        /* UPDATE */
        public static void Edit_User(int User_id, string Sub_Name, string Name)
        {
            var client = ConnectNeo4J.Connection();
            var tmp = new User {userID = User_id, sub_name = Sub_Name, name = Name};
            client.Cypher
                .Match("(user:User)")
                .Where((User user) => user.userID)
                .Set("user = {tmp}")
                .WithParam("tmp", tmp).ExecuteWithoutResultsAsync();
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
            user = client.Cypher.Match("(a:User)").Where("a.id_number = {ID}").WithParam("ID", Id_Number).AndWhere("a.password = {Pass}").WithParam("Pass", Password).Return<User>("a").Results.SingleOrDefault();
            return user;
        }

    }
}