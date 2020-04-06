import Vue from 'vue';
import App from './App.vue';

// TODO
//import './registerServiceWorker';
import router from './router';
import store from './store';

// Plugins
import vuetify from './plugins/vuetify';
import './plugins/axios';

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  vuetify,
  render: (h) => h(App),
}).$mount('#app');
