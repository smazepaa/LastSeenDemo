using LastSeenDemo;

namespace LastSeenDemo;

public class UserLoader
{
    private readonly ILoader _loader;
    private readonly string _rootUrl;
    public UserLoader(ILoader loader, string rootUrl)
    {
        _loader = loader;
        _rootUrl = rootUrl;
    }

    public User[] LoadAllUsers()
    {
        List<User> users = new();
        while (true)
        {
            var result = _loader.Load(_rootUrl + $"?offset={users.Count}");
            if (result.Data.Length == 0)
            {
                break;
            }
            users.AddRange(result.Data);
        }
        return users.ToArray();
    }
}