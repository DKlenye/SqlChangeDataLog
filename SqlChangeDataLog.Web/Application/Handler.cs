using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SqlChangeDataLog.Web.Application
{
    public abstract class Handler<TParams>:IHttpHandler
    {

        private HttpRequest _request;
        
        public void ProcessRequest(HttpContext context)
        {
            _request = context.Request;
            context.Response.ContentType = "application/json";
            context.Response.Write(
                JsonConvert.SerializeObject(
                    Process(ReadParams<TParams>()),
                    Formatting.Indented,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
                )
            );
        }

        protected abstract object Process(TParams parameters);
        
        protected T ReadParams<T>()
        {
            var obj = new JObject();
            _request.Form.AllKeys.ToList().ForEach(x => obj.Add(new JProperty(x, _request[x])));
            return obj.ToObject<T>();    
        }

        Context Context
        {
            get { return ReadParams<Context>(); }
        }

        protected IDbConnection Connect()
        {
            var context = Context;
            IDbConnection dbConnection = new SqlConnection(buildConnectionString(context.Server, context.Database));
            dbConnection.Open();
            return dbConnection;
        }

        string buildConnectionString(string server, string database)
        {
            return String.Format("data source={0}; initial catalog={1}; integrated security=SSPI;", server, database);
        }

        public bool IsReusable { get{return false;} }
    }
}
