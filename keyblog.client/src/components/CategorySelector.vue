<template>
  <div class="category-selector-line">
    <el-select
      v-model="selectedCategoryId"
      value-key="id"
      filterable
      allow-create
      default-first-option
      class="category-select"
      placeholder="选择分类"
      @change="handleCategoryChange"
    >
      <el-option
        v-for="category in categories"
        :key="category.id"
        :label="category.label"
        :value="category.id"
      />
      <template #footer>
        <el-button v-if="!isAdding" text bg size="small" @click="onAddOption">
          添加一个分类
        </el-button>
        <template v-else>
          <el-input
            v-model="optionName"
            class="option-input"
            placeholder="输入分类名称"
            size="small"
          />
          <el-button type="primary" size="small" @click="onConfirm">
            确认
          </el-button>
          <el-button size="small" @click="clear">取消</el-button>
        </template>
      </template>
    </el-select>
    <span style="margin-right: 10px;">/</span>
    <el-button
      v-if="selectedCategory && !selectedCategory.children"
      style="margin-left: 1rem"
      @click="showAddSubcategoryBox"
      >添加子分类</el-button
    >
    <template v-if="selectedCategory && selectedCategory.children">
      <CategorySelector
        :categories="selectedCategory.children"
        @add-category="handleAddCategory"
      />
    </template>
  </div>
</template>

<script setup>
import { ref, watch, onMounted } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";

const props = defineProps({
  categories: Array,
  modelValue: Number,
});

const selectedCategoryId = ref(null);
const selectedCategory = ref(null);
const isAdding = ref(false);
const optionName = ref("");

const handleCategoryChange = (value) => {
  selectedCategoryId.value = value;
  selectedCategory.value = findCategoryById(props.categories, value);
};

const onAddOption = () => {
  isAdding.value = true;
};

const onConfirm = () => {
  if (optionName.value) {
    const parentId =
      props.categories.length > 0 ? props.categories[0].parentId : 0; // 从当前选择框中的第一个选项获取 parentId
    const newCategory = {
      name: optionName.value,
      parentId: parentId,
      visible: true,
    };
    emits("add-category", newCategory); // 向主组件传递新分类数据

    clear();
  } else {
    ElMessage.error("分类名称不能为空");
  }
};

const clear = () => {
  optionName.value = "";
  isAdding.value = false;
};

const showAddSubcategoryBox = () => {
  ElMessageBox.prompt("请输入子分类名称", "添加子分类", {
    confirmButtonText: "确认",
    cancelButtonText: "取消",
  })
    .then(({ value }) => {
      if (value) {
        addSubcategory(value);
      } else {
        ElMessage.error("分类名称不能为空");
      }
    })
    .catch(() => {
      // Handle cancel button press
    });
};

const addSubcategory = (subcategoryName) => {
  if (selectedCategory.value) {
    const newSubcategory = {
      name: subcategoryName,
      parentId: selectedCategory.value.id, // 添加parentId
      visible: true,
    };

    emits("add-category", newSubcategory); // 向主组件传递新分类数据
  }
};

const findCategoryById = (categories, id) => {
  for (const category of categories) {
    if (category.id === id) {
      return category;
    }
    if (category.children) {
      const found = findCategoryById(category.children, id);
      if (found) {
        return found;
      }
    }
  }
  return null;
};

const handleAddCategory = (newCategory) => {
  emits("add-category", newCategory); // 冒泡事件到父组件
};

const emits = defineEmits(["add-category"]);

watch(
  () => props.categories,
  (newCategories) => {
    if (newCategories && newCategories.length === 1) {
      selectedCategoryId.value = newCategories[0].id;
      selectedCategory.value = newCategories[0];
    }
    if (selectedCategoryId.value) {
      selectedCategory.value = findCategoryById(
        newCategories,
        selectedCategoryId.value
      );
      if (selectedCategory.value === null) {
        selectedCategory.value = null;
        selectedCategoryId.value = null;
      }
    }
  },
  { immediate: true }
);
</script>

<style scoped>
.category-selector-line {
  display: flex;
  align-items: center;
  white-space: nowrap;
}
.category-select {
  width: 8rem;
  margin-right: 10px;
}
</style>
