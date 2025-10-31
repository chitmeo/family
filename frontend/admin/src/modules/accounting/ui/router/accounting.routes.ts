import type { RouteRecordRaw } from 'vue-router'

const accountingRoutes: RouteRecordRaw[] = [
  {
    path: '/accounting',
    children: [
      {
        path: 'chartofaccounts',        
        name: 'ChartOfAccounts',
        component: () => import('@/modules/accounting/ui/views/ChartOfAccountsView.vue'),
        meta: { title: 'Chart Of Account', requiresAuth: true },
      },
      {
        path: 'accounts',
        name: 'Accounts',
        component: () => import('@/modules/accounting/ui/views/AccountsView.vue'),
        meta: { title: 'Accounts', requiresAuth: true },
      }
      
    ],
  },
]

export default accountingRoutes
