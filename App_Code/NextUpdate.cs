using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for NextUpdate
/// </summary>
public static class NextUpdate
{
    const int STARTHOUR = 9; //9
    const int STARTMINUTE = 31; //31
    const int ENDHOUR = 16; //16
    const int DEFAULTUPDMINUTES = 15; //15

    static bool IsWorkHours(TimeSpan tm)
    {
        return tm.Hours >= STARTHOUR && tm.Hours <= ENDHOUR;
    }

    static bool IsWorkDay(DateTime dt)
    {
        return dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday;
    }

    static DateTime GetNextWorkDay(DateTime src)
    {
        do
        {
            src = src.AddDays(1);
        } while (!IsWorkDay(src));
        return src;
    }

    static DateTime GetNextOpenTime(DateTime src)
    {
        if (!IsWorkDay(src) || src.Hour > ENDHOUR)
            src = GetNextWorkDay(src.Date);

        return new DateTime(src.Year, src.Month, src.Day, STARTHOUR, STARTMINUTE, 0);
    }

    public static DateTime GetNextUpdateDT()
    {
        //DateTime dt = DateTime.Now.AddMinutes(DEFAULTUPDMINUTES);
        //DateTime dt = DateTime.Now.AddMinutes(DEFAULTUPDMINUTES).AddHours(-4);
        DateTime dt = ConvertToEST(DateTime.UtcNow).AddMinutes(DEFAULTUPDMINUTES);

        if (!IsWorkDay(dt.Date) || !IsWorkHours(dt.TimeOfDay))
            dt = GetNextOpenTime(dt);

        return dt;
    }

    public static DateTime ConvertToEST(DateTime dt)
    {
        return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dt, "Eastern Standard Time");
    }
}