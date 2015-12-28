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
        protected override object Process(SelectTableContext parameters)
        {
            var param = ReadParams<SelectTableContext>();
            using (IDbConnection dbConnection = Connect())
            {
                return new SelectTableDto(dbConnection, param.TableName).Query();
            }
        }
    }
}