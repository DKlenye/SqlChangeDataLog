namespace SqlChangeDataLog.QueryObjects
{
    public class SelectColumns
    {
        public QueryObject All()
        {
            return new QueryObject(@"
                SELECT c.name FROM sysobjects t
                LEFT JOIN syscolumns c ON c.id = t.id
                WHERE t.xtype='U'
            ");
        }

        public QueryObject ByTableName(string tableName)
        {
            return new QueryObject(All().Sql + " AND t.name = @tableName", new {tableName});
        }
    }
}
