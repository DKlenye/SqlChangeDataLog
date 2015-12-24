using System;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using SqlChangeDataLog.Dtos;
using SqlChangeDataLog.Extensions;

namespace SqlChangeDataLog.Web.Handlers
{

    public class ChangeLogParams : Context
    {
        public int from { get; set; }
        public int count { get; set; }
        public string filter { get; set; }
        public string sort { get; set; }

        public Sort Sort
        {
            get
            {
                if(String.IsNullOrEmpty(sort)) return null;
                return JsonConvert.DeserializeObject<Sort>(sort);
            }
        }

        public Filter Filter
        {
            get
            {
                if (String.IsNullOrEmpty(filter)) return null;
                return JsonConvert.DeserializeObject<Filter>(filter);
            }
        }

    }

    public class SelectChangeLog : Handler<ChangeLogParams>
    {
        protected override object Process(ChangeLogParams parameters)
        {
            using (IDbConnection dbConnection = Connect())
            {

                var query = new Queries.SelectChangeLog();

                var context = ReadParams<ChangeLogParams>();
                var data = dbConnection.Query<ChangeLogDto>(query.ByParams(context.LogTable,context.Filter,context.Sort,context.from,context.count));


                int? pos = null;
                int? count = null;
                if (context.from != 0 )
                {
                    pos = context.from;
                }
                
                {
                    count = dbConnection.Query<int>(query.CountByParams(context.LogTable, context.Filter)).FirstOrDefault();
                }

                return new DataSerializer(data, count, pos);
            }
        }
    }
}