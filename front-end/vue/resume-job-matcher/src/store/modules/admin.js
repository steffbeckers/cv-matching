import Vue from 'vue';

// initial state
const state = {
  loading: false,
  error: null,
  skills: null,
};

// actions
const actions = {
  getAllSkills({ commit }) {
    commit('GET_SKILLS');

    Vue.axios
      .get('/skills')
      .then((result) => {
        commit('GET_SKILLS_SUCCESS', result.data);
      })
      .catch((error) => {
        commit('GET_SKILLS_FAILED', error);
      });
  },
  getSkillById({ commit }, id) {
    commit('GET_SKILL_BY_ID');

    Vue.axios
      .get('/skills/' + id)
      .then((result) => {
        commit('GET_SKILL_BY_ID_SUCCESS', result.data);
      })
      .catch((error) => {
        commit('GET_SKILL_BY_ID_FAILED', error);
      });
  },
};

// mutations
const mutations = {
  GET_SKILLS(state) {
    state.loading = true;
    state.error = null;
  },
  GET_SKILLS_SUCCESS(state, skills) {
    state.loading = false;
    state.skills = skills;
  },
  GET_SKILLS_FAILED(state, error) {
    state.loading = false;
    state.error = error;
  },
  GET_SKILL_BY_ID(state) {
    state.loading = true;
    state.error = null;
  },
  GET_SKILL_BY_ID_SUCCESS(state, skill) {
    state.loading = false;

    if (state.resumes) {
      var skillIndex = state.skills.findIndex((r) => r.id === skill.id);
      if (skillIndex !== -1) {
        state.skills[skillIndex] = skill;
      }
    } else {
      state.skills = [skill];
    }
  },
  GET_SKILL_BY_ID_FAILED(state, error) {
    state.loading = false;
    state.error = error;
  },
};

export default {
  namespaced: true,
  state,
  // getters,
  actions,
  mutations,
};
