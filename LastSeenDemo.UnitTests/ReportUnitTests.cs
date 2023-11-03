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
        
        const string reportName = "TestReport";
        var dateTimeProvider = new DateTimeProvider();
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

    [Fact]
    public void ReportItem_Properties_ShouldBeSettableAndGettable()
    {
        // Arrange
        var reportItem = new ReportItem();
        var userId = Guid.NewGuid();
        var total = 10.0;
        var dailyAverage = 5.0;
        var weeklyAverage = 20.0;
        var min = 2.0;
        var max = 25.0;

        // Act
        reportItem.UserId = userId;
        reportItem.Total = total;
        reportItem.DailyAverage = dailyAverage;
        reportItem.WeeklyAverage = weeklyAverage;
        reportItem.Min = min;
        reportItem.Max = max;

        // Assert
        Assert.Equal(userId, reportItem.UserId);
        Assert.Equal(total, reportItem.Total);
        Assert.Equal(dailyAverage, reportItem.DailyAverage);
        Assert.Equal(weeklyAverage, reportItem.WeeklyAverage);
        Assert.Equal(min, reportItem.Min);
        Assert.Equal(max, reportItem.Max);
    }
    
    [Fact]
    public void ReportRequest_Properties_ShouldBeSettableAndGettable()
    {
        // Arrange
        var metrics = new List<string> { "Metric1", "Metric2" };
        var users = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var from = DateTimeOffset.Now;
        var to = DateTimeOffset.Now.AddDays(7);

        var reportRequest = new ReportRequest(metrics, users, from, to);

        // Act (no action is required for this test)

        // Assert
        Assert.Equal(metrics, reportRequest.Metrics);
        Assert.Equal(users, reportRequest.Users);
        Assert.Equal(from, reportRequest.From);
        Assert.Equal(to, reportRequest.To);
    }
}
