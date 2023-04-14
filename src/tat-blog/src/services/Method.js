import http from '../http-common';

export async function api(params) {
  try {
    const response = await http.request(params);

    const data = response.data;
    if (data.isSuccess) {
      return data.result;
    } else return null;
  } catch (error) {
    console.log('Error', error.message);
    return null;
  }
}
