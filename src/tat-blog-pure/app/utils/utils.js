export function isEmptyOrSpaces (str) {
  return (
    str == null ||
    (typeof src === 'string' &&
      (str.match(/^ *$/) !== null || str.length === 0))
  )
}

export function exactHashPath (path) {
  const url = path.split('#')[1]
  return url ? url : '/'
}

export function getCurrentPath () {
  const hashPath = window.location.hash
  if (hashPath) {
    return hashPath
  }
  return '/'
}

export function getPath () {
  return exactHashPath(getCurrentPath().split('?')[0])
}

export function getQueries () {
  const queries = {}
  const fullPath = window.location.hash.split('?')
  const currentPath = exactHashPath(fullPath[0])
  const search = fullPath[1]

  if (search) {
    const queryArr = search.split('&')
    queryArr.forEach(item => {
      const [key, value] = item.split('=')
      queries[key] = value
    })
  }
  return {
    currentPath: currentPath,
    queries: queries
  }
}

export function getPrevPath () {
  return exactHashPath(window.history.state.prevUrl.split('?')[0])
}
