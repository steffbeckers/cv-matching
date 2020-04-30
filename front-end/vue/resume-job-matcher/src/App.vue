<template>
  <v-app>
    <v-app-bar v-if="$store.state.ui.topNav && authenticated" color="primary" dark clipped-left app>
      <v-app-bar-nav-icon @click.stop="toggleLeftDrawer()" />
      <v-toolbar-title class="px-3">RJM</v-toolbar-title>
      <v-btn text class="mr-1 px-2" to="/">
        <v-icon class="mr-2">mdi-view-dashboard</v-icon>
        <span>Dashboard</span>
      </v-btn>
      <v-btn text class="mr-1 px-2" to="/resumes">
        <v-icon class="mr-2">mdi-file-account</v-icon>
        <span>Resumes</span>
      </v-btn>
      <v-btn text class="px-2" to="/jobs">
        <v-icon class="mr-2">mdi-briefcase</v-icon>
        <span>Jobs</span>
      </v-btn>
      <v-spacer></v-spacer>
      <v-menu offset-y>
        <template v-slot:activator="{ on }">
          <div v-on="on" class="d-inline-flex">
            <div class="align-self-center mr-3">
              <v-avatar color="white" size="36">
                <span class="primary--text pa-2">{{ user.firstName.substring(0, 1) }}{{ user.lastName.substring(0, 1) }}</span>
              </v-avatar>
            </div>
            <div class="flex-column">
              <div class="title">{{ user.firstName }} {{ user.lastName }}</div>
              <div class="body-2">
                {{ user.email }}
              </div>
            </div>
          </div>
        </template>
        <v-list>
          <v-list-item link to="/admin">
            <v-list-item-icon>
              <v-icon>mdi-account-circle</v-icon>
            </v-list-item-icon>
            <v-list-item-title>Admin</v-list-item-title>
          </v-list-item>
          <v-list-item @click="logout">
            <v-list-item-icon>
              <v-icon>mdi-logout</v-icon>
            </v-list-item-icon>
            <v-list-item-title>Logout</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
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
        <v-list-item link to="/resumes">
          <v-list-item-action>
            <v-icon>mdi-file-account</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Resumes</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-list-item link to="/jobs">
          <v-list-item-action>
            <v-icon>mdi-briefcase</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Jobs</v-list-item-title>
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
