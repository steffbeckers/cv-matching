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
  addSkillAliasToSkill({ commit }, payload) {
    commit('ADD_SKILL_ALIAS_TO_SKILL');

    let alias = { name: payload.alias, skillId: payload.skill.id };

    Vue.axios
      .post('/skillaliases', alias)
      .then((result) => {
        commit('ADD_SKILL_ALIAS_TO_SKILL_SUCCESS', result.data);
      })
      .catch((error) => {
        commit('ADD_SKILL_ALIAS_TO_SKILL_FAILED', error);
      });
  },
  removeSkillAliasFromSkill({ commit }, payload) {
    commit('REMOVE_SKILL_ALIAS_FROM_SKILL');

    Vue.axios
      .delete('/skillaliases/' + payload.alias.id)
      .then((result) => {
        commit('REMOVE_SKILL_ALIAS_FROM_SKILL_SUCCESS', result.data);
      })
      .catch((error) => {
        commit('REMOVE_SKILL_ALIAS_FROM_SKILL_FAILED', error);
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

    if (state.skills) {
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
  ADD_SKILL_ALIAS_TO_SKILL(state) {
    state.loading = true;
    state.error = null;
  },
  ADD_SKILL_ALIAS_TO_SKILL_SUCCESS(state, skillAlias) {
    state.loading = false;

    var skillIndex = state.skills.findIndex((r) => r.id === skillAlias.skillId);
    if (skillIndex !== -1) {
      let skill = { ...state.skills[skillIndex] };

      skill.aliases.push(skillAlias);

      state.skills[skillIndex] = skill;
    }
  },
  ADD_SKILL_ALIAS_TO_SKILL_FAILED(state, error) {
    state.loading = false;
    state.error = error;
  },
  REMOVE_SKILL_ALIAS_FROM_SKILL(state) {
    state.loading = true;
    state.error = null;
  },
  REMOVE_SKILL_ALIAS_FROM_SKILL_SUCCESS(state, skillAlias) {
    state.loading = false;

    var skillIndex = state.skills.findIndex((r) => r.id === skillAlias.skillId);
    if (skillIndex !== -1) {
      let skill = { ...state.skills[skillIndex] };

      let skillAliasIndex = skill.aliases.findIndex((a) => a.id === skillAlias.id);
      if (skillAliasIndex !== -1) {
        skill.aliases.splice(skillAliasIndex, 1);
      }

      state.skills[skillIndex] = skill;
    }
  },
  REMOVE_SKILL_ALIAS_FROM_SKILL_FAILED(state, error) {
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
