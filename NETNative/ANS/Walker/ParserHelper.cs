using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Walker
{
    public class ParserHelper
    {
        public static int GetIntFromString(string value)
        {
            return int.Parse(Regex.Replace(value.Trim(), @"[,]", ""));
        }


        public static string RemoveTrailingComma(StringBuilder sb)
        {

            if (sb.Length > 0)
            {
                if (sb.ToString().Substring(sb.Length - 1) == ",") { return sb.ToString().Substring(0, sb.Length - 1); }
                else return sb.ToString();
            }

            else { return sb.ToString(); }

        }
    }
}
