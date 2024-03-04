import { API_URL } from './config.js'

export class Categories {
  constructor () {}

  static getCategories ({ pageNumber = 1, pageSize = 20 }) {
    return fetch(
      `${API_URL}/categories?PageSize=${pageSize}&PageNumber=${pageNumber}`,
      { method: 'GET' }
    )
      .then(res => res.json())
      .then(data => data.result)
  }
}
