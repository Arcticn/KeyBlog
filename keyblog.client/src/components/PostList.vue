<template>
  <div class="input-container">
    <el-card
      style="height: 3.5rem; width: 60%; margin-bottom: 0.5rem"
      body-style="padding: 5px;"
      class="mb-3 glass-effect"
    >
      <el-input
        v-model="searchQuery"
        style="margin-top: 6px; margin-left: 6px; padding-right: 10px"
        placeholder="搜索...."
        @change="handleSearchChange"
        :prefix-icon="Search"
      />
    </el-card>
    <el-card
      style="
        width: fit-content;
        height: 3.5rem;
        margin-left: auto;
        margin-bottom: 0.5rem;
      "
      body-style="padding: 5px;"
      class="mb-3 glass-effect"
    >
      <el-select
        v-model="sortBy"
        placeholder="Select"
        @change="handleSortChange"
        style="width: 120px; margin-top: 7px; margin-left: 6px"
      >
        <el-option
          v-for="item in sortByOptions"
          :key="item.value"
          :label="item.label"
          :value="item.value"
        />
      </el-select>
      <el-radio-group
        v-model="sortType"
        @change="handleSortChange"
        style="margin-left: 0.7rem; margin-right: 6px"
      >
        <el-radio-button label="desc" value="desc">
          <el-icon>
            <SortDown />
          </el-icon>
        </el-radio-button>
        <el-radio-button label="asc" value="asc">
          <el-icon>
            <SortUp />
          </el-icon>
        </el-radio-button>
      </el-radio-group>
    </el-card>
  </div>

  <div>
    <el-card
      v-if="posts.length === 0 || loadPostStatus === true"
      v-loading="loadPostStatus"
      style="min-height: 10rem"
      shadow="always"
      class="mb-3 glass-effect"
    >
      <div class="card-body">没有文章</div>
    </el-card>
    <el-card
      v-else
      v-for="post in posts"
      :key="post.id"
      shadow="always"
      class="mb-3 glass-effect"
    >
      <div class="card-body">
        <h5 class="card-title">
          <span>{{ post.title }}</span>
          <span class="date"
            ><el-icon class="clock-icon"><Clock /></el-icon>更新时间:
            {{ formatDate(post.lastUpdateTime) }}</span
          >
        </h5>
        <p class="card-text">{{ post.summary }}</p>
        <el-button type="default" @click="viewPost(post.id)">
          查看全文
        </el-button>
      </div>
    </el-card>
    <el-pagination
      background
      style="justify-content: center; margin-left: 5rem"
      layout="prev, pager, next"
      :total="total"
      :page-size="pageSize"
      :current-page="currentPage"
      @current-change="handlePageChange"
    />
  </div>
</template>

<script setup>
import { ref } from "vue";
import "@/components/styles/style.scss";
import "@/components/styles/glass.scss";

// 定义组件的 props
const props = defineProps({
  loadPostStatus: {
    type: Boolean,
    required: true,
  },
  posts: {
    type: Array,
    required: true,
  },
  total: {
    type: Number,
    required: true,
  },
  currentPage: {
    type: Number,
    required: true,
  },
  pageSize: {
    type: Number,
    required: true,
  },
});

const formatDate = (cellValue) => {
  if (!cellValue) return "";
  // 创建 UTC 时间的 Date 对象
  const date = new Date(cellValue);

  // 将 UTC 时间转换为本地时间
  const localDate = new Date(date.getTime() - date.getTimezoneOffset() * 60000);

  return localDate.toLocaleString();
};

const sortBy = ref("LastUpdateTime");
const sortType = ref("desc");
const searchQuery = ref("");

const sortByOptions = [
  {
    value: "LastUpdateTime",
    label: "更新时间",
  },
  {
    value: "CreationTime",
    label: "创建时间",
  },

  {
    value: "Title",
    label: "博客名称",
  },
];

// 定义组件的 emits
const emit = defineEmits([
  "view-post",
  "update:currentPage",
  "page-change",
  "handleSortChange",
  "search-change",
]);

// 处理分页改变的函数
const handlePageChange = (newPage) => {
  emit("update:currentPage", newPage);
};

const handleSortChange = () => {
  emit("sort-change", sortBy.value, sortType.value);
};

const handleSearchChange = () => {
  emit("search-change", searchQuery.value);
};

// 查看文章的函数
const viewPost = (postId) => {
  emit("view-post", postId);
};
</script>

<style scoped>
.input-container {
  display: flex;
  align-items: center;
  white-space: nowrap;
}

.clock-icon {
  top: 2px;
  padding-right: 3px;
}

.date {
  font-size: 0.7em; /* 调整字体大小 */
  color: #888; /* 调整字体颜色 */
  font-weight: normal;
}
</style>
