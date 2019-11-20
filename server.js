const config = require('./utils/config')
const logger = require('./utils/logger')
const bonjwa = require('./services/bonjwa')

const express = require('express')
const app = express()

app.get('/schedule', async (_, res) => {
  try {
    const schedule = await bonjwa.getSchedule()
    res.status(200).json(schedule)
  } catch (e) {
    res.status(500).json({ status: res.statusCode, message: e.message })
  }
})

app.get('*', async (_, res) => {
  res.status(404).json({ status: res.statusCode, message: 'Not found' })
})

app.listen(config.port)
logger.info(`Listening on ${config.port}`)
