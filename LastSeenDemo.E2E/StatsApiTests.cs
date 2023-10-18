using System.Net;
using Newtonsoft.Json.Linq;
using Xunit;

namespace LastSeenDemo.E2E;

public class StatsApiTests
{
    public StatsApiTests()
    {
    }

    [Fact]
    public void When_GetStatsWithInvalidId_Should_ReturnNotFound()
    {
        var (code, _) = HttpHelper.Get("/api/stats/user?date=10/13/2023%2010:46&userId=6a4658d7-8590-4402-be6c-4d18759aa386");
        Assert.Equal(HttpStatusCode.NotFound, code);
    }

    [Fact]
    public void When_GetStatsWithValidId_Should_ReturnValidObject()
    {
        var doug93UserId = new Guid("2fba2529-c166-8574-2da2-eac544d82634");
        var (code, response) = HttpHelper.Get($"/api/stats/user?userId={doug93UserId}&date={DateTime.Now}");
        
        var result = JObject.Parse(response);
        Assert.Equal(JTokenType.Boolean, result.Property("wasUserOnline").Value.Type);
        Assert.NotNull(result.Property("nearestOnlineTime"));
    }
}