using System.Text.Json;
namespace LastSeenDemo.UnitTests;

public class ReportManagerUnitTests
{
    private ReportManager _manager = new ("test_reports.json");

    [Fact]
    public void AddReport_Should_Add_Report()
    {
        // Arrange
        var initialReportCount = _manager.Reports.Count;
        var newReport = new ReportConfiguration
        {
            Name = "Test Report",
            Metrics = new List<string> { "Metric1", "Metric2" },
            Users = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        };

        // Act
        _manager.AddReport(newReport);

        // Assert
        Assert.Equal(initialReportCount + 1, _manager.Reports.Count);
    }

    [Fact]
    public void LoadReports_Should_Load_Reports_From_File()
    {
        // Arrange
        var newReport = new ReportConfiguration
        {
            Name = "Test Report",
            Metrics = new List<string> { "Metric1", "Metric2" },
            Users = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        };
        _manager.AddReport(newReport);

        // Assert
        Assert.NotEmpty(_manager.Reports);
    }

    [Fact]
    public void SaveReports_Should_Save_Reports_To_File()
    {
        // Arrange
        var newReport = new ReportConfiguration
        {
            Name = "Test Report",
            Metrics = new List<string> { "Metric1", "Metric2" },
            Users = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        };
        _manager.AddReport(newReport);

        // Act
        _manager.SaveReports();

        // Assert
        Assert.NotEmpty(_manager.Reports);
    }

    // Clean up the test file after all tests have run
    public ReportManagerUnitTests()
    {
        if (File.Exists("test_reports.json"))
        {
            File.Delete("test_reports.json");
        }
    }
}