using System.Text.Json;

namespace LastSeenDemo;


public class ReportManager
{
    private List<ReportConfiguration> reports = new List<ReportConfiguration>();
    private string reportsFilePath = "reports.json";

    public List<ReportConfiguration> Reports
    {
        get { return reports; }
        set { reports = value; }
    }

    public ReportManager(string reportsJson)
    {
        LoadReports();
    }

    public void AddReport(ReportConfiguration report)
    {
        reports.Add(report);
        SaveReports();
    }

    private void LoadReports()
    {
        if (File.Exists(reportsFilePath))
        {
            var json = File.ReadAllText(reportsFilePath);
            reports = JsonSerializer.Deserialize<List<ReportConfiguration>>(json);
        }
    }

    private void SaveReports()
    {
        var json = JsonSerializer.Serialize(reports);
        File.WriteAllText(reportsFilePath, json);
    }
}