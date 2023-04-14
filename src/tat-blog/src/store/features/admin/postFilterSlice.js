const { createSlice } = require('@reduxjs/toolkit');

const initialState = {
  Keyword: '',
  AuthorId: '',
  CategoryId: '',
  PostedYear: '',
  PostedMonth: '',
  PageNumber: 1,
  PageSize: 4,
};

const postFilterSlice = createSlice({
  name: 'postFilterAdmin',
  initialState,
  reducers: {
    reset: (state, action) => {
      return initialState;
    },

    updateKeyword: (state, action) => {
      return {
        ...state,
        Keyword: action.payload,
        PageNumber: 1,
      };
    },

    updateAuthorId: (state, action) => {
      return {
        ...state,
        AuthorId: action.payload,
        PageNumber: 1,
      };
    },

    updateCategoryId: (state, action) => {
      return {
        ...state,
        CategoryId: action.payload,
        PageNumber: 1,
      };
    },

    updateMonth: (state, action) => {
      return {
        ...state,
        PostedMonth: action.payload,
        PageNumber: 1,
      };
    },

    updateYear: (state, action) => {
      return {
        ...state,
        PostedYear: action.payload,
        PageNumber: 1,
      };
    },

    updatePageNumber: (state, action) => {
      var newVal = action.payload + state.PageNumber;
      return {
        ...state,
        PageNumber: newVal,
      };
    },
  },
});

export const { reset, updateKeyword, updateAuthorId, updateCategoryId, updateMonth, updateYear, updatePageNumber } = postFilterSlice.actions;

export const postFilterAdminReducer = postFilterSlice.reducer;
