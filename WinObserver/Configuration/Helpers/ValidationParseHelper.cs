using System;
using System.Text.RegularExpressions;

namespace Apparat.Helpers
{
    public static partial class ValidationParseHelper
    {
        public static bool ValidationCheck(string newHostname)
        {
            if (String.IsNullOrWhiteSpace(newHostname) || newHostname.Length <= 3)
            {
                return false;
            }
            else
            {
                string patern = @"[..+]";
                var resultRegex = Regex.IsMatch(newHostname, patern, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
                if (resultRegex)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

       
    }
}
