namespace SqlChangeDataLog.QueryObjects
{
    public class SelectTables
    {
        public QueryObject All()
        {
            return new QueryObject( @"select name from sysobjects where xtype='U'");
        }
    }
}
