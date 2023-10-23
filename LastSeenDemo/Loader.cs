using System.Text.Json;

namespace LastSeenDemo;


public class Page
{
    public int Total { get; set; }
    public User[] Data { get; set; }
}

public class User
{
    public Guid UserId { get; set; }
    public DateTimeOffset? LastSeenDate { get; set; }
    public string Nickname { get; set; }
    public bool IsOnline { get; set; }
    public Dictionary<string, double> Metrics { get; set; } // User-specific metrics
}

public class UserMetricsDto
{
    public Guid UserId { get; set; }
    public Dictionary<string, double> Metrics { get; set; }
}

public interface ILoader
{
    Page Load(string url);
}

public class Loader : ILoader
{
    public Page Load(string url)
    {
        using var client = new HttpClient();
        using var result = client.Send(new HttpRequestMessage(HttpMethod.Get, url));
        using var reader = new StreamReader(result.Content.ReadAsStream());
        var stringContent = reader.ReadToEnd();
        return JsonSerializer.Deserialize<Page>(stringContent, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        })!;
    }

    public void LoadAllUsers()
    {
        throw new NotImplementedException();
    }
}