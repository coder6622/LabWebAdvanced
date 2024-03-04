import { Posts } from '../../../app/api/posts.js'
import { Component, customElement } from '../../../app/core/component.js'

const HomePage = customElement(
  'home',
  class extends Component {
    constructor () {
      super()
      this.state = {
        loading: true,
        posts: []
      }
    }

    async atTheFirstRender () {
      const posts = await Posts.getPosts({ pageNumber: 1, pageSize: 10 })
      this.setState({ loading: false, posts: posts.items })
    }

    render () {
      const { loading, posts } = this.state

      if (loading) return `<app-loading></app-loading>`
      console.log(loading)

      return `
      <div class="page-content-container">
        <div class="row">
          ${posts
            .map(
              post =>
                `<app-post-item 
                  id='${post.id}'
                  data-post='${JSON.stringify(post)}'></app-post-item>`
            )
            .join('\n')}
        </div>
      </div>`
    }

    // async updatePosts ({ pageNumber = 1, pageSize = 2 }) {
    //   this.posts = await Posts.getPosts({ pageNumber, pageSize })
    // }

    // async initView () {
    //   const postsView = await this.view.appendPosts(
    //     this.posts.items,
    //     this.posts.metadata
    //   )

    //   this.innerHTML = postsView
    //   this.setupPagination()
    // }

    // setupPagination () {
    //   this.pagination = this.querySelector('app-pager')
    //   // this.pagination.addEventListener('pageChange', async event => {
    //   //   const { page } = event.detail
    //   //   this.loading = true
    //   //   this.innerHTML = '<app-loading></app-loading>'

    //   //   await this.updatePosts({ pageNumber: page, pageSize: 2 })

    //   //   this.loading = false
    //   //   await this.initView()
    //   // })
    // }
  }
)

export default HomePage
