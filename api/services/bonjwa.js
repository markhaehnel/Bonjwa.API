const fetch = require('node-fetch')
const $ = require('cheerio')
const moment = require('moment-timezone')
const ScheduleItem = require('../models/scheduleItem')
const EventItem = require('../models/eventItem')

const url = 'https://bonjwa.de/programm'

async function getScheduleAndEvents () {
  const res = await fetch(url)
  const html = await res.text()

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
    const startDate = moment.tz(`${element.attribs['data-date']} ${element.attribs['data-hour-start']}:00:00`, 'YYYY-MM-DD HH:mm:ss', 'Europe/Berlin')
    const endDate = moment.tz(`${element.attribs['data-date']} ${element.attribs['data-hour-end']}:00:00`, 'YYYY-MM-DD HH:mm:ss', 'Europe/Berlin')
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
