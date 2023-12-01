using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Extensions
{
    public static class DateConvertorExtension
    {
     
        public static string ToShamsi(this DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            if (date!=null)
            {

                return $"{pc.GetYear(date)}/{pc.GetMonth(date)}/{pc.GetDayOfMonth(date)}";
            }
            return "";
        }

        public static string TimeAgo(this DateTime dateTime)
        {
            string result = string.Empty;

            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} ثانیه پیش", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("حدود {0} دقیقه پیش", timeSpan.Minutes) :
                    "حدود 1 دقیقه پیش";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("حدود {0} ساعت پیش", timeSpan.Hours) :
                    "حدود 1 ساعت پیش";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("حدود {0} روز پیش", timeSpan.Days) :
                    "دیروز";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("حدود {0} ماه پیش", timeSpan.Days / 30) :
                    "حدود یک ماه پیش";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("حدود {0} سال پیش", timeSpan.Days / 365) :
                    "حدود یک سال پیش";
            }

            return result;
        }
    }
}
