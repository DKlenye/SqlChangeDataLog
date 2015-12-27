using System.Data;
using System.Linq;
using SqlChangeDataLog.Dtos;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.QueryObjects;
using SqlChangeDataLog.Triggers;

namespace SqlChangeDataLog.Queries
{
    public class SelectTableDto : AbstractQuery<TableDto>
    {
        public SelectTableDto(IDbConnection connection, string tableName) : base(connection)
        {
            TableName = tableName;
        }
        
        public string TableName { get; private set; }

        public override TableDto Query()
        {
            var dto = new TableDto() {TableName = TableName};
            dto.Columns = Connection.Query<string>(new SelectColumns().ByTableName(TableName));
            dto.KeyColumns =
                Connection.Query<PrimaryKeyDto>(new SelectPrimaryKey().ByTableName(TableName)).Select(x => x.ColumnName);

            var triggers = Connection.Query<TriggerTextDto>(new SelectTriggerText().ByTableName(TableName))
                .Select(x => new Trigger(x.TableName, x.Text));

            triggers.ToList().ForEach(trigger =>
            {
                switch (trigger.Operation)
                {
                    case "insert":
                    {
                        dto.Insert = trigger;
                        break;
                    }
                    case "update":
                    {
                        dto.Update = trigger;
                        break;
                    }
                    case "delete":
                    {
                        dto.Delete = trigger;
                        break;
                    }
                }
            });
            return dto;

        }
    }
}
