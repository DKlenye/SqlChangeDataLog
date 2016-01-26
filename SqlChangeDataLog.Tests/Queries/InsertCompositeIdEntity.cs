using SqlChangeDataLog.Tests.Dto;

namespace SqlChangeDataLog.Tests.Queries
{
    public class InsertCompositeIdEntity
    {
        public QueryObject Query(CompositeIdEntity entity)
        {
            return new QueryObject(@"
                insert into CompositeIdEntity(Key1, Key2) values(@Key1, @key2);",
                new {entity.Key1, entity.Key2}
                );
        }
    }
}
