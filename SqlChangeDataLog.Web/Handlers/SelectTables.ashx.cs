using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.Queries;

namespace SqlChangeDataLog.Web.Handlers
{
    
    public class RequestParams
    {
        public string Server { get; set; }
        public string Database { get; set; }
    }
    
    public class SelectTables : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            var httpRequest = context.Request;
            
            var obj = new JObject();
            httpRequest.Form.AllKeys.ToList().ForEach(x => obj.Add(new JProperty(x,httpRequest[x])));
            var param = obj.ToObject<RequestParams>();


            IEnumerable<string> tables;
            var operations = new Dictionary<string, List<string>>(); 
            
            using (IDbConnection dbConnection = Connect(param.Server,param.Database))
            {
                tables = dbConnection.Query<string>(new SelectTable().All());
                var triggers = dbConnection.Query<TriggerDto>(new SelectLogTrigger().All());

                triggers.ToList().ForEach(trigger =>
                {
                    var tableName = trigger.TableName.ToLower();
                    var operationName = trigger.GetOperation().ToLower();

                    if (!operations.ContainsKey(tableName))
                    {
                        operations.Add(tableName,new List<string>());
                    }

                    operations[tableName].Add(operationName);

                });
            }

            var dto = tables.Select(x =>
            {
                var _dto = new TableListDto {Name = x};
                if (operations.ContainsKey(x.ToLower()))
                {
                    operations[x.ToLower()].Sort();
                    _dto.Operations = operations[x.ToLower()].ToArray();
                }
                return _dto;
            });
            context.Response.Write(JsonConvert.SerializeObject(dto));

        }


        IDbConnection Connect(string server, string database)
        {
            IDbConnection dbConnection = new SqlConnection(buildConnectionString(server,database));
            dbConnection.Open();
            return dbConnection;
        }

        string buildConnectionString(string server, string database)
        {
            return String.Format("data source={0}; initial catalog={1}; integrated security=SSPI;", server, database);
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}