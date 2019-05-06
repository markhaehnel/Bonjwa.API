const tracer = require('dd-trace')
tracer.init()

const fetch = require('node-fetch')
const $ = require('cheerio')
const express = require('express')
const NodeCache = require('node-cache')
const myCache = new NodeCache({ stdTTL: 120, checkperiod: 10 })

const app = express()
const port = 3000

const KEY_SCHEDULE = 'SCHEDULE'

const url = 'https://www.bonjwa.de/programm'

const getSchedule = async function () {
  return new Promise(async (resolve, reject) => {
    try {
      const res = await fetch(url)
      const html = await res.text()
      const elements = $('.stream-plan > table > tbody > tr > td', html)

      let data = []

      elements.each((i, element) => {
        if (element.attribs['class'] && element.attribs['class'].includes('free-streaming-slot')) return

        let content = $('p', element)
        let title = $(content[0]).text().trim()
        let caster = $(content[1]).text().trim()

        let startDate = new Date(`${element.attribs['data-date']} ${element.attribs['data-hour-start']}:00 GMT+1`)
        let endDate = new Date(`${element.attribs['data-date']} ${element.attribs['data-hour-end']}:00 GMT+1`)

        let cancelled = (element.attribs['class'] && element.attribs['class'].includes('cancelled-streaming-slot')) ? true : false

        data.push({
          title,
          caster,
          startDate,
          endDate,
          cancelled
        })
      })

      resolve(data)
    } catch (ex) {
      reject(ex)
    }
  })
}

const getCachedSchedule = async function () {
  let value = myCache.get(KEY_SCHEDULE)

  if (value === undefined) {
    value = await getSchedule()
    myCache.set(KEY_SCHEDULE, value, 120)
  }

  return value
}

app.get('/schedule', async (req, res) => res.json({
  success: true,
  data: await getCachedSchedule()
}))
app.get('*', (req, res) => res.status(404).json({ error: 'Endpoint not found' }))

app.listen(port, () => console.log(`App listening on port ${port}!`))
