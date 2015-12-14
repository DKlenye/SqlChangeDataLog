namespace SqlChangeDataLog.Queries
{
    public class SelectTriggerText
    {
        private string sql = @"
            select a.name, b.text
            from sysobjects a 
            inner join syscomments b on a.id = b.id
            where a.XType = 'TR'
            order by a.id, b.colid
            ";
    }
}
