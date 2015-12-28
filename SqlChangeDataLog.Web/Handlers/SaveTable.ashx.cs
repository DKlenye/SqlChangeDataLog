using System.Data;
using SqlChangeDataLog.Dtos;
using SqlChangeDataLog.Queries;
using SqlChangeDataLog.Web.Application;

namespace SqlChangeDataLog.Web.Handlers
{
    public class SaveTableContext : Context
    {
        public TableDto Table { get; set; }
    }

    public class SaveTable : Handler<SaveTableContext>
    {
        protected override object Process(SaveTableContext parameters)
        {
            using (IDbConnection connection = Connect())
            {
                var context = ReadParams<SaveTableContext>();
                new Commands.SaveTable(connection, context.Table);
                return new SelectTableDto(connection, context.Table.TableName);
            }
        }
    }
}