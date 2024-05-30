<template>
  <el-sub-menu
    v-if="category.children && category.children.length > 0"
    :index="category.id.toString()"
    class="glass-effect"
  >
    <template #title>{{ category.name }}</template>
    <RecursiveMenu
      v-for="child in category.children"
      :key="child.id"
      :category="child"
      @node-click="handleNodeClick"
    />
  </el-sub-menu>
  <el-menu-item
    v-else
    :index="category.id.toString()"
    class="glass-effect"
    @click="handleNodeClick(category)"
  >
    {{ category.name }}
  </el-menu-item>
</template>

<script setup>
//import "@/components/styles/glass.scss";

const props = defineProps({
  category: {
    type: Object,
    required: true,
  },
});

const emit = defineEmits(["node-click"]);

const handleNodeClick = (node) => {
  emit("node-click", node);
};
</script>

