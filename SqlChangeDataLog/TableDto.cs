using SqlChangeDataLog.Operations;

namespace SqlChangeDataLog
{
    public class TableDto
    {
        public string Name { get; set; }
        public string[] Columns { get; set; }
        public DataOperation[] Operations { get; set; }
    }
}
