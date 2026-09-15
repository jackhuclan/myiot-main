<template>
  <div class="app-container">
    <!--      style="border: 2px solid #ccc; padding: 10px 0" -->
    <el-form ref="form" :model="form" label-position="left" inline>
      <div id="configForm">
        <el-row v-for="item in formList" :key="item.id" style="padding: 0 10px">
          <el-col>
            <el-form-item :label="item.remark + ' :'" class="item">
              <el-select
                v-if="item.configType == 'enum'"
                v-model="form[item.configCode]"
                @visible-change="visibleChange($event, item)"
                @change="handleSelect"
                placeholder="请选择"
              >
                <el-option
                  v-for="v in getEnumList(item.configEnumValue)"
                  :key="v.configEnumValue"
                  :label="v.configEnumDescript + '(' + v.configEnumValue + ')'"
                  :value="v.configEnumValue"
                >
                </el-option>
              </el-select>
              <input-number
                v-else-if="item.configType == 'int'"
                :myNum="form[item.configCode] ? form[item.configCode] * 1 : 0"
                @changeNum="changeNum"
                :numName="item.configCode"
                :min="1"
              />
              <el-radio-group
                v-model="form[item.configCode]"
                v-else-if="item.configType == 'bool'"
                @change="handleRadio"
                v-removeAriaHidden
              >
                <el-radio label="true"> 是 </el-radio>
                <el-radio label="false">否 </el-radio>
              </el-radio-group>
              <el-input
                v-else
                v-model="form[item.configCode]"
                placeholder="请输入配置项值"
              />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row style="padding: 0 10px">
          <el-col>
            <el-form-item class="item">
              <el-button
                v-if="!showFixedBtn"
                type="primary"
                @click="onSubmit"
                :disabled="hasPermi(['system:config:save'])"
                >保存</el-button
              >
              <el-button
                v-else
                class="fixed_btn"
                type="primary"
                @click="onSubmit"
                :disabled="hasPermi(['system:config:save'])"
                >保存</el-button
              >
            </el-form-item>
          </el-col>
        </el-row>
      </div>
    </el-form>
  </div>
</template>
<script>
import { listSysConfig, saveBasicSysData } from "@/api/system/parameter";
export default {
  name: "QuickConfig",
  data() {
    return {
      form: {},
      formList: [],
      sysConfigEnums: [],
      showFixedBtn: false,
      itemTop: 0,
    };
  },
  created() {
    this.getList();
  },
  mounted() {
    this.$nextTick(() => {
      // 当组件挂载后，获取元素的位置信息
      const group = window.document
        .querySelector(`#configForm`)
        ?.getBoundingClientRect();

      this.itemTop = group?.top;
    });
  },
  computed: {
    ItemLength() {
      return this.formList.length;
    },
  },
  watch: {
    ItemLength: {
      handler(val) {
        this.$nextTick(() => {
          // 当组件挂载后，获取元素的位置信息
          const group = window.document
            .querySelector(`#configForm`)
            ?.getBoundingClientRect();

          // 盒子高度
          const itemHeight = group?.height;
          this.showFixedBtn = window.innerHeight <= itemHeight + this.itemTop;
        });
      },
      immediate: true,
      deep: true,
    },
  },
  methods: {
    // 获取下拉框数据
    getEnumList(val) {
      const arr = val.split(";");
      return arr.map((v) => {
        return {
          configEnumDescript: v.split(",")[1],
          configEnumValue: v.split(",")[0],
        };
      });
    },
    getList() {
      listSysConfig({
        pageNum: 1,
        pageSize: 10000,
        configCode: undefined,
        isQuickConfig: true,
      }).then((res) => {
        this.formList = res.data.list;
        res.data.list.forEach((item) => {
          this.form[item.configCode] = item.configValue;
        });
      });
    },
    onSubmit() {
      const formKey = Object.keys(this.form);
      let list = [];
      this.formList.forEach((v, i) => {
        if (v.configCode == formKey[i]) {
          list.push({
            ...v,
            configValue: this.form[formKey[i]] + "",
          });
        }
      });
      saveBasicSysData(list).then((res) => {
        if (res.code == 0) {
          this.getList();
          this.$modal.msgSuccess("配置成功");
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 下拉展开时
    visibleChange(val, item) {
      // if (val) {
      //   getSysConfigEnumList({ configCode: item.configCode }).then((res) => {
      //     this.sysConfigEnums = res.data?.sysConfigEnums;
      //   });
      // }
    },
    handleSelect(val) {
      this.$forceUpdate();
    },

    handleRadio() {
      this.$forceUpdate();
    },
  },
};
</script>
<style scoped lang="scss">
.item {
  font-size: 18px;
  margin: 10px 0;
}
.fixed_btn {
  right: 20px;
  bottom: 30px;
  position: fixed;
  cursor: pointer;
  z-index: 5;
}
</style>
