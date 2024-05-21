<template>
  <div class="">
    <el-tree
      ref="categoryTree"
      :data="categories"
      :props="defaultProps"
      @node-click="handleNodeClick"
      node-key="id"
      highlight-current
      :expand-on-click-node="true"
      :default-expanded-keys="expandedKeys"
    >
    </el-tree>
  </div>
</template>

<script setup>


 const props=defineProps({
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
 });
  const emit=defineEmits(["node-click"]);

    const toggleNodeExpansion = (node) => {
      const index = props.expandedKeys.indexOf(node.id);
      if (index > -1) {
        emit("remove-key", index);
        removeChildKeys(node.children);
      } else {
        emit("add-key", node.id);
      }
    };

    const removeChildKeys = (children) => {
      if (!children) return;
      children.forEach((child) => {
        const index = props.expandedKeys.indexOf(child.id);
        if (index > -1) {
          emit("remove-key", index);
        }
        removeChildKeys(child.children);
      });
    };

    const handleNodeClick = (node) => {
      toggleNodeExpansion(node);
      emit("node-click", node);
    };

</script>

