<template>
  <div>
    <v-toolbar flat>
      <v-toolbar-title>
        <v-icon class="mr-2">mdi-file-account</v-icon>
        <span>My resumes</span>
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <UploadResumeForToolbar v-if="resumes && resumes.length > 0" />
      <v-btn class="mr-1" icon>
        <v-icon>mdi-magnify</v-icon>
      </v-btn>
      <v-btn class="mr-1" icon>
        <v-icon>mdi-apps</v-icon>
      </v-btn>
      <v-btn icon>
        <v-icon>mdi-dots-vertical</v-icon>
      </v-btn>
    </v-toolbar>
    <v-container v-if="authenticated" fluid>
      <v-row>
        <v-col v-if="resumes && resumes.length === 0" cols="12" sm="6" md="4">
          <UploadResumeCard />
        </v-col>
        <v-col cols="4" v-for="resume in resumes" :key="resume.id">
          <v-card :to="{ name: 'ResumeDetail', params: { id: resume.id } }">
            <v-card-title primary-title>
              <div>
                <h3 class="headline mb-0">{{ resume.displayName }}</h3>
              </div>
            </v-card-title>
            <v-card-text>
              {{ resume.description }}
            </v-card-text>
            <v-card-actions>
              {{ resume.createdOn }}
            </v-card-actions>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex';

// Components
import UploadResumeCard from '../../components/resumes/UploadResumeCard';
import UploadResumeForToolbar from '../../components/resumes/UploadResumeForToolbar';

export default {
  name: 'Resumes',
  computed: {
    ...mapGetters('resumes', {
      resumes: 'getMy',
    }),
    ...mapState('auth', {
      authenticated: (state) => state.authenticated,
      user: (state) => state.user,
    }),
    ...mapGetters('auth', {
      isAdmin: 'isAdmin',
      rolesList: 'rolesList',
    }),
  },
  mounted() {
    // Load resumes
    this.$store.dispatch('resumes/getAll');
  },
  components: {
    UploadResumeCard,
    UploadResumeForToolbar,
  },
};
</script>
