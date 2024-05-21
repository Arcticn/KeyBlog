<template>
  <el-menu :default-active="activeIndex" class="el-menu-demo" mode="horizontal">
    <el-menu-item index="/" @click="pushUrl('/')">主页</el-menu-item>
    <el-menu-item index="/About" @click="pushUrl('/About')">关于</el-menu-item>
    <el-menu-item h="full">
        <el-switch :value="isDark" v-model="value" @click="toggleDark()"></el-switch>
    </el-menu-item>
  </el-menu>
</template>

<script setup>
import { useRouter, useRoute } from "vue-router";
import { ref, watch } from "vue";
import { isDark, toggleDark } from "../composables/useDarkMode";

const route = useRoute();
const activeIndex = ref("/");

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
