import Vue from 'vue';

// initial state
const state = {
  loading: false,
  error: null,
  skills: null,
  resumes: null,
};

// getters
const getters = {
  resumesByDateCreatedAsc: (state) => {
    return (
      state.resumes &&
      state.resumes.sort(function(a, b) {
        return new Date(b.createdOn) - new Date(a.createdOn);
      })
    );
  },
  resumeById: (state) => (id) => {
    return state.resumes && state.resumes.find((r) => r.id === id);
  },
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
  getAllResumes({ commit }) {
    commit('GET_RESUMES');

    Vue.axios
      .get('/resumes')
      .then((result) => {
        commit('GET_RESUMES_SUCCESS', result.data);
      })
      .catch((error) => {
        commit('GET_RESUMES_FAILED', error);
      });
  },
  getResumeById({ commit }, id) {
    commit('GET_RESUME_BY_ID');

    Vue.axios
      .get('/resumes/' + id)
      .then((result) => {
        commit('GET_RESUME_BY_ID_SUCCESS', result.data);
      })
      .catch((error) => {
        commit('GET_RESUME_BY_ID_FAILED', error);
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
  GET_RESUMES(state) {
    state.loading = true;
    state.error = null;
  },
  GET_RESUMES_SUCCESS(state, resumes) {
    state.loading = false;
    state.resumes = resumes;
  },
  GET_RESUMES_FAILED(state, error) {
    state.loading = false;
    state.error = error;
  },
  GET_RESUME_BY_ID(state) {
    state.loading = true;
    state.error = null;
  },
  GET_RESUME_BY_ID_SUCCESS(state, resume) {
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
  GET_RESUME_BY_ID_FAILED(state, error) {
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
