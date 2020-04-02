import Vue from "vue";

// initial state
const state = {
  loading: false,
  error: null,
  authenticated: false,
  user: null,
  token: null,
  rememberMe: false
};

// getters
const getters = {
  isAdmin: state => {
    return state.user && state.user.roles && state.user.roles.includes("Admin");
  },
  rolesList: state => {
    return state.user && state.user.roles && state.user.roles.join(", ");
  }

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
    commit("LOGIN");
    Vue.axios
      .post("/api/auth/login", credentials)
      .then(result => {
        commit("LOGIN_SUCCESS", result.data);

        // TODO: Token? or in action/mutation?
        localStorage.setItem("token", state.token);
      })
      .catch(error => {
        commit("LOGIN_FAILED", error);
      });
  },
  logout({ commit }) {
    commit("LOGOUT");

    // TODO: Token? or in action/mutation?
    localStorage.removeItem("token");
  }
};

// mutations
const mutations = {
  LOGIN(state) {
    state.loading = true;
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
  LOGOUT(state) {
    state.loading = false;
    state.error = null;
    state.authenticated = false;
    state.user = null;
    state.token = null;
    state.rememberMe = false;
  }
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
};
