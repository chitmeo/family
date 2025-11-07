import { ref } from 'vue'
import { api, types } from '@chitmeo/shared'
import type { ChartOfAccount } from '@/modules/accounting/types/ChartOfAccount';
import type { Account } from '@/modules/accounting/types/Account';

const activeTab = ref<'info' | 'find' | 'accounts'>('find')

const currentChartOfAccount = ref<ChartOfAccount | null>(null)
const listChartOfAccounts = ref<ChartOfAccount[]>([])
const loading = ref(false)
const error = ref<string | null>(null)

async function getChartOfAccounts(): Promise<types.SelectOption[]> {
  loading.value = true
  error.value = null
  try {
    const res = await api.privateApi.get<ChartOfAccount[]>('/accg/chartofaccount/search', {
      params: {
        searchTerm: '',
        showHidden: false
      }
    })
    return res.data.map(item => ({
      value: item.id,
      text: `${item.code} - ${item.name}`,
    }));
  } catch (err: any) {
    error.value = err.message || 'Failed to fetch accounts'
    return []
  } finally {
    loading.value = false
  }
}

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
        error.value = api.extractApiError(err)
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
        error.value = api.extractApiError(err)
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
        error.value = api.extractApiError(err)
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
        error.value = api.extractApiError(err)
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
