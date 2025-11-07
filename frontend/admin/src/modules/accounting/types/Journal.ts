export interface Journal {
    id: string
    chartOfAccountId: string
    coaName: string
    code: string
    name: string
    type: string
    defaultDebitAccountId: string
    debitAccountCode: string
    defaultCreditAccountId: string
    creditAccountCode: string
    description: string
    isActive: boolean
}