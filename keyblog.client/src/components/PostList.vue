<template>
  <div>
    <el-card v-if="posts.length === 0" shadow="always" class="mb-3">
      <div class="card-body">没有文章</div>
    </el-card>
    <el-card v-for="post in posts" :key="post.id" shadow="always" class="mb-3">
      <template #header>
        <div class="card-header">
          <span>{{ post.categoryId }}</span>
        </div>
      </template>
      <div class="card-body">
        <h5 class="card-title">{{ post.title }}</h5>
        <p class="card-text">{{ post.summary }}</p>
        <el-button
          type="link"
          class="btn-outline-secondary"
          @click="viewPost(post.id)"
        >
          查看全文
        </el-button>
      </div>
    </el-card>
    <el-pagination
      background
      style="justify-content: start"
      layout="prev, pager, next"
      :total="total"
      :page-size="pageSize"
      :current-page="currentPage"
      @current-change="handlePageChange"
    />
  </div>
</template>

<script>
import { defineComponent } from "vue";
import "@/assets/style.scss";

export default defineComponent({
  name: "PostList",
  props: {
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
  },
  emits: ["page-change", "view-post"],
  setup(props, { emit }) {
    const handlePageChange = (page) => {
      emit("update:currentPage", page);
    };

    const viewPost = (postId) => {
      emit("view-post", postId);
    };

    return {
      handlePageChange,
      viewPost,
    };
  },
});
</script>
