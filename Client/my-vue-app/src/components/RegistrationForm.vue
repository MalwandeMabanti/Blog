<template>
    <div class="container">
        <section class="register-section">
            <h2>Register</h2>
            <form @submit.prevent="register">

                <div v-if="registerSuccessMessage" class="success-message">
                    {{ registerSuccessMessage }}
                </div>

                <div v-if="existingUserError" class="error-messages">
                    {{ existingUserError }}
                </div>

                <p v-if="!isFirstNameValid" class="error-messages">First name is required.</p>
                <label>
                    First Name:
                    <input v-model="firstName" type="text" placeholder="First Name" />
                </label>

                <p v-if="!isLastNameValid" class="error-messages">Last name is required.</p>
                <label>
                    Last Name:
                    <input v-model="lastName" type="text" placeholder="Last Name" />
                </label>

                <p v-if="!isRegisterEmailValid" class="error-messages">Please enter a valid email.</p>
                <label>
                    Email:
                    <input v-model="registerEmail" type="email" placeholder="Email" />
                </label>

                <p v-if="!isRegisterPasswordValid" class="error-messages">Password should be at least 8 characters long.</p>
                <label>
                    Password:
                    <input v-model="registerPassword" type="password" placeholder="Password" />
                </label>

                <p v-if="!isConfirmPasswordValid" class="error-messages">Passwords do not match.</p>
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
    import { ref, computed } from 'vue';
    import UserService from '../services/UserService';

    export default {
        name: "RegistrationForm",
        setup() {
            const firstName = ref("");
            const lastName = ref("");
            const registerEmail = ref("");
            const registerPassword = ref("");
            const confirmPassword = ref("");
            const existingUserError = ref(null);
            const registerSuccessMessage = ref(null);
            //const loginErrors = reactive({ values: [] });

            const isFirstNameValid = computed(() => firstName.value.length > 0);
            const isLastNameValid = computed(() => lastName.value.length > 0);
            const isRegisterEmailValid = computed(() => {
              const emailRegex = /^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$/;
              return emailRegex.test(registerEmail.value);
            });
            const isRegisterPasswordValid = computed(() => registerPassword.value.length >= 8);
            const isConfirmPasswordValid = computed(() => confirmPassword.value === registerPassword.value);

            const isValidForm = computed(() => isFirstNameValid.value && isLastNameValid.value && isRegisterEmailValid.value && isRegisterPasswordValid.value && isConfirmPasswordValid.value);        

            const register = async () => {
                if (isValidForm.value) {
                    try {
                        const response = await UserService.register({
                            FirstName: firstName.value,
                            LastName: lastName.value,
                            Email: registerEmail.value,
                            Password: registerPassword.value,
                            ConfirmPassword: confirmPassword.value
                        });
                        registerSuccessMessage.value = response.data.message;
                    } catch (error) {
                        existingUserError.value = error.response.data
                    console.error(error);
                }
                }
              
            };




            return {
                firstName,
                lastName,
                registerEmail,
                registerPassword,
                confirmPassword,
                isFirstNameValid,
                isLastNameValid,
                isRegisterEmailValid,
                isRegisterPasswordValid,
                isConfirmPasswordValid,
                isValidForm,
                register,
                existingUserError,
                registerSuccessMessage
            };
        }
    }
</script>

<style scoped>

    .container {
        display: flex;
        justify-content: center;
        align-items: center;
        height: 100vh; 
        background: linear-gradient(to right, #12c2e9, #c471ed, #f64f59);
    }

    .register-section {
        display: flex;
        flex-direction: column;
        width: 400px; 
        padding: 20px;
        border: 1px solid #ced4da; 
        border-radius: 4px; 
        background: rgba(255, 255, 255, 0.8); 
        backdrop-filter: blur(5px); 
    }

        .register-section h2 {
            text-align: center; 
            margin-bottom: 20px;
        }

        .register-section form label {
            display: block;
            margin-top: 10px;
        }

        .register-section form input {
            width: 100%; 
            padding: 10px;
            border: 1px solid #ced4da;
            border-radius: 4px;
        }

        .register-section form button {
            display: block; 
            padding: 10px;
            background-color: #007bff;
            color: #fff;
            border: none;
            border-radius: 4px;
            cursor: pointer;
        }

        .register-section a {
            display: block; 
            margin-top: 20px;
            text-align: center; 
        }

        .error-messages {
            color: red;
            margin-bottom: 10px;
        }


        .success-message {
            color: green;
        }



</style>