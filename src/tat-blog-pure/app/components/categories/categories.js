import { Component, customElement } from '../../core/component.js'

const CategoriesCompnent = customElement(
  'categories',
  class extends Component {
    render () {
      const categories = this.getAttribute('data-categories')
      this.categoriesObj = JSON.parse(categories)

      return `
      <div class="mb-4">
        <h3 class="text-success mb-2">
          Các chủ đề
        </h3>

        <ul class="list-group">
          ${this.categoriesObj
            .map(
              cate =>
                `<li class='list-group-item'>
                  <a
                    title=${cate.Description}
                  >
                  ${cate.name}
                    <span>(${cate.postCount})</span>
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

export default CategoriesCompnent
