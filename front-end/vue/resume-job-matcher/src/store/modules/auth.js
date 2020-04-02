import Vue from "vue";

// initial state
const state = {
  loading: false,
  error: null,
  authenticated: false,
  user: null,
  token: null
};

// getters
const getters = {
  authUser: (state, getters) => {
    return getters.user;
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
  login({ commit }, credentials) {
    console.log(Vue.prototype.$axios);

    Vue.axios
      .post("/api/auth/login", credentials)
      .then(authenticated => {
        commit("LOGIN_SUCCESS", authenticated);
      })
      .catch(error => {
        commit("LOGIN_FAILED", error);
      });
  }
};

// mutations
const mutations = {
  LOGIN_SUCCESS(state, authenticated) {
    state.authenticated = true;
    state.user = authenticated.user;
  },
  LOGIN_FAILED(state, error) {
    state.loading = false;
    state.error = error;
  }
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
};
