<template>
  <v-app v-if="authenticated">
    <v-app-bar color="primary" dark clipped-left app>
      <v-app-bar-nav-icon @click.stop="drawer = !drawer" />
      <v-toolbar-title class="pl-2">Resume Job Matcher</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn v-if="isAdmin" text to="admin" dark>Admin</v-btn>
      <v-btn text @click="logout" dark>Logout</v-btn>
    </v-app-bar>
    <v-navigation-drawer v-model="drawer" clipped app>
      <v-list dense>
        <v-list-item link to="/">
          <v-list-item-action>
            <v-icon>mdi-view-dashboard</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Dashboard</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-list-item link to="resumes">
          <v-list-item-action>
            <v-icon>mdi-file-account</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Resumes</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>
    <v-content>
      <v-container fluid>
        <v-row>
          <v-col>
            <h1>Hi, {{ user.firstName }}</h1>
          </v-col>
        </v-row>
      </v-container>
    </v-content>
    <v-footer color="white" app>
      <v-spacer></v-spacer>
      <span>&copy; <a href="https://steffbeckers.eu" style="text-decoration: none">Steff</a></span>
    </v-footer>
  </v-app>
</template>

<script>
import { mapGetters, mapState } from 'vuex';

export default {
  name: 'Dashboard',
  data: () => ({
    drawer: false,
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
    logout() {
      this.$store.dispatch('auth/logout');
    },
  },
};
</script>
