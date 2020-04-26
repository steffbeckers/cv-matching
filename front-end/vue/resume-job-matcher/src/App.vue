<template>
  <v-app>
    <v-app-bar v-if="$store.state.ui.topNav && authenticated" color="primary" dark clipped-left app>
      <v-app-bar-nav-icon @click.stop="toggleLeftDrawer()" />
      <v-toolbar-title class="pa-2">Resume Job Matcher</v-toolbar-title>
      <v-btn class="mr-1" to="/" icon>
        <v-icon>mdi-view-dashboard</v-icon>
      </v-btn>
      <v-btn class="mr-1" to="/resumes" icon>
        <v-icon>mdi-file-account</v-icon>
      </v-btn>
      <v-btn to="/jobs" icon>
        <v-icon>mdi-briefcase</v-icon>
      </v-btn>
      <v-spacer></v-spacer>
      <v-btn v-if="isAdmin" text to="admin" dark>Admin</v-btn>
      <v-btn text @click="logout" dark>Logout</v-btn>
      <v-avatar class="ml-2" color="white" size="36">
        <span class="primary--text">{{ user.firstName.substring(0, 1) }}{{ user.lastName.substring(0, 1) }}</span>
      </v-avatar>
    </v-app-bar>
    <v-navigation-drawer v-if="$store.state.ui.drawerLeft && authenticated" v-model="$store.state.ui.drawerLeft" clipped app>
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
      <router-view />
    </v-content>
    <v-footer v-if="$store.state.ui.footer && authenticated" color="white" app>
      <v-spacer></v-spacer>
      <span>&copy; <a href="https://steffbeckers.eu" style="text-decoration: none">Steff</a></span>
    </v-footer>
  </v-app>
</template>

<style lang="scss">
main.v-content {
  background-color: #f5f5f5;
}
</style>

<script>
import { mapGetters, mapState } from 'vuex';

export default {
  name: 'App',
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
  created() {
    this.$store.dispatch('auth/getToken');
  },
  methods: {
    toggleLeftDrawer() {
      this.$store.dispatch('ui/setDrawerLeft', !this.$store.state.ui.drawerLeft);
    },
    logout() {
      this.$store.dispatch('auth/logout');
    },
  },
};
</script>
