<script setup lang="ts">
import { computed } from 'vue';
import { useChartOfAccount } from '@/modules/accounting/composables/useChartOfAccount'

const { selectedAccount } = useChartOfAccount();
const hasData = computed(() => !!selectedAccount.value)

</script>


<template>
    <div v-if="hasData">
        <p><strong>ID:</strong> {{ selectedAccount?.id }}</p>
        <p><strong>Code:</strong> {{ selectedAccount?.code }}</p>
        <p><strong>Name:</strong> {{ selectedAccount?.name }}</p>
    </div>

    <div v-else>
        <p>Select an account from the "Find" tab.</p>
    </div>
</template>