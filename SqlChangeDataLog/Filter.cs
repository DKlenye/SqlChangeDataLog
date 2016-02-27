using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlChangeDataLog
{
    public class Filter
    {
        public string idChangeLog { get; set; }
        public string date { get; set; }
        public string user { get; set; }
        public string changeType { get; set; }
        public string table { get; set; }
        public string idString { get; set; }

        public string BuildWhereClause()
        {
            if (isEmpty()) return "";

            var StringEquals = new[] {"changeType", "idString"};
            var IntEquals = new[] {"idChangeLog"};

            var clauses = new List<string>();
            var me = this;

            GetType().GetProperties().Where(x =>
            {
                var value = x.GetValue(this, null).ToString();
                return !String.IsNullOrEmpty(value) && value!="null";
            })
                .ToList().ForEach(x =>
                {
                    if (StringEquals.Contains(x.Name))
                    {
                        clauses.Add(String.Format("[{0}] = '{1}'", x.Name, x.GetValue(me, null)));
                    }
                    else if (IntEquals.Contains(x.Name))
                    {
                        clauses.Add(String.Format("[{0}] = {1}", x.Name, x.GetValue(me, null)));
                    }
                    else
                    {
                        clauses.Add(String.Format("[{0}] like '%{1}%'", x.Name, x.GetValue(me, null)));
                    }
                });

            return String.Join(" AND ", clauses);
        }

        public bool isEmpty()
        {
            var me = this;
            return GetType().GetProperties().All(x =>
            {
                var value = x.GetValue(me, null);
                return value==null || String.IsNullOrEmpty(value.ToString()) || value.ToString() == "null";
            });
        }
    
    }
}
