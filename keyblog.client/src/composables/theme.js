import { ref } from 'vue';

export const previewTheme = ref(localStorage.getItem("previewTheme")??'arknights');
export const codeTheme = ref(localStorage.getItem("codeTheme")??'atom');

