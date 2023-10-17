namespace LastSeenDemo;

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

