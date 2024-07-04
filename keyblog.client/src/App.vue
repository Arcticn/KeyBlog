<template class="dark">
  <BaseHeader />
  <RouterView />
  <el-backtop :right="150" :bottom="100" />
</template>

<script>
export default {
  name: "App",
};
</script>

<style>
#app {
  font-family:
    Inter,
    Helvetica Neue,
    Helvetica,
    PingFang SC,
    Hiragino Sans GB,
    Microsoft YaHei,
    微软雅黑,
    Arial,
    sans-serif;
  text-align: center;
  background-image: url("@/assets/Wallpaper/philia.jpg");
  background-size: cover;
  background-position: center;
  background-attachment: fixed;
  background-repeat: no-repeat;
  min-height: 100vh; /* 确保背景图片覆盖整个视口高度 */
}

body {
  transition:
    background-color 0.5s,
    color 0.5s;
  margin: 0px;
  overflow-y: scroll;
}
</style>
