namespace SqlChangeDataLog.Queries
{
    public class SelectTriggerText
    {
        private const string Sql = @"
            select 
	            t.Name AS TableName,
	            tr.Name AS TriggerName,
	            c.Text
            from sysobjects tr
            INNER JOIN sysobjects t ON tr.parent_obj = t.id AND t.XTYPE = 'U'
            LEFT JOIN syscomments c on tr.id = c.id
            WHERE tr.XType = 'TR'
        ";

        public QueryObject All()
        {
            return new QueryObject(Sql);
        }

        public QueryObject ByTableName(string tableName)
        {
            return new QueryObject(All().Sql + " AND t.Name = @TableName", new {TableName = tableName});
        }

        public QueryObject ByTriggerName(string triggerName)
        {
            return new QueryObject(All().Sql + " AND tr.Name = @TriggerName", new {TriggerName = triggerName});
        }
    }
}
