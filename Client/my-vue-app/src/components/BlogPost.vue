<template>
    <div class="container">
        <header>
            <h2>Add a Blog</h2>
            <button class="logout-button" @click="logout">Logout</button>
        </header>

        <section>
            <form class="blog-form" @submit.prevent="addBlog">
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

                <button type="submit">Add Blog</button>
            </form>
        </section>

        <section>
            <div class="blogs-list" v-for="blog in blogs" :key="blog.id">
                <h2 class="blog-title" @click="toggleDetails(blog)">{{ blog.title }}</h2>
                <div class="blog-details" v-if="blog.showDetails">
                    <input v-if="blog.isEditing" v-model="blog.title" type="text" />
                    <img v-show="!blog.isEditing" :src="blog.imageUrl" class="blog-image" />
                    <input v-show="blog.isEditing" type="file" @change="onEditFileChange(blog, $event)" />
                    <p>{{ blog.isEditing ? '' : blog.description }}</p>
                    <textarea v-if="blog.isEditing" v-model="blog.description"></textarea>
                    <div class="blog-actions">
                        <button @click="removeBlog(blog)">Remove Blog</button>
                        <button @click="blog.isEditing ? updateBlog(blog) : editBlog(blog)">
                            {{blog.isEditing ? 'Save' : 'Edit'}}
                        </button>
                    </div>
                </div>
            </div>
        </section>
    </div>
</template>


<script>
    import { ref, reactive, onMounted } from 'vue';
    //import { useRouter } from 'vue-router';
    import api from '../services/api';

    export default {
        setup() {
            const blogs = ref([]);
            const newBlog = reactive({
                title: '',
                description: '',
                file: null

            });


            //const router = useRouter();

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
                });
            };

            const onFileChange = (e) => {
                console.log(e.target.files[0], "An image was uploaded");

                newBlog.file = e.target.files[0];
            }

            const getBlogs = () => {
                api.getBlogs().then((response) => {
                    blogs.value = response.data.map((blog) => ({
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

            console.log(formData.get('id'), " Your Id");

            formData.append('title', blog.title);

            console.log(formData.get('title'), " Your Title");

            formData.append('description', blog.description);

            console.log(formData.get('description'), " Your Description");

            if (newBlog.file) {
                formData.append('image', blog.newImage);
                console.log(formData.get('image') , " Your Image");
                }
            else {
                formData.append('image', "null");
                console.log(formData.get('image') , " Your Image");
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
            });
    };





            const removeBlog = (blog) => {
                api.removeBlog(blog).then(() => {
                    blogs.value = blogs.value.filter((t) => t.id !== blog.id);
                });
        };

            const logout = () => {
                console.log("Token being removed is:", localStorage.getItem('token'));
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
            };
        },
    };

</script>
<style>

    .container {
        width: 80%;
        margin: auto;
        font-family: Arial, sans-serif;
    }

    header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 20px;
        background-color: #f8f9fa;
        border-bottom: 1px solid #dee2e6;
    }

    h2 {
        margin: 0;
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


</style>