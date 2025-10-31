import { ref } from 'vue'
import { api } from '@chitmeo/shared'
import type { ChartOfAccount } from '@/modules/accounting/types/ChartOfAccount';
import type { Account } from '@/modules/accounting/types/Account';

const activeTab = ref<'info' | 'find' | 'accounts'>('find')

const currentChartOfAccount = ref<ChartOfAccount | null>(null)
const listChartOfAccounts = ref<ChartOfAccount[]>([])
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
        listChartOfAccounts.value = res.data
    } catch (err: any) {
        error.value = err.message || 'Failed to fetch accounts'
    } finally {
        loading.value = false
    }
}

async function createCOA(payload: ChartOfAccount) {
    loading.value = true;
    error.value = null;
    try {
        await api.privateApi.post('/accg/chartofaccount', payload);
    } catch (err: any) {
        console.error(err);
        error.value = err.message || 'Failed to create chart of account';
    } finally {
        loading.value = false;
    }
}


async function updateCOA(coa: ChartOfAccount) {
    loading.value = true;
    error.value = null;
    try {
        await api.privateApi.put(`/accg/chartofaccount/${coa.id}`, coa);
    } catch (err: any) {
        if (err.response && err.response.data) {
            const data = err.response.data

            if (data.errors && typeof data.errors === 'object') {
                const messages = Object.entries(data.errors)
                    .map(([field, msgs]) => `${field}: ${(msgs as string[]).join(', ')}`)
                    .join('\n')
                error.value = messages
            }
            else if (data.title || data.detail) {
                error.value = data.detail || data.title
            }
            else {
                error.value = 'Validation failed'
            }
        }
        else {
            error.value = err.message || 'Failed to update chart of account'
        }
    } finally {
        loading.value = false;
    }
}

async function getAccounts(chartOfAccountId: string): Promise<Account[]> {
    loading.value = true
    error.value = null
    try {
        const res = await api.privateApi.get(`/accg/chartofaccount/${chartOfAccountId}/accounts`, {
            params: { showHidden: true }
        })
        return res.data
    } catch (err: any) {
        error.value = err.message || 'Failed to fetch accounts'
        return [];
    } finally {
        loading.value = false
    }
}

function setChartOfAccount(account: ChartOfAccount) {
    currentChartOfAccount.value = account
}

export function useChartOfAccount() {
    return {
        activeTab,
        listChartOfAccounts,
        currentChartOfAccount,
        loading,
        error,
        searchCOA,
        createCOA,
        updateCOA,
        getAccounts,
        setChartOfAccount,
    }
}
