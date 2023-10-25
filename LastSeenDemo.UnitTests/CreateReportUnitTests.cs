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
        var userGuids = new List<Guid>
        {
            new Guid("2fba2529-c166-8574-2da2-eac544d82634"),
            new Guid("8b0b5db6-19d6-d777-575e-915c2a77959a"),
            new Guid("e13412b2-fe46-7149-6593-e47043f39c91"),
            new Guid("cbf0d80b-8532-070b-0df6-a0279e65d0b2"),
            new Guid("de5b8815-1689-7c78-44e1-33375e7e2931")
        };
        var metrics = new List<string> { "Total", "DailyAverage", "WeeklyAverage" };
        var dateTimeProvider = new DateTimeProvider();
        var loader = new Loader();
        var detector = new OnlineDetector(dateTimeProvider);
        var userLoader = new UserLoader(loader, "https://sef.podkolzin.consulting/api/users/lastSeen");
        var userTransformer = new UserTransformer(dateTimeProvider);
        var allUsersTransformer = new AllUsersTransformer(userTransformer);
        var worker = new Worker(userLoader, allUsersTransformer);
        var reportCreator = new ReportCreator(metrics, worker, detector, userGuids);

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