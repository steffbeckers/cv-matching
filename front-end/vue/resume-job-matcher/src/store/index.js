import Vue from 'vue';
import Vuex from 'vuex';

import auth from './modules/auth';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    // drawer: false,
  },
  actions: {
    // setDrawer({ commit }, payload) {
    //   commit('SET_DRAWER', payload);
    // },
  },
  mutations: {
    // SET_DRAWER(state, payload) {
    //   state.drawer = payload;
    // },
  },
  modules: {
    auth,
  },
});
