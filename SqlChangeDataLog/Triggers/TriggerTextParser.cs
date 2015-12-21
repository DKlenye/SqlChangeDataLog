using System.Text.RegularExpressions;

namespace SqlChangeDataLog.Triggers
{
    public class TriggerTextParser
    {
        private const string OperationPattern = @"(?<=for\s+)insert|update|delete";

        public TriggerTextParser(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }



        public string ParseOperation()
        {
            var regex = new Regex(OperationPattern,RegexOptions.IgnoreCase);
            return regex.Match(Text).ToString().ToLower();
        }
    }
}
