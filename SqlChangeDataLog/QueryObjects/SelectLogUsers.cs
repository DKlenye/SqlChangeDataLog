using SqlChangeDataLog.Extensions;

namespace SqlChangeDataLog.QueryObjects
{
    public class SelectLogUsers
    {
        public QueryObject All(string tableName)
        {
            return new QueryObject(@"select distinct [user] from {tableName}".ApplyTemplate(new {tableName}));
        }
    }
}
