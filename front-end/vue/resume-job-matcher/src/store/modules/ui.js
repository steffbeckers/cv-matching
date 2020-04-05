// initial state
const state = {
  topNav: true,
  drawerLeft: false,
  footer: true,
};

// getters
const getters = {};

// actions
const actions = {
  setTopNav({ commit }, payload) {
    commit('SET_TOP_NAV', payload);
  },
  setDrawerLeft({ commit }, payload) {
    commit('SET_DRAWER_LEFT', payload);
  },
  setFooter({ commit }, payload) {
    commit('SET_FOOTER', payload);
  },
};

// mutations
const mutations = {
  SET_TOP_NAV(state, payload) {
    state.drawerLeft = payload;
  },
  SET_DRAWER_LEFT(state, payload) {
    state.drawerLeft = payload;
  },
  SET_FOOTER(state, payload) {
    state.drawerLeft = payload;
  },
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
};
