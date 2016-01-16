using SqlChangeDataLog.Extensions;

namespace SqlChangeDataLog.QueryObjects
{
    public class CreateChangeLogTable
    {
        public QueryObject Create(string tableName)
        {
            return new QueryObject(@"
                CREATE TABLE [dbo].[{tableName}](
	                [idChangeLog] [int] IDENTITY(1,1) NOT NULL,
	                [date] [datetime] NOT NULL,
	                [user] [sysname] NOT NULL,
	                [changeType] [char](1) NOT NULL,
	                [table] [varchar](50) NOT NULL,
	                [idString] [int] NOT NULL,
	                [description] [xml] NOT NULL,
	
                 CONSTRAINT [PK_{tableName}_ChangeLog] PRIMARY KEY CLUSTERED([idChangeLog] ASC)
                 WITH (
 	                PAD_INDEX  = OFF,
 	                STATISTICS_NORECOMPUTE  = OFF,
 	                IGNORE_DUP_KEY = OFF,
 	                ALLOW_ROW_LOCKS  = ON,
 	                ALLOW_PAGE_LOCKS  = ON,
 	                FILLFACTOR = 90)
                    ON [PRIMARY]
                ) ON [PRIMARY]"
                .ApplyTemplate(new {tableName}));
        }

    }
}
