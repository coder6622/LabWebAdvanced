import React, { useEffect, useState } from 'react';
import PostItem from './PostItem';
import Pager from '../../controls/Pager';
import { getPosts } from '../../../services/BlogRepository';

function PostSearch(props) {
  const { querySearch } = props;
  const [posts, setPosts] = useState({
    items: [],
    metadata: {},
  });
  const [pageNumber, setPageNumber] = useState(1);

  function updatePageNumber(inc) {
    setPageNumber((currentVal) => currentVal + inc);
  }

  useEffect(() => {
    loadPosts();

    async function loadPosts() {
      console.log(querySearch);
      const query = `${new URLSearchParams({
        pageNumber: pageNumber || 1,
        pageSize: 2,
      })}&${querySearch}`;

      console.log(query);
      getPosts(query).then((data) => {
        if (data) {
          setPosts(data);
        } else
          setPosts({
            items: [],
            metadata: {},
          });
      });
    }
  }, [pageNumber, querySearch]);

  if (posts.items.length > 0) {
    return (
      <div className='p-4'>
        {posts.items.map((item, index) => (
          <PostItem
            post={item}
            key={index}
          ></PostItem>
        ))}
        <Pager
          metadata={posts.metadata}
          onPageChange={updatePageNumber}
        />
      </div>
    );
  }
  return <></>;
}

export default PostSearch;
