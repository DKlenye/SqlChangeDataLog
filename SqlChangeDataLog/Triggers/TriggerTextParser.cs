using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SqlChangeDataLog.Triggers
{
    public class TriggerTextParser
    {
        private const string OperationPattern = @"(?<=for\s+)insert|update|delete";
        private const string SelectXmlPattern = @"(?<=set\s+@xml\s+=\s+[(]).*?(?=from)";
        private const string RemoveSelectPattern = @"select\s+(top\s+\d+\s+)?";

        public TriggerTextParser(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }


        public IEnumerable<string> ParseColumns()
        {
            var regex1 = new Regex(SelectXmlPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var selectXml = regex1.Match(Text).ToString();

            var regex2 = new Regex(RemoveSelectPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var columns = regex2.Replace(selectXml, "");

            return columns.Split(',');
        }

        public string ParseOperation()
        {
            var regex = new Regex(OperationPattern,RegexOptions.IgnoreCase);
            return regex.Match(Text).ToString().ToLower();
        }
    }
}
