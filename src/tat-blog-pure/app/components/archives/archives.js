import { Archives } from '../../api/archives.js'
import { Component, customElement } from '../../core/component.js'

const ArchivesCompnent = customElement(
  'archives',
  class extends Component {
    constructor () {
      super()
      this.state = {
        loading: true,
        archives: []
      }
    }

    async atTheFirstRender () {
      if (this.state.archives.length === 0) {
        await this.fetchArchives()
      }
    }

    async fetchArchives () {
      try {
        const archives = await Archives.getArchives({ limit: 20 })
        this.setState({ archives: archives })
      } catch (error) {
        console.log(error)
      } finally {
        this.setState({ loading: false })
      }
    }

    render () {
      const { loading, archives } = this.state

      if (loading) return `<app-loading></app-loading>`

      const archivesHTML =
        archives.length > 0
          ? archives
              .map(
                archive => `<li class="list-group-item">
              <a>
                ${archive.monthName} ${archive.year}
                <span>(${archive.postCount})</span>
              </a>
            </li>`
              )
              .join('\n')
          : `<h1>Không có lưu trữ</h1>`

      return `
      <div class="mb-4">
        <h3 class="text-success mb-2">
          Lưu trữ
        </h3>
        <ul class="list-group">
          ${archivesHTML}
        </ul>
      </div>
    `
    }
  }
)

export default ArchivesCompnent
