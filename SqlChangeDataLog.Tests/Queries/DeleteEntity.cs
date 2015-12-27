namespace SqlChangeDataLog.Tests.Queries
{
    public class DeleteEntity
    {
        public QueryObject All()
        {
            return new QueryObject("delete from Entity");
        }

        public QueryObject ById(int id)
        {
            return new QueryObject("delete from Entity where Id = @id", new {id});
        }
    }
}
