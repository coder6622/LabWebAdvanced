const { createSlice } = require('@reduxjs/toolkit');

const initialState = {
  Keyword: '',
  PageNumber: 1,
  PageSize: 5,
};

const postFilterSlice = createSlice({
  name: 'postFilterClient',
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

    updatePageNumber: (state, action) => {
      var newVal = action.payload + state.PageNumber;
      return {
        ...state,
        PageNumber: newVal,
      };
    },
  },
});

export const { reset, updateKeyword, updatePageNumber } = postFilterSlice.actions;

export const postFilterClientReducer = postFilterSlice.reducer;
