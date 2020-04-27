import Vue from 'vue';
import VueRouter from 'vue-router';

Vue.use(VueRouter);

const routes = [
  // Auth
  {
    path: '/auth/login',
    name: 'Login',
    component: () => import(/* webpackChunkName: "auth-login" */ '@/views/auth/Login.vue'),
  },
  {
    path: '/resumes/:id',
    name: 'ResumeDetail',
    component: () => import(/* webpackChunkName: "resumes" */ '@/views/resumes/ResumeDetail.vue'),
  },
  {
    path: '/resumes',
    name: 'Resumes',
    component: () => import(/* webpackChunkName: "resumes" */ '@/views/resumes/Resumes.vue'),
  },
  {
    path: '/',
    name: 'Dashboard',
    component: () => import(/* webpackChunkName: "dashboard" */ '@/views/Dashboard.vue'),
  },
];

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
});

export default router;
