using System;
using System.Collections.Generic;
using System.Linq;
using SqlChangeDataLog.Extensions;

namespace SqlChangeDataLog.Triggers
{
    public abstract class SqlTemplates
    {
        protected string InsertedWhereClauseTemplate = @"I.[{primaryKey}] = C.[{primaryKey}]";
        protected string DeletedWhereClauseTemplate = @"D.[{primaryKey}] = C.[{primaryKey}]";

        protected string BuildWhereClause(IEnumerable<string> keys, string template)
        {
            return String.Join(" AND ", keys.Select(primaryKey => template.ApplyTemplate(new { primaryKey })));
        }

        public abstract string SelectXml(IEnumerable<string> keys);
        public abstract string ChangeType();
        public abstract string IdFrom();
    }
}
