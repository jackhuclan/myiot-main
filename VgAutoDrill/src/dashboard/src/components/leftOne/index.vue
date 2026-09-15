<template>
  <div class="left_one">
    <dv-border-box-12 class="p-20">
      <p class="title">公司简介</p>
      <img class="left_img" height="150" src="../../assets/img/vega.png" />
      <p class="VGdesc">
        苏州维嘉科技股份有限公司，数字化智能高端专用装备提供商，集研发、制造、服务为一体的国家高新技术企业。公司专注于精密数控、智能制造、机器人、工业激光、视觉检测、物联网、结构及软件设计等产品和技术的研发，产品广泛应用于PCB、半导体、SMT、3C金属超精密加工等多个工业领域。
      </p>
    </dv-border-box-12>
  </div>
</template>

<script>
export default {
  name: "LeftOne",
};
</script>

<style lang="scss" scoped>
.left_one {
  height: 40%;
}
.left_img {
  width: 100%;
  height: 150px;
  display: block;
  margin: 0 auto;
  margin-bottom: 10px;
}
.VGdesc {
  text-indent: 2em;
  line-height: 25px;
}
</style>
