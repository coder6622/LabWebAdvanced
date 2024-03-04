export function isEmptyOrSpaces (str) {
  return (
    str == null ||
    (typeof src === 'string' &&
      (str.match(/^ *$/) !== null || str.length === 0))
  )
}

class Utils {
  constructor () {
    console.log(location.hash)
    this.url = location.hash.slice(1).toLowerCase() || '/'
    this.r = this.url.split('/')
    this.request = {
      resource: null,
      id: null,
      verb: null
    }
  }

  parseRequestURL () {
    this.request.resource = this.r[1]
    this.request.id = this.r[2]
    this.request.verb = this.r[3]
    console.log(this.request)
    return this.request
  }
}

export default Utils
