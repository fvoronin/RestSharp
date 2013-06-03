using System;
using System.Globalization;

namespace RestSharp.Compact.Extensions
{
    public static class ParseAssistant
    {
        /// <summary>
        /// Attempts to parse the string provided into an long value. 
        /// </summary>
        /// <remarks>Returns 0 in the result parameter if the parse fails.</remarks>
        /// <param name="s">The string to attempt to parse.</param>
        /// <param name="result">The result of the parsed string, or zero if parsing failed.</param>
        /// <returns>A boolean value indicating whether or not the parse succeeded.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "s")]
        public static bool TryParse(string s, out Int64 result)
        {
            var retVal = false;
            try
            {
                result = Convert.ToInt64(s);
                retVal = true;
            }
            catch (FormatException) { result = default(Int64); }
            catch (InvalidCastException) { result = default(Int64); }
            return retVal;
        }

        public static bool TryParse(string s, NumberStyles numberStyles, CultureInfo cultureInfo, out Int64 result)
        {
            var retVal = false;
            try
            {
                result = Int64.Parse(s, numberStyles, cultureInfo.NumberFormat);
                retVal = true;
            }
            catch (FormatException) { result = 0; }
            catch (InvalidCastException) { result = 0; }
            return retVal;
        }

        public static bool TryParse(string s, NumberStyles numberStyles, CultureInfo cultureInfo, out Double result)
        {
            var retVal = false;
            try
            {
                result = Double.Parse(s, numberStyles, cultureInfo.NumberFormat);
                retVal = true;
            }
            catch (FormatException) { result = 0; }
            catch (InvalidCastException) { result = 0; }
            return retVal;
        }

        public static bool TryParse(string s, NumberStyles numberStyles, CultureInfo cultureInfo, out UInt32 result)
        {
            var retVal = false;
            try
            {
                result = UInt32.Parse(s, numberStyles, cultureInfo.NumberFormat);
                retVal = true;
            }
            catch (FormatException) { result = 0; }
            catch (InvalidCastException) { result = 0; }
            return retVal;
        }

        public static bool TryParse(string s, CultureInfo culture, DateTimeStyles dateTimeStyles, out DateTime result)
        {
            var retVal = false;
            try
            {
                result = DateTime.Parse(s, culture.DateTimeFormat, dateTimeStyles);
                retVal = true;
            }
            catch (FormatException) { result = default(DateTime); }
            catch (InvalidCastException) { result = default(DateTime); }
            return retVal;
            
        }

        public static bool TryParseExact(string s, string[] formats, CultureInfo culture, DateTimeStyles dateTimeStyles, out DateTime result)
        {
            var retVal = false;
            try
            {
                result = DateTime.ParseExact(s, formats, culture.DateTimeFormat, dateTimeStyles);
                retVal = true;
            }
            catch (FormatException) { result = default(DateTime); }
            catch (InvalidCastException) { result = default(DateTime); }
            return retVal;

        }
    }
}