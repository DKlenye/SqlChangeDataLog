using SqlChangeDataLog.Extensions;

namespace SqlChangeDataLog.QueryObjects
{
    public class DropTable
    {
        public QueryObject Query(string tableName)
        {
            return new QueryObject("drop table {tableName}".ApplyTemplate(new {tableName}));
        }
    }
}
