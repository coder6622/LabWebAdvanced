import { API_URL } from './config.js'

export class Posts {
  constructor ({ id, attributes }) {
    this.id = id
    this.name = attributes.name
  }

  static getPosts ({ pageNumber = 1, pageSize = 5 }) {
    return fetch(
      `${API_URL}/posts?PageSize=${pageSize}&PageNumber=${pageNumber}`,
      {
        method: 'GET'
      }
    )
      .then(res => res.json())
      .then(data => data.result)
  }

  static getPostById (id) {
    return fetch(`${API_URL}/posts/${id}`, { method: 'GET' })
      .then(res => res.json())
      .then(data => data.result)
  }
}
