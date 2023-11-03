using System.Text.Json;
namespace LastSeenDemo;
using System.Text.Json;

public class ReportConfiguration
{
    public string Name { get; set; } = "";
    public List<string>? Metrics { get; set; } = new List<string>();
    public List<Guid>? Users { get; set; } = new List<Guid>();
}

public class ReportManager
{
    private readonly string _reportsFilePath;

    public ReportManager(string filePath)
    {
        _reportsFilePath = filePath;
        LoadReports();
    }

    public List<ReportConfiguration>? Reports { get; set; } = new();

    public void AddReport(ReportConfiguration report)
    {
        Reports!.Add(report);
        SaveReports();
    }

    private void LoadReports()
    {
        if (File.Exists(_reportsFilePath))
        {
            var json = File.ReadAllText(_reportsFilePath);
            Reports = JsonSerializer.Deserialize<List<ReportConfiguration>>(json);
        }
    }

    public void SaveReports()
    {
        var json = JsonSerializer.Serialize(Reports);
        File.WriteAllText(_reportsFilePath, json);
    }
}