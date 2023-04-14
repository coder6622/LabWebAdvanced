import config from '../config';
import myConfig from '../config';
import http from '../http-common';
import { api } from './Method';

const GetPosts = async (params) => {
  try {
    console.log('fetch');
    const reponse = await http.get(myConfig.endpoints.posts, { params });

    const data = reponse.data;
    if (data.isSuccess) {
      return data.result;
    } else return null;
  } catch (error) {
    console.log('Error', error.message);
  }
};

const getPostDetail = async (slug) => {
  try {
    const reponse = await http.get(myConfig.endpoints.postBySlug + `${slug}`);

    const data = reponse.data;
    if (data.isSuccess) {
      return data.result;
    } else return null;
  } catch (error) {
    console.log('Error', error.message);
  }
};

const GetFilterData = async () => {
  return api({
    method: 'get',
    url: config.endpoints.postFilterData,
  });
};

const PostsRepository = {
  GetPosts,
  getPostDetail,
  GetFilterData,
};

export default PostsRepository;
