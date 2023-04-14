import { postFilterClientReducer } from './features/client/postFilterSlice';

const { configureStore } = require('@reduxjs/toolkit');
const { postFilterAdminReducer } = require('./features/admin/postFilterSlice');

const store = configureStore({
  reducer: {
    postFilterAdmin: postFilterAdminReducer,
    postFilterClient: postFilterClientReducer,
  },
});

export default store;
