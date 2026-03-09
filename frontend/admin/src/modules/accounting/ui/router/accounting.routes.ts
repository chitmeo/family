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
      },
      {
        path: 'journalBooks',
        name: 'Journal Books',
        component: () => import('@/modules/accounting/ui/views/JournalBooksView.vue'),
        meta: { title: 'Journal Books', requiresAuth: true },
      },
      {
        path: 'journalTemplates',
        name: 'Journal Templates',
        component: () => import('@/modules/accounting/ui/views/JournalTemplatesView.vue'),
        meta: { title: 'Journals', requiresAuth: true }
      },
      {
        path: 'journal-entries',
        name: 'Journal entries',
        component: () => import('@/modules/accounting/ui/views/JournalEntriesView.vue'),
        meta: { title: 'Journal Entries', requiresAuth: true }
      }
    ],
  },
]

export default accountingRoutes
