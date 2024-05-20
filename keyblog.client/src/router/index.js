import { createWebHistory, createRouter } from 'vue-router'
import BlogList from '@/components/BlogList.vue'
import BlogList_Next from '@/components/BlogList.Next.vue'
import Article from '@/components/Article.vue'


const routes = [
    { path: '/', component: BlogList },
    { path: '/next', component: BlogList_Next },
    {path:'/arcticle/:id', component: Article},
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router

