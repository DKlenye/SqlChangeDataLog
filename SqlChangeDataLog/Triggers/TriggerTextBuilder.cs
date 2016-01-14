using System;
using System.Text;
using SqlChangeDataLog.Extensions;

namespace SqlChangeDataLog.Triggers
{
    public class TriggerTextBuilder
    {
        public TriggerTextBuilder(Trigger trigger, string primaryKey, string logTableName)
        {
            LogTableName = logTableName;
            PrimaryKey = primaryKey;
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
        public string PrimaryKey { get; private set; }
        public string LogTableName { get; private set; }
        public SqlTemplates SqlTemplates { get; private set; }

        private const string HeaderTemplate = @"
CREATE TRIGGER [dbo].[{TriggerName}]
    ON  [dbo].[{TableName}]
    FOR {Operation} NOT FOR REPLICATION
AS
BEGIN
    SET NOCOUNT ON;
";

        private const string FooterTemplate = @"
INSERT INTO {LogTableName} ([date],[user],[changeType],[table],[idString],[description])
    SELECT 
        GETDATE(),
        USER,
        '{ChangeType}',
        '{TableName}',
        CAST({PrimaryKey} as varchar(32)),
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

        private string BuildFooter()
        {
            return FooterTemplate.ApplyTemplate(new
            {
                SelectXml = BuildSelectXml(),
                PrimaryKey = PrimaryKey,
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
                Columns = String.Join(",", Trigger.Columns),
                PrimaryKey = PrimaryKey
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
