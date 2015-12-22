using System;

namespace SqlChangeDataLog.Queries
{
    public class SelectChangeLog
    {
        private const string Sql = @"SELECT TOP 1000 idChangeLog,[date],[user],changeType,[table],idString,[description] FROM {0} ";

        public QueryObject All(string tableName)
        {
            if (CheckSqlInjection(tableName))
            {
                throw new Exception("Bad table name");
            }
            
            return new QueryObject(String.Format(Sql,tableName));
        }
        
        bool CheckSqlInjection(string query)
        {
            return query.Contains(" ");
        }
    }
}
