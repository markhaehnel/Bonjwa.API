const HttpStatus = require('http-status-codes')

class ErrorHandler extends Error {
  constructor (statusCode, message) {
    super()
    this.statusCode = statusCode
    this.message = message
  }
}

const handleError = (err, req, res) => {
  const status = err.statusCode || HttpStatus.INTERNAL_SERVER_ERROR
  const message = HttpStatus.getStatusText(status)

  res.status(status).json({
    status,
    message
  })
}

module.exports = {
  ErrorHandler,
  handleError
}
