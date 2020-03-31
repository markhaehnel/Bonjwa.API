const fetch = require('node-fetch')
const $ = require('cheerio')
const moment = require('moment-timezone')
const config = require('../utils/config')
const ScheduleItem = require('../models/scheduleItem')
const EventItem = require('../models/eventItem')
const logger = require('../utils/logger')

const url = config.scheduleUrl
const DATE_FORMAT = 'YYYY-M-D HH:mm:ss'

async function getScheduleAndEvents () {
  logger.debug('Fetching schedule and events')

  const res = await fetch(url)
  const html = await res.text()

  logger.debug('Schedule and events successully fetched.')

  const schedule = extractSchedule(html)
  const events = extractEvents(html)

  return {
    schedule,
    events
  }
}


function extractSchedule (html) {
  const elements = $('.stream-plan > table > tbody > tr > td', html)

  const data = []

  elements.each((i, element) => {
    if (element.attribs.class && element.attribs.class.includes('free-streaming-slot')) return

    const content = $('p', element)

    const title = $(content[0]).text().trim()
    const caster = $(content[1]).text().trim()

    const initialDate = element.attribs['data-date']

    const dateYear = initialDate.substr(0, 4).split('').reverse().join('')
    const dateMonthDay = initialDate.substr(4, initialDate.length)

    const startDate = moment.tz(`${dateYear}${dateMonthDay} ${element.attribs['data-hour-start']}:00:00`, DATE_FORMAT, 'Europe/Berlin')
    const endDate = moment.tz(`${dateYear}${dateMonthDay} ${element.attribs['data-hour-end']}:00:00`, DATE_FORMAT, 'Europe/Berlin')
    const cancelled = (element.attribs.class && element.attribs.class.includes('cancelled-streaming-slot')) || false

    data.push(new ScheduleItem(title, caster, startDate, endDate, cancelled))
  })

  return data
}

function extractEvents (html) {
  const elements = $('.c-content-three table tr', html)

  const data = []

  elements.each((i, element) => {
    const content = $('td', element)

    if (typeof content[0] === 'undefined' || typeof content[1] === 'undefined') return

    const date = $(content[0]).text().trim()
    const title = $(content[1]).text().trim()

    data.push(new EventItem(title, date))
  })

  return data
}

module.exports = {
  getScheduleAndEvents
}
