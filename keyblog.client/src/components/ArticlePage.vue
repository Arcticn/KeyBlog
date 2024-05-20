<template>
  <el-container>
    <el-header class="header">Header Content</el-header>
    <el-container>
      <el-main class="markdown-content">
        <MdPreview
          :editorId="id"
          v-model="markdownContent"
          @onGetCatalog="onGetCatalog"
        />
      </el-main>
      <el-aside class="catalog-container" width="250px">
        <el-affix :offset="10">
          <MdCatalog :editorId="id" :scrollElement="scrollElement" />
        </el-affix>
      </el-aside>
    </el-container>
    <el-footer class="footer">Footer Content</el-footer>
  </el-container>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";
import { MdPreview, MdCatalog } from "md-editor-v3";
import "md-editor-v3/lib/style.css";
import { ElContainer, ElHeader, ElMain, ElFooter, ElAside, ElAffix } from 'element-plus';

const id = "markdown-editor";
const markdownContent = ref("");

const scrollElement = document.documentElement;


const state = ref({
  markdownContent: "",
  catalogList: [],
});

const onGetCatalog = (list) => {
  state.value.catalogList = list;
  console.log(state.value);
};

const fetchAndRenderContent = async () => {
  try {
    //const response = await axios.get("/api/posts/1c73631dbc26d500"); // 替换为实际的 API 地址
    const postData = {
        "id": "1c73631dbc26d500",
        "title": "题解",
        "slug": null,
        "status": "已发布",
        "isPublish": true,
        "summary": "题解\n7-1 String\n利用STL库函数search向后搜索子串即可，注意search返回头迭代",
        "content": "# 题解\n\n## 7-1 String\n\n利用STL库函数search向后搜索子串即可，注意search返回头迭代器，应将迭代器向后遍历一个 **（注意：不要将迭代器向后遍历多个，可能发生遗漏 *WA 10+ :)*，搜索任务交给函数即可）**\n\n```C++\nint main()\n{\n    int n; cin >> n;\n    string s, temp; cin >> s;\n    vector<string>mode;\n    for (size_t i = 0; i < n; i++)\n    {\n        cin >> temp;\n        mode.push_back(temp);\n    }\n    for (auto c : mode)\n    {\n        int sum = 0;\n        auto it = s.begin();\n        while (it != s.end()) {\n            it = search(it, s.end(), c.begin(), c.end());\n            if (it != s.end()) {\n                ++sum;\n                ++it;\n            }\n        }\n        cout << sum << endl;\n    }\n}\n```\n\n***\n\n\n## 7-2 区间\n\n分别从头从尾开始向另一端查找不同的数并计数,随后逐个插入分隔符，将分隔符左右计数相加即可 **（虽然写的简单，但我想不到......）**\n\n> 1 2 3 2 1  &emsp;*输入的数字*  \n> 1 2 3 3 3  &emsp;*从头向尾标记*  \n> 3 3 3 2 1  &emsp;*从尾向头标记*  \n> 1|2 3 3 3 &emsp;&emsp;3|3 3 2 1&emsp;&emsp;*1+3=4*\n\n```C++\nint num[1000010];\nint vis[1000010];\nint l[1000010]; int r[1000010];\n\nint main() {\n\tint n;\n\tint sum = 0;\n\tint maxn = 0;\n\tcin >> n;\n\tfor (int i = 1; i <= n; i++) {\n\t\tcin >> num[i];\n\t\tif (vis[num[i]] == 0) {\n\t\t\tl[i] = l[i - 1] + 1;\n\t\t\tvis[num[i]] = 1;\n\t\t}\n\t\telse l[i] = l[i - 1];\n\t}\n\tmemset(vis, 0, sizeof(vis));\n\tfor (int i = n; i >= 1; i--) {\n\t\tif (vis[num[i]] == 0) {\n\t\t\tr[i] = r[i + 1] + 1;\n\t\t\tvis[num[i]] = 1;\n\t\t}\n\t\telse r[i] = r[i + 1];\n\t}\n\tfor (int i = 1; i < n; i++) {\n\t\tint s = l[i] + r[i + 1];\n\t\tif (s > maxn) maxn = s;\n\t}\n\tcout << maxn;\n}\n```\n\n***\n\n\n## 7-3 小步点\n\n输入的d,n取正，随后分两种情况：  \n1）如果n<=R,说明可以沿直线跑完小步点，**注意减去一个R**  \n2）如果n>R,由于不同点之间横向距离相同，易得，到达以该点为圆心，R为半径的圆的最短路径为到达该圆（取正后的）的最下边，距离为2*sqrt(d^2+(n-R)^2)-R\n\n```C++\nint main() {\n\tint R, d, n;\n\tcin >> R >> d >> n;\n\tdouble result;\n\td = abs(d);\n\tn = abs(n);\n\tif (n <= R)result = 2* d-R;\n\telse result = 2 * sqrt(d * d + (n-R) * (n-R)) - R;\n\tcout <<fixed<<setprecision(2)<<result;\n}\n```\n\n***\n\n\n## 7-4 分糖果\n\n递归解决,模拟分糖果，注意小细节\n\n```C++\nint sum = 0;\n\nvoid count(int n) {\n\tfor (size_t i = 1; i <=n/2; i++)\n\t{\n\t\tif (n % i == 0) {\n\t\t\t++sum;\n\t\t\tcount(i);\n\t\t}\n\t}\n}\n\nint main() {\n\tint n; cin >> n;\n\tcount(n);\n\tcout << sum+1;\n}\n```\n\n***\n\n\n## 7-5 找眼镜\n\n两个结构体存储同学和提示的信息  \n随后利用迭代器逐个模拟遍历 **注意迭代器的尾迭代器为空(　*不要忘记这一点*　就会觉得这个设计还蛮人性化的)**\n\n```C++\nstruct stu {\n\tbool dir;\n\tstring name;\n};\n\nstruct hint\n{\n\tbool dir;\n\tint num;\n};\n\nint main() {\n\tint n, m;\n\tcin >> n >> m;\n\tvector<stu>a;\n\tvector<hint>b;\n\tstu temp; hint temp1;\n\tfor (size_t i = 0; i < n; i++)\n\t{\n\t\tcin >> temp.dir >> temp.name;\n\t\ta.push_back(temp);\n\t}\n\tfor (size_t i = 0; i < m; i++)\n\t{\n\t\tcin >> temp1.dir >> temp1.num;\n\t\tb.push_back(temp1);\n\t}\n\tauto it = a.begin();\n\tfor (size_t i = 0; i < m; i++)\n\t{\n\t\tif (b[i].dir == 0) {\n\t\t\tif ((*it).dir == 0) {\n\t\t\t\twhile (b[i].num--) {\n\t\t\t\t\tif (it == a.begin()) {\n\t\t\t\t\t\tit = a.end();\n\t\t\t\t\t}\n\t\t\t\t\tit--;\n\t\t\t\t}\n\t\t\t}\n\t\t\telse if((*it).dir == 1){\n\t\t\t\twhile (b[i].num--) {\n\t\t\t\t\tif (it == a.end() - 1) {\n\t\t\t\t\t\tit = a.begin();\n\t\t\t\t\t\tb[i].num--;\n\t\t\t\t\t}\n\t\t\t\t\tit ++;\n\t\t\t\t}\n\t\t\t}\n\t\t}\n\t\telse if (b[i].dir == 1) {\n\t\t\tif ((*it).dir == 0) {\n\t\t\t\twhile (b[i].num--) {\n\t\t\t\t\tif (it == a.end() - 1) {\n\t\t\t\t\t\tit = a.begin();\n\t\t\t\t\t\tb[i].num--;\n\t\t\t\t\t}\n\t\t\t\t\tit++;\n\t\t\t\t}\n\t\t\t}\n\t\t\telse if ((*it).dir == 1) {\n\t\t\t\twhile (b[i].num--) {\n\t\t\t\t\tif (it == a.begin()) {\n\t\t\t\t\t\tit = a.end();\n\t\t\t\t\t}\n\t\t\t\t\tit--;\n\t\t\t\t}\n\t\t\t}\n\t\t}\n\t}\n\tcout << (*it).name;\n}\n```\n\n***\n\n\n## 7-6 恰早饭\n\n这道题一开始以为是吃 **无限顿早餐直到时间吃完 : )**，后来大呼卧槽......应该没什么要注意的\n\n```C++\nint main() {\n\tint n, T,max=-9999;\n\tcin >> n >> T;\n\tvector<int>happy, time; happy.reserve(100000); time.reserve(100000);\n\tint temp1, temp2;\n\tfor (size_t i = 0; i < n; i++)\n\t{\n\t\tcin >> temp1 >> temp2;\n\t\thappy.push_back(temp1);\n\t\ttime.push_back(temp2);\n\t}\n\tfor (size_t i = 0; i < n; i++)\n\t{\n\t\tif (time[i] <= T) {\n\t\t\tif (happy[i] > max)max = happy[i];\n\t\t}\n\t\tif (time[i]>T) {\n\t\t\ttemp1 = happy[i] - (time[i] - T);\n\t\t\tif (temp1 > max)max = temp1;\n\t\t}\n\t}\n\tcout << max;\n}\n```",
        "path": "Week 1",
        "creationTime": "2021-10-16T10:27:35.5637275",
        "lastUpdateTime": "2021-10-16T21:22:00.2684865",
        "categoryId": 6,
        "category": {
          "id": 6,
          "name": "Week 1",
          "parentId": 0,
          "parent": null,
          "visible": true,
          "posts": null
        },
        "categories": "6"
      };
    markdownContent.value = postData.content;
  } catch (error) {
    console.error("Error fetching post data:", error);
  }
};

onMounted(() => {
  fetchAndRenderContent();
});
</script>

<style>

.header {
  padding: 10px;
  background-color: #f5f5f5;
  text-align: center;
}

.footer {
  padding: 10px;
  background-color: #f5f5f5;
  text-align: center;
}



.markdown-content {
  flex: 1;
  overflow-y: auto;
  padding: 20px;
}

.markdown-content .md-editor-preview {
  text-align: left;
}

.catalog-container {
  overflow-y: auto;
  max-height: calc(100vh - 80px); /* 80px is the combined height of header and footer */
  padding: 20px;
  position: sticky;
  top: 10px; /* Adjust as needed to offset from the top */
}

/* Custom styles to fix code block line spacing */
.markdown-content pre {
  margin: 0 !important;
}

.markdown-content code {
  white-space: pre-wrap !important;
  display: block;
}
</style>