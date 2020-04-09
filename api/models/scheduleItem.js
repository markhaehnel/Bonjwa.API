class ScheduleItem {
  constructor (title, caster, startDate, endDate, cancelled) {
    this.title = title
    this.caster = caster
    this.startDate = startDate
    this.endDate = endDate
    this.cancelled = cancelled
  }
}

module.exports = ScheduleItem
