import { defineStore } from 'pinia';
import api from '@/services/api';

export const usePicTokenStore = defineStore('picToken', {
  state: () => ({
    apiToken: null,
    apiURL: null
  }),
  actions: {
    async fetchToken() {
      try {
        const response = await api.get('Blog/picToken');
        this.apiToken = response.data.pictoken;
        this.apiURL = response.data.picurl;
      } catch (error) {
        console.error('Failed to fetch token:', error);
      }
    },
    clearToken() {
      this.apiToken = null;
      this.apiURL = null;
    }
  },
  getters: {
    getToken: (state) => state.apiToken,
    getURL: (state) => state.apiURL
  }
});
