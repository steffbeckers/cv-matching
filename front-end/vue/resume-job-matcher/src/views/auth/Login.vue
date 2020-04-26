<template>
  <v-container class="fill-height" fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="4">
        <v-form v-model="loginFormValid" ref="loginForm" lazy-validation>
          <v-card class="elevation-12">
            <v-toolbar color="primary" dark flat>
              <v-toolbar-title>Resume Job Matcher</v-toolbar-title>
            </v-toolbar>
            <v-card-title class="pb-0" primary-title>
              <div>
                <h3 class="headline mb-0">Login</h3>
              </div>
            </v-card-title>
            <v-card-text>
              <v-text-field
                label="E-mail or username"
                v-model="emailOrUsername"
                :rules="emailOrUsernameRules"
                required
                autocomplete="username"
              ></v-text-field>
              <v-text-field
                label="Password"
                v-model="password"
                :rules="passwordRules"
                type="password"
                required
                autocomplete="current-password"
              ></v-text-field>
              <v-checkbox class="mt-0" label="Remember me?" v-model="rememberMe" :hide-details="true"></v-checkbox>
            </v-card-text>
            <v-card-actions>
              <v-btn color="primary" class="elevation-0" block @click="login" :disabled="!loginFormValid">
                Login
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-form>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  name: 'Login',
  data: () => ({
    loginFormValid: false,
    emailOrUsername: '',
    emailOrUsernameRules: [(v) => !!v || 'Email or username is required'],
    password: '',
    passwordRules: [(v) => !!v || 'Password is required'],
    rememberMe: true,
  }),
  methods: {
    login() {
      if (this.$refs.loginForm.validate()) {
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
  },
};
</script>
