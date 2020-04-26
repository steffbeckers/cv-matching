<template>
  <v-form class="mt-4 ml-2 mr-2" v-model="uploadFormValid" ref="uploadForm" lazy-validation>
    <div class="d-flex flex-row">
      <v-file-input
        class="file-chooser mr-4"
        v-model="resumeToUpload"
        label="Choose file"
        append-icon="mdi-file-account"
        :show-size="1000"
        :rules="resumeToUploadRules"
        required
      ></v-file-input>
      <v-btn text color="primary" @click="upload" :disabled="!uploadFormValid">
        Upload
      </v-btn>
    </div>
  </v-form>
</template>

<style lang="scss" scoped>
.file-chooser {
  min-width: 180px;
}
</style>

<script>
export default {
  name: 'Resumes',
  data: () => ({
    uploadFormValid: false,
    resumeToUpload: null,
    resumeToUploadRules: [(v) => !!v || 'Resume is required'],
  }),
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
