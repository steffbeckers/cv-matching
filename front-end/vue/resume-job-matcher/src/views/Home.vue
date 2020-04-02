<template>
  <v-container fluid>
    <v-row>
      <v-col>
        <h1>Home</h1>
      </v-col>
    </v-row>
    <v-row v-if="authenticated">
      <v-col cols="4">
        <v-card>
          <v-card-title primary-title>
            <div>
              <h3 class="headline mb-0">Hi, {{ user.firstName }}</h3>
              <div>{{ rolesList }}</div>
            </div>
          </v-card-title>
          <v-card-actions>
            <v-btn text @click="logout">
              Logout
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
    <v-row v-else>
      <v-col cols="4">
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
                label="Email or username"
                v-model="emailOrUsername"
                :rules="emailOrUsernameRules"
                required
              ></v-text-field>
              <v-text-field label="Password" v-model="password" :rules="passwordRules" type="password"></v-text-field>
              <v-checkbox label="Remember me?" v-model="rememberMe"></v-checkbox>
            </v-card-text>
            <v-card-actions>
              <v-btn text @click="login" :disabled="!valid">
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
import { mapGetters, mapState } from "vuex";

export default {
  name: "Home",
  data: () => ({
    valid: true,
    emailOrUsername: "",
    emailOrUsernameRules: [v => !!v || "Email or username is required"],
    password: "",
    passwordRules: [v => !!v || "Password is required"],
    rememberMe: true
  }),
  computed: {
    ...mapState("auth", {
      authenticated: state => state.authenticated,
      user: state => state.user
    }),
    ...mapGetters("auth", {
      isAdmin: "isAdmin",
      rolesList: "rolesList"
    })
  },
  methods: {
    login() {
      if (this.$refs.form.validate()) {
        this.$store.dispatch("auth/login", {
          emailOrUsername: this.emailOrUsername,
          password: this.password,
          rememberMe: this.rememberMe
        });
      }
    },
    logout() {
      this.$store.dispatch("auth/logout");
    },
    clear() {
      this.$refs.form.reset();
    }
  }
};
</script>
