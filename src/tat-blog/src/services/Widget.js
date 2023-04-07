import http from '../http-common';
export async function getCategories() {
  try {
    const response = await http.get('/categories/all');
    const data = response.data;
    if (data.isSuccess) return data.result;
    else return null;
  } catch (error) {
    console.log('Error', error.message);
    return null;
  }
}
