namespace SqlChangeDataLog.Triggers
{
    public class DeleteSqlTemplates:SqlTemplates
    {
        public override string SelectXml()
        {
                return @"
                    SELECT {Columns}
			        FROM DELETED AS D FOR XML AUTO
                ";
        }


        public override string ChangeType()
        {
            return "D";
        }
    }
}
