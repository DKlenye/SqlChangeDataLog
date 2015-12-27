using SqlChangeDataLog.Extensions;

namespace SqlChangeDataLog.QueryObjects
{
    public class DropTrigger
    {
        public QueryObject Query(string triggerName)
        {
            return new QueryObject("drop trigger {triggerName}".ApplyTemplate(new { triggerName }));
        }
    }
}
