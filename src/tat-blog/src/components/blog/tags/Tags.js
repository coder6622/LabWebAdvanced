import React from 'react';
import { Link } from 'react-router-dom';
import routes from '../../config/routes';

function Tags({ tags }) {
  if (tags && Array.isArray(tags) && tags.length > 0) {
    return (
      <>
        {tags.map((item, index) => {
          let params = new URLSearchParams({ slug: item.name });
          return (
            <Link
              to={`${routes.tag}?${params}`}
              className='btn btn-sm btn-outline-secondary mx-1'
              key={index}
            >
              {item.name}
            </Link>
          );
        })}
      </>
    );
  }

  return <></>;
}

export default Tags;