import { API_URL } from './config.js'
export class Archives {
  constructor () {}

  static getArchives ({ limit = 20 }) {
    return fetch(`${API_URL}/postss/archives/${limit}`, { method: 'GET' })
      .then(res => res.json())
      .then(data => data.result)
  }
}
