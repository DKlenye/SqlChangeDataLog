using System.Collections.Generic;
using SqlChangeDataLog.Operations;

namespace SqlChangeDataLog.Dtos
{
    public class TableDto
    {
        public string Name { get; set; }
        public IEnumerable<string> Columns { get; set; }
        public IEnumerable<string> KeyColumns { get; set; }
    }
}
