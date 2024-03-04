import { Component, customElement } from '../../core/component.js'
import { Categories } from '../../api/categories.js'

const CategoriesComponent = customElement(
  'categories',
  class extends Component {
    constructor () {
      super()
      this.state = {
        loading: true,
        categories: []
      }
    }

    async fetchCategories () {
      try {
        const categories = await Categories.getCategories({
          pageNumber: 1,
          pageSize: 20
        })

        this.setState({ categories: categories.items })
      } catch (error) {
        console.log(error)
      } finally {
        this.setState({ loading: false })
      }
    }

    async atTheFirstRender () {
      if (this.state.categories.length === 0) {
        await this.fetchCategories()
      }
    }

    render () {
      const { loading, categories } = this.state

      if (loading) return `<app-loading></app-loading>`

      const categoriesHTML =
        categories.length > 0
          ? categories
              .map(
                cate => `
            <li class='list-group-item'>
              <a title=${cate.Description}>
                ${cate.name}
                <span>(${cate.postCount})</span>
              </a>
            </li>
          `
              )
              .join('\n')
          : `<h1>Không có chủ đề</h1>`

      return `
      <div class="mb-4">
        <h3 class="text-success mb-2">
          Các chủ đề
        </h3>

        <ul class="list-group">
         ${categoriesHTML}
        </ul>
      </div>
    `
    }
  }
)

export default CategoriesComponent
