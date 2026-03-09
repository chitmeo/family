import { api, types } from '@chitmeo/shared';
import { ref } from 'vue';
import type { JournalTemplate } from '@/modules/accounting/types/JournalTemplate';

const loading = ref(false)
const error = ref<string | null>(null)

async function getJournals(chartOfAccountId: string): Promise<JournalTemplate[]> {
    loading.value = true
    error.value = null
    try {
        const res = await api.privateApi.get<JournalTemplate[]>(`/accg/journalTemplate/by-chartofaccount/${chartOfAccountId}`,
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

async function createJournal(payload: JournalTemplate) {
    loading.value = true
    error.value = null
    try {
        await api.privateApi.post('/accg/journalTemplate', payload)
    } catch (err: any) {
        error.value = api.extractApiError(err)
    } finally {
        loading.value = false
    }
}

async function updateJournal(journal: JournalTemplate) {
    loading.value = true
    error.value = null
    try {
        await api.privateApi.put(`/accg/journalTemplate/${journal.id}`, journal)
    } catch (err: any) {
        error.value = api.extractApiError(err)
    }
    finally {
        loading.value = false
    }
}


export function useJournalTemplate() {
    return {
        loading,
        error,
        getJournals,
        createJournal,
        updateJournal    
    }
}