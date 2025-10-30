<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import { useChartOfAccount } from '@/modules/accounting/composables/useChartOfAccount'
import type { ChartOfAccount } from '@/modules/accounting/types/ChartOfAccount';

const { selectedAccount } = useChartOfAccount();
const hasData = computed(() => !!selectedAccount.value)

const form = ref<ChartOfAccount>({
    id: '',
    code: '',
    name: '',
    isActive: true,
})

watch(selectedAccount, (val) => {
    if (val) form.value = { ...val }
    else form.value = { id: '', code: '', name: '', isActive: true }
}, { immediate: true })

function handleNew() {
    form.value = {
        id: '',
        code: '',
        name: '',
        isActive: true,
    }
}

async function handleSave() {
    // Simulate save operation
    alert(`Saving account: ${form.value.code} - ${form.value.name}`);
}

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