<template>
  <div></div>
</template>

<script>
export default {
  name: 'ResumeDetail',
  mounted() {
    // Load resume by ID
    this.$store.dispatch('resumes/getById', this.$route.params.id);
  },
};
</script>
