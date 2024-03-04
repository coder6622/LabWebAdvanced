import { Posts } from '../../../app/api/posts.js'
import { Component, customElement } from '../../../app/core/component.js'
import { View } from './view.js'

const PostDetailComponent = customElement(
  'post-detail',
  class extends Component {
    constructor () {
      super()
      this.view = new View()
      this.loading = true
    }

    async render () {
      this.innerHTML = this.loading ? '<app-loading></app-loading>' : ''
      this.post = await Posts.getPostById(this.id)
      await this.initView()
    }

    async initView () {
      const postView = await this.view.appendPost(this.post)
      this.innerHTML = postView
    }
  }
)
export default PostDetailComponent

// export class  extends Component {
//   static create (params) {
//     const component = Component.create('post-detail')

//     if (params.id) {
//       component.id = params.id
//     }
//     return component
//   }
// }

// Component.define('post-detail', PostDetailComponent)
