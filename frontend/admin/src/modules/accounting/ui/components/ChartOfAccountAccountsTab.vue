<script lang="ts" setup>
import { computed, onMounted, ref } from 'vue';
import { useChartOfAccount } from '../../composables';
import type { Account } from '@/modules/accounting/types/Account';

const listAccounts = ref<Account[]>([])
const { getAccounts, currentChartOfAccount } = useChartOfAccount();
const hasData = computed(() => !!currentChartOfAccount.value)

onMounted(async () => {
    if (hasData.value) {
        listAccounts.value = await getAccounts(currentChartOfAccount.value!.id);
    }
})

</script>
<template>
    <div class="chart-of-account-view">
        <div v-if="hasData">            
            <table class="table is-striped is-fullwidth" v-if="listAccounts.length">
                <thead>
                    <tr>
                        <th>Parent Account</th>
                        <th>Account Code</th>
                        <th>Account Name</th>
                        <th>Account Type</th>
                        <th>Active</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="account in listAccounts" :key="account.id">
                        <td>{{ account.parentCode || '--' }}</td>
                        <td>{{ account.code }}</td>
                        <td>{{ account.name }}</td>
                        <td>{{ account.accountType }}</td>
                        <td>
                            <span class="tag" :class="account.isActive ? 'is-success' : 'is-danger'">
                                {{ account.isActive ? 'Active' : 'Inactive' }}
                            </span>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div v-else>
            <p>No accounts found for the selected Chart of Account.</p>
        </div>
    </div>
</template>
