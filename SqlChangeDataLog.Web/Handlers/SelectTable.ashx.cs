using System.Data;
using SqlChangeDataLog.Queries;
using SqlChangeDataLog.Web.Application;

namespace SqlChangeDataLog.Web.Handlers
{
    public class SelectTableContext : Context
    {
        public string TableName { get; set; }
    }

    public class SelectTable : Handler<SelectTableContext>
    {
        protected override object Process(SelectTableContext context, IDbConnection connection)
        {
            return new SelectTableDto(connection, context.TableName).Query();
        }
    }
}