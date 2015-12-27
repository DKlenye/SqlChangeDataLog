using System.Collections.Generic;
using System.Linq;

namespace SqlChangeDataLog.Web.Application
{
    public class DataSerializer
    {
        public DataSerializer(IEnumerable<object> data, int? count = null , int? pos = null)
        {
            this.data = data;
            total_count = count??data.Count();
            this.pos = pos;
        }

        public IEnumerable<object> data { get; private set; }
        public int total_count { get; private set; }
        public int? pos { get; set; }
    }
}
