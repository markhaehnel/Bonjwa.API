var router = require('express').Router()
const HttpStatus = require('http-status-codes')
const { ErrorHandler } = require('../utils/errors')

router.use('/schedule', require('./schedule'))
router.use('/events', require('./events'))
router.get('*', (req, res) => { throw new ErrorHandler(HttpStatus.NOT_FOUND) })

module.exports = router
