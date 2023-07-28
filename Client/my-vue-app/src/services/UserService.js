import axios from 'axios';

const API_URL = 'https://localhost:7142/api/Authentication';

function setAuthToken(token) {
    if (token) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + token;
    } else {
        delete axios.defaults.headers.common['Authorization'];
    }
}

const UserService = {
    register(userData) {
        return axios.post(`${API_URL}/Register`, userData);
    },

    login(userData) {
        return axios.post(`${API_URL}/Login`, userData)
            .then(response => {
                if (response.data.token) {
                    // Assuming the token is returned in the response data
                    setAuthToken(response.data.token);
                }
                return response;
            });
    },

    // Other user-related API calls can go here
};

export default UserService;