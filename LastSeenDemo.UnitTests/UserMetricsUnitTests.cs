namespace LastSeenDemo.UnitTests;
using System;
using System.Collections.Generic;
using Xunit;

public class UserMetricsUnitTests
{
    [Fact]
    public void UserId_ShouldBeSettableAndGettable()
    {
        // Arrange
        var userMetrics = new UserMetricsDto();
        var userId = Guid.NewGuid();

        // Act
        userMetrics.UserId = userId;

        // Assert
        Assert.Equal(userId, userMetrics.UserId);
    }

    [Fact]
    public void Metrics_ShouldBeSettableAndGettable()
    {
        // Arrange
        var userMetrics = new UserMetricsDto();
        var metrics = new Dictionary<string, double>
        {
            { "Metric1", 10.0 },
            { "Metric2", 20.0 }
        };

        // Act
        userMetrics.Metrics = metrics;

        // Assert
        Assert.Equal(metrics, userMetrics.Metrics);
    }

    [Fact]
    public void Metrics_ShouldBeInitializedWithEmptyDictionaryByDefault()
    {
        // Arrange
        var userMetrics = new UserMetricsDto();

        // Act

        // Assert
        Assert.NotNull(userMetrics.Metrics);
        Assert.Empty(userMetrics.Metrics);
    }
}
