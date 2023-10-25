using System.Text.Json;
using LastSeenDemo;

namespace LastSeenDemo;

public class ReportConfiguration
{
    public string Name { get; set; }
    public List<string> Metrics { get; set; }
    public List<Guid> Users { get; set; }
}

public class ReportManager
{
    private List<ReportConfiguration> reports;
    private readonly string _reportsFilePath;

    public ReportManager(string filePath)
    {
        _reportsFilePath = filePath;
        LoadReports();
    }

    public List<ReportConfiguration> Reports
    {
        get { return reports; }
        set { reports = value; }
    }

    public void AddReport(ReportConfiguration report)
    {
        reports.Add(report);
        SaveReports();
    }

    private void LoadReports()
    {
        if (File.Exists(_reportsFilePath))
        {
            var json = File.ReadAllText(_reportsFilePath);
            reports = JsonSerializer.Deserialize<List<ReportConfiguration>>(json);
        }
    }

    private void SaveReports()
    {
        var json = JsonSerializer.Serialize(reports);
        File.WriteAllText(_reportsFilePath, json);
    }
}
