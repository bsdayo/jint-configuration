namespace Jint.Extensions.Configuration.Tests;

public class ProviderTest
{
    [Fact]
    public void AddProperty_ShouldResolveCorrectly()
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

        // Arrange
        var source = File.ReadAllText("appsettings.js");
        var engine = new Engine();
        engine.Modules.Add("config", source);
        var moduleObj = engine.Modules.Import("config").Get("default").AsObject();
        var data = new Dictionary<string, string?>();
        var sectionStack = new Stack<string>();

        // Act
        foreach (var (sectionValue, descriptor) in moduleObj.GetOwnProperties())
            JintConfigurationProvider.AddProperty(data, sectionStack, sectionValue.ToString(), descriptor.Value);

        // Assert
        Assert.Equal("123", data["number"]);
        Assert.Equal("str", data["string"]);
        Assert.Equal("false", data["boolean"]);
        Assert.Equal(new DateTime(2024, 01, 20), DateTime.Parse(data["date"]!).ToUniversalTime());
        Assert.Equal("456.789", data["nested:a"]);
        Assert.Equal("b", data["nested:b"]);
        Assert.False(data.ContainsKey("nested:c"));
        Assert.Null(data["nested:c:d"]);
        Assert.Null(data["undef"]);
    }
}