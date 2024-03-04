import { Posts } from '../../../app/api/posts.js'
import { Component, customElement } from '../../../app/core/component.js'
import { View } from './view.js'

const HomeComponent = customElement(
  'home',
  class extends Component {
    constructor () {
      super()
      this.view = new View()
      this.loading = true
    }

    async render () {
      this.innerHTML = this.loading ? '<app-loading></app-loading>' : ''
      await this.updatePosts({ pageNumber: 1, pageSize: 2 })
      await this.initView()
    }

    async updatePosts ({ pageNumber = 1, pageSize = 2 }) {
      this.posts = await Posts.getPosts({ pageNumber, pageSize })
    }

    async initView () {
      const postsView = await this.view.appendPosts(
        this.posts.items,
        this.posts.metadata
      )

      this.innerHTML = postsView
      this.setupPagination()
    }

    setupPagination () {
      this.pagination = this.querySelector('app-pager')
      this.pagination.addEventListener('pageChange', async event => {
        const { page } = event.detail
        this.loading = true
        this.innerHTML = '<app-loading></app-loading>'

        await this.updatePosts({ pageNumber: page, pageSize: 2 })

        this.loading = false
        await this.initView()
      })
    }
  }
)

export default HomeComponent
