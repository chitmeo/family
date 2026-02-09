import type { RouteRecordRaw } from "vue-router";

const authenticationRoutes: RouteRecordRaw[] = [{
    path: '/authentication',
    children: [
        { path: 'roles', name: 'Roles', component: () => import('@/modules/authentication/ui/views/RolesView.vue'), meta: { requiresAuth: true }, },

    ]
}];

export default authenticationRoutes;