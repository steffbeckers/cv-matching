import Vue from 'vue';
import router from '../../router';

// initial state
const state = {
  loading: false,
  error: null,
  authenticated: false,
  user: null,
  token: localStorage.getItem('token') || null,
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

  // Examples
  //   cartProducts: (state, getters, rootState) => {
  //     return state.items.map(({ id, quantity }) => {
  //       const product = rootState.products.all.find(product => product.id === id)
  //       return {
  //         title: product.title,
  //         price: product.price,
  //         quantity
  //       }
  //     })
  //   },
  //   cartTotalPrice: (state, getters) => {
  //     return getters.cartProducts.reduce((total, product) => {
  //       return total + product.price * product.quantity
  //     }, 0)
  //   }
  // }
};

// actions
const actions = {
  login({ commit, state }, credentials) {
    commit('LOGIN');
    Vue.axios
      .post('/auth/login', credentials)
      .then((result) => {
        commit('LOGIN_SUCCESS', result.data);

        // Remember me
        if (result.data.rememberMe) {
          // TODO: Token? or in new action/mutation?
          localStorage.setItem('token', state.token);
        }

        // TODO: Routing? or in new action/mutation?
        router.push({ name: 'Dashboard' });
      })
      .catch((error) => {
        commit('LOGIN_FAILED', error);
      });
  },
  me({ commit, state }) {
    // Check if authenticated
    if (!state.token) {
      // If not, redirect to Login page
      // TODO: Routing? or in new action/mutation?
      router.push({ name: 'Login' });
      return;
    }

    commit('ME');
    Vue.axios
      .get('/auth/me')
      .then((result) => {
        commit('ME_SUCCESS', result.data);

        // TODO: Routing? or in new action/mutation?
        router.push({ name: 'Dashboard' });
      })
      .catch((error) => {
        commit('ME_FAILED', error);

        // TODO: Routing? or in new action/mutation?
        router.push({ name: 'Login' });
      });
  },
  logout({ commit }) {
    commit('LOGOUT');

    // TODO: Routing? or in new action/mutation?
    router.push({ name: 'Login' });

    // TODO: Token? or in action/mutation?
    localStorage.removeItem('token');
  },
};

// mutations
const mutations = {
  LOGIN(state) {
    state.loading = true;
    state.error = null;
  },
  LOGIN_SUCCESS(state, authenticated) {
    state.loading = false;
    state.authenticated = true;
    state.user = authenticated.user;
    state.token = authenticated.token;
    state.rememberMe = authenticated.rememberMe;
  },
  LOGIN_FAILED(state, error) {
    state.loading = false;
    state.error = error;
  },
  ME(state) {
    state.loading = true;
    state.error = null;
  },
  ME_SUCCESS(state, user) {
    state.loading = true;
    state.authenticated = true;
    state.user = user;
  },
  ME_FAILED(state, error) {
    state.loading = false;
    state.authenticated = false;
    state.error = error;
  },
  LOGOUT(state) {
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
