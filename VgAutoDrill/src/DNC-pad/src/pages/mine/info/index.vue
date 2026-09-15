<template>
  <view class="container">
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
    </uni-list>
  </view>
</template>

<script>
export default {
  computed: {
    user() {
      this.userForm = JSON.parse(
        JSON.stringify(this.$store.state.user.userInfo)
      );
      return this.$store.state.user.userInfo;
    },
  },
};
</script>

<style lang="scss">
page {
  background-color: #ffffff;
}
</style>
