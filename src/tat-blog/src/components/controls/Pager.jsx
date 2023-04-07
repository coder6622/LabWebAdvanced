import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowLeft, faArrowRight } from '@fortawesome/free-solid-svg-icons';
import Button from 'react-bootstrap/Button';

function Pager(props) {
  const { metadata, onPageChange } = props;
  let pageCount = metadata.pageCount,
    hasNextPage = metadata.hasNextPage,
    hasPreviousPage = metadata.hasPreviousPage;

  if (pageCount > 1) {
    return (
      <div className='text-center my-4'>
        {hasPreviousPage ? (
          <button
            type='button'
            onClick={() => onPageChange(-1)}
            className='btn btn-info'
          >
            <FontAwesomeIcon icon={faArrowLeft} />
            &nbsp;Trang trước
          </button>
        ) : (
          <Button
            variant='outline-secondary'
            disabled
          >
            <FontAwesomeIcon icon={faArrowLeft} />
            &nbsp;Trang trước
          </Button>
        )}
        {hasNextPage ? (
          <button
            type='button'
            onClick={() => onPageChange(1)}
            className='btn btn-info mx-2'
          >
            Trang sau&nbsp;
            <FontAwesomeIcon icon={faArrowRight} />
          </button>
        ) : (
          <Button
            className='ms-1'
            variant='outline-secondary'
            disabled
          >
            Trang sau&nbsp;
            <FontAwesomeIcon icon={faArrowRight} />
          </Button>
        )}
      </div>
    );
  }
  return <Link></Link>;
}
export default Pager;
