<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { useChartOfAccountStore } from '@/modules/accounting/stores/chartOfAccountStore';
import type { types } from '@chitmeo/shared';
import type { JournalBook } from '@/modules/accounting/types';
import { useJournalBook } from '@/modules/accounting/composables';
import MonthRangePicker from '@/app/components/MonthRangePicker.vue'

const coaStore = useChartOfAccountStore();
const coaOptions = ref<types.SelectOption[]>([]);
const listBooks = ref<JournalBook[]>([]);
const selectedChartOfAccountId = ref<string>('');
const { loading, error, createBook, updateBook, getBooks } = useJournalBook();

const fromDate = ref(new Date())
const toDate = ref(new Date())
const hasData = computed(() => listBooks.value.length > 0);

onMounted(async () => {
    coaOptions.value = await coaStore.fetchOptions();
    if (coaOptions.value.length > 0) {
        selectedChartOfAccountId.value = coaOptions.value[0]?.value.toString() ?? '';
    }
});

async function handleSearchBooks() {
    if (selectedChartOfAccountId.value) {
        listBooks.value = await getBooks(selectedChartOfAccountId.value, fromDate.value, toDate.value);
    }
}

function onRangeChange(range: { fromDate: Date; toDate: Date }) {
    fromDate.value = range.fromDate
    toDate.value = range.toDate
}

// Modal & form state
const showModal = ref(false);
const isEditMode = ref(false);
const form = ref<Partial<JournalBook>>({
    id: '',
    chartOfAccountId: '',
    code: '',
    name: '',
    periodStart: new Date(),
    periodEnd: new Date(),
    description: '',
    isActive: true,
});
function openCreateModal() {
    form.value = {
        chartOfAccountId: selectedChartOfAccountId.value,
        code: '',
        name: '',
        periodStart: new Date(),
        periodEnd: new Date(),
        description: '',
        isActive: true
    };
    isEditMode.value = false;
    showModal.value = true;
}
function openEditModal(book: JournalBook) {
    form.value = { ...book };
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
            await updateBook(form.value as JournalBook);
        } else {
            form.value.id = '00000000-0000-0000-0000-000000000000';
            await createBook(form.value as JournalBook);
        }
        listBooks.value = await getBooks(selectedChartOfAccountId.value, fromDate.value, toDate.value);
        closeModal();
    }
    catch (err) {
        console.error(err);
    }
}

</script>

<template>
    <section class="section">
        <div class="container">
            <h1 class="title is-4 mb-4">Journal Books</h1>
            <div class="field">
                <label class="label">Chart of Account</label>
                <div class="control">
                    <div class="select is-fullwidth">
                        <select v-model="selectedChartOfAccountId">
                            <option v-for="item in coaOptions" :key="item.value" :value="item.value">
                                {{ item.text }}
                            </option>
                        </select>
                    </div>
                </div>
            </div>

            <!-- Date Range -->
            <MonthRangePicker @update:range="onRangeChange" />

            <div class="field">
                <div class="control">
                    <button class="button is-primary" @click="handleSearchBooks">
                        Search
                    </button>
                </div>
            </div>

            <!-- Add button -->
            <div class="mb-4">
                <button class="button is-primary" @click="openCreateModal" :disabled="!selectedChartOfAccountId">
                    <span class="icon"><i class="fas fa-plus"></i></span>
                    <span>Add New Journal Book</span>
                </button>
            </div>

            <div v-if="loading" class="notification is-info is-light">Loading Journal books...</div>
            <div v-else-if="error" class="notification is-danger is-light">{{ error }}</div>

            <!-- Table -->
            <div v-else-if="hasData">
                <table class="table is-striped is-hoverable is-fullwidth">
                    <thead>
                        <tr>
                            <th>Code</th>
                            <th>Name</th>
                            <th>Period Start</th>
                            <th>Period End</th>
                            <th>Status</th>
                            <th style="width: 100px;">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="book in listBooks" :key="book.id">
                            <td>{{ book.code }}</td>
                            <td>{{ book.name }}</td>
                            <td>{{ book.periodStart }}</td>
                            <td>{{ book.periodEnd }}</td>
                            <td>
                                <span class="tag" :class="book.isActive ? 'is-success' : 'is-danger'">
                                    {{ book.isActive ? 'Active' : 'Inactive' }}
                                </span>
                            </td>
                            <td>
                                <button class="button is-small is-info mr-1" @click="openEditModal(book)">
                                    <i class="fas fa-edit"></i>
                                </button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div v-else class="notification is-warning is-light">
                No Journal books found for the selected Chart of Account.
            </div>

            <!-- Modal -->
            <div class="modal" :class="{ 'is-active': showModal }">
                <div class="modal-background" @click="closeModal"></div>
                <div class="modal-card">
                    <header class="modal-card-head">
                        <p class="modal-card-title">
                            {{ isEditMode ? 'Edit Journal Book' : 'Create New Journal Book' }}
                        </p>
                        <button class="delete" aria-label="close" @click="closeModal"></button>
                    </header>
                    <section class="modal-card-body">
                        <div class="field">
                            <label class="label">Code</label>
                            <div class="control">
                                <input class="input" v-model="form.code" type="text" placeholder="Enter book code" />
                            </div>
                        </div>

                        <div class="field">
                            <label class="label">Name</label>
                            <div class="control">
                                <input class="input" v-model="form.name" type="text" placeholder="Enter book name" />
                            </div>
                        </div>

                        <div class="field">
                            <label class="label">Period Start</label>
                            <div class="control">
                                <input class="input" type="date" v-model="form.periodStart" />
                            </div>
                        </div>

                        <div class="field">
                            <label class="label">Period End</label>
                            <div class="control">
                                <input class="input" type="date" v-model="form.periodEnd" />
                            </div>
                        </div>

                        <div class="field">
                            <label class="label">Description</label>
                            <div class="control">
                                <textarea class="textarea" v-model="form.description"
                                    placeholder="Enter description"></textarea>
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
