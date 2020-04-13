<template>
  <v-card>
    <v-card-title primary-title>
      <div>
        <h3 class="headline">My latest resumes</h3>
      </div>
    </v-card-title>
    <v-card-text>
      <div v-for="resume of resumes" :key="resume.id">
        {{ resume.description }}
      </div>
    </v-card-text>
  </v-card>
</template>

<script>
import { mapGetters, mapState } from 'vuex';

export default {
  name: 'ResumesCard',
  computed: {
    ...mapState({
      uploadedResume: (state) => state.uploadedResume,
    }),
    ...mapGetters('resumes', {
      resumes: 'getMyLatest',
    }),
  },
};
</script>
