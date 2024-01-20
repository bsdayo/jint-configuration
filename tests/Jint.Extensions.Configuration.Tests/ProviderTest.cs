using Jint.Runtime;

namespace Jint.Extensions.Configuration.Tests;

public class ProviderTest
{
    [Theory]
    [InlineData("appsettings.export-default.js", true)]
    [InlineData("appsettings.export-directly.js", false)]
    public void AddProperty_ShouldResolveCorrectly(string file, bool exportDefault)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

        // Arrange
        var source = File.ReadAllText(file);
        var engine = new Engine();
        engine.Modules.Add("config", source);
        var moduleObj = engine.Modules.Import("config");
        if (exportDefault)
            moduleObj = moduleObj.Get("default").AsObject();
        var data = new Dictionary<string, string?>();
        var sectionStack = new Stack<string>();

        // Act
        foreach (var key in moduleObj.GetOwnPropertyKeys(Types.String))
            JintConfigurationProvider.AddProperty(
                data, sectionStack, key.ToString(), moduleObj.GetOwnProperty(key).Value);

        // Assert
        Assert.Equal(8, data.Count);
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