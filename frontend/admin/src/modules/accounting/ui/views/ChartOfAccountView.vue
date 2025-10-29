<script setup lang="ts">
import { ref } from 'vue'

import ChartOfAccountInfoTab from '@/modules/accounting/ui/components/ChartOfAccountInfoTab.vue'
import ChartOfAccountAccountsTab from '@/modules/accounting/ui/components/ChartOfAccountAccountsTab.vue'
import ChartOfAccountFindTab from '@/modules/accounting/ui/components/ChartOfAccountFindTab.vue'

// Current active tab
const activeTab = ref<'info' | 'accounts' | 'find'>('info')

// Tab click handler
function selectTab(tab: 'info' | 'accounts' | 'find') {
  activeTab.value = tab
}
</script>

<template>
  <section class="section">
    <div class="container">
      <h1 class="title">Chart Of Account</h1>

      <!-- Bulma tabs -->
      <div class="tabs is-boxed">
        <ul>
          <li :class="{ 'is-active': activeTab === 'info' }" @click="selectTab('info')">
            <a>Info</a>
          </li>
          <li :class="{ 'is-active': activeTab === 'accounts' }" @click="selectTab('accounts')">
            <a>Accounts</a>
          </li>
          <li :class="{ 'is-active': activeTab === 'find' }" @click="selectTab('find')">
            <a>Find</a>
          </li>
        </ul>
      </div>

      <!-- Tab content -->
      <div class="tab-content">
        <ChartOfAccountInfoTab v-if="activeTab === 'info'" />
        <ChartOfAccountAccountsTab v-if="activeTab === 'accounts'" />
        <ChartOfAccountFindTab v-if="activeTab === 'find'" />
      </div>
    </div>
  </section>
</template>

<style scoped>
.tab-content {
  margin-top: 1rem;
}
</style>
