import React from 'react';
import { isEmptyOrSpaces } from 'utils/Utils';
import { Link } from 'react-router-dom';
import config from 'config';
import Tags from '../tags/Tags';

function PostEntry({ post }) {
  let imageUrl = isEmptyOrSpaces(post.imageUrl) ? '/images/image_1.jpg' : `${process.env.REACT_APP_PUBLIC_URL}/${post.imageUrl}`;
  const dateOfPost = post.modifiedDate ? new Date(post.modifiedDate) : new Date(post.postedDate);

  return (
    <div className='container'>
      <div className='row'>
        <h1>{post.title}</h1>
        <p className='lead'>
          <span>
            <i className='fa fa-user-secret'></i>
            <small className='text-secondary'>Đăng bởi:</small>
            <Link
              to={{
                pathname: `${config.routes.author}/${post.author.id}`,
              }}
              state={{ authorName: post.author.fullName }}
              className='text-decoration-none text-primary m-1'
              title={post.author.fullName}
            >
              {post.author.fullName}
            </Link>
          </span>
          &emsp;
          <span>
            <i className='fa fa-brands fa-themeisle'></i>
            <small className='text-muted'>Chủ đề:</small>
            <Link
              to={{
                pathname: `${config.routes.category}/${post.category.id}`,
              }}
              state={{ authorName: post.category.name }}
              params={{ urlSlug: post.author.urlSlug }}
              className='text-decoration-none text-primary m-1'
              title={post.category.name}
            >
              {post.category.name}
            </Link>
          </span>
          &emsp;
          <span>
            <i className='fa fa-solid fa-eye'></i>
            <span className='text-info fw-semibold'>{post.viewCount}</span>
          </span>
        </p>

        <hr />
        <p className='lead'>
          <i className='fa fa-calendar'></i>

          <>
            <small className='text-muted'>Cập nhật lần cuối:</small>
            {dateOfPost.toString()}
          </>
        </p>
        <p className='lead'>
          <i className='fa fa-tags'></i>
          <span className='text-muted'>Tags:&emsp;</span>
          {post.tags.length > 0 ? <Tags tags={post.tags}></Tags> : <p> Bài viết chưa được gắn thẻ</p>}
        </p>

        <hr />

        <p className='lead'>{post.shortDescription}</p>
        <img
          src={imageUrl}
          alt={post.title}
        />
        <p className='pt-4'>{post.shortDescription}</p>
      </div>
    </div>
  );
}

export default PostEntry;
