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
    public void CreateReport(List<User> userList, DateTimeOffset from, DateTimeOffset to)
    {
        foreach (var userTimeSpan in Users)
        {
            var userId = userTimeSpan;
            var user = userList.FirstOrDefault(u => u.UserId == userId);
            _worker.Users.TryGetValue(userId, out var userTime);
            if (user != null)
            {
                foreach (var metric in Metrics)
                {
                    switch (metric)
                    {
                        case "total":
                            user.Metrics["total"] = _detector.CalculateTotalTimeForUser(userTime);
                            break;
                        case "dailyAverage":
                            user.Metrics["dailyAverage"] = _detector.CalculateDailyAverageForUser(userTime);
                            break;
                        case "weeklyAverage":
                            user.Metrics["weeklyAverage"] = _detector.CalculateWeeklyAverageForUser(userTime);
                            break;
                        case "min":
                            var (min1, max1) = _minMax.CalculateMinMax(userTime, from, to);
                            user.Metrics["min"] = min1;
                            break;
                        case "max":
                            var (min2, max2) = _minMax.CalculateMinMax(userTime, from, to);
                            user.Metrics["max"] = max2;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}