import axios, { AxiosError, type AxiosRequestConfig, type AxiosResponse } from 'axios'

const apiBaseUrl =  import.meta.env.VITE_PRIVATE_API || import.meta.env.VITE_API_BASE_URL

// Public API instance
export const publicApi = axios.create({
  baseURL: apiBaseUrl,
  withCredentials: true,
})

// Private API instance
export const privateApi = axios.create({
  baseURL: apiBaseUrl,
  withCredentials: true,
})

// ========== Request Interceptor ==========
// Attach Authorization header automatically if token exists
privateApi.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('accessToken')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => Promise.reject(error)
)
// ========== Response Interceptor ==========
let isRefreshing = false
let refreshSubscribers: ((token: string) => void)[] = []

function onRefreshed(newToken: string): void {
  refreshSubscribers.forEach((callback) => callback(newToken))
  refreshSubscribers = []
}

function addRefreshSubscriber(callback: (token: string) => void): void {
  refreshSubscribers.push(callback)
}

// ================== Response Interceptor ==================
// Optional: handle 401 responses globally
privateApi.interceptors.response.use(
  (response: AxiosResponse) => response,
  async (error: AxiosError): Promise<any> => {
    const originalRequest = error.config as AxiosRequestConfig & { _retry?: boolean }

    // If 401 (Unauthorized) and has not retried yet
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true

      if (!isRefreshing) {
        isRefreshing = true
        try {
          const accessToken = localStorage.getItem('accessToken')
          const refreshToken = localStorage.getItem('refreshToken')

          // Call API refresh token
          const response = await axios.post<{ accessToken: string }>(
            `${apiBaseUrl}/auth/auth/refresh`,
            { accessToken, refreshToken },
            { withCredentials: true }
          )

          const newAccessToken = response.data.accessToken
          localStorage.setItem('accessToken', newAccessToken)

          isRefreshing = false
          onRefreshed(newAccessToken)
        } catch (err) {
          isRefreshing = false
          localStorage.removeItem('accessToken')
          localStorage.removeItem('refreshToken')
          window.location.href = '/login'
          return Promise.reject(err)
        }
      }

      return new Promise((resolve) => {
        addRefreshSubscriber((newToken) => {
          if (originalRequest.headers) {
            originalRequest.headers.Authorization = `Bearer ${newToken}`
          }
          resolve(privateApi(originalRequest))
        })
      })
    }

    return Promise.reject(error)
  }
)
export default privateApi
