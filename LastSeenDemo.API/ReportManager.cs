using System.Text.Json;
namespace LastSeenDemo;
using System.Text.Json;

public class ReportConfiguration
{
    public string Name { get; set; }
    public List<string> Metrics { get; set; }
    public List<Guid> Users { get; set; }
}

public class ReportManager
{
    private readonly string _reportsFilePath;
    private List<ReportConfiguration> _reports;

    public ReportManager(string filePath)
    {
        _reportsFilePath = filePath;
        LoadReports();
    }

    public List<ReportConfiguration> Reports
    {
        get { return _reports; }
        set { _reports = value; }
    }

    public void AddReport(ReportConfiguration report)
    {
        _reports.Add(report);
        SaveReports();
    }

    private void LoadReports()
    {
        if (File.Exists(_reportsFilePath))
        {
            var json = File.ReadAllText(_reportsFilePath);
            _reports = JsonSerializer.Deserialize<List<ReportConfiguration>>(json);
        }
    }

    public void SaveReports()
    {
        var json = JsonSerializer.Serialize(_reports);
        File.WriteAllText(_reportsFilePath, json);
    }
}