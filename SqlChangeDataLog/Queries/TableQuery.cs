namespace SqlChangeDataLog.Queries
{
    public class TableQuery
    {
        private const string CheckExistSql = @"
            IF EXISTS (SELECT 1 
                FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' 
                AND TABLE_NAME = @TableName) 
            SELECT 1 AS res ELSE SELECT 0 AS res;";

        private string DropSql = @"";
        
        public QueryObject CheckExist(string tableName)
        {
            return new QueryObject(CheckExistSql, new { TableName = tableName });
        }

        public QueryObject Drop(string tableName)
        {
            return new QueryObject(DropSql, new { TableName = tableName });
        }
    }
}
