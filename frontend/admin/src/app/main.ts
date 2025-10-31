import 'bulma/css/bulma.min.css'
import '@/app/assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from '@/app/App.vue'
import router from '@/app/router'

const app = createApp(App)

app.use(createPinia())
app.use(router)

app.mount('#app')
