namespace SqlChangeDataLog.Queries
{
    public class SelectLogTrigger
    {
        private const string SqlAll = @"
            SELECT
                tr.name AS TriggerName,
                t.name TableName 
            from sysobjects tr
            LEFT JOIN sysobjects t ON tr.parent_obj = t.id AND t.XTYPE = 'U'
            WHERE tr.xtype = 'TR' and tr.name like '%log'
        ";

        public QueryObject All()
        {
            return new QueryObject(SqlAll);
        }

        public QueryObject ByTableName(string tableName)
        {
            return new QueryObject(All().Sql + " AND t.tableName = @TableName", new {TableName = tableName});
        }
    }
}
