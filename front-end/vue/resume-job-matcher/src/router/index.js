import Vue from 'vue';
import VueRouter from 'vue-router';

Vue.use(VueRouter);

const routes = [
  {
    path: '/',
    name: 'Dashboard',
    component: () => import(/* webpackChunkName: "dashboard" */ '@/views/Dashboard.vue'),
  },
  {
    path: '/auth/login',
    name: 'Login',
    component: () => import(/* webpackChunkName: "login" */ '@/views/auth/Login.vue'),
  },
];

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
});

export default router;
