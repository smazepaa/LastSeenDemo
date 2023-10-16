namespace LastSeenDemo;


public class MinMaxDaily
{
    public (double, double) CalculateMinMax(List<UserTimeSpan> value, DateTimeOffset from, DateTimeOffset to)
    {
        var listOnline = new List<double>();

        while (from <= to)
        {
            var dateTimeProvider = new DateTimeProvider();
            var onlineDetector = new OnlineDetector(dateTimeProvider);
            double dailyOnlineTime = onlineDetector.CalculateTotalTimeForUser(value, from, from.AddDays(1));

            listOnline.Add(dailyOnlineTime);

            from = from.AddDays(1);
        }

        listOnline.Sort(); // sorted in ascending order
        return (listOnline[0], listOnline[-1]);
    }
}