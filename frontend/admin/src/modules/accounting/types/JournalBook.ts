export interface JournalBook {
    id: string
    chartOfAccountId: string    
    code: string
    name: string
    periodStart: Date
    periodEnd: Date
    description: string
    isActive: boolean
}