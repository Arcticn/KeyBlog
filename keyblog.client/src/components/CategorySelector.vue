<template>
  <div class="category-selector">
    <el-select
      v-model="selectedCategoryId"
      filterable
      allow-create
      default-first-option
      placeholder="选择分类或输入新分类"
      @change="handleCategoryChange"
    >
      <el-option
        v-for="category in localCategories"
        :key="category.id"
        :label="category.name"
        :value="category.id"
      />
    </el-select>
    <el-button v-if="selectedCategory && !showSubcategory" @click="addSubcategory">添加子分类</el-button>
    <CategorySelector
      v-if="showSubcategory && selectedCategory && selectedCategory.subcategories"
      :categories="selectedCategory.subcategories"
      v-model:selectedCategoryId="selectedCategory.subcategoryId"
      @update:categories="updateSubcategories"
    />
  </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue';

const props = defineProps({
  categories: Array,
  modelValue: Number
});

const emits = defineEmits(['update:modelValue', 'update:categories']);

</script>

<style scoped>
.category-selector {
  display: flex;
  align-items: center;
  white-space: nowrap;
  /* margin-bottom: 10px; */
}
</style>
