<template>
  <div class="left_three">
    <dv-border-box-12 class="p-20">
      <p class="title">设备告警</p>
      <div class="carousel_list">
        <dv-scroll-board :config="config" />
      </div>
    </dv-border-box-12>
  </div>
</template>

<script>
export default {
  name: "LeftThree",
  props: ["config"],
};
</script>

<style lang="scss" scoped>
.left_three {
  height: 30%;
} 
</style>

