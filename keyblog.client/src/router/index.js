import { createWebHistory, createRouter } from 'vue-router'
import BlogList from '@/components/BlogList.vue'
import BlogPost from '@/components/BlogPost.vue'
import OnlineEditor from '@/components/OnlineEditor.vue'

const routes = [
    {
        path: '/',
        name: 'Home',
        component: BlogList,
      },
      {
        path: '/post/:id',
        name: 'Post',
        component: BlogPost,
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

