<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { useChartOfAccountStore } from '@/modules/accounting/stores/chartOfAccountStore';
import type { Account, Journal } from '@/modules/accounting/types';
import type { types } from '@chitmeo/shared';
import { useJournal } from '@/modules/accounting/composables/useJournal';
import { useAccount } from '@/modules/accounting/composables/useAccount';

const coaOptions = ref<types.SelectOption[]>([]);
const { loading: loadingAccount, getAccounts } = useAccount();
const { loading, error, getJournals, createJournal, updateJournal } = useJournal();

const coaStore = useChartOfAccountStore();

const listJournal = ref<Journal[]>([]);
const listAccount = ref<Account[]>([])
const selectedChartOfAccountId = ref<string>('');
const hasData = computed(() => listJournal.value.length > 0);
// Modal & form state
const showModal = ref(false);
const isEditMode = ref(false);
const form = ref<Partial<Journal>>({});

onMounted(async () => {
  coaOptions.value = await coaStore.fetchOptions();
  if (coaOptions.value.length > 0) {
    selectedChartOfAccountId.value = coaOptions.value[0]?.value.toString() ?? ''
    listJournal.value = await getJournals(selectedChartOfAccountId.value);
    listAccount.value = await getAccounts(selectedChartOfAccountId.value);
  }
});

async function handleChartOfAccountChange() {
  if (selectedChartOfAccountId.value) {
    listJournal.value = await getJournals(selectedChartOfAccountId.value);
    listAccount.value = await getAccounts(selectedChartOfAccountId.value);
  } else {
    listJournal.value = [];
    listAccount.value = [];
  }
}
// === Modal form logic ===
function openCreateModal() {
  form.value = {
    id: '',
    chartOfAccountId: selectedChartOfAccountId.value,
    code: '',
    name: '',
    isActive: true
  };
  isEditMode.value = false;
  showModal.value = true;
}

function openEditModal(journal: Journal) {
  form.value = { ...journal };
  isEditMode.value = true;
  showModal.value = true;
}

function closeModal() {
  showModal.value = false;
}
async function handleSubmit() {
  if (!selectedChartOfAccountId.value) return alert('Please select a Chart of Account first.');
  form.value.chartOfAccountId = selectedChartOfAccountId.value;
  try {
    if (isEditMode.value) {
      await updateJournal(form.value as Journal)
    } else {
      form.value.id = '00000000-0000-0000-0000-000000000000';
      await createJournal(form.value as Journal)
    }
    listJournal.value = await getJournals(selectedChartOfAccountId.value);
    closeModal();
  } catch (err) {
    console.error(err);
  }
}
</script>

<template>
  <section class="section">
    <div class="container">
      <h1 class="title is-4 mb-4">Manage Journal</h1>

      <!-- Chart of Account Selector -->
      <div class="field">
        <label class="label">Chart of Account</label>
        <div class="control">
          <div class="select is-fullwidth">
            <select v-model="selectedChartOfAccountId" @change="handleChartOfAccountChange">
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
      <div v-if="loading" class="notification is-info is-light">Loading journals...</div>
      <div v-else-if="error" class="notification is-danger is-light">{{ error }}</div>
      <table v-else-if="hasData" class="table is-striped is-hoverable is-fullwidth">
        <thead>
          <tr>
            <th>Chart Of Account</th>
            <th>Code</th>
            <th>Name</th>
            <th>Credit</th>
            <th>Debit</th>
            <th>Status</th>
            <th style="width: 100px;">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="journal in listJournal" :key="journal.id">
            <td>{{ journal.coaName }}</td>
            <td>{{ journal.code }}</td>
            <td>{{ journal.name }}</td>
            <td>{{ journal.creditAccountCode }}</td>
            <td>{{ journal.debitAccountCode }}</td>
            <td>
              <span class="tag" :class="journal.isActive ? 'is-success' : 'is-danger'">
                {{ journal.isActive ? 'Active' : 'Inactive' }}
              </span>
            </td>
            <td>
              <button class="button is-small is-info mr-1" @click="openEditModal(journal)">
                <i class="fas fa-edit"></i>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
      <div v-else class="notification is-warning is-light">
        No journals found for the selected Chart of Account.
      </div>

      <!-- Modal -->
      <div class="modal" :class="{ 'is-active': showModal }">
        <div class="modal-background" @click="closeModal"></div>
        <div class="modal-card">
          <header class="modal-card-head">
            <p class="modal-card-title">
              {{ isEditMode ? 'Edit Journal' : 'Create New Journal' }}
            </p>
            <button class="delete" aria-label="close" @click="closeModal"></button>
          </header>
          <section class="modal-card-body">
            <div class="field">
              <label class="label">Journal Code</label>
              <div class="control">
                <input class="input" v-model="form.code" type="text" placeholder="Enter journal code" />
              </div>
            </div>

            <div class="field">
              <label class="label">Journal Name</label>
              <div class="control">
                <input class="input" v-model="form.name" type="text" placeholder="Enter journal name" />
              </div>
            </div>

            <div class="field">
              <label class="label">Journal Type</label>
              <div class="control">
                <input class="input" v-model="form.type" type="text" placeholder="Enter journal type" />
              </div>
            </div>

            <div class="field">
              <label class="label">Default Dedit Account</label>
              <div class="control">
                <div class="select is-fullwidth">
                  <select v-model="form.defaultDebitAccountId">
                    <option v-for="account in listAccount" :key="account.id" :value="account.id">
                      {{ account.code }} - {{ account.name }}
                    </option>
                  </select>
                </div>
              </div>
            </div>

            <div class="field">
              <label class="label">Default Credit Account</label>
              <div class="control">
                <div class="select is-fullwidth">
                  <select v-model="form.defaultCreditAccountId">
                    <option v-for="account in listAccount" :key="account.id" :value="account.id">
                      {{ account.code }} - {{ account.name }}
                    </option>
                  </select>
                </div>
              </div>
            </div>

            <div class="field">
              <label class="label">Description</label>
              <div class="control">
                <textarea class="textarea" v-model="form.description" placeholder="Enter description"></textarea>
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