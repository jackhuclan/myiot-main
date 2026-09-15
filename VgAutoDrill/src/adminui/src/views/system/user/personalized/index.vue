<template>
  <div>
    <el-form ref="form" :model="form" label-position="left" inline>
      <el-row v-for="item in list" :key="item.name" style="padding: 0 10px">
        <el-col :span="16">
          <el-form-item :label="item.label" class="item">
            <!-- 
              :myNum="form[item.name] ? form[item.name] * 1 : 0"
             -->
            <input-number
              :myNum="getMyNum(item)"
              @changeNum="changeNum"
              :numName="item.name"
              :min="1"
              style="width: 200px;"
            />
          </el-form-item>
        </el-col> </el-row
      ><el-row style="padding: 0 10px">
        <el-col>
          <el-form-item class="item">
            <el-button
              type="primary"
              @click="onSubmit"
              v-hasPermi="['system:config:save']"
              >保存</el-button
            >
          </el-form-item>
        </el-col>
      </el-row></el-form
    >
  </div>
</template>

<script>
import { list } from "@/personalized";
export default {
  name: "Personalized",
  data() {
    return {
      list: [],
    };
  },
  created() {
    this.list = list;
  },
  computed: {
    form() {
      return JSON.parse(JSON.stringify(this.$store.state.personalized));
    },
    getMyNum() {
      return (item) => {
        let num = 0;
        if (this.form[item.name]) {
          if (item.name == "maxShowFormItem") {
            num = this.form[item.name];
          } else {
            num = this.form[item.name];
          }
        } else {
          num = 0;
        }
        return num;
      };
    },
  },
  methods: {
    onSubmit() {
      this.$modal.loading("正在保存到本地，请稍候...");
      this.$cache.local.set("layout-personalized", JSON.stringify(this.form));
      for (const property in this.form) {
        this.$store.dispatch("personalized/changePersonalized", {
          key: property,
          value: this.form[property],
        });
      }
      setTimeout(this.$modal.closeLoading(), 1000);
    },
  },
};
</script>

<style lang="scss" scoped>
.item {
  font-size: 18px;
  margin: 10px 0;
}
</style>