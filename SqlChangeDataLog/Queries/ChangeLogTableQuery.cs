namespace SqlChangeDataLog.Queries
{
    public class ChangeLogTableQuery
    {

        private string CreateSql = @"
            CREATE TABLE [dbo].[@TableName](
	            [idChangeLog] [int] IDENTITY(1,1) NOT NULL,
	            [date] [datetime] NOT NULL,
	            [user] [sysname] NOT NULL,
	            [changeType] [char](1) NOT NULL,
	            [table] [varchar](50) NOT NULL,
	            [idString] [int] NOT NULL,
	            [description] [xml] NOT NULL,
	
             CONSTRAINT [PK_@TableName_ChangeLog] PRIMARY KEY CLUSTERED([idChangeLog] ASC)
             WITH (
 	            PAD_INDEX  = OFF,
 	            STATISTICS_NORECOMPUTE  = OFF,
 	            IGNORE_DUP_KEY = OFF,
 	            ALLOW_ROW_LOCKS  = ON,
 	            ALLOW_PAGE_LOCKS  = ON,
 	            FILLFACTOR = 90)
            ON [PRIMARY]
            ) ON [PRIMARY]";




        public QueryObject Create(string tableName)
        {
            return new QueryObject(CreateSql,new {TableName=tableName});
        }

       
    }
}
