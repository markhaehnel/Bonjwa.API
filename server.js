const fetch = require('node-fetch')
const $ = require('cheerio')
const express = require('express')
const NodeCache = require( "node-cache" );
const myCache = new NodeCache( { stdTTL: 120, checkperiod: 10 } );

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
        
        let splitDate = element.attribs['data-date'].split('-')

        let theDate = new Date()
        theDate.setUTCFullYear(splitDate[0], splitDate[1], splitDate[2])
        theDate.setUTCHours(0)
        theDate.setUTCMinutes(0)
        theDate.setUTCSeconds(0)
        theDate.setUTCMilliseconds(0)

        let date = theDate.toISOString()
        if (!data.find(x => x.date === date)) {
          data.push({
            date,
            elements: []
          })
        }
        
        data.find(x => x.date === date).elements.push({
          title,
          caster,
          date,
          startHour: element.attribs['data-hour-start'],
          endHour: element.attribs['data-hour-end'],
        })
      })
      
      resolve(data)
    } catch (ex) {
      reject(ex)
    }
  })
}

const getCachedSchedule = async function () {
  let value = myCache.get(KEY_SCHEDULE);
  
  if (value == undefined){
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
