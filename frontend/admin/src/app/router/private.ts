import type { RouteRecordRaw } from 'vue-router';
import AdminLayout from '@/app/layouts/AdminLayout.vue'
export const privateRoutes: RouteRecordRaw[] = [
  {
    path: '/',
    component: AdminLayout,
    meta: { requiresAuth: true },
    children: [
      {
        path: '',
        name: 'Dashboard',
        component: () => import('@/app/views/DashboardView.vue'),
      },
    ],      
  },
]
