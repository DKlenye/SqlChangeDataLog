namespace SqlChangeDataLog.Tests.Queries
{
    public class CreateBug1Table
    {
        public QueryObject Query()
        {
            return new QueryObject(@"
                CREATE TABLE [dbo].[FondTransfer](
                     [IdFondTransfer] [int] IDENTITY(1,1) NOT NULL,
                     [IdFondMoveFrom] [int] NULL,
                     [IdFondFrom] [int] NULL,
                     [IdFondMoveTo] [int] NULL,
                     [IdFondTo] [int] NULL,
                  CONSTRAINT [PK_FondTransfer] PRIMARY KEY CLUSTERED (
                     [IdFondTransfer] ASC
                )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                ) ON [PRIMARY]
            ");
        }
    }
}
