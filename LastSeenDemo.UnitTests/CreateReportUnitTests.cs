using System;
using System.Collections.Generic;
using LastSeenDemo;
using Xunit;

public class ReportCreatorTests
{
    [Fact]
    public void CreateReport_ReturnsExpectedResult()
    {
        // Arrange
        var metrics = new List<string> { "Total", "DailyAverage", "WeeklyAverage" };
        var dateTimeProvider = new DateTimeProvider();
        var loader = new Loader();
        var detector = new OnlineDetector(dateTimeProvider);
        var userLoader = new UserLoader(loader, "https://sef.podkolzin.consulting/api/users/lastSeen");
        var userTransformer = new UserTransformer(dateTimeProvider);
        var allUsersTransformer = new AllUsersTransformer(userTransformer);
        var worker = new Worker(userLoader, allUsersTransformer);
        var reportCreator = new ReportCreator(metrics, worker, detector);

        // Sample input data
        var from = DateTimeOffset.Now;
        var to = DateTimeOffset.Now.AddDays(-1);
        var userList = new User[] { /* Sample user data */ };

        // Act
        var result = reportCreator.CreateReport(userList, from, to);

        // Assert
        // Add your assertions here to validate the result
        Assert.NotNull(result); // For example, check if the result is not null
        // Add more assertions based on your expected result
    }
}