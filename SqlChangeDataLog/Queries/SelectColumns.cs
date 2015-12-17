namespace SqlChangeDataLog.Queries
{
    public class SelectColumns
    {

        private string Sql = @"
            SELECT c.name FROM sysobjects t
            LEFT JOIN syscolumns c ON c.id = t.id
            WHERE t.xtype='U'       
        ";

        public QueryObject ByTableName(string tableName)
        {
            return new QueryObject(Sql + " AND t.name = @TableName", new {TableName = tableName});
        }
    }
}
