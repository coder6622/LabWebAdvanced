import { Archives } from '../../../api/archives.js'
import { Categories } from '../../../api/categories.js'
import { Component, customElement } from '../../../core/component.js'

const SidebarComponent = customElement(
  'sidebar',
  class extends Component {
    async render () {
      this.categories = await Categories.getCategories({
        pageNumber: 1,
        pageSize: 20
      })

      this.archives = await Archives.getArchives({
        limit: 20
      })

      return `
      <div class="my-4">
        <app-categories data-categories='${JSON.stringify(
          this.categories.items
        )}'></app-categories>
        <app-archives data-archives='${JSON.stringify(
          this.archives
        )}'></app-archives>
      </div>
    `
    }
  }
)

export default SidebarComponent
