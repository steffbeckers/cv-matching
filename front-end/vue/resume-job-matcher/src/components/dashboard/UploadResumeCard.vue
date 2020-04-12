<template>
  <v-form v-model="uploadFormValid" ref="uploadForm" lazy-validation>
    <v-card>
      <v-card-title class="pb-0" primary-title>
        <div>
          <h3 class="headline mb-0">Upload your own resume</h3>
        </div>
      </v-card-title>
      <v-card-text>
        <v-file-input
          v-model="resumeToUpload"
          label="Select your resume"
          prepend-icon="mdi-file-account"
          :show-size="1000"
          :rules="resumeToUploadRules"
          required
        ></v-file-input>
      </v-card-text>
      <v-card-actions>
        <v-btn color="primary" class="elevation-0" block @click="upload" :disabled="!uploadFormValid">
          Upload
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-form>
</template>

<script>
import { mapState } from 'vuex';

export default {
  name: 'UploadResumeCard',
  data: () => ({
    uploadFormValid: false,
    resumeToUpload: null,
    resumeToUploadRules: [(v) => !!v || 'Resume is required'],
  }),
  computed: {
    ...mapState({
      uploadedResume: (state) => state.uploadedResume,
    }),
  },
  methods: {
    upload() {
      if (this.$refs.uploadForm.validate()) {
        this.$store.dispatch('documents/upload', {
          typeName: 'uploaded-resume',
          file: this.resumeToUpload,
          fileLastModified: this.resumeToUpload.lastModifiedDate,
        });
      }
    },
  },
};
</script>
