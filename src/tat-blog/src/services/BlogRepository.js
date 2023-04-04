import axios from 'axios';

export async function getPosts(parameters) {
  try {
    console.log('fetch');
    const reponse = await axios.get(`${process.env.REACT_APP_API_URL}/posts?${parameters}`);

    const data = reponse.data;
    if (data.isSuccess) {
      return data.result;
    } else return null;
  } catch (error) {
    console.log('Error', error.message);
  }
}
