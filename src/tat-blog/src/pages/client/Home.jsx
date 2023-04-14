/* eslint-disable react-hooks/exhaustive-deps */
import React from 'react';
import PostSearch from '../../components/client/blog/posts/PostSearch';
import { useSelector } from 'react-redux';

function Home() {
  const keyword = useSelector((state) => state.postFilterClient.Keyword);

  return (
    <div className='container'>
      <h1 className='mt-3'>
        {keyword.length !== 0 ? (
          <>
            Bài viết theo từ khóa:
            <span className='text-danger'> {keyword}</span>
          </>
        ) : (
          'Danh sách bài viết'
        )}
      </h1>

      <PostSearch />
    </div>
  );
}

export default Home;
