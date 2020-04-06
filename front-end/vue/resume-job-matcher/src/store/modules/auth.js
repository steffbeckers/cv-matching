import Vue from 'vue';
import router from '../../router';

// initial state
const state = {
  loading: false,
  error: null,
  credentials: null,
  authenticated: false,
  user: null,
  token: null,
  rememberMe: true,
};

// getters
const getters = {
  isAdmin: (state) => {
    return state.user && state.user.roles && state.user.roles.includes('Admin');
  },
  rolesList: (state) => {
    return state.user && state.user.roles && state.user.roles.join(', ');
  },
};

// actions
const actions = {
  login({ commit, state }, credentials) {
    commit('LOGIN', credentials);

    Vue.axios
      .post('/auth/login', credentials)
      .then((result) => {
        commit('LOGIN_SUCCESS', result.data);

        // TOKEN
        actions.setToken({ commit, state }, state.token);

        router.push({ name: 'Dashboard' });
      })
      .catch((error) => {
        commit('LOGIN_FAILED', error);
      });
  },
  setToken({ commit, state }, token) {
    commit('SET_TOKEN', token);

    // Remember me
    if (state.rememberMe) {
      localStorage.setItem('token', state.token);
    }

    // API auth
    Vue.axios.defaults.headers.common['Authorization'] = 'Bearer ' + state.token;

    commit('SET_TOKEN_SUCCESS');

    // Get me
    actions.me({ commit, state });
  },
  getToken({ commit, state }) {
    commit('GET_TOKEN');

    let token = localStorage.getItem('token');
    if (token) {
      commit('GET_TOKEN_SUCCESS');

      actions.setToken({ commit, state }, token);
    } else {
      commit('GET_TOKEN_FAILED');

      router.push({ name: 'Login' });
    }
  },
  me({ commit, state }) {
    // Check if authenticated
    if (!state.token) {
      router.push({ name: 'Login' });
      return;
    }

    commit('ME');

    Vue.axios
      .get('/auth/me')
      .then((result) => {
        commit('ME_SUCCESS', result.data);
      })
      .catch((error) => {
        commit('ME_FAILED', error);

        router.push({ name: 'Login' });
      });
  },
  logout({ commit }) {
    commit('LOGOUT');

    localStorage.removeItem('token');

    commit('LOGOUT_SUCCESS');

    router.push({ name: 'Login' });
  },
};

// mutations
const mutations = {
  LOGIN(state, credentials) {
    state.loading = true;
    state.error = null;
    state.credentials = credentials;
  },
  LOGIN_SUCCESS(state, authenticated) {
    state.loading = false;
    state.credentials = null;
    state.authenticated = true;
    state.user = authenticated.user;
    state.token = authenticated.token;
    state.rememberMe = authenticated.rememberMe;
  },
  LOGIN_FAILED(state, error) {
    state.loading = false;
    state.credentials = null;
    state.error = error;
  },
  GET_TOKEN() {},
  GET_TOKEN_SUCCESS() {},
  GET_TOKEN_FAILED() {},
  SET_TOKEN(state, token) {
    state.token = token;
  },
  SET_TOKEN_SUCCESS() {},
  ME(state) {
    state.loading = true;
    state.error = null;
  },
  ME_SUCCESS(state, user) {
    state.loading = false;
    state.authenticated = true;
    state.user = user;
  },
  ME_FAILED(state, error) {
    state.loading = false;
    state.authenticated = false;
    state.error = error;
  },
  LOGOUT(state) {
    state.loading = true;
  },
  LOGOUT_SUCCESS() {
    state.loading = false;
    state.error = null;
    state.authenticated = false;
    state.user = null;
    state.token = null;
    state.rememberMe = false;
  },
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
};
