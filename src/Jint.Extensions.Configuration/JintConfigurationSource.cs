using Microsoft.Extensions.Configuration;

namespace Jint.Extensions.Configuration;

internal class JintConfigurationSource : IConfigurationSource
{
    private readonly string _filePath;
    private readonly Action<Options>? _configureEngineOptions;

    public JintConfigurationSource(string filePath, Action<Options>? configureEngineOptions)
    {
        _filePath = filePath;
        _configureEngineOptions = configureEngineOptions;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new JintConfigurationProvider(_filePath, _configureEngineOptions);
    }
}