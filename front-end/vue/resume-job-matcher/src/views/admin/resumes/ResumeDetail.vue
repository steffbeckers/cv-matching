<template>
  <div v-if="resume" class="elevation-2">
    <v-toolbar flat>
      <v-toolbar-title>
        <v-icon class="mr-2">mdi-file-account</v-icon>
        <span>{{ resume.displayName }}</span>
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn icon>
        <v-icon>mdi-dots-vertical</v-icon>
      </v-btn>
    </v-toolbar>
    <v-card class="elevation-0">
      <v-card-text class="pa-0">
        <div class="pl-4 pr-4">{{ resume.user.firstName }} {{ resume.user.lastName }}</div>
        <v-data-table
          dense
          v-if="resume.documents"
          :headers="headersDocuments"
          :items="resume.documents"
          :search="searchDocuments"
          item-key="id"
          class="elevation-0"
          @click:row="navigateToDocument"
        >
          <template v-slot:top>
            <v-toolbar dense flat>
              <v-toolbar-title>
                <span>Documents</span>
              </v-toolbar-title>
              <v-spacer></v-spacer>
              <v-text-field v-model="search" label="Search" single-line clearable hide-details></v-text-field>
            </v-toolbar>
          </template>
        </v-data-table>
      </v-card-text>
    </v-card>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';

export default {
  name: 'AdminResumeDetail',
  data: () => ({
    searchDocuments: null,
    headersDocuments: [
      {
        text: 'Name',
        align: 'start',
        sortable: true,
        value: 'displayName',
      },
      { text: 'Description', value: 'description' },

      { text: 'Last modified on', value: 'modifiedOn' },
    ],
  }),
  computed: {
    ...mapGetters('admin', ['resumeById']),
    resume() {
      return this.resumeById(this.$route.params.id);
    },
  },
  mounted() {
    // Load resume by ID
    this.$store.dispatch('admin/getResumeById', this.$route.params.id);
  },
  methods: {
    navigateToDocument(document) {
      this.$router.push({ name: 'AdminDocumentDetail', params: { id: document.id } });
    },
  },
};
</script>
