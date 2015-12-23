using System;
using System.Collections.Generic;

namespace SqlChangeDataLog.Triggers
{
    public class Trigger
    {
        public Trigger(){}

        public Trigger(string tableName, string triggerText)
        {
            TriggerText = triggerText;
            TableName = tableName;

            if (!String.IsNullOrEmpty(TriggerText))
            {
                var parser = new TriggerTextParser(triggerText);
                Operation = parser.ParseOperation();
                Columns = parser.ParseColumns();
                ExtendedLogic = parser.ParseExtendedLogic();
            }

        }
        
        public string TableName { get; set; }
        public string LogTableName { get; set; }
        public string Operation { get; set; }
        public IEnumerable<string> Columns { get; set; }
        public string TriggerText { get; set; }
        public string ExtendedLogic { get; set; }
    }
}
