import { api } from "@chitmeo/shared";
import { ref } from "vue";

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

function toApiDateString(date: Date) {
  const local = new Date(date.getTime() - date.getTimezoneOffset() * 60000)
  return local.toISOString().split('T')[0]
}

export function useJournalBook() {
  return {
    loading,
    error,
    getBooks
  }
}