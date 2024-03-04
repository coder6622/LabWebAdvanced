import { IMAGE_URL_BASE } from '../../../app/api/config.js'
import { isEmptyOrSpaces } from '../../../app/utils/utils.js'

export class View {
  appendPost (post) {
    const {
      id,
      title,
      shortDescription,
      description,
      imageUrl,
      meta,
      urlSlug,
      viewCount,
      postedDate,
      modifiedDate,
      author,
      category
    } = post

    const urlImage = isEmptyOrSpaces(imageUrl)
      ? '../../assets/images/image_1.jpg'
      : `${IMAGE_URL_BASE}/${postObj.imageUrl}`

    const view = `
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
              ${author.fullName}
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
                ${category.name}
              </a>
            </span>
            &emsp;
            <span>
              <i class="fa fa-solid fa-eye"></i>
              <span class="text-info fw-semibold">
              ${viewCount}
              </span>
            </span>
          </p>

          <hr>
          <p class="lead">
            <i class="fa fa-calendar"></i>
            <small class="text-muted">
              Ngày đăng:
            </small>
            ${postedDate.toString()}
          </p>
          <p class="lead">
            <i class="fa fa-tags"></i>
            <span class="text-muted">Tags:&emsp;</span>
          </p>
          <hr />
          <p class="lead">${shortDescription}</p>
          <img src="${urlImage}" class="img-responsive">
          <p class="pt-4">${description}</p>
        </div>
      </div>
    `

    return view
  }
}
