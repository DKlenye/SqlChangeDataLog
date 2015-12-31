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
    FOR {Operation}
AS
BEGIN
    SET NOCOUNT ON;
";

        private const string DeclareXmlTemplate = @"
    DECLARE @xml xml
    SET @xml = ({SelectXml}
    )    
";

        private const string FooterTemplate = @"
    IF @xml IS NOT NULL
    BEGIN
        DECLARE @idString VARCHAR(32)
        SELECT @idString = CAST({PrimaryKey} AS VARCHAR) FROM {IdFrom}
        INSERT INTO {LogTableName} ([date],[user],[changeType],[table],[idString],[description])
        VALUES (GETDATE(), USER, '{ChangeType}', '{TableName}', @idString, @xml)
    END
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
                PrimaryKey = PrimaryKey,
                IdFrom = SqlTemplates.IdFrom(),
                ChangeType = SqlTemplates.ChangeType(),
                TableName = Trigger.TableName,
                LogTableName = LogTableName
            });
        }

        private string BuildSelectXml()
        {
            return DeclareXmlTemplate.ApplyTemplate(new
            {
                SelectXml = SqlTemplates.SelectXml().ApplyTemplate(new {
                    Columns = String.Join(",",Trigger.Columns)
                })
            });
        }
        
        public string BuildTriggerText()
        {
            var sb = new StringBuilder();
            sb.Append(BuildHeader())
                .AppendLine(Trigger.ExtendedLogic)
                .Append(BuildSelectXml())
                .Append(BuildFooter());
            return sb.ToString();
        }

        public QueryObject Query()
        {
            return new QueryObject(BuildTriggerText());
        }
        
    }
}
