namespace SqlChangeDataLog.Queries
{
    public class SelectPrimaryKey
    {
        private const string Sql = @"
           
            SELECT Tab.Table_Name as TableName, Col.Column_Name as ColumnName from 
	            INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, 
                INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col 
            WHERE 
	            Col.Constraint_Name = Tab.Constraint_Name
                AND Col.Table_Name = Tab.Table_Name
                AND Constraint_Type = 'PRIMARY KEY'
        ";

        public QueryObject All()
        {
            return new QueryObject(Sql);
        }

        public QueryObject ByTableName(string tableName)
        {
            return new QueryObject(All().Sql + " AND Col.Table_Name = @TableName", new { TableName = tableName });
        }

    }
}
