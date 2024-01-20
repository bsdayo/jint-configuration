using Microsoft.Extensions.Configuration;

namespace Jint.Extensions.Configuration;

public static class JintConfigurationExtensions
{
    public static IConfigurationBuilder AddJavaScriptModule(this IConfigurationBuilder builder,
        string filePath, Action<Options>? configureEngineOptions = null)
    {
        return builder.Add(new JintConfigurationSource(filePath, configureEngineOptions));
    }
}