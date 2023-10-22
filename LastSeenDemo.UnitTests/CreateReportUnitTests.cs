namespace LastSeenDemo.UnitTests;

using Mocks;
using System;
using System.Collections.Generic;
using Xunit;

public class CreateReportUnitTests
{
    private OnlineDetector _detector;
    private Worker _worker;
    private DateTimeOffset _from;
    private DateTimeOffset _to;
    private User[] _userList;

    public void SetUp()
    {
        var date = new MockDateTimeProvider();
        var loader = new UserLoader(new Loader(),"https://sef.podkolzin.consulting/api/users/lastSeen");
        var transformer = new UserTransformer(date);
        _worker = new Worker(loader, new AllUsersTransformer(transformer));
        _detector = new(date);
        _to = DateTimeOffset.Now;
        _from = DateTimeOffset.Now.AddDays(-1);
        _userList = loader.LoadAllUsers();

    }
    [Fact]
    public void GetCorrectNumberOfMetrics()
    {
        var metrics = new List<string> { "Total", "DailyAverage", "WeeklyAverage"};
        var creator = new ReportCreator(metrics, _worker, _detector);
        var response = creator.CreateReport(_userList, _from, _to);
        
        Assert.Equal(metrics.Count, response.Values.Count);
    }
}