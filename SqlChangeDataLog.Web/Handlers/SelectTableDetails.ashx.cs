using System.Data;
using System.Linq;
using SqlChangeDataLog.Dtos;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.Queries;
using SqlChangeDataLog.Triggers;

namespace SqlChangeDataLog.Web.Handlers
{

    public class TableReadParams : Context
    {
        public string TableName { get; set; }
    }

    public class SelectTableDetails : Handler<TableReadParams>
    {
        protected override object Process(TableReadParams parameters)
        {
            var param = ReadParams<TableReadParams>();
            var dto = new TableDto(){Name = param.TableName};

            using (IDbConnection dbConnection = Connect())
            {
                dto.Columns = dbConnection.Query<string>(new SelectColumns().ByTableName(param.TableName));
                dto.KeyColumns = dbConnection.Query<PrimaryKeyDto>(new SelectPrimaryKey().ByTableName(param.TableName)).Select(x=>x.ColumnName);


                var triggers = dbConnection.Query<TriggerTextDto>(new SelectTriggerText().ByTableName(param.TableName))
                    .Select(x => new Trigger(x.TableName, x.Text));

                triggers.ToList().ForEach(trigger =>
                {
                    switch(trigger.Operation)
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
                
            }

            return dto;
        }
    }
}