namespace Dima.Core.Common.Extensions;

public static class DateTimeExtension
{
    public static DateTime FirstDay(this DateTime date)
    {
        return new(date.Year, date.Month, 1);
    }
    
    public static DateTime LastDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
    }
}