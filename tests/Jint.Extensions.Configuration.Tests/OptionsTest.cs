// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable PropertyCanBeMadeInitOnly.Local

using Microsoft.Extensions.Configuration;

namespace Jint.Extensions.Configuration.Tests;

public class OptionsTest
{
    private sealed class Config
    {
        public int Number { get; set; }

        public string? String { get; set; }

        public bool Boolean { get; set; }

        public DateTime Date { get; set; }

        public NestedConfig? Nested { get; set; }

        public string? Undef { get; set; }
    }

    private sealed class NestedConfig
    {
        public double? A { get; set; }

        public string? B { get; set; }

        public NestedNestedConfig? C { get; set; }
    }

    private sealed class NestedNestedConfig
    {
        public string? D { get; set; }
    }

    [Fact]
    public void Options_ShouldBeResolvedCorrectly()
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

        // Arrange
        var builder = new ConfigurationBuilder();
        builder.AddJavaScriptModule("appsettings.js");
        var root = builder.Build();

        // Act
        var config = root.Get<Config>();

        // Assert
        Assert.NotNull(config);
        Assert.Equal(123, config.Number);
        Assert.Equal("str", config.String);
        Assert.False(config.Boolean);
        Assert.Equal(new DateTime(2024, 01, 20), config.Date.ToUniversalTime());

        Assert.NotNull(config.Nested);
        Assert.Equal(456.789, config.Nested.A);
        Assert.Equal("b", config.Nested.B);

        Assert.NotNull(config.Nested.C);
        Assert.Null(config.Nested.C.D);

        Assert.Null(config.Undef);
    }
}