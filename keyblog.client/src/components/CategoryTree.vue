<template>
  <div>
    <el-menu
      v-loading="categories.length === 0"
      :default-active="currentCategoryId.toString()"
      class="el-menu-sidebar glass-effect"
      mode="vertical"
      style="max-height: 31rem;min-height: 5rem;"
      :data="categories"
      :props="defaultProps"
      :collapse-transition="false"
    >
      <RecursiveMenu
        v-for="category in categories"
        :key="category.id"
        :category="category"
        @node-click="handleNodeClick"
      />
    </el-menu>
  </div>
</template>

<script setup>
import RecursiveMenu from "./RecursiveMenu.vue";
import "@/components/styles/glass.scss";

const props = defineProps({
  categories: {
    type: Array,
    required: true,
  },
  currentCategoryId: {
    type: Number,
    required: true,
  },
});

const emit = defineEmits(["node-click"]);

const handleNodeClick = (node) => {
  //toggleNodeExpansion(node)
  emit("node-click", node);
};
</script>

<style scoped>
.el-menu-sidebar {
  max-width: 16.7rem;
  max-height: 600px; /* 设置最大高度 */
  overflow-y: auto; /* 超出高度时添加滚动条 */
  border: 1px solid #dcdfe6;
  margin: 1rem;
  border-radius: 6px;
}
</style>
