export interface Journal {
    id: string
    chartOfAccountId: string
    code: string
    name: string
    type: string
    defaultDebitAccountId: string
    defaultCreditAccountId: string
    description: string
    isActive: boolean
}