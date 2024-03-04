import { IMAGE_URL_BASE } from '../../api/config.js'
import { Component, customElement } from '../../core/component.js'
import router from '../../core/router.js'
import { isEmptyOrSpaces } from '../../utils/utils.js'
import TagItemComponent from '../tag/tag-item.js'

const PostItemComponent = customElement(
  'post-item',
  class extends Component {
    goToDetail (id) {
      router.navigate(`/#/posts/post?id=${id}`)
    }

    render () {
      const post = this.getAttribute('data-post')
      const postObj = JSON.parse(post)
      const imageUrl = isEmptyOrSpaces(postObj.imageUrl)
        ? '../../assets/images/image_1.jpg'
        : `${IMAGE_URL_BASE}/${postObj.imageUrl}`

      return `
      <article class="blog-entry mb-4">
        <div class="card">
          <div class="row g-2">
            <div class="col-md-4">
              <img src="${imageUrl}"
                  class="card-img"
                  alt="${postObj.title}" />
            </div>
            <div class="col-md-8 py-1">
              <a class="text-reset text-decoration-none h5">${postObj.title}</a>
              <p class="card-text">
                <small class="text-muted">
                  Tác giả:
                </small>
                <a class="text-decoration-none">${postObj.author.fullName}</a>
                <small class="text-muted">
                  Chủ đề:
                </small>
                <a class="text-decoration-none">${postObj.category.name}</a>
                <small class="text-muted">
                  Ngày:
                </small>
                <span class="text-primary">${postObj.postedDate}</span>
              </p>
              <p class="card-text">
              ${postObj.shortDescription}
              </p>
              <div class="tag-list pb-3">
              ${postObj.tags
                .map(tag => {
                  return `<${TagItemComponent} 
                    data-tag='${JSON.stringify(tag)}'>
                  </${TagItemComponent}>`
                })
                .join('\n')}
              </div>
              <div class="text-end pe-2">
                <a class="btn btn-primary" 
                  href="#/posts/post?id=${postObj.id}">
                  Xem chi tiết
                </a>
              </div>
            </div>
          </div>
        </div>
      </article>
    `
    }
  }
)
export default PostItemComponent
