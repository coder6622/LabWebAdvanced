import config from 'config';
import { api } from './Method';

const GetAll = async () => {
  return api({ method: 'get', url: config.endpoints.authorsAll });
};

const AuthorsRepository = {
  GetAll,
};

export default AuthorsRepository;
