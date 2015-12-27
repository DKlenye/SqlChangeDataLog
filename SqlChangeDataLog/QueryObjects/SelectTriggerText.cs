namespace SqlChangeDataLog.QueryObjects
{
    public class SelectTriggerText
    {
        public QueryObject All()
        {
            return new QueryObject(@"
                select 
	                t.Name AS TableName,
	                tr.Name AS TriggerName,
	                c.Text
                from sysobjects tr
                INNER JOIN sysobjects t ON tr.parent_obj = t.id AND t.XTYPE = 'U'
                LEFT JOIN syscomments c on tr.id = c.id
                WHERE tr.XType = 'TR'
            ");
        }

        public QueryObject ByTableName(string tableName)
        {
            return new QueryObject(All().Sql + " AND t.Name = @tableName", new {tableName});
        }

        public QueryObject ByTriggerName(string triggerName)
        {
            return new QueryObject(All().Sql + " AND tr.Name = @triggerName", new {triggerName});
        }
    }
}
