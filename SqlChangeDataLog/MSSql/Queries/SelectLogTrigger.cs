namespace SqlChangeDataLog.MSSql.Queries
{
    public class SelectLogTrigger
    {
        private const string SqlAll = @"select * from sysobjects where xtype = 'TR' and name like '%_log'";

        public QueryObject All()
        {
            return new QueryObject(SqlAll);
        }
    }
}
