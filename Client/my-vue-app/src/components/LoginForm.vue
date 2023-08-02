<template>
    <div class="container">
        <header class="login-section">
            <h2>Login</h2>
            <form @submit.prevent="login">

                <div v-if="unuthorizedUserError" class="error-messages">
                    {{ unuthorizedUserError }}
                </div>

                <p v-if="!isLoginEmailValid" class="error-messages">Email is required.</p>
                <label>
                    Email:
                    <input v-model="loginEmail" type="email" />
                </label>

                <p v-if="!isLoginPasswordValid" class="error-messages">First name is required.</p>
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
    import { ref, onMounted, watch, computed } from 'vue';
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
            const unuthorizedUserError = ref(null);

            const isLoginEmailValid = computed(() => loginEmail.value.length > 0);
            const isLoginPasswordValid = computed(() => loginPassword.value.length > 0);

            const isValidForm = computed(() => isLoginEmailValid.value && isLoginPasswordValid.value);

            const login = async () => {
                if (isValidForm.value) {


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
                        unuthorizedUserError.value = error.response.data
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
                isLoginEmailValid,
                isLoginPasswordValid,
                unuthorizedUserError


            };
        }
    };
</script>

<style scoped>
    .container {
        display: flex;
        flex-direction: row; /* Arrange children horizontally */
        width: 100%;
        align-items: center;
        height: 100vh;
        background: linear-gradient(to right, #12c2e9, #c471ed, #f64f59);
    }

    .login-section {
        display: flex;
        flex-direction: column; /* Arrange children vertically */
        align-items: start; /* Align items to the start of the parent container */
        width: 30%; /* Adjust the width as per your needs */
        padding: 20px;
    }

        .login-section form {
            width: 100%;
            margin-bottom: 20px;
        }

            .login-section form label {
                display: block;
                margin-bottom: 10px;
            }

            .login-section form input {
                width: 100%;
                padding: 10px;
                margin-bottom: 20px;
                border: 1px solid #ced4da;
                border-radius: 4px;
            }

            .login-section form button {
                padding: 10px 20px;
                background-color: #007bff;
                color: #fff;
                border: none;
                border-radius: 4px;
                cursor: pointer;
            }

        .login-section a {
            margin-top: 20px; /* Optional margin for better spacing */
        }

    .search-bar {
        width: 100%;
        padding: 10px;
        margin-top: 10px;
        border: 1px solid #ced4da;
        border-radius: 4px;
    }

    .blogs-section {
        flex: 1; /* Take up the remaining width of the parent container */
        padding: 20px; /* Optional padding for visual separation */
    }

    /* Additional styling for your elements (if needed) */
    .error-messages {
        color: red;
    }

    .blog-item {
        margin-bottom: 20px;
    }

    .blog-title {
        cursor: pointer;
    }

    .blog-image {
        max-width: 100%;
        height: auto;
    }

    .blog-author {
        font-weight: bold;
    }
</style>


