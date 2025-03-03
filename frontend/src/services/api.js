import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5000', // Ajusta la URL del backend
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor para incluir el token en cada petición
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('sessionToken'); 
    if (token) {
      config.headers['sessionToken'] = token; // Ahora el token se pasa con "sessionToken"
    }
    return config;
  },
  (error) => Promise.reject(error)
);

export default api;
