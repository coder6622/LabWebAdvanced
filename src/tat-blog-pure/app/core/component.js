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

  addEvents () {
    this.querySelectorAll('*').forEach(element => {
      element.getAttributeNames().forEach(attribute => {
        // if the attribute starts with '@'
        // then we will add the event to the element
        // example : @click="handleClick" will add the event 'click' to the element
        // and the function 'handleClick' will be called when the event is triggered
        if (attribute.startsWith('@')) {
          const event = attribute.split('@')[1]
          let callback = element.getAttribute(attribute)

          if (callback.match(/[()]/)) {
            // if the callback has parameters then get the parameters between the parenthesis
            callback = callback.split('(')[0] // get the function name without the parameters

            const parameters = element
              .getAttribute(attribute)
              .split('(')[1]
              .split(')')[0]

            if (this[callback]) {
              // And call the function with the parameters
              element.addEventListener(event, () => {
                try {
                  const parsed = JSON.parse(parameters)
                  this[callback].bind(this)(JSON.parse(parsed))
                } catch (e) {
                  this[callback].bind(this)(parameters)
                }
              })
            }
          } else {
            // if the callback is a function in the component
            // then call the function
            if (this[callback]) {
              element.addEventListener(event, this[callback].bind(this))
            }
          }
        }
      })
    })
  }

  connectedCallback () {
    this.setProps()
    this.innerHTML = this.render()
    this.addEvents()
    this.callLifeCycles()
  }

  disconnectedCallback () {
    this.atTheRemoved()
    this.destroy?.()
  }

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
