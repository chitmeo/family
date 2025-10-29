<template>
  <div class="admin-layout">
    <nav>
      <ul>
        <li><router-link to="/">Home</router-link></li>
        <li><router-link to="/accounting/chartofaccount">Chart of Account</router-link></li>
        <!-- <li>
          <span>Quản lý</span>
          <ul>
            <li><router-link to="/home">Home</router-link></li>
            <li><router-link to="/about">About</router-link></li>
          </ul>
        </li> -->
        <li>
          <button @click="handleLogout" class="logout-btn">Logout</button>
        </li>
      </ul>
    </nav>
    <main>
      <router-view />
    </main>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/app/stores/auth';
import { useRouter } from 'vue-router';

const auth = useAuthStore();
const router = useRouter();

async function handleLogout() {
  try {
    await auth.logout();
    router.push('/login');
  } catch (error) {
    console.error('Logout error:', error);
  }
}
</script>

<style scoped>
nav {
  background: #333;
  color: #fff;
  padding: 10px;
}

nav ul {
  list-style: none;
  display: flex;
  gap: 20px;
}

nav ul ul {
  display: none;
  position: absolute;
  background: #444;
}

nav li:hover > ul {
  display: block;
}
</style>
