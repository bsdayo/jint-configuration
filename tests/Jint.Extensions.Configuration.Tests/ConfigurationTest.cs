using Microsoft.Extensions.Configuration;

namespace Jint.Extensions.Configuration.Tests;

public class ConfigurationTest
{
    [Theory]
    [InlineData("appsettings.export-default.js")]
    [InlineData("appsettings.export-directly.js")]
    public void Configuration_ShouldMatchSource(string file)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

        // Arrange
        var builder = new ConfigurationBuilder();
        builder.AddJavaScriptModule(file);

        // Act
        var config = builder.Build();

        // Assert
        Assert.Equal("123", config.GetSection("number").Value);
        Assert.Equal("str", config.GetSection("string").Value);
        Assert.Equal("false", config.GetSection("boolean").Value);
        Assert.Equal(new DateTime(2024, 01, 20), DateTime.Parse(config.GetSection("date").Value!).ToUniversalTime());

        Assert.True(config.GetSection("nested").Exists());
        Assert.Null(config.GetSection("nested").Value);
        Assert.Equal("456.789", config["nested:a"]);
        Assert.Equal("b", config["nested:b"]);

        Assert.True(config.GetSection("nested:c").Exists());
        Assert.Null(config.GetSection("nested:c").Value);
        Assert.Null(config.GetSection("nested:c:d").Value);

        Assert.Null(config.GetSection("undef").Value);
    }
}