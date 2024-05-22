<template>
  <div>
    <el-menu
      :default-active="currentCategoryId.toString()"
      class="el-menu-sidebar"
      mode="vertical"
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
import RecursiveMenu from './RecursiveMenu.vue';

const props = defineProps({
  categories: {
    type: Array,
    required: true,
  },
  expandedKeys: {
    type: Array,
    required: true,
  },
  currentCategoryId: {
    type: Number,
    required: true,
  },
})

const emit = defineEmits(['node-click', 'add-key', 'remove-key'])

// const toggleNodeExpansion = (node) => {
//   const index = props.expandedKeys.indexOf(node.id)
//   if (index > -1) {
//     emit('remove-key', index)
//     removeChildKeys(node.children)
//   } else {
//     emit('add-key', node.id)
//   }
// }

// const removeChildKeys = (children) => {
//   if (!children) return
//   children.forEach((child) => {
//     const index = props.expandedKeys.indexOf(child.id)
//     if (index > -1) {
//       emit('remove-key', index)
//     }
//     removeChildKeys(child.children)
//   })
// }

const handleNodeClick = (node) => {
  //toggleNodeExpansion(node)
  emit('node-click', node)
}
</script>

<style scoped>
.el-menu-sidebar {
  width: 300px;
}
</style>
