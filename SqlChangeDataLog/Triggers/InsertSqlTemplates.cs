using System.Collections.Generic;
using SqlChangeDataLog.Extensions;

namespace SqlChangeDataLog.Triggers
{
    public class InsertSqlTemplates:SqlTemplates
    {
        public override string SelectXml(IEnumerable<string> keys)
        {
            return @"SELECT {Columns}
            FROM INSERTED AS I WHERE {whereClause} FOR XML AUTO"
                .ApplyTemplate(new {whereClause=BuildWhereClause(keys,InsertedWhereClauseTemplate)});
        }

        public override string ChangeType()
        {
            return "I";
        }

        public override string IdFrom()
        {
            return "INSERTED";
        }
    }
}
