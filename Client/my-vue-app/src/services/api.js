import axios from 'axios';

const apiClient = axios.create({
    baseURL: 'https://localhost:7010/api',
    withCredentials: false,
    headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
    },
});

apiClient.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    config.headers['Authorization'] = `Bearer ${token}`;

    if (config.data instanceof FormData) {
        delete config.headers['Content-Type'];
    } else {
        config.headers['Content-Type'] = 'application/json';
        config.data = JSON.stringify(config.data);
    }

    return config;
});

export default {

    getAllBlogs() {
        return apiClient.get('/Blogs/all')
    },

    getBlogs() {
        return apiClient.get('/Blogs/myblogs');
    },

    createBlog(formData) {
        return apiClient.post('/Blogs', formData);
    },

    updateBlog(formData) {

        for (let pair of formData.entries()) {
            console.log(pair[0] + ', ' + pair[1]);
        }

        return apiClient.put(`/Blogs/${formData.get("id")}`, formData);
    },

    removeBlog(blog) {
        return apiClient.delete(`/Blogs/${blog.id}`);
    },

    searchBlogs(term) {
        return apiClient.get(`/Blogs/search?term=${term}`);
    }
};
