using System.Data;
using SqlChangeDataLog.Queries;
using SqlChangeDataLog.Web.Application;

namespace SqlChangeDataLog.Web.Handlers
{
    public class SelectLogUsers : Handler<Context>
    {
        protected override object Process(Context context, IDbConnection connection)
        {
            return new SelectUsers(connection, context.LogTable).Query();
        }
    }
}