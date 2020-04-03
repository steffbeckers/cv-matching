<template>
  <router-view />
</template>

<script>
export default {
  name: 'App',
  created() {
    // Retrieve user on app load
    this.$store.dispatch('auth/me');
  },
};
</script>
