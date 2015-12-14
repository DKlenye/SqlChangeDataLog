using System;
using SqlChangeDataLog.Operations;

namespace SqlChangeDataLog.Queries
{
    public class CreateLogTrigger
    {
  
        public QueryObject Insert()
        {
            return new QueryObject("");
        }

        
        private string BuildTriggerSql(string tableName, DataOperation operation)
        {

            var triggerName = getTriggerName(tableName, operation);


            return String.Format(@"
                ALTER TRIGGER [dbo].[{0}] ON [dbo].[{1}]
	                FOR DELETE NOT FOR REPLICATION
	                AS
	                BEGIN
		                SET NOCOUNT ON;
		                IF (@@RowCount > 1)	RETURN
		
		                DECLARE @xml xml
		
		                SET @xml = ( 
			                SELECT NormId,VehicleId,WorkTypeId,isMain,CounterId,MotoToMachineKoef
			                FROM deleted AS D FOR XML AUTO
		                )
		
		                IF @xml IS NOT NULL
		
		                BEGIN
			                DECLARE @idString VARCHAR(32)
			                SELECT @idString = CAST(NormId AS VARCHAR) FROM DELETED
			
			                INSERT INTO ChangeLog ([date],[user],[changeType],[table],[idString],[description])
			                VALUES (GETDATE(),USER,'D','{1}',@idString,@xml)
		                END
                    END            
            ",triggerName,tableName);
        }


        string getTriggerName(string tableName, DataOperation operation)
        {
            return String.Format("{0}_{1}_Log", tableName, operation.Name);
        }

        
    }
}
