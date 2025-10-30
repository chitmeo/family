<script setup lang="ts">

import { ref, onMounted, computed } from 'vue'
import { useChartOfAccount } from '@/modules/accounting/composables/useChartOfAccount';

const { search, selectAccount, list, activeTab } = useChartOfAccount();

const searchTerm = ref('')
const showHidden = ref(true);

onMounted(() => {
    search(searchTerm.value, showHidden.value);
});

function handleClick(item: any) {
    selectAccount(item)
    activeTab.value = 'info'
}
function handleSearch() {
    search(searchTerm.value, showHidden.value);
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
            <table class="table is-striped is-fullwidth" v-if="list.length">
                <thead>
                    <tr>
                        <th>Code</th>
                        <th>Name</th>
                        <th>Active</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in list" :key="item.id">                        
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

            <!-- Empty state -->
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
