import { api } from "@chitmeo/shared";
import { ref } from "vue";
import type { JournalBook } from "@/modules/accounting/types";
const loading = ref(false)
const error = ref<string | null>(null)

async function getBooks(chartOfAccountId: string, startDate: Date, endDate: Date) {
    loading.value = true
    error.value = null
    try {
        const res = await api.privateApi.post(`/accg/journalBook/getByDateRanges`, {
            chartOfAccountId: chartOfAccountId,
            startDate: startDate,
            endDate: endDate
        })
        return res.data
    } catch (err: any) {
        error.value = api.extractApiError(err)
    } finally {
        loading.value = false
    }
}

async function updateBook(book: JournalBook) {
    loading.value = true
    error.value = null
    try {
        const res = await api.privateApi.put(`accg/journalBook`)

    }
    catch (err: any) {
        error.value = api.extractApiError(err)
    }
    finally {
        loading.value = false
    }

}


export function useJournalBook() {
    return {
        loading,
        error,
        getBooks
    }
}