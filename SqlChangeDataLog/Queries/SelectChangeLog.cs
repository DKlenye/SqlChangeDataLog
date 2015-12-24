using System;
using SqlChangeDataLog.Extensions;

namespace SqlChangeDataLog.Queries
{
    public class SelectChangeLog
    {
        private const string Sql = @"SELECT TOP 50 idChangeLog,[date],[user],changeType,[table],idString,[description] FROM {0} order by idChangeLog desc ";

        private const string SqlByParams = @"
        SELECT TOP {Count} idChangeLog,[date],[user],changeType,[table],idString,[description]
        FROM (SELECT *,ROW_NUMBER() OVER(ORDER BY {Order}) AS row_number FROM {TableName} {Filter}) a
        WHERE row_number > {From}
        ORDER BY {Order}";

        private const string SqlCountByParams = @"
            SELECT COUNT(*) from {TableName} {Filter}
        ";

        public QueryObject All(string tableName)
        {
            if (CheckSqlInjection(tableName))
            {
                throw new Exception("Bad table name");
            }
            
            return new QueryObject(String.Format(Sql,tableName));
        }

        public QueryObject CountByParams(string tableName, Filter filter)
        {
            return new QueryObject(SqlCountByParams.ApplyTemplate(new
            {
                TableName = tableName,
                Filter = BuildFilter(filter)
            }));
        }

        public QueryObject ByParams(string tableName, Filter filter, Sort sort, int from, int count)
        {

            var order = "idChangeLog";
            if (sort != null)
            {
                order = String.Format("[{0}] {1}", sort.id, sort.dir);
            }
            
            return new QueryObject(SqlByParams.ApplyTemplate(new
            {
                From = from,
                Count = count,
                TableName = tableName,
                Order = order,
                Filter = BuildFilter(filter)
            }));
        }

        string BuildFilter(Filter filter)
        {
            var _filter = "";
            if (filter != null && !filter.isEmpty())
            {
                _filter = "Where " + filter.BuildWhereClause();
            }
            return _filter;
        }

        bool CheckSqlInjection(string query)
        {
            return query.Contains(" ");
        }
    }
}
