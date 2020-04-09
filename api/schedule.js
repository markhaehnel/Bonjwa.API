const HttpStatus = require('http-status-codes')
const { ErrorHandler } = require('./utils/errors')
const { getScheduleAndEvents } = require('./services/bonjwa')

module.exports = async (req, res) => {
  try {
    const { schedule } = await getScheduleAndEvents()
    res.setHeader('Cache-Control', 'max-age=0, s-maxage=900')
    res.status(HttpStatus.OK).json(schedule)
  } catch (e) {
    res.json(new ErrorHandler(HttpStatus.INTERNAL_SERVER_ERROR, 'Couldn\'t fetch schedule'))
  }
}
