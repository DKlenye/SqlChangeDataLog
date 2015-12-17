using System.Collections.Generic;
using System.Data;
using System.Linq;
using SqlChangeDataLog.Dtos;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.Queries;

namespace SqlChangeDataLog.Web.Handlers
{
    public class ReadParams:Context
    {}

    public class SelectTables : Handler<ReadParams>
    {
        protected override object Process(ReadParams parameters)
        {
            IEnumerable<string> tables;
            var operations = new Dictionary<string, List<string>>();

            using (IDbConnection dbConnection = Connect())
            {
                tables = dbConnection.Query<string>(new SelectTable().All());
                var triggers = dbConnection.Query<TriggerDto>(new SelectLogTrigger().All());

                triggers.ToList().ForEach(trigger =>
                {
                    var tableName = trigger.TableName.ToLower();
                    var operationName = trigger.GetOperation().ToLower();

                    if (!operations.ContainsKey(tableName))
                    {
                        operations.Add(tableName, new List<string>());
                    }

                    operations[tableName].Add(operationName);

                });
            }

            var dto = tables.Select(x =>
            {
                var _dto = new TableListDto { Name = x };
                if (operations.ContainsKey(x.ToLower()))
                {
                    operations[x.ToLower()].Sort();
                    _dto.Operations = operations[x.ToLower()].ToArray();
                }
                return _dto;
            });
            return dto;
        }
    }
}