# Jint.Extensions.Configuration

Add JavaScript modules as your configuration source! Powered by [Jint](https://github.com/sebastienros/jint).

## Usage

Install this package using your preferred way.

### Simple usage

Prepare a JavaScript module that exports an object:

```javascript
// appsettings.js

export default {
  host: 'localhost',
  port: 5432
}
```

Then, add a JavaScript module source to `Microsoft.Extensions.Configuration.IConfigurationBuilder`:

```csharp
var builder = new ConfigurationBuilder()
    .AddJavaScriptModule("./path/to/appsettings.js");

var configuration = builder.Build();

// Use configuartion
var host = configuration["host"];
```

The entire JavaScript code will be executed (with [Jint](https://github.com/sebastienros/jint)),
and the exported object will be added to the configuration.

### With ASP.NET Core

In ASP.NET Core, you can simply add a JavaScript module source to `builder.Configuration`:

```csharp
builder.Configuration.AddJavaScriptModule("appsettings.js");
builder.Configuration.AddJavaScriptModule($"appsettings.{builder.Environment.EnvironmentName}.js");
```

As an example,

```javascript
// appsettings.js

const LogLevel = {
  Information: 'Information',
  Warning: 'Warning',
  // ...
}

export default {
  logging: {
    logLevel: {
      default: LogLevel.Information,
      'Microsoft.AspNetCore': LogLevel.Warning,
    },
  },
  allowedHosts: '*',
}
```

Export separately is also supported (but cannot mix with `export default`):

```javascript
export const logging = {
  logLevel: {
    default: LogLevel.Information,
    'Microsoft.AspNetCore': LogLevel.Warning,
  },
}

export const allowedHosts = '*'
```

## License

[MIT](./LICENSE)
