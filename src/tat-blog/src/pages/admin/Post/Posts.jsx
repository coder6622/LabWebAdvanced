import PostFilterPane from 'components/admin/posts/PostFilterPane';
import PostSearch from 'components/admin/posts/PostSearch';
import React, { useEffect } from 'react';

export default function Posts() {
  useEffect(() => {
    document.title = 'Danh sách bài viết';
  }, []);

  return (
    <div className='container-fluid'>
      <h1 className='mt-3'>Danh sách bài viết</h1>

      <PostFilterPane />

      <PostSearch />
    </div>
  );
}
