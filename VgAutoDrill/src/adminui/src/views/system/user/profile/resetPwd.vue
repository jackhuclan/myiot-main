<template>
  <el-form ref="form" :model="user" :rules="rules" label-width="100px">
    <el-form-item label="旧密码" prop="oldPassword">
      <el-input
        v-model="user.oldPassword"
        placeholder="请输入旧密码"
        type="password"
        show-password
      />
    </el-form-item>
    <el-form-item label="新密码" prop="newPassword">
      <el-input
        v-model="user.newPassword"
        placeholder="请输入新密码"
        type="password"
        show-password
      />
    </el-form-item>
    <el-form-item label="确认密码" prop="confirmPassword">
      <el-input
        v-model="user.confirmPassword"
        placeholder="请确认新密码"
        type="password"
        show-password
      />
    </el-form-item>
    <el-form-item>
      <el-button type="primary" size="mini" @click="submit" v-debounce
        >保存</el-button
      >
      <el-button type="danger" size="mini" @click="close" v-debounce
        >关闭</el-button
      >
    </el-form-item>
  </el-form>
</template>

<script>
import { updateUserPwd } from "@/api/system/user";
import md5 from "js-md5";

export default {
  props: ["userId"],
  data() {
    const equalToPassword = (rule, value, callback) => {
      if (this.user.newPassword !== value) {
        callback(new Error("两次输入的密码不一致"));
      } else {
        callback();
      }
    };
    const checkUserPassword = (rule, value, callback) => {
      const reg =
        /^(?=.*[0-9])(?=.*[!@#$%^&*.])(?=.*[a-zA-Z])(?=.*[A-Z]).{10,}$/;
      if (!value) return callback(new Error("新密码不能为空"));
      if (value.length < 10 || value.length > 16) {
        callback(new Error("密码长度需要在10-16个字符之间"));
      } else if (!reg.test(value)) {
        callback(new Error("需由大小写字母、数字及字符（!@#$%^&*.）组成"));
      } else {
        callback();
      }
    };
    return {
      user: {
        oldPassword: undefined,
        newPassword: undefined,
        confirmPassword: undefined,
      },
      // 表单校验
      rules: {
        oldPassword: [
          { required: true, message: "旧密码不能为空", trigger: "blur" },
        ],
        newPassword: [
          { required: true, validator: checkUserPassword, trigger: "blur" },
        ],
        confirmPassword: [
          { required: true, validator: equalToPassword, trigger: "blur" },
        ],
      },
    };
  },
  methods: {
    submit() {
      this.$refs["form"].validate((valid) => {
        if (valid) {
          updateUserPwd({
            userId: this.userId,
            oldPassword: md5(this.user.oldPassword),
            newPassword: md5(this.user.newPassword),
          }).then((res) => {
            if (res.code == 0) {
              new Promise((resolve, reject) => {
                this.$modal.msgSuccess("修改成功，请重新登录");
                setTimeout(() => {
                  resolve();
                }, 1000);
              }).then(() => {
                this.$store.dispatch("LogOut").then(() => {
                  location.href = "/";
                });
              });
            } else {
              this.$modal.notifyError(res.message);
            }
          });
        }
      });
    },
    close() {
      this.$tab.closePage();
    },
  },
};
</script>
