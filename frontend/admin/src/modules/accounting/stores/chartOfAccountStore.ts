import { api, type types } from '@chitmeo/shared'
import { defineStore } from 'pinia'

export const useChartOfAccountStore = defineStore('chartOfAccounts', {
    state: () => ({
        options: [] as types.SelectOption[],
        loaded: false,
        lastFetched: 0,
    }),
    actions: {
        async fetchOptions(force = false) {
            const cacheTTL = 24 * 60 * 60 * 1000 // 1 day
            const now = Date.now()

            const cached = localStorage.getItem('chartOfAccounts')
            if (cached && !force) {
                const { options, lastFetched } = JSON.parse(cached)
                if (now - lastFetched < cacheTTL) {
                    this.options = options
                    this.loaded = true
                    return options
                }
            }

            const res = await api.privateApi.get('/accg/chartofaccount/search', {
                params: { searchTerm: '', showHidden: false },
            })
            const options = res.data.map((item: any) => ({
                value: item.id,
                text: `${item.code} - ${item.name}`,
            }))

            this.options = options
            this.loaded = true
            this.lastFetched = now

            localStorage.setItem('chartOfAccounts', JSON.stringify({
                options,
                lastFetched: now,
            }))

            return options
        },

        clearCache() {
            const cacheKey = 'chartOfAccounts'
            localStorage.removeItem(cacheKey)
            this.options = []
            this.loaded = false
            this.lastFetched = 0
        }
    }
})
