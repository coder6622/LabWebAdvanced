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
