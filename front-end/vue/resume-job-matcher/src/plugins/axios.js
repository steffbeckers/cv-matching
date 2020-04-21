// https://www.npmjs.com/package/vue-axios

import Vue from 'vue';
import axios from 'axios';
import VueAxios from 'vue-axios';
import store from '../store';

Vue.use(VueAxios, axios);

// API endpoint
Vue.axios.defaults.baseURL = process.env.VUE_APP_API;

Vue.axios.interceptors.response.use(
  function(response) {
    return response;
  },
  function(error) {
    // If request status is 0 (no connection to API)
    if (error.request.status === 0) {
      return Promise.reject({ message: 'Cannot create a connection to the server. Please check your network connection.' });
    }

    // Log out on unauthorized
    if (error.response && error.response.status === 401) {
      store.dispatch('auth/logout');
    }

    return Promise.reject(error);
  }
);
