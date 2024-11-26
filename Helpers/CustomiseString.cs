using System.Globalization;
using System.Reflection;

namespace Demo2CoreAPICrud.Helpers
{
    public class CustomiseString
    {
        public static string CapitalizeFirstLetter(string input, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(input)) return input;

            if(input.Trim() == string.Empty) return input;
            
            input = input.Trim().ToLower();
            
            var textInfo = culture.TextInfo;
            
            return textInfo.ToTitleCase(input);
        }

        public static bool HasEmptyString(object dto)
        {
            foreach (PropertyInfo property in dto.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    string value = (string)property.GetValue(dto)!;
                    if (string.IsNullOrEmpty(value.Trim()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
