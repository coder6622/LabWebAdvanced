import React, { useState, useEffect } from 'react';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import { Link } from 'react-router-dom';
import { monthsLong } from 'utils/Utils';
import { useDispatch, useSelector } from 'react-redux';
import PostsRepository from 'services/PostsRepository';
import { updateAuthorId, updateCategoryId, updateKeyword, updateMonth, updateYear } from 'store/features/admin/postFilterSlice';

export default function PostFilterPane() {
  const current = new Date(),
    [filterData, setFilterData] = useState({
      authors: [],
      categories: [],
    });

  const postFilter = useSelector((state) => state.postFilterAdmin),
    dispatch = useDispatch();

  const handleSubmit = (e) => {
    e.preventDefault();
  };

  useEffect(() => {
    PostsRepository.GetFilterData().then((data) => {
      if (data) {
        setFilterData({
          authors: data.authors,
          categories: data.categories,
        });
      } else {
        setFilterData({
          authors: [],
          categories: [],
        });
      }
    });
  }, []);
  return (
    <Form
      method='get'
      onSubmit={handleSubmit}
      className='row gy-2 gx-3 align-items-center p-2'
    >
      <Form.Group className='col-auto'>
        <Form.Label className='visually-hidden'>Keyword</Form.Label>
        <Form.Control
          type='text'
          placeholder='Nhập từ khóa...'
          name='keyword'
          value={postFilter.Keyword}
          onChange={(e) => dispatch(updateKeyword(e.target.value))}
        />
      </Form.Group>
      <Form.Group className='col-auto'>
        <Form.Label className='visually-hidden'>AuthorId</Form.Label>
        <Form.Select
          name='authorId'
          value={postFilter.AuthorId}
          onChange={(e) => dispatch(updateAuthorId(e.target.value))}
          title='Author Id'
        >
          <option value=''>-- Chọn tác giả --</option>
          {filterData.authors.length > 0 &&
            filterData.authors.map((item, index) => (
              <option
                key={index}
                value={item.id}
              >
                {item.fullName}
              </option>
            ))}
        </Form.Select>
      </Form.Group>
      <Form.Group className='col-auto'>
        <Form.Label className='visually-hidden'>CategoryId</Form.Label>
        <Form.Select
          name='categoryId'
          value={postFilter.CategoryId}
          onChange={(e) => dispatch(updateCategoryId(e.target.value))}
          title='Category Id'
        >
          <option value=''>-- Chọn chủ đề --</option>
          {filterData.categories.length > 0 &&
            filterData.categories.map((item, index) => (
              <option
                key={index}
                value={item.id}
              >
                {item.name}
              </option>
            ))}
        </Form.Select>
      </Form.Group>
      <Form.Group className='col-auto'>
        <Form.Label className='visually-hidden'>Year</Form.Label>
        <Form.Control
          type='number'
          placeholder='Nhập năm...'
          name='year'
          value={postFilter.PostedYear}
          max={current.getFullYear()}
          onChange={(e) => dispatch(updateYear(e.target.value))}
        />
      </Form.Group>
      <Form.Group className='col-auto'>
        <Form.Label className='visually-hidden'>Month</Form.Label>
        <Form.Select
          name='month'
          value={postFilter.PostedMonth}
          onChange={(e) => dispatch(updateMonth(e.target.value))}
          title='Month'
        >
          <option value=''>-- Chọn tháng --</option>
          {monthsLong.length > 0 &&
            monthsLong.map((item, index) => {
              return (
                <option
                  key={index}
                  value={item.value}
                >
                  {item.nameMonth}
                </option>
              );
            })}
        </Form.Select>
      </Form.Group>
      <Form.Group className='col-auto'>
        <Button
          variant='primary'
          type='submit'
        >
          Tìm/lọc
        </Button>
        <Link
          to='/admin/posts/edit'
          className='btn btn-success ms-2'
        >
          Thêm mới
        </Link>
      </Form.Group>
    </Form>
  );
}
