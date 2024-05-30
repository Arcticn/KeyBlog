import { createWebHistory, createRouter } from "vue-router";
import BlogList from "@/components/BlogList.vue";
import BlogPost from "@/components/BlogPost.vue";
import OnlineEditor from "@/components/OnlineEditor.vue";
import ManagementSystem from "@/components/ManagementSystem.vue";

const routes = [
  {
    path: "/",
    name: "主页",
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
    name: "管理系统",
    component: ManagementSystem,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
