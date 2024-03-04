import { Posts } from '../../../app/api/posts.js'
import { Component, customElement } from '../../../app/core/component.js'
import PostItemComponent from '../../../app/components/post-iem/post-item.js'

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

      return `
      <div class="page-content-container">
        <div class="row">
          ${posts
            .map(
              post =>
                `<${PostItemComponent} 
                  id='${post.id}'
                  data-post='${JSON.stringify(post)}'></${PostItemComponent}>`
            )
            .join('\n')}
        </div>
      </div>`
    }
  }
)

export default HomePage
