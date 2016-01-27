using System.Collections.Generic;
using SqlChangeDataLog.Extensions;

namespace SqlChangeDataLog.Triggers
{
    public class DeleteSqlTemplates:SqlTemplates
    {
        public override string SelectXml(IEnumerable<string> keys)
        {
            return @"SELECT {Columns}
            FROM DELETED AS D WHERE {whereClause} FOR XML AUTO"
                .ApplyTemplate(new {whereClause=BuildWhereClause(keys,DeletedWhereClauseTemplate)});
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
