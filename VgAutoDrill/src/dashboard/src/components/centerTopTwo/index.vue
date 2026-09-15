<template>
  <div class="center_two">
    <dv-border-box-7 class="p-20">
      <p class="center_title">操作车间展示</p>
      <img
        src="@/assets/img/workshop_img.jpg"
        alt=""
        class="workshop_img mt-20"
      />
      <div class="carousel_list h280">
        <dv-scroll-board :config="config" class="h240" />
      </div>
    </dv-border-box-7>
  </div>
</template>

<script>
export default {
  name: "CenterTopTwo",
  props: ["config"],
};
</script>
<style lang="scss" scoped>
.center_two {
  height: 645px;
  margin-left: 5px;
  width: 100%;
}

.workshop_img {
  width: 100%;
  display: block;
}
</style>