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
const { getBooks } = useJournalBook();

const fromDate = ref(new Date())
const toDate = ref(new Date())

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
        </div>
    </section>
</template>
