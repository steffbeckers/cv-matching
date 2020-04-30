<template>
  <div v-if="skills">
    <v-data-table
      :headers="headers"
      :items="skills"
      :expanded.sync="expanded"
      :search="search"
      item-key="id"
      :single-expand="true"
      show-expand
      class="elevation-2"
    >
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>Skills</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-text-field v-model="search" label="Search" single-line hide-details></v-text-field>
        </v-toolbar>
      </template>
      <template v-slot:expanded-item="{ headers, item }">
        <td :colspan="headers.length">More info about {{ item.name }}</td>
      </template>
    </v-data-table>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex';

export default {
  name: 'AdminSkills',
  data: () => ({
    expanded: [],
    search: null,
    headers: [
      {
        text: 'Name',
        align: 'start',
        sortable: true,
        value: 'displayName',
      },
      { text: 'Description', value: 'description' },
      { text: '', value: 'data-table-expand' },
    ],
  }),
  computed: {
    ...mapState('admin', {
      skills: (state) => state.skills,
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
    // Load skills
    this.$store.dispatch('admin/getAllSkills');
  },
};
</script>
