<template>
  <v-container v-if="authenticated" fluid>
    <v-row>
      <v-col>
        <h1>Hi, {{ user.firstName }}</h1>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { mapGetters, mapState } from 'vuex';

export default {
  name: 'Dashboard',
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
};
</script>
