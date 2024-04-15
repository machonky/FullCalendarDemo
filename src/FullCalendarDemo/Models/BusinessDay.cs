namespace FullCalendarDemo.Models;

public class BusinessDay
{
    public int Id { get; set; }
}

public static class BusinessDayExtensions
{
    public static DateOnly FromDateIndex(this int dateIndex) => new DateOnly(year: dateIndex / 10000, month: (dateIndex / 100) % 100, day: dateIndex % 100);
    public static DateTime DateTimeFromDateIndex(this int dateIndex) => new DateTime(year: dateIndex / 10000, month: (dateIndex / 100) % 100, day: dateIndex % 100);
    public static int ToDateIndex(this DateOnly date) => int.Parse(date.ToString("yyyyMMdd"));
    public static int DateTimeToDateIndex(this DateTime date) => int.Parse(date.ToString("yyyyMMdd"));
}