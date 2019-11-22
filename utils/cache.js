const apiCache = require('apicache')

const cache = apiCache.middleware
const onlyStatus200 = (req, res) => res.statusCode === 200
const cacheSuccesses = cache('5 minutes', onlyStatus200)

module.exports = cacheSuccesses
