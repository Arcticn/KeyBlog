import { createWebHistory, createRouter } from 'vue-router'
import BlogList from '@/components/BlogList.vue'
import ArticlePage from '@/components/ArticlePage.vue'
import OnlineEditor from '@/components/OnlineEditor.vue'

const routes = [
    {
        path: '/',
        name: 'Home',
        component: BlogList,
      },
      {
        path: '/article/:id',
        name: 'Article',
        component: ArticlePage,
        props: true,
      },
      {
        path:'/editor',
        name:'Editor',
        component:OnlineEditor,
        
      }
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router

