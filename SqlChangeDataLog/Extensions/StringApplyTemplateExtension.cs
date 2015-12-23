using System.Linq;
using System.Text.RegularExpressions;

namespace SqlChangeDataLog.Extensions
{
    public static class StringApplyTemplateExtension
    {
        public static string ApplyTemplate(this string tpl, object o)
        {
            var rezult = tpl;
            var map = o.GetType().GetProperties().ToDictionary(x => x.Name);
            var regex = new Regex("{.+?}");
            foreach (Match match in regex.Matches(tpl))
            {
                var index = match.Value.Replace("{", "").Replace("}", "");
                if (map.ContainsKey(index))
                {
                    rezult = rezult.Replace(match.Value, map[index].GetValue(o, null).ToString());
                }
            }
            return rezult;
        }

    }
}
