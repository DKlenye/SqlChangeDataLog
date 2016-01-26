using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlChangeDataLog.Extensions;

namespace SqlChangeDataLog.Triggers
{
    public class TriggerTextBuilder
    {
        public TriggerTextBuilder(Trigger trigger, IEnumerable<string> primaryKeys, string logTableName)
        {
            LogTableName = logTableName;
            PrimaryKeys = primaryKeys;
            SqlTemplates = GetTemplates(trigger.Operation);
            Trigger = trigger;
        }
        
        private SqlTemplates GetTemplates(string operation)
        {
            switch (operation.ToLower())
            {
                case "insert": { return new InsertSqlTemplates(); }
                case "update": { return new UpdateSqlTemplates(); }
                case "delete": { return new DeleteSqlTemplates(); }
            }
            throw (new Exception("Unknown trigger operation"));
        }
        
        public Trigger Trigger { get; private set; }
        public IEnumerable<string> PrimaryKeys { get; private set; }
        public string LogTableName { get; private set; }
        public SqlTemplates SqlTemplates { get; private set; }

        private const string HeaderTemplate = @"
CREATE TRIGGER [dbo].[{TriggerName}]
    ON  [dbo].[{TableName}]
    FOR {Operation} NOT FOR REPLICATION
AS
BEGIN
";

        private const string FooterTemplate = @"
SET NOCOUNT ON;
INSERT INTO {LogTableName} ([date],[user],[changeType],[table],[idString],[description])
    SELECT 
        GETDATE(),
        USER,
        '{ChangeType}',
        '{TableName}',
        {PrimaryKeys},
        (
            {SelectXml}
        )
    FROM {IdFrom} C

END";
        
        private string TriggerName
        {
            get
            {
                return String.Format(@"{0}_{1}_log", Trigger.TableName.ToLower(), Trigger.Operation.ToLower());
            }
        }

        private string BuildHeader()
        {
            return HeaderTemplate.ApplyTemplate(new
            {
                TriggerName = TriggerName,
                TableName = Trigger.TableName,
                Operation = Trigger.Operation.ToUpper()
            });
        }

        private string BuildPrimaryKeys()
        {
            return String.Join(" + ',' + ",
                PrimaryKeys.Select(PrimaryKey => "CAST([{PrimaryKey}] as varchar(32))".ApplyTemplate(new {PrimaryKey})));
        }

        private string BuildFooter()
        {
            return FooterTemplate.ApplyTemplate(new
            {
                SelectXml = BuildSelectXml(),
                PrimaryKeys = BuildPrimaryKeys(),
                IdFrom = SqlTemplates.IdFrom(),
                ChangeType = SqlTemplates.ChangeType(),
                TableName = Trigger.TableName,
                LogTableName = LogTableName
            });
        }

        private string BuildSelectXml()
        {
            return SqlTemplates.SelectXml().ApplyTemplate(new
            {
                Columns = String.Join(",", Trigger.Columns.Select(x => "[" + x + "]")),
                PrimaryKey = PrimaryKeys.First()
            });
        }
        
        public string BuildTriggerText()
        {
            var sb = new StringBuilder();
            sb.Append(BuildHeader())
                .AppendLine(Trigger.ExtendedLogic)
                .Append(BuildFooter());
            return sb.ToString();
        }

        public QueryObject Query()
        {
            return new QueryObject(BuildTriggerText());
        }
        
    }
}
