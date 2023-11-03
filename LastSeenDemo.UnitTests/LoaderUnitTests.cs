namespace LastSeenDemo.UnitTests;
using System;
using LastSeenDemo;
using Xunit;

public class LoaderTests
{
    [Fact]
    public void Load_Calls_HttpClient_And_Deserializes_Response()
    {
        // Arrange
        var loader = new Loader();
        const string url = "https://sef.podkolzin.consulting/api/users/lastSeen?offset=0";
        
        // Act
        var result = loader.Load(url);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void LoadAllUsers_Throws_NotImplementedException()
    {
        // Arrange
        var loader = new Loader();

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => loader.LoadAllUsers());
    }
}
