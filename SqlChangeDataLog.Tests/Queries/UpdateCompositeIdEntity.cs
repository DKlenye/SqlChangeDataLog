namespace SqlChangeDataLog.Tests.Queries
{
    public class UpdateCompositeIdEntity
    {
        public QueryObject Key2ForAllEntities(string key2)
        {
            return new QueryObject(@"update CompositeIdEntity
                                     set Key2 = @key2", new { key2 });
        }
        
    }
}
