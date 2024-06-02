import { createWebHistory, createRouter } from "vue-router";
import BlogList from "@/components/BlogList.vue";
import BlogPost from "@/components/BlogPost.vue";
import OnlineEditor from "@/components/OnlineEditor.vue";
import ManagementSystem from "@/components/ManagementSystem.vue";

const routes = [
  {
    path: "/",
    name: "Home",
    component: BlogList,
  },
  {
    path: "/p/:id",
    name: "Post",
    component: BlogPost,
    props: true,
  },
  {
    path: "/editor",
    name: "Editor",
    component: OnlineEditor,
  },
  {
    path: "/manage",
    name: "Management",
    component: ManagementSystem,
    meta: {
      requiresAuth: true,
      role: 'Admin'
    },
  },
];


const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from) => {
  const token = localStorage.getItem('token');
  const currentUser = JSON.parse(localStorage.getItem('currentUser'));
  const userRole = currentUser ? (currentUser.isAdmin ? 'Admin' : 'User') : null;

  if (to.matched.some(record => record.meta.requiresAuth)) {
    if (!token) {
      return false // 使用 return 进行重定向
    } else {
      const role = to.meta.role;
      if (userRole && (userRole === role)) {
        return true; // 允许导航
      } else {
        return false // 使用 return 进行重定向
      }
    }
  } else {
    return true; // 如果目标路由不需要身份验证，直接允许导航
  }
});


export default router;
