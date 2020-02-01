const router = require('express').Router()
const { ErrorHandler } = require('../utils/errors')
const HttpStatus = require('http-status-codes')
const appStorage = require('../stores/appStorage')

router.get('/', async (_, res, next) => {
  try {
    res.status(200).json(appStorage.data.events)
  } catch (e) {
    next(new ErrorHandler(HttpStatus.INTERNAL_SERVER_ERROR, 'Couldn\'t fetch events'))
  }
})

module.exports = router
