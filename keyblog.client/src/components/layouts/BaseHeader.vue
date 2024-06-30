<template>
  <el-affix :offset="0">
    <el-menu
      :default-active="activeIndex"
      style=""
      class="el-menu glass-effect"
      mode="horizontal"
    >
      <el-menu-item index="/" @click="pushUrl('/')"
        ><el-icon><HomeFilled /></el-icon>主页</el-menu-item
      >
      <el-menu-item index="/editor" @click="pushUrl('/editor')"
        ><el-icon><EditPen /></el-icon>在线书写</el-menu-item
      >
      <el-menu-item v-if="isAdmin" index="/manage" @click="pushUrl('/manage')"
        >管理</el-menu-item
      >
      <!-- <el-menu-item index="/about" @click="pushUrl('/about')"
        >关于</el-menu-item
      > -->
      <el-menu-item h="full" class="menu-item-switch">
        <el-switch
          v-model="isDark"
          inline-prompt
          active-text="暗"
          inactive-text="亮"
        />
      </el-menu-item>
      <el-menu-item h="full">
        文章主题：
        <el-select
          v-model="previewTheme"
          @change="saveTheme"
          placeholder="Select"
          style="width: 120px"
        >
          <el-option
            v-for="item in previewThemeOptions"
            :key="item"
            :value="item"
          />
        </el-select>
      </el-menu-item>
      <el-menu-item h="full">
        代码主题：
        <el-select
          v-model="codeTheme"
          @change="saveTheme"
          placeholder="Select"
          style="width: 120px"
        >
          <el-option
            v-for="item in codeThemeOptions"
            :key="item"
            :value="item"
          />
        </el-select>
      </el-menu-item>
    </el-menu>
  </el-affix>
</template>

<script setup>
import { useRouter, useRoute } from "vue-router";
import { onMounted, ref, watch } from "vue";
import { codeTheme, previewTheme } from "@/composables/theme";
import { useDarkMode } from "@/composables/useDarkMode";
import "@/components/styles/glass.scss";

const route = useRoute();
const activeIndex = ref(route.path);

const { isDark } = useDarkMode();

const previewThemeOptions = [
  "default",
  "github",
  "vuepress",
  "mk-cute",
  "smart-blue",
  "cyanosis",
  "arknights",
];
const codeThemeOptions = [
  "atom",
  "a11y",
  "github",
  "gradient",
  "kimbie",
  "paraiso",
  "qtcreator",
  "stackoverflow",
];

// 获取路由实例并立即调用push方法
const router = useRouter();

const pushUrl = (url) => {
  router.push(url);
};

const saveTheme = () => {
  localStorage.setItem("codeTheme", codeTheme.value);
  localStorage.setItem("previewTheme", previewTheme.value);
};

// 当参数更改时获取用户信息
watch(
  () => route.path,
  (newId) => {
    // console.log('路由变化了', newId)
    activeIndex.value = newId;
  }
);

const isAdmin = ref(false);

onMounted(() => {
  const currentUser = JSON.parse(localStorage.getItem("currentUser"));
  if (currentUser && currentUser.isAdmin === true) {
    isAdmin.value = true;
  } else {
    isAdmin.value = false;
  }
});
</script>

<style scoped></style>
