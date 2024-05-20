<template>
    <el-container>
        <el-header>
            <h2>Blog</h2>
            <span> Posts</span>
        </el-header>
        <el-main>
            <el-row :gutter="20">
                <el-col :span="16">
                    <!--                <el-space direction="vertical" fill>-->
                    <el-card v-if="posts.count === 0" shadow="always" class="mb-3">
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
                            <el-button type="link" class="btn-outline-secondary" @click="viewPost(post.id)">
                                查看全文
                            </el-button>
                        </div>
                    </el-card>
                    <!--         </el-space>-->
                    <el-pagination background style="justify-content:start" layout="prev, pager, next" :total="total"
                                   :page-size="pageSize" :current-page.sync="currentPage" @current-change="fetchPosts" />
                </el-col>
                <el-col :span="8" class="mb-3" id="categories">
                    <!-- Category Tree Component -->
                    <!--                    <category-tree :categories="categoryNodes"></category-tree>-->
                </el-col>
            </el-row>
        </el-main>
        <el-footer>Footer</el-footer>
    </el-container>
</template>

<script>
import { ref, onMounted } from 'vue';
import axios from 'axios';

export default {
    name: 'BlogList',
    setup() {
        const posts = ref([]);
        const total = ref(0);
        const currentPage = ref(1);
        const pageSize = ref(10);

        const fetchPosts = async (page = 1) => {
            try {
                const response = await axios.get('/api/Blog/posts', {
                    params: {
                        page,
                        pageSize: pageSize.value,
                    },
                });
                posts.value = response.data.items;
                total.value = response.data.totalCount;
                currentPage.value = page;
            } catch (error) {
                console.error('Error fetching posts:', error);
            }
        };

        onMounted(() => {
            fetchPosts(currentPage.value);
        });

        const viewPost = (postId) => {
            this.$router.push({ name: 'Post', params: { id: postId } });
        };

        return {
            posts,
            total,
            currentPage,
            pageSize,
            fetchPosts,
            viewPost,
        };
    },
};
</script>

<style scoped>
    .mb-3 {
        width: 80%;
        padding-left:2rem;
        margin-bottom: 1rem;
    }

    .card-container {
        display: flex;
        flex-direction: column;
        gap: 20px;
    }

    .card-title {
        font-size: 1.25rem;
        font-weight: bold;
    }

    .card-text {
        margin-bottom: 1rem;
    }

    .btn-outline-secondary {
        border: 1px solid #dcdcdc;
        color: #606266;
    }
</style>
