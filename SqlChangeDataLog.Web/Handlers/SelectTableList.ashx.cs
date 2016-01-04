using System.Data;
using SqlChangeDataLog.Queries;
using SqlChangeDataLog.Web.Application;

namespace SqlChangeDataLog.Web.Handlers
{
    public class SelectTableList : Handler<Context>
    {
        protected override object Process(Context context, IDbConnection connection)
        {
            return new SelectTableListDto(connection).Query();
        }
    }
}