using System;

namespace SqlChangeDataLog
{
    public class QueryObject
    {
        /// <summary>
        ///     Create QueryObject for <paramref name="sql" /> string only
        /// </summary>
        /// <param name="sql">SQL string</param>
        /// <param name="queryParams">Parameter list</param>
        public QueryObject(string sql, object queryParams = null)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentNullException("sql");

            Sql = sql;
            QueryParams = queryParams;
        }

        /// <summary>
        ///     SQL string
        /// </summary>
        public string Sql { get; private set; }

        /// <summary>
        ///     Parameter list
        /// </summary>
        public object QueryParams { get; private set; }
    }
}
