import type { RouteRecordRaw } from 'vue-router'

const accountingRoutes: RouteRecordRaw[] = [
  {
    path: '/accounting',
    children: [
      {
        path: 'chartofaccount',
        name: 'ChartOfAccount',
        component: () => import('@/modules/accounting/ui/views/ChartOfAccountView.vue'),
        meta: { title: 'Chart Of Account', requiresAuth: true },
      }
    ],
  },
]

export default accountingRoutes
