using System;
using System.Text;

namespace SqlChangeDataLog.Triggers
{
    public abstract class TriggerTextBuilder
    {
        protected TriggerTextBuilder(Trigger trigger, SqlTemplates sqlTemplates)
        {
            SqlTemplates = sqlTemplates;
            Trigger = trigger;
        }

        public Trigger Trigger { get; private set; }
        public SqlTemplates SqlTemplates { get; private set; }

        private string HeaderTemplate = @"
        ALTER TRIGGER [dbo].[{TriggerName}]
            ON  [dbo].[{TableName}]
            FOR {Operation}
        AS
        BEGIN
	        SET NOCOUNT ON;
        ";

        private string DeclareXmlTemplate = @"
            DECLARE @xml xml
			SET @xml = (
                {SelectXml}
            )    
        ";

        private string FooterTemplate = @"
        IF @xml IS NOT NULL
		    BEGIN
			    DECLARE @idString VARCHAR(32)
			    SELECT @idString = CAST({PrimaryKey} AS VARCHAR) FROM DELETED
				
				INSERT INTO ChangeLog ([date],[user],[changeType],[table],[idString],[description])
				VALUES (GETDATE(), USER, '{ChangeType}', '{TableName}', @idString, @xml)
			END
		END
        ";

        private string TriggerName
        {
            get
            {
                return String.Format(@"{0}_{1}_log", Trigger.TableName.ToLower(), Trigger.Operation.ToLower());
            }
        }

        private string BuildHeader()
        {
            return "";
        }

        private string BuildFooter()
        {
            return "";
        }

        private string BuildSelectXml()
        {
            return "";
        }
        

        public string BuildTriggerText()
        {
            var sb = new StringBuilder();
            sb.Append(BuildHeader());
            sb.Append(Trigger.ExtendedLogic);
            sb.Append(BuildSelectXml());
            sb.Append(BuildFooter());
            return sb.ToString();
        }

    }
}
