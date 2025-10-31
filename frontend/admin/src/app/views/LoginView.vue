<script setup lang="ts">
import { ref } from 'vue'
import { api } from '@chitmeo/shared'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/app/stores/auth'
const errors = ref<{ email?: string; password?: string; loginResult?: string }>({})

const email = ref('')
const password = ref('')

const router = useRouter()
const authStore = useAuthStore()

function validate() {
    errors.value = {}

    if (!email.value) {
        errors.value.email = 'Email is required'
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.value)) {
        errors.value.email = 'Invalid email format'
    }

    if (!password.value) {
        errors.value.password = 'Password is required'
    } else if (password.value.length < 6) {
        errors.value.password = 'Password must be at least 6 characters'
    }

    return Object.keys(errors.value).length === 0
}

const handleLogin = async () => {
    if (!validate()) return

    try {
        const res = await api.publicApi.post('/auth/auth/login', {
            userName: email.value,
            password: password.value
        })
        authStore.setAuth({
            accessToken: res.data.accessToken,
            refreshToken: res.data.refreshToken,
            user: res.data.user,
        })
        router.push('/')
    } catch (error: any) {
        if (error.response) {
            const data = error.response.data
            if (data?.detail) {
                errors.value.loginResult = data.detail
            } else if (data?.title) {
                errors.value.loginResult = data.title
            } else {
                errors.value.loginResult = 'Login failed. Please try again.'
            }
        } else {
            errors.value.loginResult = 'Cannot connect to server. Please check your network.'
        }
    }
}
</script>

<template>
    <section class="section">
        <div class="container">
            <div class="columns is-centered">
                <div class="column is-4">
                    <div class="box">
                        <h1 class="title has-text-centered">Login</h1>
                        <form @submit.prevent="handleLogin">
                            <div class="field">
                                <label class="label">Email</label>
                                <div class="control">
                                    <input class="input" type="email" v-model="email"
                                        placeholder="e.g. alex@example.com" />
                                </div>
                                <p v-if="errors.email" class="help is-danger">{{ errors.email }}</p>
                            </div>
                            <div class="field">
                                <label class="label">Password</label>
                                <div class="control">
                                    <input class="input" type="password" v-model="password" placeholder="********" />
                                </div>
                                <p v-if="errors.password" class="help is-danger">{{ errors.password }}</p>
                                <p v-if="errors.loginResult" class="help is-danger">{{ errors.loginResult }}</p>

                            </div>
                            <div class="field">
                                <div class="control">
                                    <button class="button is-primary is-fullwidth" type="submit">Login</button>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>
