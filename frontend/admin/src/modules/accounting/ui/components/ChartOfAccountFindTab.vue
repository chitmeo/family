<script setup lang="ts">
import { ref } from 'vue'
import { useChartOfAccount } from '@/modules/accounting/composables';
import type { ChartOfAccount } from '@/modules/accounting/types/ChartOfAccount';

const { searchCOA, setChartOfAccount, listChartOfAccounts, activeTab } = useChartOfAccount();

const searchTerm = ref('')
const isNew = ref(false)

// Form model
const current = ref<ChartOfAccount>({
  id: '',
  code: '',
  name: '',
  isActive: true,
})

function handleClick(item: ChartOfAccount) {
    setChartOfAccount(item)
    activeTab.value = 'info'
}

function handleSearch() {
    searchCOA(searchTerm.value, true);
}

</script>

<template>
    <section class="section">
        <div class="container">
            <div class="field has-addons">
                <div class="control is-expanded">
                    <input v-model="searchTerm" class="input" type="text" placeholder="Enter search term..."
                        @keyup.enter="handleSearch" />
                </div>
                <div class="control">
                    <button class="button is-info" @click="handleSearch">
                        Search
                    </button>
                </div>
            </div>

            <!-- Table -->
            <table class="table is-striped is-fullwidth" v-if="listChartOfAccounts.length">
                <thead>
                    <tr>
                        <th>Code</th>
                        <th>Name</th>
                        <th>Active</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in listChartOfAccounts" :key="item.id">
                        <td>
                            <a href="#" @click.prevent="handleClick(item)">
                                {{ item.code }}
                            </a>
                        </td>
                        <td>{{ item.name }}</td>
                        <td>
                            <span class="tag" :class="item.isActive ? 'is-success' : 'is-danger'">
                                {{ item.isActive ? 'Active' : 'Inactive' }}
                            </span>
                        </td>
                    </tr>
                </tbody>
            </table>
            <p v-else class="has-text-grey">No data found. Try searching.</p>
        </div>
    </section>
</template>



<style>
/* optional: center container a bit */
.section {
    max-width: 900px;
    margin: 0 auto;
}
</style>
