import Store from '../store/store.js'

const PREFIX = 'app'

class Component extends HTMLElement {
  constructor () {
    super()
    this.props = {}
    this.state = {}
    this.firstRender = true
  }

  render () {
    return `<span>Default content</span>`
  }

  initState () {
    return {}
  }

  setState (newState) {
    this.state = { ...this.state, ...newState }
    this.connectedCallback()
  }

  callLifeCycles () {
    if (this.firstRender) {
      this.atTheFirstRender()
      this.firstRender = false
    }
    this.isComponentUpdated()
  }

  $ (el) {
    let element = null
    if (el) element = this.querySelector(el)
    return element
  }

  setProps () {
    this.getAttributeNames().forEach(attribute => {
      this.props[attribute] = this.getAttribute(attribute)
    })
  }

  connectedCallback () {
    this.setProps()
    this.innerHTML = this.render()
    this.style.display = 'relative'
    this.callLifeCycles()
  }

  disconnectedCallback () {
    this.atTheRemoved()
    this.destroy?.()
  }

  addEvents () {}

  // run at the first render
  atTheFirstRender () {}

  // run at every uupdate
  isComponentUpdated () {}

  // run after element's been removed
  atTheRemoved () {}

  static create (name) {
    return document.createElement(name ? `${PREFIX}-${name}` : 'div')
  }
}

const customElement = (name, component) => {
  const prefixName = `${PREFIX}-${name}`
  customElements.define(prefixName, component)
  return prefixName
}

export { Component, customElement }
