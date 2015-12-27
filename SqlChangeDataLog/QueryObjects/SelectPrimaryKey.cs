namespace SqlChangeDataLog.QueryObjects
{
    public class SelectPrimaryKey
    {
        public QueryObject All()
        {
            return new QueryObject(@"
                SELECT Tab.Table_Name as TableName, Col.Column_Name as ColumnName from 
	                INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, 
                    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col 
                WHERE 
	                Col.Constraint_Name = Tab.Constraint_Name
                    AND Col.Table_Name = Tab.Table_Name
                    AND Constraint_Type = 'PRIMARY KEY'
            ");
        }

        public QueryObject ByTableName(string tableName)
        {
            return new QueryObject(All().Sql + " AND Col.Table_Name = @tableName", new {tableName});
        }

    }
}
