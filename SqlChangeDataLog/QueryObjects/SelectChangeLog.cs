using System;
using SqlChangeDataLog.Extensions;

namespace SqlChangeDataLog.QueryObjects
{

    public class SelectChangeLog
    {

        public QueryObject All(string tableName)
        {
            return
                new QueryObject(
                    @"SELECT idChangeLog,[date],[user],changeType,[table],idString,[description] FROM {tableName}".ApplyTemplate(
                        new {tableName}));
        }

        public QueryObject CountByParams(string tableName, Filter filter)
        {
            return new QueryObject(@"SELECT COUNT(*) from {tableName} {Filter}".ApplyTemplate(new
            {
                tableName,
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

            return new QueryObject(@"
                SELECT TOP {Count} idChangeLog,[date],[user],changeType,[table],idString,[description]
                FROM (SELECT *,ROW_NUMBER() OVER(ORDER BY {Order}) AS row_n FROM {TableName} {Filter}) a
                WHERE row_n > {From}
                ORDER BY {Order}".ApplyTemplate(new
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

        public QueryObject ById(string tableName, int id)
        {
            return
                new QueryObject(
                    @"SELECT idChangeLog,[date],[user],changeType,[table],idString,[description] FROM {tableName} where idChangeLog = {id}"
                        .ApplyTemplate(
                            new {tableName, id}));
        }
    }
}
