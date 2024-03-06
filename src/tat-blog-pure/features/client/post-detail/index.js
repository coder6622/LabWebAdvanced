import { Posts } from '../../../app/api/posts.js'
import { Component, customElement } from '../../../app/core/component.js'
import router from '../../../app/core/router.js'
import { getQueries, isEmptyOrSpaces } from '../../../app/utils/utils.js'

const PostDetailComponent = customElement(
  'post-detail',
  class extends Component {
    constructor () {
      super()
      this.state = {
        loading: true,
        post: {}
      }
    }

    async fetchPost (id) {
      try {
        const post = await Posts.getPostById(id)
        this.setState({ post: post })
      } catch (error) {
        console.log(error)
      } finally {
        this.setState({ loading: false })
      }
    }

    async atTheFirstRender () {
      const { id } = getQueries().queries
      await this.fetchPost(id)
    }

    render () {
      const { loading, post } = this.state

      if (loading) return `<app-loading></app-loading>`

      return `
        <div class="container">
          <div class="row">
            <h1>${post.title}</h1>
            <p class="lead">
              <span>
                <i class="fa fa-user-secret"></i>
                <small class="text-secondary">
                  Đăng bởi:
                </small>
                <a class="text-decoration-none text-info fw-semibold">
                ${post.author.fullName}
                </a>
              </span>
              &emsp;
              <span>
                <i class="fa fa-brands fa-themeisle"></i>
                <small class="text-muted">
                  Chủ đề:
                </small>
                <a   
                  class="text-decoration-none text-info fw-semibold">
                  ${post.category.name}
                </a>
              </span>
              &emsp;
              <span>
                <i class="fa fa-solid fa-eye"></i>
                <span class="text-info fw-semibold">
                ${post.viewCount}
                </span>
              </span>
            </p>

            <hr>
            <p class="lead">
              <i class="fa fa-calendar"></i>
              <small class="text-muted">
                Ngày đăng:
              </small>
              ${post.postedDate.toString()}
            </p>
            <p class="lead">
              <i class="fa fa-tags"></i>
              <span class="text-muted">Tags:&emsp;</span>
            </p>
            <hr />
            <p class="lead">${post.shortDescription}</p>
            <img src="${
              isEmptyOrSpaces(post.imageUrl)
                ? '../../assets/images/image_1.jpg'
                : `${IMAGE_URL_BASE}/${post.imageUrl}`
            }" 
              class="img-responsive">
            <p class="pt-4">${post.description}</p>
          </div>
        </div>
      `
    }
  }
)
export default PostDetailComponent
