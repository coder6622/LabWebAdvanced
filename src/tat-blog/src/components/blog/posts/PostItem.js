import { isEmptyOrSpaces } from '../../../utils/Utils';
import Card from 'react-bootstrap/Card';
import { Link } from 'react-router-dom';

import React from 'react';

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
              <Card.Title>{post.title}</Card.Title>
              <Card.Text>
                <small className='text-muted'>Tác giả:</small>
                <span className='text-primary m-1'>{post.author.fullName}</span>
                <small className='text-muted'>Chủ đề:</small>
                <span className='text-primary m-1'>{post.category.name}</span>
              </Card.Text>
              Phát triển ứng dụng Web nâng cao 2023 Lab 5 Trang 17
              <Card.Text>{post.shortDescription}</Card.Text>
              <div className='tag-list'>{/* <Tags tags={post.tags} /> */}</div>
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
