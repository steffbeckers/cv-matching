<template>
  <v-app v-if="authenticated">
    <v-app-bar dark clipped-left app>
      <v-app-bar-nav-icon @click.stop="toggleDrawer()" />
      <v-toolbar-title>Resume Job Matcher</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn v-if="isAdmin" text to="admin" dark>Admin</v-btn>
      <v-btn text @click="logout" dark>Logout</v-btn>
    </v-app-bar>
    <v-navigation-drawer v-model="drawer" clipped app>
      <v-list dense>
        <v-list-item link to="/">
          <v-list-item-action>
            <v-icon>mdi-home</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Home</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-list-item link to="about">
          <v-list-item-action>
            <v-icon>mdi-info</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>About</v-list-item-title>
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
    <v-footer app dark>
      <v-spacer></v-spacer>
      <span class="white--text"> &copy; 2020 <a href="https://steffbeckers.eu" class="white--text">Steff</a> </span>
    </v-footer>
  </v-app>
</template>

<script>
import { mapGetters, mapState } from 'vuex';

export default {
  name: 'Home',
  data: () => ({
    valid: true,
    emailOrUsername: '',
    emailOrUsernameRules: [(v) => !!v || 'Email or username is required'],
    password: '',
    passwordRules: [(v) => !!v || 'Password is required'],
    rememberMe: true,
  }),
  computed: {
    ...mapState({
      drawer: (state) => state.drawer,
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
  methods: {
    login() {
      if (this.$refs.form.validate()) {
        this.$store.dispatch('auth/login', {
          emailOrUsername: this.emailOrUsername,
          password: this.password,
          rememberMe: this.rememberMe,
        });
      }
    },
    logout() {
      this.$store.dispatch('auth/logout');
    },
    clear() {
      this.$refs.form.reset();
    },
    toggleDrawer() {
      this.$store.dispatch('setDrawer', !this.drawer);
    },
  },
};
</script>
