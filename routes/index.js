var router = require('express').Router()
const HttpStatus = require('http-status-codes')
const { ErrorHandler } = require('../utils/errors')
const cacheSuccesses = require('../utils/cache')

router.use('/schedule', cacheSuccesses, require('./schedule'))
router.get('*', (req, res) => { throw new ErrorHandler(HttpStatus.NOT_FOUND) })

module.exports = router
