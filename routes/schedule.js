const router = require('express').Router()
const { getSchedule } = require('../services/bonjwa')
const { ErrorHandler } = require('../utils/errors')
const HttpStatus = require('http-status-codes')

router.get('/', async (_, res, next) => {
  try {
    const schedule = await getSchedule()
    res.status(200).json(schedule)
  } catch (e) {
    next(new ErrorHandler(HttpStatus.INTERNAL_SERVER_ERROR, 'Couldn\'t fetch schedule'))
  }
})

module.exports = router
