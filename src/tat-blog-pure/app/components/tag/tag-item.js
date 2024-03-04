import { Component, customElement } from '../../core/component.js'

const TagItemComponent = customElement(
  'tag-item',
  class extends Component {
    render () {
      const tag = this.getAttribute('data-tag')
      const tagObj = JSON.parse(tag)
      return `
      <a title="Tag: ${tagObj.name}"
        class="btn btn-sm btn-outline-secondary">
        ${tagObj.name}
      </a>
    `
    }
  }
)

export default TagItemComponent
