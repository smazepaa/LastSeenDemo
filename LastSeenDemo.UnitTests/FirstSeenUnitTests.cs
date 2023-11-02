using System;
using System.Collections.Generic;
using System.Linq;
using LastSeenDemo;
using Xunit;
namespace LastSeenDemo.UnitTests;

public class FirstSeenUnitTests
{
    [Fact]
    public void FindFirstSeenDate_ShouldReturnFirstSeenDate_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userTimeSpans = new List<UserTimeSpan>
        {
            new UserTimeSpan { Login = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero) },
            new UserTimeSpan { Login = new DateTimeOffset(2023, 2, 1, 0, 0, 0, TimeSpan.Zero) }
        };

        var worker = new Worker(null, null);
        worker.Users[userId] = userTimeSpans;

        // Act
        var firstSeenDate = worker.FindFirstSeenDate(userId);

        // Assert
        Assert.NotNull(firstSeenDate);
        Assert.Equal(new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero), firstSeenDate);
    }

    [Fact]
    public void FindFirstSeenDate_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var worker = new Worker(null, null);
        var userId = Guid.NewGuid();

        // Act
        var firstSeenDate = worker.FindFirstSeenDate(userId);

        // Assert
        Assert.Null(firstSeenDate);
    }
}