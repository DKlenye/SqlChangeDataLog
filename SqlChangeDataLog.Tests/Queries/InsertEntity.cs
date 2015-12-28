using SqlChangeDataLog.Tests.Dto;

namespace SqlChangeDataLog.Tests.Queries
{
    public class InsertEntity
    {
        public QueryObject Query(EntityDto entity)
        {
            return new QueryObject(@"
                insert into Entity(Name) values(@Name); Select SCOPE_IDENTITY();",
                new {entity.Name}
                );
        }
    }
}
