namespace SqlChangeDataLog.Triggers
{
    public class DeleteSqlTemplates:SqlTemplates
    {
        public override string SelectXml()
        {
                return @"SELECT {Columns}
            FROM DELETED AS D WHERE D.[{PrimaryKey}] = C.[{PrimaryKey}] FOR XML AUTO";
        }
        
        public override string ChangeType()
        {
            return "D";
        }

        public override string IdFrom()
        {
            return "DELETED";
        }
    }
}
