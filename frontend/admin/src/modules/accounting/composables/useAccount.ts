import { ref } from 'vue';
import type { Account } from '@/modules/accounting/types/Account';
import type { ChartOfAccount } from '@/modules/accounting/types/ChartOfAccount';
import { api, types } from '@chitmeo/shared';

const currentAccount = ref<Account | null>(null)
const listAccount = ref<Account[]>([])

const loading = ref(false)
const error = ref<string | null>(null)

function setAccount(account: Account) {
  currentAccount.value = account
}

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

async function createAccount(payload: Account) {
  loading.value = true;
  error.value = null;
  try {
    await api.privateApi.post(`/accg/account`, payload);
  } catch (err: any) {
    console.error(err);
    error.value = err.message || 'Failed to create account';
  } finally {
    loading.value = false;
  }
}

async function updateAccount(account: Account) {
  loading.value = true;
  error.value = null;
  try {
    await api.privateApi.put(`/accg/account/${account.id}`, account);
  } catch (err: any) {
    console.error(err);
    error.value = err.message || 'Failed to update account';
  } finally {
    loading.value = false;
  }
}

async function getAccounts(chartOfAccountId: string) {
  loading.value = true
  error.value = null
  try {
    const res = await api.privateApi.get(`/accg/chartofaccount/${chartOfAccountId}/accounts`, {
      params: { showHidden: true }
    })
    listAccount.value = res.data
  } catch (err: any) {
    error.value = err.message || 'Failed to fetch accounts'
  } finally {
    loading.value = false
  }
}


export function useAccount() {
  return {
    currentAccount,
    listAccount,
    loading,
    error,
    setAccount,
    createAccount,
    updateAccount,
    getChartOfAccounts,
    getAccounts
  }
}