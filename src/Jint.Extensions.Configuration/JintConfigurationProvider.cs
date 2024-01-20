using Jint.Native;
using Jint.Runtime;
using Microsoft.Extensions.Configuration;

namespace Jint.Extensions.Configuration;

internal class JintConfigurationProvider : ConfigurationProvider
{
    private readonly Action<Options>? _configureEngineOptions;
    private readonly string _baseDirectory;
    private readonly string _relativeFilePath;

    public JintConfigurationProvider(string filePath, Action<Options>? configureEngineOptions)
    {
        ArgumentNullException.ThrowIfNull(filePath);

        _configureEngineOptions = configureEngineOptions;
        var absolutePath = Path.GetFullPath(filePath);
        _baseDirectory = Path.GetDirectoryName(absolutePath) ?? Directory.GetCurrentDirectory();
        _relativeFilePath = Path.GetFileName(absolutePath);
    }

    public override void Load()
    {
        var engine = new Engine(options =>
        {
            options.EnableModules(_baseDirectory);
            _configureEngineOptions?.Invoke(options);
        });

        var module = engine.Modules.Import($"./{_relativeFilePath}");
        var configObj = module.HasOwnProperty("default")
            ? module.Get("default").AsObject()
            : module;
        var sectionStack = new Stack<string>();
        foreach (var key in configObj.GetOwnPropertyKeys(Types.String))
            AddProperty(Data, sectionStack, key.ToString(), configObj.GetOwnProperty(key).Value);
    }

    internal static void AddProperty(IDictionary<string, string?> data, Stack<string> sectionStack,
        string section, JsValue jsValue)
    {
        var key = string.Join(':', sectionStack.Reverse().Append(section));

        if (jsValue.IsNull() || jsValue.IsUndefined())
        {
            data.Add(key, null);
        }
        else if (jsValue.IsDate())
        {
            data.Add(key, jsValue.AsDate().ToDateTime().ToString("O"));
        }
        else if (jsValue.IsArray())
        {
            sectionStack.Push(section);

            var arr = jsValue.AsArray();
            for (var i = 0; i < arr.Length; i++) AddProperty(data, sectionStack, i.ToString(), arr[i]);

            sectionStack.Pop();
        }
        else if (jsValue.IsObject())
        {
            sectionStack.Push(section);

            var obj = jsValue.AsObject();
            foreach (var propKey in obj.GetOwnPropertyKeys(Types.String))
                AddProperty(data, sectionStack, propKey.ToString(), obj.GetOwnProperty(propKey).Value);

            sectionStack.Pop();
        }
        else
        {
            // number, string, boolean, etc.
            data.Add(key, jsValue.ToString());
        }
    }
}