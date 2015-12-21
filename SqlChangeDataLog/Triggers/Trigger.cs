using System.Collections.Generic;

namespace SqlChangeDataLog.Triggers
{
    public class Trigger
    {

        private TriggerTextParser _parser;

        public Trigger(string tableName, string triggerText)
        {

            _parser = new TriggerTextParser(triggerText);
            
            TriggerText = triggerText;
            TableName = tableName;

            Operation = _parser.ParseOperation();

        }


        public string TableName { get; private set; }
        public string LogTableName { get; set; }
        public string Operation { get; set; }
        public IEnumerable<string> Columns { get; set; }
        public string TriggerText { get; private set; }
    }
}
