import { LogLevel } from './appsettings.Common.js'

export default {
  logging: {
    logLevel: {
      default: LogLevel.Information,
      'Microsoft.AspNetCore': LogLevel.Warning,
    }
  }
}
