import { Component, customElement } from '../../core/component.js'

const PagerComponent = customElement(
  'pager',
  class extends Component {
    constructor () {
      super()
      this.pagination = null
      this.paginationPrevItem = null
      this.paginationNextItem = null
      this.paginationContainerListItems = null
      this.currentPage = 0
      this.pageSize = 0
      this.pageCount = 0
    }

    connectedCallback () {
      this.hasPrevPage = this.getAttribute('data-has-prev-page')
      this.hasNextPage = this.getAttribute('data-has-next-page')
      this.currentPage = Number(this.getAttribute('data-current-page'))
      this.pageSize = Number(this.getAttribute('data-page-size'))
      this.pageCount = Number(this.getAttribute('data-page-count'))

      this.render()
      this.setupEventListeners()
    }

    render () {
      return `
      <nav class="pagination justify-content-end">
        <ul class="pagination__list d-flex gap-1" style="list-style: none;">
          <li id="paginationPrev" class="pagination__item">
            ${
              this.hasPrevPage
                ? `<button type="button" class="btn btn-info">
                    <i class="fa fa-arrow-left"></i>
                    Trang trước
                  </button>`
                : `<button type="button" class="btn btn-outline-secondary" disabled>
                    <i class="fa fa-arrow-left"></i>
                    Trang trước
                  </button>`
            }
          </li>

          <div id="paginationContainerListItems" class="d-flex flex-row"></div>

          <li id="paginationNext" class="pagination__item">
            ${
              this.hasNextPage
                ? `<button type="button" class="btn btn-info">
                    Trang sau
                    <i class="fa fa-arrow-right"></i>
                  </button>`
                : `<button type="button" class="btn btn-outline-secondary" disabled>
                    Trang sau
                    <i class="fa fa-arrow-right"></i>
                  </button>`
            }
          </li>
        </ul>
      </nav>
    `
    }

    setupEventListeners () {
      this.paginationPrevItem.addEventListener('click', this.goPrev.bind(this))
      this.paginationNextItem.addEventListener('click', this.goNext.bind(this))
    }

    goPrev () {
      if (this.currentPage > 1) {
        this.currentPage--
        this.pagination.setAttribute('data-current-page', this.currentPage)
        this.notifyPageChange(this.currentPage)
      }
    }

    goNext () {
      if (this.currentPage < this.pageCount) {
        this.currentPage++
        this.pagination.setAttribute('data-current-page', this.currentPage)
        this.notifyPageChange(this.currentPage)
      }
    }

    notifyPageChange (page) {
      this.dispatchEvent(new CustomEvent('pageChange', { detail: { page } }))
    }
  }
)

export default PagerComponent
