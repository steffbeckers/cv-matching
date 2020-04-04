import Vue from 'vue';
import App from './App.vue';

import './registerServiceWorker';
import router from './router';
import store from './store';
import './plugins/axios';

import vuetify from './plugins/vuetify';
import './scss/overrides.scss';
import './scss/main.scss';

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  vuetify,
  render: (h) => h(App),
}).$mount('#app');
