import { LogLevel } from './appsettings.Common.js'

const mainDb = {
  host: 'localhost',
  port: 5432,
}

export default {
  logging: {
    logLevel: {
      default: LogLevel.Information,
      'Microsoft.AspNetCore': LogLevel.Warning,
    },
  },

  allowedHosts: '*',

  databases: {
    user: {
      ...mainDb,
      database: 'user',
    },
    movie: {
      ...mainDb,
      database: 'movie',
    },
  },
}
