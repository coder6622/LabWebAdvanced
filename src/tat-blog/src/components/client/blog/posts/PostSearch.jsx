/* eslint-disable react-hooks/exhaustive-deps */
import React, { useEffect } from 'react';
import PostItem from './PostItem';
import Pager from 'components/controls/Pager';
import { useDispatch, useSelector } from 'react-redux';
import useFetch from 'hooks/useFetch';
import config from 'config';
import { updatePageNumber } from 'store/features/client/postFilterSlice';
import Loading from 'components/widgets/Loading';
import Error from 'pages/shared/Error';

function PostSearch(props) {
  const { params } = props;

  const postFilter = useSelector((state) => state.postFilterClient);

  const dispatch = useDispatch();

  const { response, error, loading, axiosFetchAsync } = useFetch();

  function updatePage(inc) {
    dispatch(updatePageNumber(inc));
  }

  useEffect(() => {
    loadPosts();

    async function loadPosts() {
      const postFilterNotEmpty = Object.entries(postFilter).reduce((a, [k, v]) => (v === '' ? a : ((a[k] = v), a)), {});

      const query = new URLSearchParams({
        ...postFilterNotEmpty,
        ...params,
      });

      await axiosFetchAsync({
        method: 'get',
        url: config.endpoints.posts,
        params: query,
      });
    }
  }, [postFilter, params]);

  return (
    <>
      {loading ? (
        <Loading />
      ) : error ? (
        <Error message={error.message} />
      ) : (
        <div className='p-4'>
          {response.items.length > 0 ? (
            response.items.map((item, index) => (
              <PostItem
                post={item}
                key={index}
              ></PostItem>
            ))
          ) : (
            <h1>Không có bài viết nào</h1>
          )}
          <Pager
            metadata={response.metadata}
            onPageChange={updatePage}
          />
        </div>
      )}
    </>
  );
}

export default PostSearch;
