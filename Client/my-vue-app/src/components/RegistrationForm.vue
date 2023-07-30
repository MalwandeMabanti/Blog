<template>
    <div>
        <h2>Register</h2>
        <form @submit.prevent="register">
            <label>
                <input v-model="firstName" type="text" placeholder="First Name" />
            </label>
            <br />
            <label>
                <input v-model="lastName" type="text" placeholder="Last Name" />
            </label>
            <br />
            <label>
                <input v-model="registerEmail" type="email" placeholder="Email" />
            </label>
            <br />
            <label>
                <input v-model="registerPassword" type="password" placeholder="Password" />
            </label>
            <br />
            <label>
                <input v-model="confirmPassword" type="password" placeholder="Confirm Password" />
            </label>
            <br />
            <button type="submit">Register</button>
            <a href="/">Already have an account? Login</a>
        </form>
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
