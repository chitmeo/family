<script setup lang="ts">
import { ref, watch } from 'vue'
import { useRoute } from 'vue-router'

const activeDropdown = ref<string | null>(null)

const menus = [
    {
        name: 'accounting',
        label: 'Accounting',
        items: [
            { text: 'Chart of Accounts', to: '/accounting/chartofaccounts' },
            { text: 'Accounts', to: '/accounting/accounts' },
            { text: 'Books', to: '/accounting/journalBooks' },
            { text: 'Journal Entries', to: '/accounting/journalEntries' },
        ],
    },
    {
        name: 'reports',
        label: 'Reports',
        items: [
            { text: 'Sales Report', to: '/reports/sales' },
            { text: 'Finance Report', to: '/reports/finance' },
        ],
    },
]

const toggleDropdown = (name: string) => {
    activeDropdown.value = activeDropdown.value === name ? null : name
}

const closeDropdown = () => {
    activeDropdown.value = null
}

const route = useRoute()
watch(
    () => route.fullPath,
    () => closeDropdown()
)

</script>

<template>
    <div v-for="menu in menus" :key="menu.name" class="navbar-item has-dropdown"
        :class="{ 'is-active': activeDropdown === menu.name }">
        <a class="navbar-link" @click="toggleDropdown(menu.name)">
            {{ menu.label }}
        </a>
        <div class="navbar-dropdown">
            <router-link v-for="item in menu.items" :key="item.to" :to="item.to" class="navbar-item"
                @click="closeDropdown">
                {{ item.text }}
            </router-link>
        </div>
    </div>
</template>