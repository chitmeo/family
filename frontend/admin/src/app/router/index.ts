import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'

import { publicRoutes } from './public'
import { privateRoutes } from './private'
import { useAuthStore } from '../stores/auth'

const notFoundRoute: RouteRecordRaw = {
  path: '/:pathMatch(.*)*',
  name: 'NotFound',
  component: () => import('@/app/views/NotFoundView.vue'),
}

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    ...publicRoutes,
    ...privateRoutes,
    notFoundRoute, // always last
  ],
})

let privateRoutesAdded = false

router.beforeEach((to, _from, next) => {

  const authStore = useAuthStore()

  if ((to.meta.requiresAuth as boolean) && !authStore.isAuthenticated) {
    return next('/login')
  }

  // If user is logged in and tries to access login page → redirect dashboard
  if (to.path === '/login' && authStore.isAuthenticated) {
    return next('/')
  }

  // if (authStore.isAuthenticated && !privateRoutesAdded) {
  //   privateRoutes.forEach((route) => {
  //     router.addRoute(route)
  //   })
  //   privateRoutesAdded = true

  //   //add NotFound last
  //   router.addRoute({
  //     path: '/:pathMatch(.*)*',
  //     name: 'NotFound',
  //     component: () => import('@/app/views/NotFoundView.vue')
  //   })
  //   // Refresh navigation
  //   return next({ ...to, replace: true })
  // }

  // if ((to.meta.requiresAuth as boolean) && !authStore.isAuthenticated) {
  //   return next('/login')
  // }

  // if (to.path === '/login' && authStore.isAuthenticated) {
  //   return next('/')
  // }

  next()
})


export default router
