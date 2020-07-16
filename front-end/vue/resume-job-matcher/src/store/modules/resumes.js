import Vue from 'vue';

// initial state
const state = {
  loading: false,
  error: null,
  resumes: null,
};

// getters
const getters = {
  getMy: (state, getters, rootState) => {
    // Authenticated?
    if (!rootState.auth || !rootState.auth.authenticated) {
      return null;
    }

    return state.resumes && state.resumes.filter((r) => r.userId === rootState.auth.user.id);
  },
  getMyLatest: (state, getters, rootState) => {
    // Authenticated?
    if (!rootState.auth || !rootState.auth.authenticated) {
      return null;
    }

    // TODO: Add sort?
    return state.resumes && state.resumes.filter((r) => r.userId === rootState.auth.user.id).slice(0, 5);
  },
  getById: (state, getters, rootState) => (id) => {
    // Authenticated?
    if (!rootState.auth || !rootState.auth.authenticated) {
      return null;
    }

    return state.resumes && state.resumes.find((r) => r.id === id);
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
  getById({ commit }, id) {
    commit('GET_BY_ID');

    Vue.axios
      .get('/resumes/' + id)
      .then((result) => {
        commit('GET_BY_ID_SUCCESS', result.data);
      })
      .catch((error) => {
        commit('GET_BY_ID_FAILED', error);
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
  GET_BY_ID(state) {
    state.loading = true;
    state.error = null;
  },
  GET_BY_ID_SUCCESS(state, resume) {
    state.loading = false;

    if (state.resumes) {
      var resumeIndex = state.resumes.findIndex((r) => r.id === resume.id);
      if (resumeIndex !== -1) {
        state.resumes[resumeIndex] = resume;
      }
    } else {
      state.resumes = [resume];
    }
  },
  GET_BY_ID_FAILED(state, error) {
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
