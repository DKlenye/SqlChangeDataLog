using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.Queries;

namespace SqlChangeDataLog.Web.Handlers
{

    public class SelectTables : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            var database = "transport";
            var server = "db2";
            IEnumerable<string> tables;
            var operations = new Dictionary<string, List<string>>(); 
            
            using (IDbConnection dbConnection = Connect(server,database))
            {
                tables = dbConnection.Query<string>(new SelectTable().All());
                var triggers = dbConnection.Query<string>(new SelectLogTrigger().All());

                triggers.ToList().ForEach(trigger =>
                {
                    var nameArray = trigger.Split('_');
                    var tableName = nameArray[0].ToLower();
                    var operationName = nameArray[1];

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