<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'

// Emit type
const emit = defineEmits<{
  (e: 'update:range', payload: { fromDate: Date; toDate: Date }): void
}>()

const months = [
  'January', 'February', 'March', 'April', 'May', 'June',
  'July', 'August', 'September', 'October', 'November', 'December'
]

const yearRange = 10
const currentYear = new Date().getFullYear()
const years = computed(() => {
  const start = currentYear - yearRange
  const end = currentYear + yearRange
  const arr = []
  for (let y = start; y <= end; y++) arr.push(y)
  return arr
})

// From/To Month/Year
const fromMonth = ref(1)
const fromYear = ref(currentYear)
const toMonth = ref(12)
const toYear = ref(currentYear)

// Helpers
function getStartOfMonth(year: number, month: number) {
  return new Date(year, month - 1, 1)
}
function getEndOfMonth(year: number, month: number) {
  return new Date(year, month, 0)
}

function emitChange() {
  const fromDate = getStartOfMonth(fromYear.value, fromMonth.value)
  const toDate = getEndOfMonth(toYear.value, toMonth.value)
  emit('update:range', { fromDate, toDate })
}

// Ensure to >= from
function onFromChange() {
  if (fromYear.value > toYear.value || (fromYear.value === toYear.value && fromMonth.value > toMonth.value)) {
    toYear.value = fromYear.value
    toMonth.value = fromMonth.value
  }
  emitChange()
}

function onToChange() {
  if (toYear.value < fromYear.value || (toYear.value === fromYear.value && toMonth.value < fromMonth.value)) {
    fromYear.value = toYear.value
    fromMonth.value = toMonth.value
  }
  emitChange()
}

onMounted(() => {
  fromMonth.value = 1
  fromYear.value = currentYear
  toMonth.value = 12
  toYear.value = currentYear
  emitChange()
})
</script>

<style scoped>
.month-range-picker .field.is-horizontal {
  flex-wrap: wrap;        
  align-items: flex-start; 
  justify-content: flex-start; 
}

.month-range-picker .field-label {
  margin-right: 0.5rem;
}

.month-range-picker .field-body.is-grouped {
  display: flex;
  gap: 0.5rem;
  align-items: flex-start; 
}
</style>


<template>
  <div class="month-range-picker">
    <div class="field is-horizontal">
      <!-- From -->
      <div class="field-label is-normal">
        <label class="label">From:</label>
      </div>
      <div class="field-body is-grouped">
        <div class="field">
          <div class="control">
            <div class="select">
              <select v-model="fromMonth" @change="onFromChange">
                <option v-for="(month, i) in months" :key="i" :value="i + 1">{{ month }}</option>
              </select>
            </div>
          </div>
        </div>
        <div class="field">
          <div class="control">
            <div class="select">
              <select v-model="fromYear" @change="onFromChange">
                <option v-for="year in years" :key="year" :value="year">{{ year }}</option>
              </select>
            </div>
          </div>
        </div>
      </div>

      <!-- To -->
      <div class="field-label is-normal ml-4">
        <label class="label">To:</label>
      </div>
      <div class="field-body is-grouped">
        <div class="field">
          <div class="control">
            <div class="select">
              <select v-model="toMonth" @change="onToChange">
                <option v-for="(month, i) in months" :key="i" :value="i + 1">{{ month }}</option>
              </select>
            </div>
          </div>
        </div>
        <div class="field">
          <div class="control">
            <div class="select">
              <select v-model="toYear" @change="onToChange">
                <option v-for="year in years" :key="year" :value="year">{{ year }}</option>
              </select>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>