<template>
  <view class="rack-l">
    <view class="common"
      ><text class="label">生料区:</text>
      <view>
        <text v-for="(v, i) in locationCodes" :key="i">
          {{ v
          }}{{
            locationCodes.length > 1 && i + 1 != locationCodes.length
              ? "，"
              : ""
          }}
        </text>
      </view>
    </view>
    <view class="common"
      ><text class="label">当前操作:</text>
      <text>
        {{ currentOperate }}
      </text></view
    >

    <view class="common"
      ><text class="label">运行状态:</text
      ><text> {{ operationalStatus }}</text></view
    >
    <view class="common">
      <button
        v-for="item in btnInfo.leftList"
        :key="item.type"
        size="mini"
        :style="{
          background: form.agvOperateType == item.type ? 'yellow' : '',
        }"
        @click="handleButton(item)"
      >
        {{ item.label }}
      </button></view
    >
    <uni-forms
      style="margin-top: 5px"
      ref="valiFormL"
      :rules="rulesInfo.leftRules"
      :modelValue="form"
    >
      <!-- 
            lot号：叫料、上机解绑
            托盘号：退空盘 、上机解绑
            -->
      <uni-row>
        <uni-col :span="24" :xs="24" :sm="24" :lg="12">
          <uni-forms-item
            label="lot号"
            required
            name="lot"
            v-if="form.agvOperateType == 1 || form.agvOperateType == 3"
          >
            <combox
              v-if="lotList.length > 0"
              :candidates="lotList"
              :disabled="isDisabled"
              placeholder="请选择"
              v-model="form.lot"
              @input="handleChange"
            >
            </combox>
            <uni-easyinput
              style="width: 200px"
              v-else
              :disabled="isDisabled"
              v-model="form.lot"
              placeholder="请输入Lot号"
          /></uni-forms-item>
        </uni-col>
        <uni-col :span="24" :xs="24" :sm="24" :lg="12">
          <uni-forms-item
            label="托盘号"
            required
            name="podCode"
            v-if="form.agvOperateType == 2 || form.agvOperateType == 3"
          >
            <uni-easyinput
              style="width: 200px"
              :disabled="isDisabled"
              v-model="form.podCode"
              placeholder="请输入托盘号"
            /> </uni-forms-item
        ></uni-col>
      </uni-row>
      <uni-row>
        <uni-col>
          <button
            type="primary"
            style="float: right"
            size="mini"
            @click="handleSubmit('valiFormL')"
            :disabled="isDisabled"
          >
            提交
          </button>
        </uni-col>
      </uni-row>
    </uni-forms>
  </view>
</template>

<script>
import config from "@/pages/index/config";
import { getAgvTaskByLocationCode, agvOperate } from "@/api/home";
export default {
  mixins: [config],
  props: {
    lotList: {
      type: Array,
      default: [],
    },
    currentForm: {
      type: String,
      default: "",
    },
  },
  data() {
    return {
      // 初始化时设置为 false，表示按钮不被禁用
      isDisabled: false,
      // 表单数据
      form: {
        // 操作类型
        agvOperateType: undefined,
        lot: undefined,
        podCode: undefined,
      },
      initialForm: {},
      // 当前操作
      currentOperate: undefined,
      // 运行状态
      operationalStatus: undefined,
    };
  },
  watch: {
    "form.agvOperateType": {
      handler(val) {
        this.$emit("changeIsShow", val === 1);
      },
      deep: true,
    },
  },
  computed: {
    isFormModified() {
      let form = {};
      for (let key in this.form) {
        if (this.form[key] === "" || key == "agvOperateType") {
          form[key] = undefined;
        } else {
          form[key] = this.form[key];
        }
      }
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      return JSON.stringify(form) !== JSON.stringify(this.initialForm);
    },
    deviceCode() {
      return this.$store.state.setting.deviceCode;
    },
    locationCodes() {
      console.log(this.$store.state.setting.rawLocationCodes);
      return this.$store.state.setting.rawLocationCodes;
    },
  },
  methods: {
    handleChange() {
      this.$emit("changeLot");
    },
    handleButton(item) {
      if (!this.deviceCode)
        return this.$modal.showToast("当前未选择钻机，无法操作");
      this.$emit("handleBtn");
      this.handleFormModified(item.type);
    },
    handleFormModified(type) {
      if (this.form.agvOperateType) {
        if (this.isFormModified) {
          this.$modal
            .confirm("切换操作将清空当前表单信息", { title: "生料区" })
            .then((result) => {
              this.changeForm(type);
            });
        } else {
          this.changeForm(type);
        }
      } else {
        this.initialForm = Object.assign(
          {},
          { ...this.form, agvOperateType: undefined }
        );
        this.changeForm(type);
      }
    },
    // 左侧是否改变
    changeForm(type) {
      if (type == 2) {
        const localCode =
          this.locationCodes.length > 0 ? this.locationCodes[0] : "";
        getAgvTaskByLocationCode(localCode).then((res) => {
          this.form.podCode = res.podCode;
        });
      } else if (type == 3) {
        const localCode =
          this.locationCodes.length > 0 ? this.locationCodes[0] : "";
        getAgvTaskByLocationCode(localCode).then((res) => {
          this.form.lot = res.itemCode;
          this.form.podCode = res.podCode;
        });
      } else {
        this.form.lot = undefined;
        this.form.podCode = undefined;
      }
      this.isDisabled = false;
      this.form.agvOperateType = type;
    },
    handleSubmit(ref) {
      if (!this.form.agvOperateType)
        return this.$modal.showToast("生料区未选择任何操作");

      this.$refs[ref].validate((res) => {
        if (!res) {
          this.$modal
            .confirm("确认提交生料区信息？", {
              cancelText: "否",
              confirmText: "是",
              title: "生料区",
            })
            .then((result) => {
              if (result) {
                this.$emit("update");
                agvOperate({
                  ...this.form,
                  locationCode: this.locationCodes[0],
                  deviceCode: this.deviceCode,
                }).then((res) => {
                  const label = this.getLabel(
                    "leftList",
                    this.form.agvOperateType
                  );
                  if (res.code == 0) {
                    this.isDisabled = true;

                    this.$modal.showToast(label + "成功");
                    // 提交成功后更新当前操作
                    this.currentOperate = label;
                  } else {
                    this.$modal.showToast(label + "失败," + res.message);
                  }
                });
              }
            });
        }
      });
    },
  },
};
</script>

<style lang="scss" scoped>
@import "@/common/form.scss";
</style>
