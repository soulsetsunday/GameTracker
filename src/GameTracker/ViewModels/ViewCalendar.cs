using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace CalendarTesting2.ViewModels
{
    public class ViewCalendar
    {
        public static DateTime thisDay = DateTime.Today;
        public static Calendar myCal = CultureInfo.InvariantCulture.Calendar;
        public static int daysinmonth = myCal.GetDaysInMonth(myCal.GetYear(thisDay), myCal.GetMonth(thisDay));
        public static DateTime startOfMonth = new DateTime(myCal.GetYear(thisDay), myCal.GetMonth(thisDay), 1);
        public List<int> dates = GenCalendar();
        //startOfMonth: first day of this month
        //topWeek: first sunday on row of first day of the month

        public static List<int> GenCalendar()
        {
            List<int> returnList = new List<int>();

            DateTime topWeek = startOfMonth;
            int count = 0;
            while (topWeek.ToString("ddd") != "Sun")
            {
                topWeek = topWeek.AddDays(-1);
                count++;
            }

            for (int i = 0; i < (daysinmonth + count); i++)
            {
                if (i < count)
                {
                    returnList.Add(0);
                    topWeek = topWeek.AddDays(1);
                }
                else
                {
                    returnList.Add(topWeek.Day);
                    topWeek = topWeek.AddDays(1);
                }
            }

            return returnList;
        }


    }
}
