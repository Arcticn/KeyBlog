import { ref, watch } from 'vue';

const isDark = ref(localStorage.getItem('isDark') === 'true');
const theme = ref(isDark.value ? 'dark' : 'light');

// 初始化函数
const initializeTheme = () => {
  if (isDark.value) {
    document.documentElement.classList.add('dark');
  } else {
    document.documentElement.classList.remove('dark');
  }
};

watch(isDark, (newVal) => {
  theme.value = newVal ? 'dark' : 'light';
  if (newVal) {
    document.documentElement.classList.add('dark');
    localStorage.setItem('isDark', 'true');
  } else {
    document.documentElement.classList.remove('dark');
    localStorage.setItem('isDark', 'false');
  }
});

export const useDarkMode = () => {
  initializeTheme(); // 确保在导出时初始化主题
  return {
    isDark,
    theme,
  };
};
