namespace SqlChangeDataLog.MSSql.Queries
{
    public class SelectTable
    {
        private const string SqlAll = @"select name from sysobjects where xtype='U' and name <> 'ChangeDataLog'";

        public QueryObject All()
        {
            return new QueryObject(SqlAll);
        }
        
    }
}
