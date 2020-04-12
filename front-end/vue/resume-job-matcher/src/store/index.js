import Vue from 'vue';
import Vuex from 'vuex';

import ui from './modules/ui';
import auth from './modules/auth';
import documents from './modules/documents';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {},
  actions: {},
  mutations: {},
  modules: {
    ui,
    auth,
    documents,
  },
});
