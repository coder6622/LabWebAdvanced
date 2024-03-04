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

    const view = `
      
    `

    return view
  }
}
