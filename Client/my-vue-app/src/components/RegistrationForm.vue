<template>
    <div class="container">
        <section class="register-section">
            <h2>Register</h2>
            <form @submit.prevent="register">
                <label>
                    First Name:
                    <input v-model="firstName" type="text" placeholder="First Name" />
                </label>
                <label>
                    Last Name:
                    <input v-model="lastName" type="text" placeholder="Last Name" />
                </label>
                <label>
                    Email:
                    <input v-model="registerEmail" type="email" placeholder="Email" />
                </label>
                <label>
                    Password:
                    <input v-model="registerPassword" type="password" placeholder="Password" />
                </label>
                <label>
                    Confirm Password:
                    <input v-model="confirmPassword" type="password" placeholder="Confirm Password" />
                </label>
                <button type="submit">Register</button>
            </form>
            <a href="/">Already have an account? Login</a>
        </section>
    </div>
</template>


<script>
    import { ref } from 'vue';
    import UserService from '../services/UserService';

    export default {
        name: "RegistrationForm",
        setup() {
            const firstName = ref("");
            const lastName = ref("");
            const registerEmail = ref("");
            const registerPassword = ref("");
            const confirmPassword = ref("");

            const register = () => {
                if (registerPassword.value !== confirmPassword.value) {
                    alert("Passwords do not match!");
                    return;
                }

                UserService.register({
                    FirstName: firstName.value,
                    LastName: lastName.value,
                    Email: registerEmail.value,
                    Password: registerPassword.value,
                    ConfirmPassword: confirmPassword.value
                })
                    .then(response => {
                        console.log(response);
                    })
                    .catch(error => {
                        console.log(error.config, "Config");
                    });
            };

            return {
                firstName,
                lastName,
                registerEmail,
                registerPassword,
                confirmPassword,
                register
            };
        }
    };
</script>

<style>
    .container {
        display: flex;
        justify-content: center;
        align-items: center;
        height: 100vh;
        background: #f5f5f5;
    }

    .register-section {
        width: 500px;
        padding: 20px;
        background: #fff;
        border-radius: 8px;
        box-shadow: 0px 0px 10px 0px rgba(0,0,0,0.1);
    }

        .register-section h2 {
            margin-bottom: 20px;
            text-align: center;
        }

        .register-section form {
            display: flex;
            flex-direction: column;
        }

            .register-section form label {
                margin-bottom: 10px;
            }

            .register-section form input[type="text"],
            .register-section form input[type="email"],
            .register-section form input[type="password"] {
                padding: 10px;
                border-radius: 4px;
                border: 1px solid #ddd;
                outline: none;
            }

            .register-section form button {
                padding: 10px 0;
                margin-top: 20px;
                background: #007BFF;
                color: #fff;
                border: none;
                border-radius: 4px;
                cursor: pointer;
                font-size: 16px;
            }

                .register-section form button:hover {
                    background: #0056b3;
                }

        .register-section a {
            display: block;
            text-align: center;
            margin-top: 20px;
            color: #007BFF;
            text-decoration: none;
        }

</style>
