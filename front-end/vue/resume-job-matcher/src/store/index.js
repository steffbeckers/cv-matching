import Vue from 'vue';
import Vuex from 'vuex';

import ui from './modules/ui';
import auth from './modules/auth';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    loading: false,
    error: null,
    uploadedResume: null,
    resumes: [],
  },
  actions: {
    uploadResume({ commit }, resumeData) {
      commit('RESUME_UPLOAD', resumeData);

      const formData = new FormData();
      formData.append('file', resumeData.file, resumeData.file.Name);
      formData.append('lastModified', resumeData.lastModified.toISOString());

      Vue.axios
        .post('/resumes/upload', formData)
        .then((result) => {
          commit('RESUME_UPLOAD_SUCCESS', result.data);
        })
        .catch((error) => {
          commit('RESUME_UPLOAD_FAILED', error);
        });
    },
  },
  mutations: {
    RESUME_UPLOAD(state) {
      state.loading = true;
      state.error = null;
    },
    RESUME_UPLOAD_SUCCESS(state, resume) {
      state.loading = false;
      state.uploadedResume = resume;
      state.resumes.unshift(state.uploadedResume);
    },
    RESUME_UPLOAD_FAILED(state, error) {
      state.loading = false;
      state.error = error;
    },
  },
  modules: {
    ui,
    auth,
  },
});
