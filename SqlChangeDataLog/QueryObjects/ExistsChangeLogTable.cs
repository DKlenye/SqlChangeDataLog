namespace SqlChangeDataLog.QueryObjects
{
    public class ExistsChangeLogTable
    {
        public QueryObject CheckExist(string tableName)
        {
            return new QueryObject(@"
                    IF EXISTS (SELECT 1 
                        FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' 
                        AND TABLE_NAME = @tableName) 
                    SELECT 1 ELSE SELECT 0"
                , new {tableName});
        }
    }
}
