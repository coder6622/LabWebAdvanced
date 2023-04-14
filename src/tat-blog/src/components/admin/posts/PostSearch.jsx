/* eslint-disable react-hooks/exhaustive-deps */
import React, { useEffect } from 'react';

import useFetch from 'hooks/useFetch';

import { Table } from 'react-bootstrap';
import Loading from 'components/widgets/Loading';
import config from 'config';
import { Link } from 'react-router-dom';
import Pager from 'components/controls/Pager';
import { useDispatch, useSelector } from 'react-redux';
import { updatePageNumber } from 'store/features/admin/postFilterSlice';
import Error from 'pages/shared/Error';

export default function PostSearch(props) {
  const postFilter = useSelector((state) => state.postFilterAdmin);

  const { response, error, loading, axiosFetchAsync } = useFetch();

  const dispatch = useDispatch();

  function updatePage(inc) {
    dispatch(updatePageNumber(inc));
  }

  useEffect(() => {
    loadPosts();

    async function loadPosts() {
      const postFilterNotEmpty = Object.entries(postFilter).reduce((a, [k, v]) => (v === '' ? a : ((a[k] = v), a)), {});

      const query = new URLSearchParams({
        ...postFilterNotEmpty,
      });

      await axiosFetchAsync({
        method: 'get',
        url: config.endpoints.posts,
        params: query,
      });
    }
  }, [postFilter]);

  return (
    <>
      {loading ? (
        <Loading />
      ) : error ? (
        <Error message={error.message} />
      ) : (
        <>
          <Table
            striped
            responsive
            bordered
          >
            <thead>
              <tr>
                <th>Tiêu đề</th>
                <th>Tác giả</th>
                <th>Chủ đề</th>
                <th>Xuất bản</th>
              </tr>
            </thead>
            <tbody>
              {response.items.length > 0 ? (
                response.items.map((item, index) => (
                  <tr key={index}>
                    <td>
                      <Link
                        to={`/admin/posts/edit/${item.id}`}
                        className='text-bold'
                      >
                        {item.title}
                      </Link>
                      <p className='text-muted'>{item.shortDescription}</p>
                    </td>
                    <td>{item.author.fullName}</td>
                    <td>{item.category.name}</td>
                    <td>{item.published ? 'Có' : 'Không'}</td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={4}>
                    <h4 className='text-danger text-center'>Không tìm thấy bài viết nào</h4>
                  </td>
                </tr>
              )}
            </tbody>
          </Table>
          <Pager
            metadata={response.metadata}
            onPageChange={updatePage}
          />
        </>
      )}
    </>
  );
}
