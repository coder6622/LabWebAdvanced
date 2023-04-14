const { configureStore } = require('@reduxjs/toolkit');
const { reducer } = require('./features/admin/postFilterSlice');

const store = configureStore({
  reducer: {
    postFilterAdmin: reducer,
    postFilterClient: reducer,
  },
});

export default store;
