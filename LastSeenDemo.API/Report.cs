namespace LastSeenDemo;
using LastSeenDemo;

public class ReportRequest
{
    public List<string> Metrics { get; set; }
    public List<Guid> Users { get; set; }
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
}

public class ReportItem
{
    public Guid UserId { get; set; }
    public double Total { get; set; }
    public double DailyAverage { get; set; }
    public double WeeklyAverage { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
}

public class Report
{
    public string Name { get; set; }
    public List<Guid> Users { get; set; }
    public List<string> Metrics { get; set; }
    private readonly Worker _worker;
    private readonly OnlineDetector _detector;
    private MinMaxDaily _minMax;
    
    public Report(string reportName, List<Guid> users, List<string> metrics, Worker worker, OnlineDetector onlineDetector)
    {
        Name = reportName;
        Metrics = metrics;
        _worker = worker;
        _detector = onlineDetector;
        _minMax = new MinMaxDaily(_detector);
        Users = users;
    }
    public List<Dictionary<string, object>> CreateReport(DateTimeOffset from, DateTimeOffset to)
    {
        var report = new List<Dictionary<string, object>>();

        foreach (var userId in Users)
        {
            if (_worker.Users.TryGetValue(userId, out var user))
            {
                var userReport = new Dictionary<string, object>
                {
                    { "UserId", userId }
                };

                foreach (var metric in Metrics)
                {
                    switch (metric)
                    {
                        case "total":
                            userReport["total"] = _detector.CalculateTotalTimeForUser(user);
                            break;
                        case "dailyAverage":
                            userReport["dailyAverage"] = _detector.CalculateDailyAverageForUser(user);
                            break;
                        case "weeklyAverage":
                            userReport["weeklyAverage"] = _detector.CalculateWeeklyAverageForUser(user);
                            break;
                        case "min":
                            var (min1, max1) = _minMax.CalculateMinMax(user, from, to);
                            userReport["min"] = min1;
                            break;
                        case "max":
                            var (min2, max2) = _minMax.CalculateMinMax(user, from, to);
                            userReport["max"] = max2;
                            break;
                        default:
                            break;
                    }
                }
                report.Add(userReport);
            }
        }
        return report;
    }
}