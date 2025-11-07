import { ref } from 'vue';
import type { Account } from '@/modules/accounting/types/Account';
import type { ChartOfAccount } from '@/modules/accounting/types/ChartOfAccount';
import { api, types } from '@chitmeo/shared';

const currentAccount = ref<Account | null>(null)

const loading = ref(false)
const error = ref<string | null>(null)

function setAccount(account: Account) {
  currentAccount.value = account
}

async function createAccount(payload: Account) {
  loading.value = true;
  error.value = null;
  try {
    await api.privateApi.post(`/accg/account`, payload);
  } catch (err: any) {
    error.value = api.extractApiError(err)
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
    error.value = api.extractApiError(err)
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
    return res.data
  } catch (err: any) {
    error.value = api.extractApiError(err)
  } finally {
    loading.value = false
  }
}


export function useAccount() {
  return {
    currentAccount,
    loading,
    error,
    setAccount,
    createAccount,
    updateAccount,
    getAccounts
  }
}