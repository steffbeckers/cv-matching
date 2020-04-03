<template>
  <v-app>
    <v-content>
      <v-container fluid>
        <v-row>
          <v-col>
            <v-form v-model="loginFormValid" ref="loginForm" lazy-validation>
              <v-card>
                <v-card-title primary-title>
                  <div>
                    <h3 class="headline mb-0">Login</h3>
                    <div></div>
                  </div>
                </v-card-title>
                <v-card-text>
                  <v-text-field
                    label="Email or username"
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
                  <v-checkbox label="Remember me?" v-model="rememberMe"></v-checkbox>
                </v-card-text>
                <v-card-actions>
                  <v-btn block text @click="login" :disabled="!loginFormValid">
                    Login
                  </v-btn>
                </v-card-actions>
              </v-card>
            </v-form>
          </v-col>
        </v-row>
      </v-container>
    </v-content>
  </v-app>
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
    rememberMe: false,
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
    clear() {
      this.$refs.form.reset();
    },
  },
};
</script>
