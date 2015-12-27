using System.Data;
using SqlChangeDataLog.Queries;
using SqlChangeDataLog.Web.Application;

namespace SqlChangeDataLog.Web.Handlers
{

    public class TableReadParams : Context
    {
        public string TableName { get; set; }
    }

    public class SelectTable : Handler<TableReadParams>
    {
        protected override object Process(TableReadParams parameters)
        {
            var param = ReadParams<TableReadParams>();
            using (IDbConnection dbConnection = Connect())
            {
                return new SelectTableDto(dbConnection, param.TableName).Query();
            }
        }
    }
}