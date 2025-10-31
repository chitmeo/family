import { defineStore } from 'pinia'

interface User {
  id: string
  username: string
  email: string
  roles: string
}

interface AuthState {
  isAuthenticated: boolean
  accessToken: string
  refreshToken: string
  user: User | null
}

export const useAuthStore = defineStore('auth', {
  state: (): AuthState => ({
    isAuthenticated: false,
    accessToken: '',
    refreshToken: '',
    user: null,
  }),

  actions: {
    // Set auth info after login
    setAuth(payload: {
      accessToken: string
      refreshToken: string
      user: User
    }) {
      this.accessToken = payload.accessToken
      this.refreshToken = payload.refreshToken
      this.user = payload.user
      this.isAuthenticated = true

      localStorage.setItem('accessToken', payload.accessToken)
      localStorage.setItem('refreshToken', payload.refreshToken)
      localStorage.setItem('user', JSON.stringify(payload.user))
    },

    // Load auth info from localStorage (on app init)
    loadAuth() {
      const accessToken = localStorage.getItem('accessToken')
      const refreshToken = localStorage.getItem('refreshToken')
      const userStr = localStorage.getItem('user')

      if (accessToken && refreshToken && userStr) {
        this.accessToken = accessToken
        this.refreshToken = refreshToken
        this.user = JSON.parse(userStr)
        this.isAuthenticated = true
      } else {
        this.isAuthenticated = false
        this.accessToken = ''
        this.refreshToken = ''
        this.user = null
      }
    },

    // Clear auth info on logout
    logout() {
      this.isAuthenticated = false
      this.accessToken = ''
      this.refreshToken = ''
      this.user = null

      localStorage.removeItem('accessToken')
      localStorage.removeItem('refreshToken')
      localStorage.removeItem('user')
    },
  },
})
