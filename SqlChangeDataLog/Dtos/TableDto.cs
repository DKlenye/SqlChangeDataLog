using SqlChangeDataLog.Operations;

namespace SqlChangeDataLog.Dtos
{
    public class TableDto
    {
        public string Name { get; set; }
        public string[] Columns { get; set; }
        public string KeyColumn { get; set; }
        public DataOperation[] Operations { get; set; }
    }
}
