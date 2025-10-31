export interface Account {
  id: string 
  chartOfAccountId: string
  parentId?: string | null
  parentCode?: string | null
  code: string
  name: string
  accountType: string
  isActive: boolean
}