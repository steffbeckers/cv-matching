// https://www.npmjs.com/package/vue-axios

import Vue from 'vue';
import axios from 'axios';
import VueAxios from 'vue-axios';

Vue.use(VueAxios, axios);

// API endpoint
Vue.axios.defaults.baseURL = process.env.VUE_APP_API;
