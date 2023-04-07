import config from '../config';
import http from '../http-common';

const getPosts = async (params) => {
  try {
    console.log('fetch');
    const reponse = await http.get(config.endpoints.posts, { params });

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
    const reponse = await http.get(config.endpoints.postBySlug + `${slug}`);

    const data = reponse.data;
    if (data.isSuccess) {
      return data.result;
    } else return null;
  } catch (error) {
    console.log('Error', error.message);
  }
};

const PostsRepository = {
  getPosts,
  getPostDetail,
};

export default PostsRepository;
