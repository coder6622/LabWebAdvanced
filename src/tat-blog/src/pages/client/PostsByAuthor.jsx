import React from 'react';
import { useLocation, useParams } from 'react-router-dom';
import PostSearch from 'components/client/blog/posts/PostSearch';

function PostsByAuthor() {
  const params = useParams();
  const location = useLocation();
  const { authorName } = location.state;

  return (
    <div className='container'>
      <h1 className='mt-3'>
        {params ? (
          <>
            Bài viết theo tác giả:
            <span className='text-danger'> {authorName}</span>
          </>
        ) : (
          'Danh sách bài viết'
        )}
      </h1>

      <PostSearch params={params} />
    </div>
  );
}

export default PostsByAuthor;
