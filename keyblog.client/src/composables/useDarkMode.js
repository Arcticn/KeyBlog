// src/composables/useDarkMode.js
import { ref, watch } from 'vue';

const isDark = ref(document.documentElement.classList.contains('dark'));
const theme = ref(isDark.value ? 'dark' : 'light');

watch(isDark, (newVal) => {
  theme.value = newVal ? 'dark' : 'light';
  if (newVal) {
    document.documentElement.classList.add('dark');
  } else {
    document.documentElement.classList.remove('dark');
  }
});

export const useDarkMode = () => {
  return {
    isDark,
    theme,
  };
};
