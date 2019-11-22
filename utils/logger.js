const config = require('../utils/config')
const { createLogger, format, transports } = require('winston')
const { combine, simple, colorize } = format

const logLevel = config.logLevel

const logger = createLogger({
  format: combine(
    colorize(),
    simple()
  ),
  level: logLevel,
  transports: [new transports.Console()]
})

module.exports = logger
