import { useEffect, useState } from 'react';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { useLocation, useNavigate } from 'react-router-dom';
const SearchForm = () => {
  const querySearch = useLocation().search;
  const [keyword, setKeyword] = useState('');

  useEffect(() => {
    const paramsSearch = new URLSearchParams(querySearch);
    setKeyword(paramsSearch.get('Keyword') ?? '');
  }, [querySearch]);
  const navigate = useNavigate();

  const handleSubmit = (e) => {
    e.preventDefault();
    navigate(`?Keyword=${keyword}`);
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
