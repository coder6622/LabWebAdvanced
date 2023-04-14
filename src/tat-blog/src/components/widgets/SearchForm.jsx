import { useState } from 'react';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { useDispatch } from 'react-redux';
import { updateKeyword } from 'store/features/client/postFilterSlice';
const SearchForm = () => {
  const [keyword, setKeyword] = useState('');

  const dispatch = useDispatch();

  const handleSubmit = (e) => {
    e.preventDefault();
    const search = keyword.trim();
    dispatch(updateKeyword(search));
  };

  return (
    <div className='mb-4'>
      <Form onSubmit={handleSubmit}>
        <Form.Group className='input-group mb-3'>
          <Form.Control
            type='text'
            value={keyword}
            onChange={(e) => setKeyword(e.target.value)}
            aria-label='Enter keyword'
            aria-describedby='btnSearchPost'
            placeholder='Enter keyword'
          />
          <Button
            id='btnSearchPost'
            variant='outline-secondary'
            type='submit'
          >
            <FontAwesomeIcon icon={faSearch} />
          </Button>
        </Form.Group>
      </Form>
    </div>
  );
};
export default SearchForm;
