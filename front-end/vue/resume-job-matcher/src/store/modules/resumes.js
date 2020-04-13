import Vue from 'vue';

// initial state
const state = {
  loading: false,
  error: null,
  resumes: null,
};

// getters
const getters = {
  getMyLatest: (state, getters, rootState) => {
    // Authenticated?
    if (!rootState.auth || !rootState.auth.authenticated) {
      return null;
    }

    return state.resumes && state.resumes.filter((r) => r.userId === rootState.auth.user.id).slice(0, 5);
  },
};

// actions
const actions = {
  getAll({ commit }) {
    commit('GET_ALL');

    Vue.axios
      .get('/resumes')
      .then((result) => {
        commit('GET_ALL_SUCCESS', result.data);
      })
      .catch((error) => {
        commit('GET_ALL_FAILED', error);
      });
  },
};

// mutations
const mutations = {
  GET_ALL(state) {
    state.loading = true;
    state.error = null;
  },
  GET_ALL_SUCCESS(state, resumes) {
    state.loading = false;
    state.resumes = resumes;
  },
  GET_ALL_FAILED(state, error) {
    state.loading = false;
    state.error = error;
  },
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
};
