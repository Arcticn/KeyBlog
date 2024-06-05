<template>
  <el-card class="glass-effect" style="margin-left: 14px; width: 16.7rem">
    <div v-if="!isLoggedin">
      <el-form
        ref="formDataRef"
        style="max-width: 600px"
        :model="formData"
        status-icon
        :rules="rules"
        label-width="auto"
      >
        <el-form-item label="用户名" prop="name">
          <el-input v-model="formData.name" autocomplete="off" />
        </el-form-item>
        <el-form-item label="密码" prop="pass">
          <el-input
            v-model="formData.pass"
            type="password"
            show-password
            autocomplete="off"
          />
        </el-form-item>
        <div v-if="isRegister">
          <el-form-item label="确认密码" prop="checkPass">
            <el-input
              v-model="formData.checkPass"
              show-password
              type="password"
              autocomplete="off"
            />
          </el-form-item>
        </div>
        <el-form-item>
          <el-button
            type="primary"
            @click="LoginorRegister(formDataRef)"
            style="margin-left: 2rem"
          >
            {{ isRegister ? "注册" : "登陆" }}
          </el-button>
          <el-button type="info" @click="handleIsRegisterState">{{
            isRegister ? "<<返回登陆" : ">>前往注册"
          }}</el-button>
        </el-form-item>
      </el-form>
    </div>
    <div v-else>
      <p>欢迎回来, {{ currentUser.username }}</p>
      <el-button type="warning" @click="handleLogOut">Logout</el-button>
    </div>
  </el-card>
</template>

<script setup>
import { ErrorMessage, SuccessMessage } from "@/composables/PopupMessage";
import api from "@/services/api";
import { onMounted, reactive, ref } from "vue";

const isRegister = ref(false);
const isLoggedin = ref(false);
const formDataRef = ref(null);

const validateName = (rule, value, callback) => {
  if (value === "") {
    callback(new Error("输入用户名"));
  } else {
    callback();
  }
};

const validatePass = (rule, value, callback) => {
  if (value === "") {
    callback(new Error("请输入密码"));
  } else {
    if (isRegister.value && formData.checkPass !== "") {
      if (!formDataRef.value) return;
      formDataRef.value.validateField("checkPass");
    }
    callback();
  }
};

const validatePass2 = (rule, value, callback) => {
  if (value === "") {
    callback(new Error("请再次输入密码"));
  } else if (value !== formData.pass) {
    callback(new Error("两次输入不一致！"));
  } else {
    callback();
  }
};

const formData = reactive({
  name: "",
  pass: "",
  checkPass: "",
});

const rules = reactive({
  name: [{ validator: validateName, trigger: "change" }],
  pass: [{ validator: validatePass, trigger: "change" }],
  checkPass: [{ validator: validatePass2, trigger: "change" }],
});

const LoginorRegister = (formEl) => {
  formEl.validate((valid) => {
    if (valid) {
      if (isRegister.value) {
        Register(formEl);
      } else {
        Login();
      }
    }
  });
};

const currentUser = ref(null);

const Login = async () => {
  console.log(formData.name, formData.pass);
  try {
    const newUser = {
      Username: formData.name,
      PasswordHash: formData.pass,
    };
    const response = await api.post("User/login", newUser);
    currentUser.value = response.data.user;
    const token = response.data.token;
    localStorage.setItem("token", token);
    localStorage.setItem("currentUser", JSON.stringify(currentUser.value));
    isLoggedin.value = true;
    SuccessMessage("欢迎回来,", currentUser.value.username);
    window.location.reload();
  } catch (error) {
    ErrorMessage(error.response.data);
  }
  console.log(localStorage.getItem("token"));
};

const handleIsRegisterState = () => {
  isRegister.value = !isRegister.value;
  formDataRef.value.resetFields();
};

const Register = async () => {
  console.log(formData.name, formData.pass);
  try {
    const newUser = {
      Username: formData.name,
      PasswordHash: formData.pass,
    };
    const response = await api.post("User/register", newUser);
    currentUser.value = response.data.user;
    const token = response.data.token;
    localStorage.setItem("token", token);
    localStorage.setItem("currentUser", JSON.stringify(currentUser.value));
    isLoggedin.value = true;
    SuccessMessage("欢迎回来,", currentUser.value.username);
    window.location.reload();
  } catch (error) {
    ErrorMessage(error.response.data);
  }
};

const handleLogOut = () => {
  localStorage.removeItem("token");
  localStorage.removeItem("currentUser");
  isLoggedin.value = false;
  currentUser.value = null;
  window.location.reload();
};

onMounted(async () => {
  currentUser.value = JSON.parse(localStorage.getItem("currentUser"));
  console.log(currentUser);

  if (currentUser.value) {
    console.log(localStorage.getItem("token"));
    if (currentUser.value.IsAdmin) {
      try {
        await api.get("User/admin"); //令牌过期则退出
        isLoggedin.value = true;
      } catch (error) {
        localStorage.removeItem("token");
        localStorage.removeItem("currentUser");
        isLoggedin.value = false;
        currentUser.value = null;
        window.location.reload();
      }
    } else {
      try {
        await api.get("User/user"); //令牌过期则退出
        isLoggedin.value = true;
      } catch (error) {
        localStorage.removeItem("token");
        localStorage.removeItem("currentUser");
        isLoggedin.value = false;
        currentUser.value = null;
        window.location.reload();
      }
    }
  }
});
</script>
