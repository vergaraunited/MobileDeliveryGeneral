using System;
using System.Linq;

namespace UMDGeneral.Utilities
{
    public static class Converters
    {
        public static long ToJulian(DateTime dateTime)
        {
            int day = dateTime.Day;
            int month = dateTime.Month;
            int year = dateTime.Year;

            if (month < 3)
            {
                month = month + 12;
                year = year - 1;
            }

            return day + (153 * month - 457) / 5 + 365 * year + (year / 4) - (year / 100) + (year / 400) + 1721119;
        }

        public static string FromJulian(this long julianDate, string format= "dd/MM/yyyy")
        {
            var date = new DateTime(1800, 12, 28, 0, 0, 0);
            var theDate = date.AddDays(julianDate);

            // example format "dd/MM/yyyy"
            return theDate.ToString(format);
        }
        public static DateTime FromJulian(this long julianDate)
        {
            var date = new DateTime(1800, 12, 28, 0, 0, 0);
            var theDate = date.AddDays(julianDate);

            // example format "dd/MM/yyyy"
            return theDate;
        }
        public static byte[] ParseKey(string key)
        {
            int spaces = key.Count(x => x == ' ');
            var digits = new String(key.Where(Char.IsDigit).ToArray());

            var value = (Int32)(Int64.Parse(digits) / spaces);

            byte[] result = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);
            return result;
        }

        public delegate void ActionDlg(Action<bool> action);
        public delegate void CanExecuteDlg(Func<object, bool> canExec);
    }
}
