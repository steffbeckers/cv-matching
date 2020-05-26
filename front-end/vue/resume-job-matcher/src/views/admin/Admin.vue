<template>
  <div>
    <v-toolbar flat>
      <v-toolbar-title>
        <v-icon class="mr-2">mdi-account-circle</v-icon>
        <span>Administration</span>
      </v-toolbar-title>
    </v-toolbar>
    <v-container v-if="authenticated" fluid>
      <v-row>
        <v-col cols="2">
          <v-list nav>
            <v-list-item-group color="primary">
              <v-list-item to="/admin/skills">
                <v-list-item-content>
                  <v-list-item-title>Skills</v-list-item-title>
                </v-list-item-content>
              </v-list-item>
              <v-list-item to="/admin/resumes">
                <v-list-item-content>
                  <v-list-item-title>Resumes</v-list-item-title>
                </v-list-item-content>
              </v-list-item>
              <v-list-item to="/admin/system">
                <v-list-item-content>
                  <v-list-item-title>System</v-list-item-title>
                </v-list-item-content>
              </v-list-item>
            </v-list-item-group>
          </v-list>
        </v-col>
        <v-col cols="10">
          <router-view></router-view>
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex';

export default {
  name: 'Admin',
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
