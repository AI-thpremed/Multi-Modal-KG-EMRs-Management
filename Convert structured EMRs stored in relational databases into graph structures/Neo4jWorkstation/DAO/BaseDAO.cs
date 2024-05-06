using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Threading.Tasks;

namespace Neo4jSocial.DAO
{
    public class BaseDAO : IDisposable
    {
        public IDriver Driver { get; }
        public BaseDAO()
        {
            //string uri = ConfigurationManager.AppSettings["uri"];
            //string userNeo4j = ConfigurationManager.AppSettings["userNeo4j"];
            //string passNeo4j = ConfigurationManager.AppSettings["passNeo4j"];


            string uri = "bolt://192.168.2.240:7687";
            string userNeo4j = "neo4j";
            string passNeo4j = "sigsrfj@neo4j";


            //string uri = "neo4j://127.0.0.1:7687";
            //string userNeo4j = "neo4j";
            //string passNeo4j = "888888";



            Driver = GraphDatabase.Driver(uri, AuthTokens.Basic(userNeo4j, passNeo4j));
        }

        public void Dispose()
        {
            Driver?.Dispose();
        }
        protected async Task WriteAsync(string query, object parameters)
        {
            var session = Driver.AsyncSession();
            try
            {
                await session.ExecuteWriteAsync(async tx => await tx.RunAsync(query, parameters));
                
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        protected async Task WriteAsync(string query, IDictionary<string, object> parameters = null)
        {
            var session = Driver.AsyncSession();
            try
            {
                await session.ExecuteWriteAsync(async tx => await tx.RunAsync(query, parameters));
            }
            finally
            {
                await session.CloseAsync();
            }
        }









        public void ReplaceInput(ref string str)
        {
            //str = str.Replace(" ", "");
            str = str.Replace("'", "");
            str = str.Replace("/", "");
            str = str.Replace("\\", "");
            //str = str.Replace(".", "");
            //str = str.Replace(",", "");
            str = str.Replace("<", "");
            str = str.Replace(">", "");
            str = str.Replace("?", "");
            str = str.Replace("`", "");
            str = str.Replace("!", "");
            str = str.Replace("@", "");
            str = str.Replace("#", "");
            str = str.Replace("&", "");
            str = str.Replace("*", "");
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            str = str.Replace("-", "");
            str = str.Replace("+", "");
            str = str.Replace("_", "");
            str = str.Replace(":", "");
            str = str.Replace(";", "");
            str = str.Replace("~", "");
            str = str.Replace("=", "");
            str = str.Replace("^", "");
            str = str.Replace("%", "");
            str = str.Replace("$", "");
            str = str.Replace("\"", "");
        }
    }
}