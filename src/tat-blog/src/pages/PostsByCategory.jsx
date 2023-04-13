import React from 'react';
import { useLocation, useParams } from 'react-router-dom';
import PostSearch from '../components/blog/posts/PostSearch';

function PostsByCategory() {
  const params = useParams();
  const location = useLocation();
  const { authorName } = location.state;

  return (
    <div className='container'>
      <h1 className='mt-3'>
        {params ? (
          <>
            Bài viết theo chủ đề:
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

export default PostsByCategory;
