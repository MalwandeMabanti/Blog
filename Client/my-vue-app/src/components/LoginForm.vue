<template>
    <div class="container">
        <header class="login-section">
            <h2>Login</h2>
            <form @submit.prevent="login">
                <label>
                    Email:
                    <input v-model="loginEmail" type="email" />
                </label>
                <label>
                    Password:
                    <input v-model="loginPassword" type="password" />
                </label>
                <button type="submit">Log in</button>
            </form>
            <a href="/register">Don't have an account? Register</a>
            <input v-model="searchTerm" type="text" placeholder="Search for a blog..." class="search-bar" />
            <!--<p v-html="highlightTerm(blog.title, searchTerm)"></p>-->

        </header>
        <main class="blogs-section">
            <div class="blog-item" v-for="blog in blogs" :key="blog.id">
                <h2 class="blog-title" @click="toggleDetails(blog)">{{ blog.title }}</h2>
                <div class="blog-details" v-if="blog.showDetails">
                    <img :src="blog.imageUrl" class="blog-image" />
                    <p class="blog-author">Author: {{ blog.author }}</p>
                    <p>{{ blog.description }}</p>
                </div>
            </div>
        </main>
    </div>
</template>


<script>
    import { ref, onMounted, watch } from 'vue';
    //import { useRouter } from 'vue-router';
    import UserService from '../services/UserService';
    import api from '../services/api';

    export default {
        name: "LoginForm",
        setup() {
            const loginEmail = ref('');
            const loginPassword = ref('');
            const searchTerm = ref('');
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

            const searchBlogs = (searchTerm) => {
                api.searchBlogs(searchTerm).then((response) => {
                    blogs.value = response.data.map((blog) => ({
                        ...blog,
                        showDetails: false
                    }));
                });
            };

            watch(searchTerm, (newVal, oldVal) => {
                if (newVal !== oldVal) {
                    searchBlogs(newVal);
                }
            });

            const toggleDetails = (blog) => {
                blog.showDetails = !blog.showDetails;
            };

            //const highlightTerm = (text, query) => {
            //    let check = new RegExp(query, "ig");
            //    return text.toString().replace(check, function (matchedText) {
            //        return '<mark>' + matchedText + '</mark>';
            //    });
            //};

            onMounted(getAllBlogs);

            return {
                loginEmail,
                loginPassword,
                login,
                searchTerm,
                getAllBlogs,
                searchBlogs,
                toggleDetails,
                blogs,
               

            };
        }
    };
</script>
<style>
    .container {
        width: 80%;
        margin: auto;
        font-family: Arial, sans-serif;
    }

    h2 {
        margin-bottom: 20px;
    }

    form {
        margin-bottom: 20px;
    }

        form label {
            display: block;
            margin-bottom: 10px;
        }

        form input {
            width: 100%;
            padding: 10px;
            margin-bottom: 20px;
            border: 1px solid #ced4da;
            border-radius: 4px;
        }

        form button {
            padding: 10px 20px;
            background-color: #007bff;
            color: #fff;
            border: none;
            border-radius: 4px;
            cursor: pointer;
        }

    a {
        display: inline-block;
        margin-bottom: 40px;
        color: #007bff;
        text-decoration: none;
    }

    .blog-list-item {
        border-bottom: 1px solid #dee2e6;
        padding: 20px 0;
    }

    .blog-title {
        cursor: pointer;
    }

    .blog-details {
        margin-top: 20px;
    }

    .blog-image {
        max-width: 100%;
        height: auto;
    }

    .search-bar {
        width: 100%;
        padding: 10px;
        margin-bottom: 20px;
        border: 1px solid #ced4da;
        border-radius: 4px;
    }

    .login-section {
        margin-bottom: 30px;
    }

    .blogs-section {
        margin-top: 30px;
    }

    header, main {
        width: 100%;
        display: block;
    }
</style>

