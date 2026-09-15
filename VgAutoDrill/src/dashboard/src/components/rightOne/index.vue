<template>
  <div class="right_one">
    <dv-border-box-12 class="p-20">
      <p class="title">板料追踪</p>
      <div class="carousel_list">
        <dv-scroll-board :config="config" />
      </div>
    </dv-border-box-12>
  </div>
</template>

<script>
export default {
  name: "RightOne",
  props: ["config"],
};
</script>

<style lang="scss" scoped>
.right_one {
  height: 47%;
}
</style>
