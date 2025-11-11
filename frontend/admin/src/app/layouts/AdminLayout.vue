<script setup lang="ts">
import { ref } from 'vue';
import { useAuthStore } from '@/app/stores/auth';
import { useRouter } from 'vue-router';

const auth = useAuthStore();
const router = useRouter();
const isActive = ref(false);

async function handleLogout() {
  try {
    await auth.logout();
    router.push('/login');
  } catch (error) {
    console.error('Logout error:', error);
  }
}
</script>

<template>
  <div class="admin-layout">
    <nav class="navbar is-dark" role="navigation" aria-label="main navigation">
      <div class="navbar-brand">
        <router-link class="navbar-item" to="/">
          <strong>Chitmeo</strong>
        </router-link>
        <!-- toggle button -->
        <a role="button" class="navbar-burger" :class="{ 'is-active': isActive }" aria-label="menu"
          aria-expanded="false" @click="isActive = !isActive">
          <span aria-hidden="true"></span>
          <span aria-hidden="true"></span>
          <span aria-hidden="true"></span>
        </a>
      </div>

      <div class="navbar-menu" :class="{ 'is-active': isActive }">
        <div class="navbar-start">
          <router-link class="navbar-item" to="/">Home</router-link>

          <!-- Dropdown Accounting -->
          <div class="navbar-item has-dropdown is-hoverable">
            <a class="navbar-link">Accounting</a>

            <div class="navbar-dropdown">
              <router-link class="navbar-item" to="/accounting/chartofaccounts">
                Chart of Accounts
              </router-link>
              <router-link class="navbar-item" to="/accounting/accounts">
                Accounts
              </router-link>
              <router-link class="navbar-item" to="/accounting/journalBooks">
                Journal Books
              </router-link>
              <router-link class="navbar-item" to="/accounting/journalTemplates">
                Journal Templates
              </router-link>
              <router-link class="navbar-item" to="/accounting/journalEntries">
                Journal Entries
              </router-link>
            </div>
          </div>
        </div>

        <div class="navbar-end">
          <div class="navbar-item has-dropdown is-hoverable">
            <a class="navbar-link">{{ auth.user?.username || 'User' }}</a>
            <div class="navbar-dropdown is-right">
              <router-link class="navbar-item" to="/profile">Profile</router-link>
              <router-link class="navbar-item" to="/settings">Settings</router-link>
              <hr class="navbar-divider" />
              <a href="#" class="navbar-item has-text-danger" @click.prevent="handleLogout">
                Logout
              </a>
            </div>
          </div>
        </div>
      </div>
    </nav>

    <main class="section">
      <router-view />
    </main>
  </div>
</template>