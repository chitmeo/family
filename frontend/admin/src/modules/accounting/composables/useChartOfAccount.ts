import { ref } from 'vue'
import { api } from '@chitmeo/shared'
import type { ChartOfAccount } from '@/modules/accounting/types/ChartOfAccount';

const activeTab = ref<'info' | 'find' | 'accounts'>('find')

const selectedAccount = ref<ChartOfAccount | null>(null)
const list = ref<ChartOfAccount[]>([])
const loading = ref(false)
const error = ref<string | null>(null)

async function searchCOA(searchTerm: string, showHidden: boolean) {
    loading.value = true
    error.value = null
    try {
        const res = await api.privateApi.get('/accg/chartofaccount/search', {
            params: {
                searchTerm: searchTerm,
                showHidden: showHidden
            }
        })
        list.value = res.data
    } catch (err: any) {
        error.value = err.message || 'Failed to fetch accounts'
    } finally {
        loading.value = false
    }
}

async function fetchDetail(id: number) {
    loading.value = true
    error.value = null
    try {
        const res = await api.privateApi.get(`/chart-of-accounts/${id}`)
        selectedAccount.value = res.data
    } catch (err: any) {
        error.value = err.message || 'Failed to fetch detail'
    } finally {
        loading.value = false
    }
}

function selectAccount(account: ChartOfAccount) {
    selectedAccount.value = account
}

export function useChartOfAccount() {
    return {
        activeTab,
        list,
        selectedAccount,
        loading,
        error,
        searchCOA,
        fetchDetail,
        selectAccount,
    }
}
