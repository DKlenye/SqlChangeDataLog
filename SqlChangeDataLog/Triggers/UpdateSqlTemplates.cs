namespace SqlChangeDataLog.Triggers
{
    public class UpdateSqlTemplates:SqlTemplates
    {
        public override string SelectXml()
        {
                return @"
            SELECT {Columns}
			FROM 			
			(
				SELECT  {Columns}
				FROM INSERTED 
					UNION ALL 
				SELECT {Columns}
				FROM DELETED
			) 
			AS U FOR XML AUTO
        ";
        }

        public override string ChangeType()
        {
            return "U";
        }
    }
}
