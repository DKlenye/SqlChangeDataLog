using System.Collections.Generic;
using SqlChangeDataLog.Triggers;

namespace SqlChangeDataLog.Dtos
{
    public class TableDto
    {
        public string Name { get; set; }
        public IEnumerable<string> Columns { get; set; }
        public IEnumerable<string> KeyColumns { get; set; }

        public Trigger Insert { get; set; }
        public Trigger Update { get; set; }
        public Trigger Delete { get; set; }
    }
}
