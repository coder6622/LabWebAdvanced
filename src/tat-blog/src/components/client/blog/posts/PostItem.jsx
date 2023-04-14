import Card from 'react-bootstrap/Card';
import { Link } from 'react-router-dom';
import { isEmptyOrSpaces } from 'utils/Utils';

import React from 'react';
import Tags from '../tags/Tags';
import config from 'config';

function PostItem({ post }) {
  let imageUrl = isEmptyOrSpaces(post.imageUrl) ? '/images/image_1.jpg' : `${process.env.REACT_APP_PUBLIC_URL}/${post.imageUrl}`;

  let postedDate = new Date(post.postedDate);

  return (
    <article className='blog-entry mb-4'>
      <Card>
        <div className='row g-0'>
          <div className='col-md-4'>
            <Card.Img
              variant='top'
              src={imageUrl}
              alt={post.title}
            />
          </div>
          <div className='col-md-8'>
            <Card.Body>
              <Card.Title>
                <Link
                  to={config.publicRoutes.post + `/${post.urlSlug}`}
                  className='text-decoration-none text-black'
                >
                  {post.title}
                </Link>
              </Card.Title>
              <Card.Text>
                <small className='text-muted'>Tác giả:</small>
                <Link
                  to={{
                    pathname: `${config.publicRoutes.author}/${post.author.id}`,
                  }}
                  state={{ authorName: post.author.fullName }}
                  className='text-decoration-none text-primary m-1'
                  title={post.author.fullName}
                >
                  {post.author.fullName}
                </Link>
                <small className='text-muted'>Chủ đề:</small>
                <Link
                  to={{
                    pathname: `${config.publicRoutes.category}/${post.category.id}`,
                  }}
                  state={{ authorName: post.category.name }}
                  params={{ urlSlug: post.author.urlSlug }}
                  className='text-decoration-none text-primary m-1'
                  title={post.category.name}
                >
                  {post.category.name}
                </Link>
              </Card.Text>
              <Card.Text>{post.shortDescription}</Card.Text>
              <div className='tag-list'>
                <Tags tags={post.tags} />
              </div>
              <div className='text-end'>
                <Link
                  to={`/blog/post?year=${postedDate.getFullYear()}&month=${postedDate.getMonth()}&day=${postedDate.getDay()}&slug=${post.urlSlug}`}
                  className='btn btn-primary'
                  title={post.title}
                >
                  Xem chi tiết
                </Link>
              </div>
            </Card.Body>
          </div>
        </div>
      </Card>
    </article>
  );
}

export default PostItem;
