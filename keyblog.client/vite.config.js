import { fileURLToPath, URL } from 'node:url';
import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';

const targetURL = 'http://localhost:5179';

export default defineConfig({
    plugins: [plugin()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        host: '0.0.0.0',
        port: 5173,
        https: false, // 禁用 HTTPS
        proxy: {
            '/api': {
                target: targetURL,
                changeOrigin: true
            }
        }
    },
    
});
