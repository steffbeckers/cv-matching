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
  // Admin
  {
    path: '/admin',
    name: 'Admin',
    component: () => import(/* webpackChunkName: "admin" */ '@/views/admin/Admin.vue'),
    children: [
      {
        path: 'skills',
        name: 'AdminSkills',
        component: () => import(/* webpackChunkName: "admin" */ '@/views/admin/skills/Skills.vue'),
      },
      {
        path: 'resumes',
        name: 'AdminResumes',
        component: () => import(/* webpackChunkName: "admin" */ '@/views/admin/resumes/Resumes.vue'),
      },
      {
        path: 'resumes/:id',
        name: 'AdminResumeDetail',
        component: () => import(/* webpackChunkName: "admin" */ '@/views/admin/resumes/ResumeDetail.vue'),
      },
    ],
  },
  // Resumes
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
  // Dashboard
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
