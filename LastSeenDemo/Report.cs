namespace LastSeenDemo;

public class Report
{
    public string Name { get; set; }
    public List<Guid> Users { get; set; }
    public List<string> Metrics { get; set; }
    private readonly OnlineDetector _detector;

    public Report(string reportName, List<Guid> users, List<string> metrics, Worker worker, OnlineDetector onlineDetector)
    {
        Name = reportName;
        Metrics = metrics;
        _detector = onlineDetector;
        new MinMaxDaily(_detector);
        Users = users;
    }
}

public class ReportCreator
{
    public List<string>? Metrics { get; set; }
    public Worker Worker { get; set; }
    private readonly OnlineDetector _detector;
    private readonly MinMaxDaily _minMax;
    private readonly List<Guid>? _userGuids;
    
    public ReportCreator(List<string>? metrics, Worker worker, OnlineDetector onlineDetector, List<Guid>? users)
    {
        Metrics = metrics;
        Worker = worker;
        _detector = onlineDetector;
        _minMax = new MinMaxDaily(_detector);
        _userGuids = users;
    }
    
    public Dictionary<string, object> CreateReport(User[] userList, DateTimeOffset from,
        DateTimeOffset to)
    {
        var userMetrics = new List<UserMetricsDto>();
        var metricsDescription = new Dictionary<string, string>();

        if (Worker.Users != null)
        {
            foreach (var userId in _userGuids!)
            {
                if (Worker.Users.TryGetValue(userId, out var userTimeSpans))
                {
                    var user = Array.Find(userList, u => u.UserId == userId);
                    if (user != null)
                    {
                        user.Metrics = new Dictionary<string, double>();
                        foreach (var metric in Metrics!)
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
                                    var (min, _) = _minMax.CalculateMinMax(userTimeSpans, from, to);
                                    user.Metrics[metric] = min;
                                    if (!metricsDescription.ContainsKey(metric))
                                    {
                                        metricsDescription.Add("Min",
                                            "<minimum daily online time of the user (in seconds)>");
                                    }

                                    break;
                                case "Max":
                                    var (_, max2) = _minMax.CalculateMinMax(userTimeSpans, from, to);
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
        }

        var response = new Dictionary<string, object>
        {
            { "users", userMetrics },
            { "Metrics", metricsDescription },
        };

        return response;
    }

    public List<Dictionary<string, object?>> FirstSeenReport(User[] users)
    {
        var response = new List<Dictionary<string, object?>>();

        foreach (var user in users)
        {
            user.FirstSeen = Worker.FindFirstSeenDate(user.UserId);
            var userMetrics = new Dictionary<string, object?>
            {
                { "username", user.Nickname },
                { "userId", user.UserId },
                { "firstSeen", user.FirstSeen }
            };

            response.Add(userMetrics);
        }

        return response;
    }

}
