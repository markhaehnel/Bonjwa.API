const config = require('./utils/config')
const logger = require('./utils/logger')
const swaggerUi = require('swagger-ui-express')
const cors = require('cors')
const { handleError } = require('./utils/errors')

const express = require('express')
const app = express()

// Routes
app.use(express.static('static'))
app.use('/api-docs', swaggerUi.serve, swaggerUi.setup(null, { swaggerOptions: { url: '/openapi.json' } }))
app.use(require('./routes'))

// Middlewares
app.use(cors())
app.use((err, req, res, next) => { handleError(err, req, res) })

app.listen(config.port, () => { logger.info(`Listening on ${config.port}`) })
