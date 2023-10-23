namespace LastSeenDemo.UnitTests;

using Mocks;
using System;
using System.Collections.Generic;
using Xunit;

public class MinMaxDailyUnitTests
{
    private static readonly OnlineDetector OnlineDetector = new(new MockDateTimeProvider());
    private readonly MinMaxDaily _minMaxCalculate = new (OnlineDetector);

    [Fact]

    public void CalculateMinMax_WhenUserWasOnline()
    {
        // Arrange
        var userTimeSpans = new List<UserTimeSpan>
        {
            new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 10:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 13:00:00") },
            new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-02 14:00:00"), Logout = DateTimeOffset.Parse("2022-01-02 16:00:00") },
            new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-03 18:00:00"), Logout = DateTimeOffset.Parse("2022-01-03 23:00:00") }
        };
        
        // Act
        var minMax = _minMaxCalculate.CalculateMinMax(userTimeSpans, 
            DateTimeOffset.Parse("2022-01-01 00:00:00"), DateTimeOffset.Parse("2022-01-03 23:59:59"));

        // Assert
        Assert.Equal(7200, minMax.Item1);
        Assert.Equal(18000, minMax.Item2);
    }

    [Fact]
    public void CalculateMinMax_WhenUserWasNeverOnline()
    {
        // Arrange
        var userTimeSpans = new List<UserTimeSpan>();
        
        // Act
        var minMax = _minMaxCalculate.CalculateMinMax(userTimeSpans, 
            DateTimeOffset.Parse("2022-01-01 00:00:00"), DateTimeOffset.Parse("2022-01-03 23:59:59"));

        // Assert
        Assert.Equal(0, minMax.Item1);
        Assert.Equal(0, minMax.Item2);
    }

    [Fact]
    public void CalculateMinMax_WhenUserWasNotOnline()
    {
        // Arrange
        var userTimeSpans = new List<UserTimeSpan>
        {
            new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-03 18:00:00"), Logout = DateTimeOffset.Parse("2022-01-03 23:00:00") }
        };
        
        // Act
        var minMax = _minMaxCalculate.CalculateMinMax(userTimeSpans, 
            DateTimeOffset.Parse("2022-01-04 00:00:00"), DateTimeOffset.Parse("2022-01-04 23:59:59"));

        // Assert
        Assert.Equal(0, minMax.Item1);
        Assert.Equal(0, minMax.Item2);
    }
}