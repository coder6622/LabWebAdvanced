import React, { useEffect, useState } from 'react';
import PostsRepository from 'services/PostsRepository';
import PostEntry from './PostEntry';

function PostDetail(props) {
  const { params } = props;

  const [post, setPost] = useState();

  useEffect(() => {
    loadPost();

    async function loadPost() {
      PostsRepository.getPostDetail(params.slug).then((data) => {
        console.log(data);
        if (data) {
          setPost(data);
        } else {
          setPost();
        }
      });
    }
  }, [params.slug]);

  if (post) {
    return <PostEntry post={post}></PostEntry>;
  } else {
    return <h1>Đang tải</h1>;
  }
}

export default PostDetail;
