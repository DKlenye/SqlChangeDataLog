using SqlChangeDataLog.QueryObjects;

namespace SqlChangeDataLog.Tests.Queries
{
    public class DropEntityTable
    {
        public QueryObject Query()
        {
            return new DropTable().Query("Entity");
        }
    }
}
