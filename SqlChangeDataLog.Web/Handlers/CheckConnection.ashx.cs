using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SqlChangeDataLog.Dtos;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.QueryObjects;
using SqlChangeDataLog.Web.Application;

public class CheckConnectionResponse : Context
{
    public bool BadTableName { get; set; }
}

namespace SqlChangeDataLog.Web.Handlers
{
    public class CheckConnection : Handler<Context>
    {
        protected override object Process(Context context, IDbConnection connection)
        {

            var response = new CheckConnectionResponse();

            //try server
            try
            {
                using (IDbConnection testConnection = new SqlConnection(buildConnectionString(context.Server, "")))
                {
                    testConnection.Execute(new QueryObject("select 1"));
                }

                response.Server = context.Server;
            }
            catch
            {
                return response;
            }

            //try database
            try
            {
                connection.Execute(new QueryObject("select 1"));
                response.Database = context.Database;
            }
            catch
            {
                return response;
            }

            //try logTable
            try
            {
                var exists = connection.Query<int>(new ExistsChangeLogTable().CheckExist(context.LogTable)).Single();
                if (exists == 1)
                {
                    //if exists try select from table
                    connection.Query<ChangeLogDto>(new QueryObjects.SelectChangeLog().ById(context.LogTable, 1));
                    response.LogTable = context.LogTable;
                }
            }
            catch
            {
               //Error if existing table not changeLog
                response.BadTableName = true;
            }
            
            return response;
        }
    }
}