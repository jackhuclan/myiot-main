<template>
  <div class="one">
    <dv-border-box-7 class="p-20">
      <p class="center_title">工单统计</p>
      <div class="round_box">
        <li>
          <dv-water-level-pond
            :config="config.workOrderConfig"
            style="width: 150px; height: 150px"
          />
          <p class="mtb-30">订单增率</p>
        </li>

        <li>
          <dv-water-level-pond
            :config="config.feedBackConfig"
            style="width: 150px; height: 150px"
          />
          <p class="mtb-30">生产增率</p>
        </li>
      </div>
    </dv-border-box-7>
  </div>
</template>

<script>
export default {
  name: "CenterTopOne",
  props: ["config"],
};
</script>

<style lang="scss" scoped>
// 工单统计水位图
.round_box {
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: space-around;
  margin-top: 30px;
  li {
    display: flex;
    flex-direction: column;
    width: 50%;
    align-items: center;
    p {
      font-size: 22px;
    }
  }
}
</style>
