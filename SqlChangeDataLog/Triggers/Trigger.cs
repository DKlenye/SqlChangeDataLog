using System.Collections.Generic;

namespace SqlChangeDataLog.Triggers
{
    public class Trigger
    {
        public Trigger(string tableName, string triggerText)
        {
            TriggerText = triggerText;
            TableName = tableName;
            Operation = "insert";
        }


        public string TableName { get; private set; }
        public string LogTableName { get; set; }
        public string Operation { get; set; }
        public IEnumerable<string> Columns { get; set; }
        public string TriggerText { get; private set; }
    }
}
