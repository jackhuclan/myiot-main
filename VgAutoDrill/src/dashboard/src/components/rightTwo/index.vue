<template>
  <div class="right_two">
    <dv-border-box-12 class="p-20">
      <p class="title">工单进度</p>
      <div class="carousel_list">
        <dv-scroll-board :config="config"/>
      </div>
    </dv-border-box-12>
  </div>
</template>

<script>
export default {
  name: "RightTwo",
  props: ["config"],
};
</script>


<style lang="scss" scoped>
.right_two {
  height: 48%;
}
</style>
