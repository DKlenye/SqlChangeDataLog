using System.Data;
using SqlChangeDataLog.Dtos;
using SqlChangeDataLog.Extensions;

namespace SqlChangeDataLog.Web.Handlers
{
    public class SelectChangeLog : Handler<Context>
    {
        protected override object Process(Context parameters)
        {
            using (IDbConnection dbConnection = Connect())
            {
                var context = ReadParams<Context>();
                return dbConnection.Query<ChangeLogDto>(new Queries.SelectChangeLog().All(context.LogTable));
            }
        }
    }
}