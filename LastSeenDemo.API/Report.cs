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
    
}

public class ReportCreator
{
    public List<string> Metrics { get; set; }
    private readonly Worker _worker;
    private readonly OnlineDetector _detector;
    private MinMaxDaily _minMax;
    private List<Guid> _userGuids;
    
    public ReportCreator(List<string> metrics, Worker worker, OnlineDetector onlineDetector)
    {
        Metrics = metrics;
        _worker = worker;
        _detector = onlineDetector;
        _minMax = new MinMaxDaily(_detector);
        _userGuids = new List<Guid>
        {
            new Guid("2fba2529-c166-8574-2da2-eac544d82634"),
            new Guid("8b0b5db6-19d6-d777-575e-915c2a77959a"),
            new Guid("e13412b2-fe46-7149-6593-e47043f39c91"),
            new Guid("cbf0d80b-8532-070b-0df6-a0279e65d0b2"),
            new Guid("de5b8815-1689-7c78-44e1-33375e7e2931")
        };
    }
    
    public Dictionary<string, object> CreateReport(User[] userList, DateTimeOffset from,
        DateTimeOffset to)
    {
        var userMetrics = new List<UserMetricsDto>();
        var metricsDescription = new Dictionary<string, string>();

        foreach (var userId in _userGuids)
        {
            if (_worker.Users.TryGetValue(userId, out var userTimeSpans))
            {
                var user = userList.FirstOrDefault(u => u.UserId == userId);
                if (user != null)
                {
                    user.Metrics = new Dictionary<string, double>();
                    foreach (var metric in Metrics)
                    {
                        switch (metric)
                        {
                            case "Total":
                                user.Metrics[metric] = _detector.CalculateTotalTimeForUser(userTimeSpans);
                                if (!metricsDescription.ContainsKey(metric))
                                {
                                    metricsDescription.Add("Total",
                                        "<total time that user was online during selected date range (in seconds)>");
                                }

                                break;
                            case "DailyAverage":
                                user.Metrics[metric] = _detector.CalculateDailyAverageForUser(userTimeSpans);
                                if (!metricsDescription.ContainsKey(metric))
                                {
                                    metricsDescription.Add("DailyAverage",
                                        "<average time that user was online during the day (in seconds)>");
                                }

                                break;
                            case "WeeklyAverage":
                                user.Metrics[metric] = _detector.CalculateWeeklyAverageForUser(userTimeSpans);
                                if (!metricsDescription.ContainsKey(metric))
                                {
                                    metricsDescription.Add("WeeklyAverage",
                                        "<average time that user was online during the week (in seconds)>");
                                }

                                break;
                            case "Min":
                                var (min, max) = _minMax.CalculateMinMax(userTimeSpans, from, to);
                                user.Metrics[metric] = min;
                                if (!metricsDescription.ContainsKey(metric))
                                {
                                    metricsDescription.Add("Min",
                                        "<minimum daily online time of the user (in seconds)>");
                                }

                                break;
                            case "Max":
                                var (min2, max2) = _minMax.CalculateMinMax(userTimeSpans, from, to);
                                user.Metrics[metric] = max2;
                                if (!metricsDescription.ContainsKey(metric))
                                {
                                    metricsDescription.Add("Max",
                                        "<maximum daily online time of the user (in seconds)>");
                                }

                                break;
                        }
                    }

                    var userMetricsDto = new UserMetricsDto
                    {
                        UserId = user.UserId,
                        Metrics = user.Metrics
                    };

                    userMetrics.Add(userMetricsDto);
                }
            }
        }

        var response = new Dictionary<string, object>
        {
            { "users", userMetrics },
            { "Metrics", metricsDescription },
        };

        return response;
    }
}