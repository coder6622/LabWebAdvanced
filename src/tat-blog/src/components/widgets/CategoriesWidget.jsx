import React, { useEffect, useState } from 'react';
import { getCategories } from '../../services/Widget';
import ListGroup from 'react-bootstrap/ListGroup';
import { Link } from 'react-router-dom';

function CategoriesWidget() {
  const [categories, setCategories] = useState([]);

  useEffect(() => {
    getCategories().then((data) => {
      if (data) setCategories(data);
      else setCategories([]);
    });
  });

  return (
    <div className='mb-4'>
      <h3 className='text-success mb-2'>Các chủ đề</h3>
      {categories.length > 0 && (
        <ListGroup>
          {categories.map((item, index) => {
            return (
              <ListGroup.Item key={index}>
                <Link
                  to={`/blog/category?slug=${item.urlSlug}`}
                  title={item.description}
                  key={index}
                >
                  {item.name}
                  <span>&nbsp;({item.postCount})</span>
                </Link>
              </ListGroup.Item>
            );
          })}
        </ListGroup>
      )}
    </div>
  );
}

export default CategoriesWidget;
