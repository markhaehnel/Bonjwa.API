const config = {
  port: process.env.PORT || 3000,
  scrapeInterval: process.env.SCRAPE_INTERVAL || 30000,
  scheduleUrl: process.env.SCHEDULE_URL || 'https://www.bonjwa.de/programm',
  logLevel: process.env.LOG_LEVEL || 'debug'
}

module.exports = config
