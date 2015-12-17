using System.Data;
using System.Linq;
using SqlChangeDataLog.Dtos;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.Queries;

namespace SqlChangeDataLog.Web.Handlers
{

    public class DetailsReadParams : Context
    {
        public string TableName { get; set; }
    }

    public class SelectTableDetails : Handler<DetailsReadParams>
    {
        protected override object Process(DetailsReadParams parameters)
        {
            var param = ReadParams<DetailsReadParams>();
            var dto = new TableDto(){Name = param.TableName};

            using (IDbConnection dbConnection = Connect())
            {
                dto.Columns = dbConnection.Query<string>(new SelectColumns().ByTableName(param.TableName));
                dto.KeyColumns = dbConnection.Query<PrimaryKeyDto>(new SelectPrimaryKey().ByTableName(param.TableName)).Select(x=>x.ColumnName);
            }

            return dto;
        }
    }
}