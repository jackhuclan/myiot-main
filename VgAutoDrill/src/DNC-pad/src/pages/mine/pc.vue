<template>
  <view class="mine-container">
    <uni-row>
      <uni-col :span="10" :xs="24">
        <uni-card class="box-card">
          <uni-list>
            <uni-list-item
              showExtraIcon="true"
              :extraIcon="{ type: 'person-filled' }"
              title="账号"
              :rightText="user.userName"
            />
            <uni-list-item
              showExtraIcon="true"
              :extraIcon="{ type: 'phone-filled' }"
              title="手机号码"
              :rightText="user.mobile"
            />
            <uni-list-item
              showExtraIcon="true"
              :extraIcon="{ type: 'email-filled' }"
              title="邮箱"
              :rightText="user.email"
            />
            <uni-list-item
              showExtraIcon="true"
              :extraIcon="{ type: 'auth-filled' }"
              title="所属部门"
              :rightText="user.departmentName"
            />
            <uni-list-item
              showExtraIcon="true"
              :extraIcon="{ type: 'calendar-filled' }"
              title="创建日期"
              :rightText="user.createTime"
            />
          </uni-list> </uni-card
      ></uni-col>
      <uni-col :span="14" :xs="24">
        <uni-card class="box-card">
          <uni-forms
            ref="form"
            :rules="rules"
            :modelValue="userForm"
            label-width="100px"
          >
            <uni-forms-item label="用户昵称" name="realName">
              <uni-easyinput v-model="userForm.realName" maxlength="30" />
            </uni-forms-item>
            <uni-forms-item label="手机号码" name="mobile">
              <uni-easyinput v-model="userForm.mobile" maxlength="11" />
            </uni-forms-item>
            <uni-forms-item label="邮箱" name="email">
              <uni-easyinput v-model="userForm.email" maxlength="50" />
            </uni-forms-item>
            <uni-forms-item label="性别">
              <uni-data-checkbox v-model="userForm.sex" :localdata="sexs" />
            </uni-forms-item>
            <uni-forms-item>
              <view class="flex">
                <button type="primary" size="mini" @click="submit">保存</button>
                <button type="warn" size="mini" @click="handleReset">
                  重置
                </button>
              </view>
            </uni-forms-item>
          </uni-forms>
        </uni-card></uni-col
      ></uni-row
    >
  </view>
</template>

<script>
import { updateUserProfile } from "@/api/user";

export default {
  data() {
    return {
      userForm: {},
      // 单选数据源
      sexs: [
        {
          text: "男",
          value: 1,
        },
        {
          text: "女",
          value: 0,
        },
      ],
      // 表单校验
      rules: {
        realName: {
          rules: [
            {
              required: true,
              errorMessage: "用户昵称不能为空",
            },
          ],
        },
        email: {
          rules: [
            {
              required: true,
              errorMessage: "邮箱地址不能为空",
            },
          ],
        },
        realName: {
          rules: [
            {
              required: true,
              errorMessage: "用户昵称不能为空",
            },
            {
              type: "email",
              message: "请输入正确的邮箱地址",
              trigger: ["blur", "change"],
            },
          ],
        },
        mobile: {
          rules: [
            {
              required: true,
              errorMessage: "手机号码不能为空",
            },
            {
              pattern: /^1[3|4|5|6|7|8|9][0-9]\d{8}$/,
              message: "请输入正确的手机号码",
              trigger: "blur",
            },
          ],
        },
      },
    };
  },
  computed: {
    user() {
      this.userForm = JSON.parse(
        JSON.stringify(this.$store.state.user.userInfo)
      );
      return this.$store.state.user.userInfo;
    },
  },

  methods: {
    submit() {
      this.$refs["form"].validate((res) => {
        if (!res) {
          updateUserProfile(this.userForm).then((response) => {
            this.$modal.showToast("保存成功");
          });
        }
      });
    },
    handleReset() {
      this.userForm = JSON.parse(
        JSON.stringify(this.$store.state.user.userInfo)
      );
    },
  },
};
</script>
<style lang="scss" scoped>
page {
  background-color: #fff;
}

.list-group-striped > .list-group-item {
  border-left: 0;
  border-right: 0;
  border-radius: 0;
  padding-left: 0;
  padding-right: 0;
}

.list-group {
  padding-left: 0px;
  list-style: none;
}

.list-group-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-bottom: 1px solid #e7eaec;
  border-top: 1px solid #e7eaec;
  margin-bottom: -1px;
  padding: 11px 0px;
  font-size: 12px;
  span {
    margin-left: 3px;
  }
}

.mine-container {
  width: 100%;
  height: 100%;
}
</style>
