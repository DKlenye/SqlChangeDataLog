using System.Collections.Generic;
using SqlChangeDataLog.Extensions;

namespace SqlChangeDataLog.Triggers
{
    public class UpdateSqlTemplates:SqlTemplates
    {
        public override string SelectXml(IEnumerable<string> keys)
        {
            return @"SELECT {Columns}
            FROM(
                SELECT *, 1 as _order FROM INSERTED as I WHERE {insertedWhereClause}
                UNION ALL
                SELECT *, 2 FROM DELETED as D WHERE {deletedWhereClause}
            )AS U order by _order FOR XML AUTO"
                .ApplyTemplate(new
                {
                    insertedWhereClause = BuildWhereClause(keys,InsertedWhereClauseTemplate),
                    deletedWhereClause = BuildWhereClause(keys,DeletedWhereClauseTemplate)
                });
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
