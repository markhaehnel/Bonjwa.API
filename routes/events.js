const router = require('express').Router()
const { getEvents } = require('../services/bonjwa')
const { ErrorHandler } = require('../utils/errors')
const HttpStatus = require('http-status-codes')

router.get('/', async (_, res, next) => {
  try {
    const events = await getEvents()
    res.status(200).json(events)
  } catch (e) {
    next(new ErrorHandler(HttpStatus.INTERNAL_SERVER_ERROR, 'Couldn\'t fetch events'))
  }
})

module.exports = router
