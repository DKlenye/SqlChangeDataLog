using System.Data;

namespace SqlChangeDataLog.Queries
{
    public abstract class AbstractQuery<TResult>
    {
        protected AbstractQuery(IDbConnection connection)
        {
            Connection = connection;
        }

        public IDbConnection Connection { get; private set; }

        public abstract TResult Query();
    }
}
