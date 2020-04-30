import Vue from 'vue';
import Vuex from 'vuex';

import ui from './modules/ui';
import auth from './modules/auth';
import documents from './modules/documents';
import resumes from './modules/resumes';
import admin from './modules/admin';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {},
  actions: {},
  mutations: {},
  modules: {
    ui,
    auth,
    documents,
    resumes,
    admin,
  },
});
