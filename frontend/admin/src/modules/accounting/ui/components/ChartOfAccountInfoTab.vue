<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import { useChartOfAccount } from '@/modules/accounting/composables/useChartOfAccount'
import type { ChartOfAccount } from '@/modules/accounting/types/ChartOfAccount';

const { currentChartOfAccount, error, loading, setChartOfAccount, createCOA, updateCOA } = useChartOfAccount();
const hasData = computed(() => !!currentChartOfAccount.value)
const isNew = ref(false)
const form = ref<ChartOfAccount>({
    id: '',
    code: '',
    name: '',
    isActive: true,
})

watch(currentChartOfAccount, (val) => {
    if (val) {
        form.value = { ...val }
        isNew.value = false
    }
})

watch(currentChartOfAccount, (val) => {
    if (val) form.value = { ...val }
    else form.value = { id: '', code: '', name: '', isActive: true }
}, { immediate: true })

function handleNew() {
    isNew.value = true
    form.value = {
        id: '00000000-0000-0000-0000-000000000000',
        code: '',
        name: '',
        isActive: true
    }
}

async function handleSave() {
    if (isNew.value) {
        await createCOA(form.value)
    } else {
        await updateCOA(form.value)
    }
    setChartOfAccount(form.value)
    isNew.value = false
}

</script>


<template>
    <section class="section">
        <div v-if="hasData">
            <div class="container box">
                <article v-if="error" class="message is-danger mb-4">
                    <div class="message-body">{{ error }}</div>
                </article>

                <div class="field">
                    <label class="label">Code</label>
                    <div class="control">
                        <input v-model="form.code" class="input" type="text" placeholder="Enter code" />
                    </div>
                </div>

                <div class="field">
                    <label class="label">Name</label>
                    <div class="control">
                        <input v-model="form.name" class="input" type="text" placeholder="Enter name" />
                    </div>
                </div>

                <div class="field">
                    <label class="checkbox">
                        <input type="checkbox" v-model="form.isActive" />
                        Active
                    </label>
                </div>

                <div class="buttons">
                    <button class="button is-primary" @click="handleNew" :disabled="loading">
                        <span class="icon"> <i class="fas fa-plus"></i></span>
                        <span>New</span>
                    </button>
                    <button class="button is-success" @click="handleSave" :disabled="loading">
                        <span class="icon"><i class="fas fa-save"></i></span>
                        <span>{{ loading ? 'Saving...' : 'Save' }}</span>
                    </button>
                </div>
            </div>
        </div>

        <div v-else>
            <p>Select an account from the "Find" tab.</p>
        </div>
    </section>
</template>