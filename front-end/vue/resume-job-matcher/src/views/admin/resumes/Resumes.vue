<template>
  <div v-if="resumes">
    <v-data-table :headers="headers" :items="resumes" :search="search" item-key="id" class="elevation-2" @click:row="navigateToResume">
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>
            <v-icon class="mr-2">mdi-file-account</v-icon>
            <span>Resumes</span>
          </v-toolbar-title>
          <v-spacer></v-spacer>
          <v-text-field v-model="search" label="Search" single-line clearable hide-details></v-text-field>
        </v-toolbar>
      </template>
    </v-data-table>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex';

export default {
  name: 'AdminResumes',
  data: () => ({
    search: null,
    headers: [
      {
        text: 'Name',
        align: 'start',
        sortable: true,
        value: 'displayName',
      },
      { text: 'Description', value: 'description' },
    ],
  }),
  computed: {
    ...mapState('admin', {
      //resumes: (state) => state.resumes,
    }),
    ...mapGetters('admin', {
      resumes: 'resumesByDateCreatedAsc',
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
    this.$store.dispatch('admin/getAllResumes');
  },
  methods: {
    navigateToResume(resume) {
      this.$router.push({ name: 'AdminResumeDetail', params: { id: resume.id } });
    },
  },
};
</script>
