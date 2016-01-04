using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace SqlChangeDataLog.Web.Application
{
    public abstract class Handler<TContext>:IHttpHandler
        where TContext:Context
    {
        public void ProcessRequest(HttpContext context)
        {
            var appContext = GetContext(context.Request);

            context.Response.ContentType = "application/json";

            using (IDbConnection connection = Connect(appContext))
            {
                context.Response.Write(
                    SerializeObject(
                        Process(GetContext(context.Request),connection)
                    )
                );    
            }
        }

        protected abstract object Process(TContext context, IDbConnection connection);

        protected string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(
                obj,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = {new IsoDateTimeConverter {DateTimeFormat = "yyyy'/'MM'/'dd' 'HH':'mm':'ss"}}
                }
                );
        }

        protected TContext GetContext(HttpRequest request)
        {
            var obj = new JObject();
            request.Form.AllKeys.ToList().ForEach(x => obj.Add(new JProperty(x, request[x])));
            return obj.ToObject<TContext>();    
        }

        protected IDbConnection Connect(TContext context)
        {
            IDbConnection dbConnection = new SqlConnection(buildConnectionString(context.Server,context.Database));
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
