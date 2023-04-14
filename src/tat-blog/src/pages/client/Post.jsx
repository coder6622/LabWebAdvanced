import React, { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import PostDetail from 'components/client/blog/posts/PostDetail';

function Post() {
  const params = useParams();
  return <PostDetail params={params}></PostDetail>;
}

export default Post;
