import { createWebHistory, createRouter } from 'vue-router'
import BlogList from '@/components/BlogList.vue'
import BlogList_Next from '@/components/BlogList.Next.vue'
import ArticlePage from '@/components/ArticlePage.vue'


const routes = [
    { path: '/', component: BlogList },
    { path: '/next', component: BlogList_Next },
    { path: '/article', component: ArticlePage },
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router

