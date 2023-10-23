namespace LastSeenDemo;


public class MinMaxDaily
{
    private readonly OnlineDetector _onlineDetector;
    public MinMaxDaily(OnlineDetector onlineDetector)
    {
        _onlineDetector = onlineDetector;
    }

    
    public (double, double) CalculateMinMax(List<UserTimeSpan> value, DateTimeOffset from, DateTimeOffset to)
    {
        var listOnline = new List<double>();
        var minimum = 0.0;
        var maximum = 0.0;
        while (from <= to)
        {
            double dailyOnlineTime = _onlineDetector.CalculateTotalTimeForUser(value, from, from.AddDays(1));
            listOnline.Add(dailyOnlineTime);
            from = from.AddDays(1);
            minimum = listOnline.Min();
            maximum = listOnline.Max();
        }
        return (minimum, maximum);
    }
}