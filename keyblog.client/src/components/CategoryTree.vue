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

<script>
import { ref, defineComponent } from "vue";
import "@/assets/style.scss";

export default defineComponent({
  name: "CategoryTree",
  props: {
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
  },
  emits: ["node-click"],
  setup(props, { emit }) {
    const categoryTree = ref(null);
    const defaultProps = ref({
      children: "children",
      label: "label",
    });

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

    return {
      categoryTree,
      defaultProps,
      handleNodeClick,
    };
  },
});
</script>

