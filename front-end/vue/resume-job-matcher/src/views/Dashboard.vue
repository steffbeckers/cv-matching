<template>
  <v-container v-if="authenticated" fluid>
    <v-row>
      <v-col>
        <h1>Hi, {{ user.firstName }}</h1>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12" sm="8" md="4">
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
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { mapGetters, mapState } from 'vuex';

export default {
  name: 'Dashboard',
  data: () => ({
    uploadFormValid: false,
    resumeToUpload: null,
    resumeToUploadRules: [(v) => !!v || 'Resume is required'],
  }),
  computed: {
    ...mapState('auth', {
      authenticated: (state) => state.authenticated,
      user: (state) => state.user,
    }),
    ...mapGetters('auth', {
      isAdmin: 'isAdmin',
      rolesList: 'rolesList',
    }),
  },
  methods: {
    upload() {
      if (this.$refs.uploadForm.validate()) {
        console.log(this.resumeToUpload);
      }
    },
  },
};
</script>
