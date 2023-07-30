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
        
        <div v-for="todo in todos" :key="todo.id">

            <h2 @click="toggleDetails(todo)">{{ todo.title }}</h2>

            <div v-if="todo.showDetails">

                <img :src="todo.imageUrl" class="todo-image" />
                <p>{{ todo.description }}</p>

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
            const todos = ref([]);

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


            const getAllTodos = () => {
                api.getAllTodos().then((response) => {
                    todos.value = response.data.map((todo) => ({
                        ...todo,
                        showDetails: false
                    }));
                    console.log(todos.value);
                });
            };

            const toggleDetails = (todo) => {
                todo.showDetails = !todo.showDetails;
            };

            onMounted(getAllTodos);

            return {
                loginEmail,
                loginPassword,
                login,
                getAllTodos,
                toggleDetails,
                todos
            };
        }
    };
</script>

<style>

    .todo-image {
        width: 100px;
        height: 100px;
    }
</style>