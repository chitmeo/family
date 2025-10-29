import type { RouteRecordRaw } from 'vue-router';

export const privateRoutes: RouteRecordRaw[] = [
  {
    path: '/dashboard',
    name: 'Dashboard',
    component: () => import('@/app/views/DashboardView.vue'),
    meta: { requiresAuth: true },
  },
]
