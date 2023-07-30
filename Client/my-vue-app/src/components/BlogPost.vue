<template>
    <div>
        <h2>Add a ToDo</h2>
        <button @click="logout">Logout</button>
        <div>

            <form @submit.prevent="addTodo">

                <input class="title-input" v-model="newTodo.title" type="text" placeholder="New Todo Title" /><br />
                <br />
                <textarea class="description-input" v-model="newTodo.description" placeholder="New Todo Description"></textarea><br />
                <label for="image">Select image:</label>
                <input id="image" type="file" @change="onFileChange" />
                <br />
                <button type="submit">Add Todo</button>
            </form>


            <div v-for="todo in todos" :key="todo.id">

                <h2 @click="toggleDetails(todo)">{{ todo.title }}</h2>

                <div v-if="todo.showDetails">


                    <input v-if="todo.isEditing" v-model="todo.title" type="text" />

                    <img v-show="!todo.isEditing" :src="todo.imageUrl" class="todo-image" />
                    <input v-show="todo.isEditing" type="file" @change="onEditFileChange(todo, $event)" />

                    <p>{{ todo.isEditing ? '' : todo.description }}</p>
                    <textarea v-if="todo.isEditing" v-model="todo.description"></textarea>

                    <input type="checkbox" @change="removeTodo(todo)" />

                    <button @click="todo.isEditing ? updateTodo(todo) : editTodo(todo)">
                        {{todo.isEditing ? 'Save' : 'Edit'}}
                    </button>



                </div>
            </div>

        </div>
    </div>
</template>

<script>
    import { ref, reactive, onMounted } from 'vue';
    //import { useRouter } from 'vue-router';
    import api from '../services/api';

    export default {
        setup() {
            const todos = ref([]);
            const newTodo = reactive({
                title: '',
                description: '',
                file: null

            });


            //const router = useRouter();

            const toggleDetails = (todo) => {
                todo.showDetails = !todo.showDetails;
            };

            const addTodo = () => {

                const formData = new FormData();
                formData.append('title', newTodo.title);
                formData.append('description', newTodo.description);
                if (newTodo.file) {
                    formData.append('image', newTodo.file);
                }

                api.createTodo(formData).then((response) => {
                    todos.value.push(response.data);
                    newTodo.title = '';
                    newTodo.description = '';
                    newTodo.file = null;
                });
            };

            const onFileChange = (e) => {
                console.log(e.target.files[0], "An image was uploaded");

                newTodo.file = e.target.files[0];
            }

            const getTodos = () => {
                api.getTodos().then((response) => {
                    todos.value = response.data.map((todo) => ({
                        ...todo,
                        isEditing: false,
                        editingText: todo.title,
                        imageUrl: todo.imageUrl,
                        showDetails: false,
                        file: null,
                        newImage: null

                    }));
                });
            };

            const editTodo = (todo) => {
                todo.isEditing = true;
                todo.file = null;
            };


            const onEditFileChange = (todo, e) => {

                let file = e.target.files[0];
                todo.newImage = file;
                newTodo.file = true;
        };

        const updateTodo = (todo) => {
           


            let formData = new FormData();
            formData.append('id', todo.id);

            console.log(formData.get('id'), " Your Id");

            formData.append('title', todo.title);

            console.log(formData.get('title'), " Your Title");

            formData.append('description', todo.description);

            console.log(formData.get('description'), " Your Description");

            if (newTodo.file) {
                formData.append('image', todo.newImage);
                console.log(formData.get('image') , " Your Image");
                }
            else {
                formData.append('image', "null");
                console.log(formData.get('image') , " Your Image");
                }


        


            api.updateTodo(formData).then((response) => {
                const index = todos.value.findIndex((t) => t.id === response.data.id);
                if (todos.value[index]) {
                    todos.value.splice(index, 1, response.data);
                    todos.value[index].isEditing = false;
                    todos.value[index].editingText = '';
                }
                if (response.data.imageUrl) {
                    todo.imageUrl = response.data.imageUrl;
                    todo.newImage = null;
                }
            });
    };





            const removeTodo = (todo) => {
                api.removeTodo(todo).then(() => {
                    todos.value = todos.value.filter((t) => t.id !== todo.id);
                });
        };

            const logout = () => {
                console.log("Token being removed is:", localStorage.getItem('token'));
                localStorage.removeItem('token');
                window.location.href = '/';
            }

            onMounted(getTodos);

            return {
                todos,
                newTodo,
                addTodo,
                onFileChange,
                getTodos,
                editTodo,
                updateTodo,
                removeTodo,
                logout,
                onEditFileChange,
                toggleDetails,
            };
        },
    };

</script>
<style scoped>
    th {
        padding: 40px;
    }

    td {
        text-align: center;
    }

    .description-input {
        width: 300px;
        height: 50px;
        padding: 10px;
    }

    .description-input {
        resize: none;
        overflow: hidden;
        min-height: 50px;
        max-height: 200px;
    }

    .title-input {
        width: 315px;
        height: 20px;
    }

    tbody tr:nth-child(2n) {
        background-color: white;
    }

    tbody tr:nth-child(2n+1) {
        background-color: lightgrey;
    }

    .completed-column {
        width: 20px;
    }

    .todo-image {
        width: 100px;
        height: 100px;
    }
</style>