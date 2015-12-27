using SqlChangeDataLog.Tests.Dto;

namespace SqlChangeDataLog.Tests.Queries
{
    public class UpdateEntity
    {
        public QueryObject NameForAllEntities(string newName)
        {
            return new QueryObject(@"update Entity
                                     set Name = @newName", new { newName });
        }

        public QueryObject Query(EntityDto entity)
        {
            return new QueryObject("update Entity set Name = @Name where Id = @Id", entity);
        }
    }
}
