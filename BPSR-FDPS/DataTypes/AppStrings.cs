using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPSR_FDPS.DataTypes
{
    public static class AppStrings
    {
        public static string CurrentLocale { get; set; } = "en";//CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        public static Dictionary<string, Dictionary<string, string>> Strings = new();

        public static string GetLocalized(string key)
        {
            Strings.TryGetValue(key, out var value);
            if (value != null && value.TryGetValue(CurrentLocale, out var localizedString))
            {
                return localizedString;
            }

            // Fallback to English if current locale not found
            if (value != null && value.TryGetValue("en", out var enString))
            {
                return enString;
            }

            // Return key if no translation found
            return key;
        }
    }
}
