
<template>
  <el-affix :offset="0">
    <el-menu
      :default-active="activeIndex"
      class="el-menu glass-effect"
      mode="horizontal"
    >
      <el-menu-item index="/" @click="pushUrl('/')"><el-icon><HomeFilled /></el-icon>主页</el-menu-item>
      <el-menu-item index="/editor" @click="pushUrl('/editor')"
        ><el-icon><EditPen /></el-icon>在线书写</el-menu-item
      >
      <el-menu-item index="/manage" @click="pushUrl('/manage')"
        >管理</el-menu-item
      >
      <el-menu-item index="/about" @click="pushUrl('/about')"
        >关于</el-menu-item
      >
      <el-menu-item h="full" class="menu-item-switch">
        <el-switch
          v-model="isDark"
          inline-prompt
          active-text="暗"
          inactive-text="亮"
        />

      </el-menu-item>
    </el-menu>
  </el-affix>
</template>

<script setup>
import { useRouter, useRoute } from "vue-router";
import { ref, watch } from "vue";
import { useDarkMode} from "@/composables/useDarkMode";
import "@/components/styles/glass.scss"

const route = useRoute();
const activeIndex = ref(route.path);

const { isDark } = useDarkMode();

// 获取路由实例并立即调用push方法
const router = useRouter();

const pushUrl = (url) => {
  router.push(url);
};

// 当参数更改时获取用户信息
watch(
  () => route.path,
  (newId) => {
    // console.log('路由变化了', newId)
    activeIndex.value = newId;
  }
);


</script>

<style scoped>

</style>