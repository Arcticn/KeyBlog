import axios from 'axios';

const api = axios.create({
    baseURL: '/api', // 这里的路径与 Vite 配置中的代理路径匹配
});

// 请求拦截器，添加 JWT 令牌到请求头
api.interceptors.request.use(config => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
}, error => {
    return Promise.reject(error);
});

export default api;
