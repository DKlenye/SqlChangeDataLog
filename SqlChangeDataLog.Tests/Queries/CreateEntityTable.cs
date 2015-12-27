namespace SqlChangeDataLog.Tests.Queries
{
    public class CreateEntityTable
    {
        public QueryObject Query()
        {
            return new QueryObject(@"
                CREATE TABLE [dbo].[Entity](
	                [Id] [int] IDENTITY(1,1) NOT NULL,
	                [Name] [varchar](50) NOT NULL,
                 CONSTRAINT [PK_Entity] PRIMARY KEY CLUSTERED 
                (
	                [Id] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY]
            ");
        }
    }
}
