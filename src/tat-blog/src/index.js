import React from 'react';
import ReactDOM from 'react-dom/client';
import GlobalStyles from './components/GlobalStyles';
import App from './App';
import { Provider } from 'react-redux';
import store from 'store/store';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <Provider store={store}>
      <GlobalStyles>
        <App />
      </GlobalStyles>
    </Provider>
  </React.StrictMode>,
  // <>
  //   <GlobalStyles>
  //     <App />
  //   </GlobalStyles>
  // </>,
);
