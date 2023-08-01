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

            console.log(isValidForm.value, " With value") 
            console.log(isValidForm, " Without value") 

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

        .error-messages {
            color: red;
            margin-bottom: 10px;
        }

        button[disabled] {
          background: grey;
          cursor: not-allowed;
        }

        .success-message {
            color: green;
        }

</style>
