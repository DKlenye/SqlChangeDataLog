namespace SqlChangeDataLog.Triggers
{
    public class UpdateSqlTemplates:SqlTemplates
    {
        public override string SelectXml()
        {
                return @"SELECT {Columns}
            FROM(
                SELECT *, 1 as _order FROM INSERTED as I WHERE I.[{PrimaryKey}] = C.[{PrimaryKey}]
                UNION ALL
                SELECT *, 2 FROM DELETED as D WHERE D.[{PrimaryKey}] = C.[{PrimaryKey}]
            )AS U order by _order FOR XML AUTO";
        }

        public override string ChangeType()
        {
            return "U";
        }

        public override string IdFrom()
        {
            return "DELETED";
        }
    }
}
