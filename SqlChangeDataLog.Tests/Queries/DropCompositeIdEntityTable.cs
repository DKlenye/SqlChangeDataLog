using SqlChangeDataLog.QueryObjects;

namespace SqlChangeDataLog.Tests.Queries
{
    public class DropCompositeIdEntityTable
    {
        public QueryObject Query()
        {
            return new DropTable().Query("CompositeIdEntity");
        }
    }
}
