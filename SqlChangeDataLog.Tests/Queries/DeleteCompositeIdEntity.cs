using SqlChangeDataLog.Tests.Dto;

namespace SqlChangeDataLog.Tests.Queries
{
    public class DeleteCompositeIdEntity
    {
        public QueryObject All()
        {
            return new QueryObject("delete from CompositeIdEntity");
        }

        public QueryObject Query(CompositeIdEntityDto entity)
        {
            return new QueryObject("delete from CompositeIdEntity where Key1 = @Key1 and Key2=@Key2",
                new {entity.Key1, entity.Key2});
        }
    }
}
