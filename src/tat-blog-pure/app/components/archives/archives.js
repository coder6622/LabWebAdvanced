import { Component, customElement } from '../../core/component.js'

const ArchivesCompnent = customElement(
  'archives',
  class extends Component {
    render () {
      const archives = this.getAttribute('data-archives')
      this.archivesObj = JSON.parse(archives)

      return `
      <div class="mb-4">
        <h3 class="text-success mb-2">
          Lưu trữ
        </h3>
        <ul class="list-group">
          ${this.archivesObj
            .map(
              archive => `<li class="list-group-item">
              <a>
                ${archive.monthName} ${archive.year}
                <span>(${archive.postCount})</span>
              </a>
            </li>`
            )
            .join('\n')}
        </ul>
      </div>
    `
    }
  }
)

export default ArchivesCompnent
