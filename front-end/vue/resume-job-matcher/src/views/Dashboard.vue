<template>
  <v-container v-if="authenticated" fluid>
    <v-row>
      <v-col>
        <h1>Hi, {{ user.firstName }}</h1>
        <p class="mb-0">This is your personal dashboard.</p>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12" sm="6" md="4">
        <UploadResumeCard />
      </v-col>
      <v-col cols="12" sm="6" md="8">
        <MyLatestResumesCard />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { mapGetters, mapState } from 'vuex';

// Components
import UploadResumeCard from '../components/dashboard/UploadResumeCard';
import MyLatestResumesCard from '../components/dashboard/MyLatestResumesCard';

export default {
  name: 'Dashboard',
  data: () => ({}),
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
  mounted() {
    // Load resumes
    this.$store.dispatch('resumes/getAll');
  },
  components: {
    UploadResumeCard,
    MyLatestResumesCard,
  },
};
</script>
