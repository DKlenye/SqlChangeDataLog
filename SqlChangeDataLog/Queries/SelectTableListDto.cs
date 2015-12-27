using System.Collections.Generic;
using System.Data;
using System.Linq;
using SqlChangeDataLog.Dtos;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.QueryObjects;

namespace SqlChangeDataLog.Queries
{
    public class SelectTableListDto : AbstractQuery<IEnumerable<TableListDto>>
    {
        public SelectTableListDto(IDbConnection connection) : base(connection)
        {
        }

        public override IEnumerable<TableListDto> Query()
        {
            var operations = new Dictionary<string, List<string>>();

            Connection.Query<TriggerDto>(new SelectLogTrigger().All())
                .ToList().ForEach(trigger =>
            {
                var tableName = trigger.TableName.ToLower();
                var operationName = trigger.GetOperation().ToLower();
                if (!operations.ContainsKey(tableName))
                {
                    operations.Add(tableName, new List<string>());
                }
                operations[tableName].Add(operationName);
            });
            
            var dto = Connection.Query<string>(new SelectTables().All())
                .Select(x =>
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
