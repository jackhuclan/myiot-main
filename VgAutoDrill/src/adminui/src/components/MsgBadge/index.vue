<template>
  <div @click="click">
    <svg-icon icon-class="msg"></svg-icon>
    <el-badge :value="12" class="badge"></el-badge>
  </div>
</template>

<script>
export default {
  name: "MsgBadge",
  methods: {
    click() {
      this.$router.push({ path: "/message/message_page" });
    },
  },
};
</script>

<style lang="scss">
.badge {
  margin-top: -10px;
  margin-left: -10px;
}
</style>
