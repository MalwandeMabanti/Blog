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
    getTodos() {
        return apiClient.get('/Blogs');
    },

    createTodo(formData) {
        return apiClient.post('/Blogs', formData);
    },

    updateTodo(formData) {

        for (let pair of formData.entries()) {
            console.log(pair[0] + ', ' + pair[1]);
        }

        let some = formData.get("id");

        console.log(`/Blogs/${some}`);

        return apiClient.put(`/Blogs/${formData.get("id")}`, formData);
    },

    removeTodo(todo) {
        return apiClient.delete(`/Todos/${todo.id}`);
    }
};
