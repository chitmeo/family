<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useChartOfAccountStore } from '../../stores/chartOfAccountStore';
const coaStore = useChartOfAccountStore();


</script>

<template>
  <div class="journal-view">
    <h1>Journal Entries</h1>
    <!-- <table>
      <thead>
        <tr>
          <th>Date</th>
          <th>Description</th>
          <th>Debit</th>
          <th>Credit</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="entry in journalEntries" :key="entry.id">
          <td>{{ entry.date }}</td>
          <td>{{ entry.description }}</td>
          <td>{{ entry.debit }}</td>
          <td>{{ entry.credit }}</td>
        </tr>
      </tbody>
    </table> -->
  </div>
</template>