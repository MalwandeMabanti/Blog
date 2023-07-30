<template>
    <div>
        <div>
            <h2>Login</h2>
            <form @submit.prevent="login">
                <label>
                    <input v-model="loginEmail" type="email" />
                </label>
                <br />
                <label>
                    <input v-model="loginPassword" type="password" />
                </label>
                <br />
                <button type="submit">Log in</button>
            </form>
            <a href="/register">Don't have an account? Register</a>
        </div>
        
        <div v-for="blog in blogs" :key="blog.id">

            <h2 @click="toggleDetails(blog)">{{ blog.title }}</h2>

            <div v-if="blog.showDetails">

                <img :src="blog.imageUrl" class="blog-image" />
                <p>{{ blog.description }}</p>

            </div>
        </div>
        
        

    </div>
</template>

<script>
    import { ref, onMounted } from 'vue';
    //import { useRouter } from 'vue-router';
    import UserService from '../services/UserService';
    import api from '../services/api';

    export default {
        name: "LoginForm",
        setup() {
            const loginEmail = ref('');
            const loginPassword = ref('');
            //const router = useRouter();
            const blogs = ref([]);

            const login = async () => {
                console.log('login function called');
                try {
                    const response = await UserService.login({
                        Email: loginEmail.value,
                        Password: loginPassword.value,
                    });



                    if (response.status === 200) {
                        localStorage.setItem('token', response.data.token);

                        window.location.href = '/blogpost';

                       
                        
                    }
                } catch (error) {
                    if (error.response) {
                        console.log(error.response.data, "Data");
                        console.log(error.response.status, "Status");
                        console.log(error.response.headers, "Headers");
                    } else if (error.request) {
                        console.log(error.request, "Request");
                    } else {
                        console.log('Error', error.message);
                    }
                }
            };


            const getAllBlogs = () => {
                api.getAllBlogs().then((response) => {
                    blogs.value = response.data.map((blog) => ({
                        ...blog,
                        showDetails: false
                    }));
                    console.log(blogs.value);
                });
            };

            const toggleDetails = (blog) => {
                blog.showDetails = !blog.showDetails;
            };

            onMounted(getAllBlogs);

            return {
                loginEmail,
                loginPassword,
                login,
                getAllBlogs,
                toggleDetails,
                blogs
            };
        }
    };
</script>

<style>

    .blog-image {
        width: 100px;
        height: 100px;
    }
</style>