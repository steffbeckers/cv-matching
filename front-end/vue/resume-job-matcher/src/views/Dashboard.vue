<template>
  <v-container v-if="authenticated" fluid>
    <v-row>
      <v-col>
        <h1>Hi, {{ user.firstName }}</h1>
        <p>This is your personal dashboard.</p>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12" sm="8" md="4">
        <UploadResumeCard />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { mapGetters, mapState } from 'vuex';
import UploadResumeCard from '../components/dashboard/UploadResumeCard';

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
  components: {
    UploadResumeCard,
  },
};
</script>
