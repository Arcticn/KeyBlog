<template>
    <el-sub-menu v-if="category.children && category.children.length > 0" :index="category.id.toString()">
      <template #title>{{ category.label }}</template>
      <RecursiveMenu
        v-for="child in category.children"
        :key="child.id"
        :category="child"
        @node-click="handleNodeClick"
      />
    </el-sub-menu>
    <el-menu-item v-else :index="category.id.toString()" @click="handleNodeClick(category)">
      {{ category.label }}
    </el-menu-item>
  </template>
  
  <script setup>
  
  const props = defineProps({
    category: {
      type: Object,
      required: true,
    },
  })
  
  const emit = defineEmits(['node-click'])
  
  const handleNodeClick = (node) => {
    emit('node-click', node)
  }
  </script>
  
  <style scoped>
  /* 根据需要添加样式 */
  </style>
  