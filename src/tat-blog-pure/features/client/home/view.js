export class View {
  appendPosts (posts, metadata) {
    const view = `
      <div class="page-content-container">
        <div class="row">
          ${posts
            .map(
              post =>
                `<app-post-item 
                  id='${post.id}'
                  data-post='${JSON.stringify(post)}'></app-post-item>`
            )
            .join('\n')}
        </div>
        <app-pager 
          data-current-page=${metadata.pageNumber} 
          data-page-size=${metadata.pageSize} 
          data-page-count=${metadata.pageCount} 
          data-has-next-page=${metadata.hasNextPage} 
          data-has-prev-page=${metadata.hasPreviousPage} 
          class='d-flex justify-content-center'>
        </app-pager>
      </div>
    `

    return view
  }
}
