<template>
    <div class="container">
        <header>

            <div class="user-info">
                <span class="username">Welcome {{name}}!</span>
                <button class="logout-button" @click="logout">Logout</button>
            </div>

        </header>

        <div class="content">


            <section class="section-left">
                <h2>Add a Blog</h2>
                <form class="blog-form" @submit.prevent="addBlog">


                    <ul v-if="errors.addBlogError.length"  class="error-text">
                        <li v-for="(error, index) in errors.addBlogError" :key="index">{{ error }}</li>
                    </ul>
                    <label>
                        Title:
                        <input class="title-input" v-model="newBlog.title" type="text" placeholder="New Blog Title" />
                    </label>

                    <label>
                        Body:
                        <textarea class="description-input" v-model="newBlog.description" placeholder="New Blog Body"></textarea>
                    </label>

                    <label for="image">Select image:</label>
                    <input id="image" type="file" @change="onFileChange" />

                    <button type="submit" class="submit-button">Add Blog</button>
                </form>
            </section>

            <section class="section-right">
                <div class="blogs-list" v-for="blog in blogs" :key="blog.id">
                    <h2 class="blog-title" @click="toggleDetails(blog)">{{ blog.title }}</h2>
                    <div class="blog-details" v-if="blog.showDetails">
                        <input v-if="blog.isEditing" v-model="blog.title" type="text" />
                        <img v-show="!blog.isEditing" :src="blog.imageUrl" class="blog-image" />
                        <input v-show="blog.isEditing" type="file" @change="onEditFileChange(blog, $event)" />
                        <p>{{ blog.isEditing ? '' : blog.description }}</p>
                        <textarea v-if="blog.isEditing" v-model="blog.description"></textarea>
                        <div class="blog-actions">



                            <ul v-if="errors.updateBlogError[blog.id] && errors.updateBlogError[blog.id].length">
                                <li v-for="(error, index) in errors.updateBlogError[blog.id]" :key="index">{{ error }}</li>
                            </ul>




                            <button @click="removeBlog(blog)">Remove Blog</button>
                            <button @click="blog.isEditing ? updateBlog(blog) : editBlog(blog)">
                                {{blog.isEditing ? 'Save' : 'Edit'}}
                            </button>
                        </div>
                    </div>
                </div>
            </section>
        </div>


    </div>
</template>


<script>
    import { ref, reactive, onMounted } from 'vue';
    //import { useRouter } from 'vue-router';
    import api from '../services/api';

    export default {
        setup() {
            const blogs = ref([]);

            const name = ref('');

            const newBlog = reactive({
                title: '',
                description: '',
                file: null
            });



            const errors = reactive({
                addBlogError: [],
                updateBlogError: {}
            });

            const toggleDetails = (blog) => {
                blog.showDetails = !blog.showDetails;
            };

            const addBlog = () => {

                const formData = new FormData();
                formData.append('title', newBlog.title);
                formData.append('description', newBlog.description);
                if (newBlog.file) {
                    formData.append('image', newBlog.file);
                }

                api.createBlog(formData).then((response) => {
                    blogs.value.push(response.data);
                    newBlog.title = '';
                    newBlog.description = '';
                    newBlog.file = null;
                }).catch(error => {
                    if (error.response && error.response.status === 400) {
                        errors.addBlogError = error.response.data;
                    }
                });
            };

            const onFileChange = (e) => {
                newBlog.file = e.target.files[0];
            }

            const getBlogs = () => {
                api.getBlogs().then((response) => {
                    name.value = response.data.name;
                    blogs.value = response.data.blogs.map((blog) => ({
                        ...blog,
                        isEditing: false,
                        editingText: blog.title,
                        imageUrl: blog.imageUrl,
                        showDetails: false,
                        file: null,
                        newImage: null

                    }));
                });
            };

            const editBlog = (blog) => {
                blog.isEditing = true;
                blog.file = null;
            };


            const onEditFileChange = (blog, e) => {

                let file = e.target.files[0];
                blog.newImage = file;
                newBlog.file = true;
            };

            const updateBlog = (blog) => {

                let formData = new FormData();
                formData.append('id', blog.id);

                formData.append('title', blog.title);

                formData.append('description', blog.description);

                if (newBlog.file) {
                    formData.append('image', blog.newImage);
                }
                else {
                    formData.append('image', "null");
                }

                api.updateBlog(formData).then((response) => {
                    const index = blogs.value.findIndex((t) => t.id === response.data.id);
                    if (blogs.value[index]) {
                        blogs.value.splice(index, 1, response.data);
                        blogs.value[index].isEditing = false;
                        blogs.value[index].editingText = '';
                    }
                    if (response.data.imageUrl) {
                        blog.imageUrl = response.data.imageUrl;
                        blog.newImage = null;
                    }
                }).catch(error => {
                    if (error.response && error.response.status === 400) {
                        errors.updateBlogError[blog.id] = error.response.data;
                    }
                });
            };





            const removeBlog = (blog) => {
                api.removeBlog(blog).then(() => {
                    blogs.value = blogs.value.filter((t) => t.id !== blog.id);
                });
            };

            const logout = () => {
                localStorage.removeItem('token');
                window.location.href = '/';
            }

            onMounted(getBlogs);

            return {
                blogs,
                newBlog,
                addBlog,
                onFileChange,
                getBlogs,
                editBlog,
                updateBlog,
                removeBlog,
                logout,
                onEditFileChange,
                toggleDetails,
                errors,
                name
            };
        },
    };

</script>

<style scoped>
    .container {
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 100%;
        margin: auto;
        font-family: Arial, sans-serif;
        background: linear-gradient(to right, #12c2e9, #c471ed, #f64f59);
        height: 100vh;
    }

    header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        width: 100%;
        padding: 5px;
        background-color: #f8f9fa;
        border-bottom: 1px solid #dee2e6;
    }

    .content {
        display: flex;
        width: 100%;
    }

    .section-left, .section-right {
        flex: 1;
        padding: 10px;
    }

    .user-info {
        display: flex;
        align-items: center;
        gap: 1rem;
        justify-content: flex-end; 
    }

    .username {
        font-size: 1.2rem;
        font-weight: 600;
        color: #3a3a3a;
        padding: 0.5rem;
        border-radius: 5px;
        background-color: #f5f5f5;
    }

    h2 {
        margin: 0;
        font-size: 20px;
    }

    .logout-button {
        padding: 10px 20px;
        background-color: #007bff;
        color: #fff;
        border: none;
        border-radius: 4px;
        cursor: pointer;
    }

    .blog-form {
        padding: 20px;
        margin-bottom: 20px;
        width: 200px;
    }

        .blog-form label {
            display: block;
            margin-bottom: 10px;
        }

        .blog-form input,
        .blog-form textarea {
            width: 100%;
            padding: 10px;
            margin-bottom: 20px;
            border: 1px solid #ced4da;
            border-radius: 4px;
        }

    .blogs-list {
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

    .blog-actions {
        margin-top: 20px;
    }

        .blog-actions button {
            padding: 10px 20px;
            margin-right: 10px;
            background-color: #007bff;
            color: #fff;
            border: none;
            border-radius: 4px;
            cursor: pointer;
        }


    .description-input {
        height: 100px; 
        width: 100%;
    }

   
    .submit-button {
        padding: 10px 20px;
        background-color: #007bff;
        color: #fff;
        border: none;
        border-radius: 4px;
        cursor: pointer;
    }

    .error-text {
        color: red;
    }

</style>

