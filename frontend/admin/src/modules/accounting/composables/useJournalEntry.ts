import { ref } from "vue";

const loading = ref(false)
const error = ref<string | null>(null)



export function useJournalEntry() {
    return {
        loading,
        error
    }
}