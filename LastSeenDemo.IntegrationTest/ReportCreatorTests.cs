using System;
using LastSeenDemo;
using Xunit;

namespace LastSeenDemo.IntegrationTest;

public class ReportCreatorTests
{
    [Fact]
    public void FirstSeenReport_ShouldReturnFirstSeenDates_ForGivenUsers()
    {
        // Arrange
        // Create an instance of Worker, OnlineDetector, and other required dependencies
        var worker = new Worker(new UserLoader(null, null), new AllUsersTransformer(null));
        var onlineDetector = new OnlineDetector(null);
        var users = new User[]
        {
            new User
            {
                UserId = Guid.NewGuid(),
                Nickname = "User1",
                FirstSeen = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(10)),
            },
            new User
            {
                UserId = Guid.NewGuid(),
                Nickname = "User2",
                FirstSeen = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(5)),
            },
        };

        // Create an instance of ReportCreator
        var reportCreator = new ReportCreator(new List<string> { "Total" }, worker, onlineDetector, users.Select(u => u.UserId).ToList());

        // Act
        var firstSeenReport = reportCreator.FirstSeenReport(users);

        // Assert
        Assert.NotNull(firstSeenReport);
        Assert.Equal(users.Length, firstSeenReport.Count);

        // Validate the contents of the response
        foreach (var user in users)
        {
            var userReport = firstSeenReport.Find(u => u["userId"].Equals(user.UserId));
            Assert.NotNull(userReport);
            Assert.Equal(user.Nickname, userReport["username"]);
            Assert.Equal(user.FirstSeen, userReport["firstSeen"]);
        }
    }
}