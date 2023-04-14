const { default: config } = require('config');
const { api } = require('./Method');

const GetAll = async () => {
  return api({ method: 'get', url: config.endpoints.categoriesAll });
};

const CategoriesRespository = {
  GetAll,
};

export default CategoriesRespository;
