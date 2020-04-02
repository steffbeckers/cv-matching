<template>
  <v-container fluid>
    <v-row>
      <v-col>
        <h1>Home</h1>
        <v-form v-model="valid" ref="form" lazy-validation>
          <v-card>
            <v-card-title primary-title>
              <div>
                <h3 class="headline mb-0">Login</h3>
                <div></div>
              </div>
            </v-card-title>
            <v-card-text>
              <v-text-field
                label="Username or email"
                v-model="usernameOrEmail"
                :rules="usernameOrEmailRules"
                required
              ></v-text-field>
              <v-text-field label="Password" v-model="password" :rules="passwordRules" type="password"></v-text-field>
              <v-checkbox label="Remember me?" v-model="rememberMe"></v-checkbox>
            </v-card-text>
            <v-card-actions>
              <v-btn text @click="submit" :disabled="!valid">
                Login
              </v-btn>
              <v-btn text @click="clear">Clear</v-btn>
            </v-card-actions>
          </v-card>
        </v-form>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  data: () => ({
    valid: true,
    usernameOrEmail: "steff",
    usernameOrEmailRules: [v => !!v || "Username or email is required"],
    password: "Steff12345!",
    passwordRules: [v => !!v || "Password is required"],
    rememberMe: true
  }),

  methods: {
    submit() {
      if (this.$refs.form.validate()) {
        this.$store.dispatch("auth/login", {
          usernameOrEmail: this.usernameOrEmail,
          rememberMe: this.rememberMe
        });
      }
    },
    clear() {
      this.$refs.form.reset();
    }
  }
};
</script>
