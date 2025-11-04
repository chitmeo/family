import { api, types } from '@chitmeo/shared';
import { ref } from 'vue/dist/vue.js';
import type { Journal } from '@/modules/accounting/types/Journal';

const loading = ref(false)
const error = ref<string | null>(null)

async function getJournals(): Promise<Journal[]> {
    loading.value = true
    error.value = null
    try {
        const res = await api.privateApi.get<Journal[]>('/accg/journal')
        return res.data
    } catch (err: any) {
        error.value = api.extractApiError(err)
        return []
    } finally {
        loading.value = false
    }
}

async function createJournal(payload: Journal) {
    loading.value = true
    error.value = null
    try {
        await api.privateApi.post('/accg/journal', payload)
    } catch (err: any) {
        error.value = api.extractApiError(err)
    } finally {
        loading.value = false
    }
}

export function useJournal() {
    return {
        loading,
        error,
        createJournal,
        getJournals,
    }
}