import { api, types } from '@chitmeo/shared';
import { ref } from 'vue';
import type { Journal } from '@/modules/accounting/types/Journal';

const loading = ref(false)
const error = ref<string | null>(null)

async function getJournals(chartOfAccountId: string): Promise<Journal[]> {
    loading.value = true
    error.value = null
    try {
        const res = await api.privateApi.get<Journal[]>(`/accg/journal/by-chartofaccount/${chartOfAccountId}`,
            { params: { showHidden: true } }
        );        
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

async function updateJournal(journal: Journal) {
    loading.value = true
    error.value = null
    try {
        await api.privateApi.put(`/accg/journal/${journal.id}`, journal)
    } catch (err: any) {
        error.value = api.extractApiError(err)
    }
    finally {
        loading.value = false
    }
}


export function useJournal() {
    return {
        loading,
        error,
        getJournals,
        createJournal,
        updateJournal    
    }
}