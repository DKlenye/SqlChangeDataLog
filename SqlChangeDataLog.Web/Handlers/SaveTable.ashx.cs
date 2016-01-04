using System.Data;
using Newtonsoft.Json;
using SqlChangeDataLog.Dtos;
using SqlChangeDataLog.Queries;
using SqlChangeDataLog.Web.Application;

namespace SqlChangeDataLog.Web.Handlers
{
    public class SaveTableContext : Context
    {
        public string Table { get; set; }
        public TableDto Dto { get { return JsonConvert.DeserializeObject<TableDto>(Table); }}
    }

    public class SaveTable : Handler<SaveTableContext>
    {
        protected override object Process(SaveTableContext parameters, IDbConnection connection)
        {
            var dto = parameters.Dto;

            if (dto.Insert != null) dto.Insert.LogTableName = parameters.LogTable;
            if (dto.Update != null) dto.Update.LogTableName = parameters.LogTable;
            if (dto.Delete != null) dto.Delete.LogTableName = parameters.LogTable;

            new Commands.SaveTable(connection, dto).Execute();
            return new SelectTableDto(connection, dto.TableName).Query();
        }
    }
}