using System.Collections.Generic;
using System.Data;
using System.Linq;
using SqlChangeDataLog.Dtos;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.QueryObjects;
using SqlChangeDataLog.Triggers;

namespace SqlChangeDataLog.Commands
{
    public class SaveTable
    {
        public SaveTable(IDbConnection connection, TableDto dto)
        {
            this.dto = dto;
            Connection = connection;
        }

        public IDbConnection Connection { get; private set; }
        public TableDto dto { get;private set; }

        public void Execute()
        {
            var tx = Connection.BeginTransaction();
            
            Connection.Query<TriggerDto>(new SelectLogTrigger().ByTableName(dto.TableName),tx).ToList()
                .ForEach(x => Connection.Execute(new DropTrigger().Query(x.TriggerName),tx));

            new List<Trigger>() {dto.Insert, dto.Update, dto.Delete}
            .ForEach(trigger =>
            {
                if (trigger != null)
                {
                    Connection.Execute(
                        new TriggerTextBuilder(trigger, dto.KeyColumns, trigger.LogTableName)
                        .Query(),
                        tx
                    );
                }
            });

            tx.Commit();
        }
    }
}
