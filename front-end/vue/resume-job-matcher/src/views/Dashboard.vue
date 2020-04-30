<template>
  <v-container v-if="authenticated" fluid>
    <v-row>
      <v-col>
        <h1>Hi, {{ user.firstName }}</h1>
        <p class="mb-0">This is your personal dashboard.</p>
      </v-col>
    </v-row>
    <v-row>
      <v-col v-if="resumes && resumes.length === 0" cols="12" sm="6" md="4">
        <UploadResumeCard />
      </v-col>
      <v-col cols="12" sm="6" md="8">
        <h2>My latest resumes</h2>
        <v-row>
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
            </v-card>
          </v-col>
        </v-row>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { mapGetters, mapState } from 'vuex';

// Components
import UploadResumeCard from '../components/resumes/UploadResumeCard';

export default {
  name: 'Dashboard',
  data: () => ({}),
  computed: {
    ...mapGetters('resumes', {
      resumes: 'getMyLatest',
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
  },
};
</script>
