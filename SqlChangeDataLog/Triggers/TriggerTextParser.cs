using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SqlChangeDataLog.Triggers
{
    public class TriggerTextParser
    {
        private const string OperationPattern = @"(?<=for\s+)insert|update|delete";
        private const string SelectXmlPattern = @"(?<=[(]\s*)SELECT.*FOR XML AUTO";
        private const string SelectColumnsPattern = @"^.*?(?=\s*\n?\s*from)";
        private const string RemoveSelectPattern = @"\s*select\s+(top\s+\d+\s+)?";
        private const string RemoveBracketsPattern = @"[\[\]]";

        public TriggerTextParser(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }


        public IEnumerable<string> ParseColumns()
        {
            var regex1 = CreateRegex(SelectXmlPattern);
            var selectXml = regex1.Match(Text).ToString();

            var regex2 = CreateRegex(SelectColumnsPattern);
            var selectColumns = regex2.Match(selectXml).ToString();
            
            var regex3 = CreateRegex(RemoveSelectPattern);
            var columnsWithBrackets = regex3.Replace(selectColumns, "");

            var regex4 = CreateRegex(RemoveBracketsPattern);
            var columns = regex4.Replace(columnsWithBrackets, "");

            return columns.Split(',');
        }

        public string ParseOperation()
        {
            var regex = new Regex(OperationPattern,RegexOptions.IgnoreCase);
            return regex.Match(Text).ToString().ToLower();
        }

        
        Regex CreateRegex(string Pattern)
        {
            return new Regex(Pattern,RegexOptions.IgnoreCase|RegexOptions.Singleline);
        }

        public string ParseExtendedLogic()
        {
            return "";
        }
    }
}
