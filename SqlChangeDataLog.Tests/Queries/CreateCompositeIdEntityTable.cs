namespace SqlChangeDataLog.Tests.Queries
{
    public class CreateCompositeIdEntityTable
    {
        public QueryObject Query()
        {
            return new QueryObject(@"
               CREATE TABLE [dbo].[CompositeIdEntity](
	                [Key1] [int] NOT NULL,
	                [Key2] [varchar](50) NOT NULL,
                 CONSTRAINT [PK_CompositeIdEntity] PRIMARY KEY CLUSTERED 
                (
	                [Key1] ASC,
	                [Key2] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY]
            ");
        }
    }
}
