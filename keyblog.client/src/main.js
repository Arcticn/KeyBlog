import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import ElementPlus from 'element-plus';
import 'element-plus/dist/index.css';
import 'element-plus/theme-chalk/dark/css-vars.css'
import "@/composables/markdown-it-config";
import * as ElementPlusIconsVue from '@element-plus/icons-vue'
import { createPinia } from 'pinia';


const app = createApp(App)
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
  app.component(key, component)
}

app.use(router);
app.use(ElementPlus);
app.use(createPinia());
app.mount('#app');

