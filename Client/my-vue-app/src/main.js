import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import App from './App.vue'
import BlogPost from './components/BlogPost.vue';
import LoginForm from './components/LoginForm.vue'
import RegistrationForm from './components/RegistrationForm.vue'


const routes = [
    {
        path: '/',
        name: 'login',
        component: LoginForm
    },
    {
        path: '/register',
        name: 'register',
        component: RegistrationForm
    },
    {
        path: '/blogpost',
        name: 'blogpost',
        component: BlogPost
    },
    
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

createApp(App)
    .use(router)
    .mount('#app')
