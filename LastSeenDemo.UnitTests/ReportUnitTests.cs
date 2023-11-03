namespace LastSeenDemo.UnitTests;

using System;
using System.Collections.Generic;
using LastSeenDemo;
using Xunit;

public class ReportTests
{
    [Fact]
    public void Report_Constructor_Sets_Properties()
    {
        // Arrange
        var dateTimeProvider = new DateTimeProvider();
        const string reportName = "TestReport";
        var loader = new Loader();
        var userTransformer = new UserTransformer(dateTimeProvider);
        var allUsersTransformer = new AllUsersTransformer(userTransformer);
        var userLoader = new UserLoader(loader, "https://sef.podkolzin.consulting/api/users/lastSeen");
        List<Guid> users = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        List<string> metrics = new List<string> { "Metric1", "Metric2" };
        var worker = new Worker(userLoader, allUsersTransformer);
        var onlineDetector = new OnlineDetector(dateTimeProvider);

        // Act
        var report = new Report(reportName, users, metrics, worker, onlineDetector);

        // Assert
        Assert.Equal(reportName, report.Name);
        Assert.Equal(users, report.Users);
        Assert.Equal(metrics, report.Metrics);
    }
}
