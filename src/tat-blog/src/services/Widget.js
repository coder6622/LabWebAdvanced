import axios from 'axios';
export async function getCategories() {
  try {
    const response = await axios.get(`${process.env.REACT_APP_API_URL}/categories/all`);
    const data = response.data;
    if (data.isSuccess) return data.result;
    else return null;
  } catch (error) {
    console.log('Error', error.message);
    return null;
  }
}
