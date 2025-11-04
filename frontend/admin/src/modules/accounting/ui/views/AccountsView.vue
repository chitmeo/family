<script lang="ts" setup>
import { computed, ref, onMounted } from 'vue';
import { useAccount } from '@/modules/accounting/composables/useAccount';
import type { types } from '@chitmeo/shared';
import type { Account } from '@/modules/accounting/types/Account';
import { useChartOfAccountStore } from '@/modules/accounting/stores/chartOfAccountStore';

const { currentAccount, listAccount, loading, error, getAccounts, createAccount, updateAccount } = useAccount();
const coaStore = useChartOfAccountStore();
const selectedChartOfAccountId = ref<string>('');
const coaOptions = ref<types.SelectOption[]>([]);
const hasData = computed(() => listAccount.value.length > 0);

// Modal & form state
const showModal = ref(false);
const isEditMode = ref(false);
const form = ref<Partial<Account>>({
    chartOfAccountId: '',
    code: '',
    name: '',
    accountType: '',
    isActive: true,
});

onMounted(async () => {
    coaOptions.value = await coaStore.fetchOptions();
});

async function handleChartOfAccountChange() {
    if (selectedChartOfAccountId.value) {
        await getAccounts(selectedChartOfAccountId.value);
        const selected = coaOptions.value.find(
            (opt) => opt.value === selectedChartOfAccountId.value
        );
        if (selected) {
            currentAccount.value = { id: selected.value, name: selected.text } as any;
        }
    } else {
        listAccount.value = [];
    }
}
// === Modal form logic ===

// === Parent Account options ===
const parentOptions = computed<types.SelectOption[]>(() => {
    return listAccount.value
        .filter(a => !a.parentId)
        .sort((a, b) => a.code.localeCompare(b.code))
        .map(a => ({
            text: `${a.code} - ${a.name}`,
            value: a.id,
        }));
});

function openCreateModal() {
    form.value = { id: '', name: '', accountType: '', isActive: true };
    isEditMode.value = false;
    showModal.value = true;
}
function openEditModal(account: Account) {
    form.value = { ...account };
    isEditMode.value = true;
    showModal.value = true;
}
function closeModal() {
    showModal.value = false;
}

async function handleSubmit() {
    if (!selectedChartOfAccountId.value) return alert('Please select a Chart of Account first.');
    //if (!form.value.name || !form.value.accountType) return alert('Please fill all required fields.');
    form.value.chartOfAccountId = selectedChartOfAccountId.value;
    try {
        if (isEditMode.value) {
            await updateAccount(form.value as Account);
        } else {
            form.value.id = '00000000-0000-0000-0000-000000000000';
            await createAccount(form.value as Account);
        }
        await getAccounts(selectedChartOfAccountId.value);
        closeModal();
    } catch (err) {
        console.error(err);
    }
}
</script>

<template>
    <section class="section">
        <div class="container">
            <h1 class="title is-4 mb-4">Manage Accounts</h1>

            <!-- Chart of Account Selector -->
            <div class="field">
                <label class="label">Chart of Account</label>
                <div class="control">
                    <div class="select is-fullwidth">
                        <select v-model="selectedChartOfAccountId" @change="handleChartOfAccountChange">
                            <option value="">-- Select Chart of Account --</option>
                            <option v-for="item in coaOptions" :key="item.value" :value="item.value">
                                {{ item.text }}
                            </option>
                        </select>
                    </div>
                </div>
            </div>

            <!-- Add button -->
            <div class="mb-4">
                <button class="button is-primary" @click="openCreateModal" :disabled="!selectedChartOfAccountId">
                    <span class="icon"><i class="fas fa-plus"></i></span>
                    <span>Add New Account</span>
                </button>
            </div>

            <!-- Status display -->
            <div v-if="loading" class="notification is-info is-light">Loading accounts...</div>
            <div v-else-if="error" class="notification is-danger is-light">{{ error }}</div>

            <!-- Table -->
            <div v-else-if="hasData">
                <table class="table is-striped is-hoverable is-fullwidth">
                    <thead>
                        <tr>                            
                            <th>Parent Account</th>
                            <th>Account Code</th>
                            <th>Account Name</th>
                            <th>Account Type</th>
                            <th>Status</th>
                            <th style="width: 100px;">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="account in listAccount" :key="account.id">                            
                            <td>{{ account.parentCode || '--' }}</td>
                            <td>{{ account.code }}</td>
                            <td>{{ account.name }}</td>
                            <td>{{ account.accountType }}</td>
                            <td>
                                <span class="tag" :class="account.isActive ? 'is-success' : 'is-danger'">
                                    {{ account.isActive ? 'Active' : 'Inactive' }}
                                </span>
                            </td>
                            <td>
                                <button class="button is-small is-info mr-1" @click="openEditModal(account)">
                                    <i class="fas fa-edit"></i>
                                </button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div v-else class="notification is-warning is-light">
                No accounts found for the selected Chart of Account.
            </div>

            <!-- Modal -->
            <div class="modal" :class="{ 'is-active': showModal }">
                <div class="modal-background" @click="closeModal"></div>
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title">
                            {{ isEditMode ? 'Edit Account' : 'Create New Account' }}
                        </p>
                        <button class="delete" aria-label="close" @click="closeModal"></button>
                    </header>
                    <section class="modal-card-body">
                        <div class="field">
                            <label class="label">Parent Account</label>
                            <div class="control">
                                <div class="select is-fullwidth">
                                    <select v-model="form.parentId">
                                        <option :value="null">-- No Parent (Root) --</option>
                                        <option v-for="p in parentOptions" :key="p.value" :value="p.value">
                                            {{ p.text }}
                                        </option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="field">
                            <label class="label">Account Code</label>
                            <div class="control">
                                <input class="input" v-model="form.code" type="text" placeholder="Enter account code" />
                            </div>
                        </div>

                        <div class="field">
                            <label class="label">Account Name</label>
                            <div class="control">
                                <input class="input" v-model="form.name" type="text" placeholder="Enter account name" />
                            </div>
                        </div>

                        <div class="field">
                            <label class="label">Account Type</label>
                            <div class="control">
                                <div class="select is-fullwidth">
                                    <select v-model="form.accountType">
                                        <option value="">-- Select Account Type --</option>
                                        <option value="asset">Asset</option>
                                        <option value="liability">Liability</option>
                                        <option value="equity">Equity</option>
                                        <option value="income">Income</option>
                                        <option value="expense">Expense</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="field">
                            <label class="checkbox">
                                <input type="checkbox" v-model="form.isActive" />
                                Active
                            </label>
                        </div>
                    </section>
                    <footer class="modal-card-foot">
                        <button class="button is-success" @click="handleSubmit">
                            {{ isEditMode ? 'Update' : 'Create' }}
                        </button>
                        <button class="button" @click="closeModal">Cancel</button>
                    </footer>
                </div>
            </div>

        </div>
    </section>
</template>
