<template>
    <div>
        <h2>Add a Blog</h2>
        <button @click="logout">Logout</button>
        <div>

            <form @submit.prevent="addBlog">

                <input class="title-input" v-model="newBlog.title" type="text" placeholder="New Blog Title" /><br />
                <br />
                <textarea class="description-input" v-model="newBlog.description" placeholder="New Blog Body"></textarea><br />
                <label for="image">Select image:</label>
                <input id="image" type="file" @change="onFileChange" />
                <br />
                <button type="submit">Add Blog</button>
            </form>


            <div v-for="blog in blogs" :key="blog.id">

                <h2 @click="toggleDetails(blog)">{{ blog.title }}</h2>

                <div v-if="blog.showDetails">


                    <input v-if="blog.isEditing" v-model="blog.title" type="text" />

                    <img v-show="!blog.isEditing" :src="blog.imageUrl" class="blog-image" />
                    <input v-show="blog.isEditing" type="file" @change="onEditFileChange(blog, $event)" />

                    <p>{{ blog.isEditing ? '' : blog.description }}</p>
                    <textarea v-if="blog.isEditing" v-model="blog.description"></textarea>

                    <button @click="removeBlog(blog)">Remove Blog</button>

                    <button @click="blog.isEditing ? updateBlog(blog) : editBlog(blog)">
                        {{blog.isEditing ? 'Save' : 'Edit'}}
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

    .blog-image {
        width: 100px;
        height: 100px;
    }
</style>