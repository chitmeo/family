import type { RouteRecordRaw } from 'vue-router';

export const publicRoutes: RouteRecordRaw[] = [
  {
    path: '/login',
    name: 'Login',
    component: () => import('@/app/views/LoginView.vue'),
    meta: { requiresAuth: false },
  }
]