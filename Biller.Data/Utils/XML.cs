using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Biller.Data.Utils
{
    public class XML
    {
        static Lazy<Regex> ControlChars = new Lazy<Regex>(() => new Regex("[\x00-\x1f]", RegexOptions.Compiled));

        private static string FixData_Replace(Match match)
        {
            if ((match.Value.Equals("\t")) || (match.Value.Equals("\n")) || (match.Value.Equals("\r")))
                return match.Value;

            return "&#" + ((int)match.Value[0]).ToString("X4") + ";";
        }

        public static string Fix(object data, MatchEvaluator replacer = null)
        {
            if (data == null) return null;
            string fixed_data;
            if (replacer != null) fixed_data = ControlChars.Value.Replace(data.ToString(), replacer);
            else fixed_data = ControlChars.Value.Replace(data.ToString(), FixData_Replace);
            return fixed_data;
        }
    }
}
