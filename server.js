const config = require('./utils/config')
const logger = require('./utils/logger')
const swaggerUi = require('swagger-ui-express')
const cors = require('cors')
const { handleError } = require('./utils/errors')
const CronJob = require('cron').CronJob
const appStorage = require('./stores/appStorage')
const { getScheduleAndEvents } = require('./services/bonjwa')

const express = require('express')
const app = express()

const job = new CronJob({
  cronTime: '0,30 * * * *',
  onTick: async () => {
    const { schedule, events } = await getScheduleAndEvents()
    appStorage.data.schedule = schedule
    appStorage.data.events = events
  },
  runOnInit: true
})
job.start()

app.use(cors())

// Routes
app.use(express.static('static'))
app.use('/api-docs', swaggerUi.serve, swaggerUi.setup(null, { swaggerOptions: { url: '/openapi.json' } }))
app.use(require('./routes'))

app.use((err, req, res, next) => { handleError(err, req, res) })

app.listen(config.port, () => { logger.info(`Listening on ${config.port}`) })
