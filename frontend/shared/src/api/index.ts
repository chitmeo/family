import axios from 'axios'

// Public API instance (no auth required)
export const publicApi = axios.create({
  baseURL: import.meta.env.VITE_PUBLIC_API || import.meta.env.VITE_API_BASE_URL,
  withCredentials: true,
})

// Private API instance (requires authentication)
export const privateApi = axios.create({
  baseURL: import.meta.env.VITE_PRIVATE_API || import.meta.env.VITE_API_BASE_URL,
  withCredentials: true,
})

// Attach Authorization header automatically if token exists
privateApi.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) {
    // Add Bearer token to request headers
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

// Optional: handle 401 responses globally
privateApi.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && error.response.status === 401) {
      console.warn('Unauthorized: redirecting to login...')
      // You could trigger logout or redirect here
    }
    return Promise.reject(error)
  }
)

export default privateApi
