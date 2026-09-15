<template>
  <edit-form-dialog title="板料详情" v-model="open" optType="view" width="90%">
    <div>板料详情</div>
  </edit-form-dialog>
</template>

<script>
export default {
  data() {
    return {
      title: "",
      open: false,
    };
  },
};
</script>

<style lang="scss" scoped>
</style>