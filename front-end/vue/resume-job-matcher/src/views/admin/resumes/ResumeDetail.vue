<template>
  <div v-if="resume">
    <v-toolbar flat>
      <v-toolbar-title>
        <v-icon class="mr-2">mdi-file-account</v-icon>
        <span>{{ resume.displayName }}</span>
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn class="mr-1" icon>
        <v-icon>mdi-magnify</v-icon>
      </v-btn>
      <v-btn class="mr-1" icon>
        <v-icon>mdi-download</v-icon>
      </v-btn>
      <v-btn icon>
        <v-icon>mdi-dots-vertical</v-icon>
      </v-btn>
    </v-toolbar>
    <v-card class="elevation-0">
      <v-card-title primary-title>
        <h3 class="display-2">{{ resume.user.firstName }} {{ resume.user.lastName }}</h3>
      </v-card-title>
      <v-card-text>
        {{ resume.createdOn }}
      </v-card-text>
    </v-card>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';

export default {
  name: 'AdminResumeDetail',
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
};
</script>
