import Vue from 'vue';

// initial state
const state = {
  loading: false,
  error: null,
  lastUploadedDocument: null,
};

// actions
const actions = {
  upload({ commit, dispatch }, payload) {
    commit('UPLOAD', payload);

    const formData = new FormData();
    formData.append('typeName', payload.typeName);
    formData.append('file', payload.file, payload.file.Name);
    formData.append('fileLastModified', payload.fileLastModified.toISOString());

    Vue.axios
      .post('/documents', formData)
      .then((result) => {
        commit('UPLOAD_SUCCESS', result.data);

        // Load resumes again
        dispatch('resumes/getAll', null, { root: true });
      })
      .catch((error) => {
        commit('UPLOAD_FAILED', error);
      });
  },
};

// mutations
const mutations = {
  UPLOAD(state) {
    state.loading = true;
    state.error = null;
  },
  UPLOAD_SUCCESS(state, document) {
    state.loading = false;
    state.lastUploadedDocument = document;
  },
  UPLOAD_FAILED(state, error) {
    state.loading = false;
    state.error = error;
  },
};

export default {
  namespaced: true,
  state,
  actions,
  mutations,
};
