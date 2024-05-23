import { fileURLToPath, URL } from "node:url";
import { defineConfig } from "vite";

import vue from '@vitejs/plugin-vue';
import VueDevTools from 'vite-plugin-vue-devtools';
import AutoImport from "unplugin-auto-import/vite";
import Components from "unplugin-vue-components/vite";
import { ElementPlusResolver } from "unplugin-vue-components/resolvers";

const targetURL = "http://localhost:5179";

export default defineConfig({
    plugins: [
        vue(),
        VueDevTools(),
        AutoImport({
            resolvers: [ElementPlusResolver()],
        }),
        Components({
            resolvers: [ElementPlusResolver()],
        }),
    ],
    resolve: {
        alias: {
            "@": fileURLToPath(new URL("./src", import.meta.url)),
        },
    },
    server: {
        host: "0.0.0.0",
        port: 5173,
        https: false, // 禁用 HTTPS
        proxy: {
            "/api": {
                target: targetURL,
                changeOrigin: true,
            },
        },
    },
    // configureWebpack: {
    //     devtool: "source-map",
    // },
});
