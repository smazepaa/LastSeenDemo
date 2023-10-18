using LastSeenDemo.E2E;

namespace LastSeenDemo;

public class OverallTests
{
    [Fact]
    public void OverallShouldReturnResult()
    {
        var result = HttpHelper.Get("/api/report/overall");
        Assert.NotEmpty(result.response);
    }
}