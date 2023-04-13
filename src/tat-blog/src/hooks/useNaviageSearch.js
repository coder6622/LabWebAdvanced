import { createSearchParams, useNavigate } from 'react-router-dom';
const useNaviageSearch = () => {
  const naviagate = useNavigate();
  return (pathName, params) => naviagate(`${pathName}?${createSearchParams(params)}`);
};

export default useNaviageSearch;
