export interface JournalEntry {
    id: string
    journalId: string
    entryDate: Date
    reference: string
    description: string
    totalDebit: number
    totalCredit: number
    status: number        
}