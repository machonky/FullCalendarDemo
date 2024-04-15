var weekDays = new HashSet<DayOfWeek> {
    DayOfWeek.Monday,
    DayOfWeek.Tuesday,
    DayOfWeek.Wednesday,
    DayOfWeek.Thursday,
    DayOfWeek.Friday,
};

DateOnly startDate = new DateOnly(year:2024, month:01, day: 01);
DateOnly endDate = new DateOnly(year: 2024, month: 12, day: 31);
for(DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
{
    if (weekDays.Contains(date.DayOfWeek))
    {
        int dateIndex = date.Year*10000+date.Month*100+date.Day;
        Console.WriteLine($"INSERT INTO BusinessDays ( Id ) VALUES ({dateIndex});");
    }
    //INSERT INTO BusinessDays ( Id ) VALUES ('Id');

}
