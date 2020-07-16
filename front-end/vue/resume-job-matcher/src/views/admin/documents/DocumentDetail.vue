<template>
  <div v-if="document" class="elevation-2">
    <v-toolbar flat>
      <v-toolbar-title>
        <span>{{ document.displayName }}</span>
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn class="mr-1" icon>
        <v-icon>mdi-download</v-icon>
      </v-btn>
      <v-btn icon>
        <v-icon>mdi-dots-vertical</v-icon>
      </v-btn>
    </v-toolbar>
    <v-card class="elevation-0">
      <v-card-text class="pa-0">
        {{ document | json }}
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
    this.$store.dispatch('admin/getDocumentById', this.$route.params.id);
  },
  methods: {
    navigateToDocument(document) {
      this.$router.push({ name: 'AdminDocumentDetail', params: { id: document.id } });
    },
  },
};
</script>
