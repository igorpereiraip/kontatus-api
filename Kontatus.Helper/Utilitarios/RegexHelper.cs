using System.Text.RegularExpressions;

namespace ConsigIntegra.Helper.Utilitarios
{
    public static class RegexHelper
    {
        public static Regex GetRegex(string expression)
        {
            var regex = new Regex(expression, RegexOptions.IgnoreCase);
            return regex;
        }
    }

}
