using SqlChangeDataLog.Extensions;

namespace SqlChangeDataLog.QueryObjects
{
    public class SelectLogTrigger
    {
        public QueryObject All()
        {
            return new QueryObject(@"
                SELECT
                    tr.name AS TriggerName,
                    t.name TableName 
                from sysobjects tr
                LEFT JOIN sysobjects t ON tr.parent_obj = t.id AND t.XTYPE = 'U'
                WHERE tr.xtype = 'TR' and tr.name like '%log'
            ");
        }

        public QueryObject ByTableName(string tableName)
        {
            return new QueryObject(All().Sql + " AND t.name = '{tableName}'".ApplyTemplate(new {tableName}));
        }
    }
}
