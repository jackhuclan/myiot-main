<template>
  <view>
    <view class="uni-header">
      <view class="uni-group">
        当前钻机：<text style="font-weight: bold">{{ deviceCode }}</text>
      </view>
    </view>

    <view class="uni-container">
      <!-- 生料熟料区 -->
      <uni-row style="margin-bottom: 10px" :gutter="20" v-if="!deviceIsAuto">
        <uni-col
          :span="12"
          :xs="24"
          :class="currentForm == 'left' ? 'active' : ''"
        >
          <LeftForm
            ref="left_form"
            :lotList="lotList"
            @handleBtn="handleSubmitLeft"
            @changeLot="changeLot"
            @changeIsShow="changeIsShow"
          />
        </uni-col>
        <uni-col
          :span="12"
          :xs="24"
          :class="currentForm == 'right' ? 'active' : ''"
        >
          <RightForm
            ref="right_form"
            :lotList="lotList"
            @handleBtn="handleSubmitRight"
          />
        </uni-col>
      </uni-row>
      <uni-table :loading="loading" border stripe emptyText="没有更多数据">
        <uni-tr>
          <uni-th
            align="center"
            :width="50"
            v-if="taskList.length > 0 && !deviceIsAuto && isShow"
          ></uni-th>
          <uni-th align="center">生产任务</uni-th>
          <uni-th class="operate-td" align="center">操作</uni-th>
        </uni-tr>
        <uni-tr v-for="(item, index) in taskList" :key="index">
          <uni-td
            align="center"
            style="vertical-align: middle"
            v-if="!deviceIsAuto && isShow"
          >
            <radio-group @change="radioChange" style="width: 30px">
              <radio
                :value="item.id + ''"
                :checked="changeTaskId == item.id"
              /> </radio-group
          ></uni-td>
          <uni-td>
            <view class="table-content" style="width: 100%">
              <view>
                产品编码：<span class="bold"> {{ item.itemCode }} </span>
              </view>
              <view>
                产品名称：<span class="bold"> {{ item.itemName }} </span>
              </view>
              <view> 任务ID：{{ item.code }} </view>
              <view> 组计划ID：{{ item.cutterGroupNo }} </view>
              <view>
                组板数：<span class="bold">{{ item.nowWadCount }} </span>
              </view>
              <view>
                PNL数：<span class="bold">{{ item.quantity }} </span>
              </view>
              <view>
                任务状态：

                <uni-tag
                  :text="getTaskStatus(item.taskStatus)"
                  :style="{
                    fontWeight: 'bold',
                    border: 'none',
                    letterSpacing: '2px',
                    background: getTaskBackground(item.taskStatus),
                  }"
                />
              </view>
              <view> 预计耗时（分钟）：{{ item.plannedTime }} </view>
              <view> 预计开始时间：{{ item.startTime }} </view>
              <view> 实际开始时间：{{ item.realStartTime }} </view>
              <view> 实际完成时间：{{ item.realEndTime }} </view>
            </view>
          </uni-td>

          <uni-td align="center" class="operate-td">
            <button size="mini" @click="handleValidate(item)">刀盒校验</button>
            <button size="mini" @click="handleLoadATP(item)">加载ATP</button>
            <button size="mini" @click="handleLoadDrillBelt(item)">
              加载钻带
            </button>

            <button size="mini" @click="handleBegin(item)">开始</button>

            <button size="mini" @click="handleFinish(item)" type="primary">
              完成
            </button>
          </uni-td>
        </uni-tr>
      </uni-table>
    </view>
    <validate-dialog ref="validate-dialog" />
  </view>
</template>
<script>
import {
  listTask,
  getAgvTaskByLocationCode,
  getAgvRunStatus,
  udateTaskStatus,
} from "@/api/home";
import config from "./config";
import LeftForm from "@/components/leftForm";
import RightForm from "@/components/rightForm";
import { loadingATPFile, loadingDrillFile } from "@/api/home";
import ValidateDialog from "./validateDialog.vue";
export default {
  mixins: [config],
  components: { LeftForm, RightForm, ValidateDialog },
  data() {
    return {
      loading: false,
      lotList: [],
      taskList: [],
      // 当前操作的是那个表单
      currentForm: null,
      // 选中任务Id
      changeTaskId: undefined,
      // 定时器
      timer: null,
      isShow: false,
    };
  },
  mounted() {
    this.getAgvOperate();
  },
  onShow() {
    this.getTaskList();
    // 设置一个定时器，每5秒重复执行一次
    this.timer = setInterval(() => {
      this.fetchData();
    }, 5000);
  },
  onHide() {
    this.clearForm();
    if (this.timer) {
      clearInterval(this.timer);
      this.timer = null;
    }
  },
  onUnload() {
    if (this.timer) {
      clearTimeout(this.timer);
      this.timer = null;
    }
  },
  computed: {
    // 叫料时选择lot任务列表联动选中
    checked() {
      return (val) => {
        return this.changeTaskId == val.id;
      };
    },
    getTaskStatus() {
      return (status) => {
        return this.taskOptions.find((v) => v.value == status)?.label
          ? this.taskOptions.find((v) => v.value == status)?.label
          : "";
      };
    },
    getTaskBackground() {
      return (status) => {
        return this.taskOptions.find((v) => v.value == status)?.color
          ? this.taskOptions.find((v) => v.value == status)?.color
          : "";
      };
    },
    deviceIsAuto() {
      return this.$store.state.setting.deviceIsAuto;
    },
    deviceCode() {
      return this.$store.state.setting.deviceCode;
    },
    rawLocationCodes() {
      return this.$store.state.setting.rawLocationCodes;
    },
    clinkerLocationCodes() {
      return this.$store.state.setting.clinkerLocationCodes;
    },
  },

  methods: {
    // 获取任务列表
    getTaskList() {
      if (!this.deviceCode) return (this.taskList = []);
      listTask({
        deviceCode: this.deviceCode,
        taskStatusList: [10, 20, 30, 40],
      }).then((res) => {
        this.taskList = res.data.list;
        const lotList = [...new Set(this.taskList.map((v) => v.itemCode))];
        this.lotList = lotList;
      });
    },
    // 获取当前操作
    getAgvOperate() {
      if (this.deviceIsAuto) return;
      const rawLocationCode = this.rawLocationCodes[0];
      const clinkerLocationCode = this.clinkerLocationCodes[0];
      // 生料区当前操作
      getAgvTaskByLocationCode(rawLocationCode).then((res) => {
        const label = this.getLabel("leftList", res.agvOperateType);
        if (res.agvOperateType == 3) {
          this.$refs.left_form.currentOperate =
            // isBind=false 解绑成功,否则失败
            label + "--" + `${!res.isBind ? "成功" : "失败"}`;
        } else {
          this.$refs.left_form.currentOperate = label;
        }
      });
      // 熟料区当前操作
      getAgvTaskByLocationCode(clinkerLocationCode).then((res) => {
        const label = this.getLabel("rightList", res.agvOperateType);
        if (res.agvOperateType == 6) {
          // isBind=true绑定成功 ,否则失败
          this.$refs.right_form.currentOperate =
            label + "--" + `${res.isBind ? "成功" : "失败"}`;
        } else {
          this.$refs.right_form.currentOperate = label;
        }
      });
      this.fetchData();
    },
    // 循环获取当前运行状态
    fetchData() {
      const rawLocationCode = this.rawLocationCodes[0];
      const clinkerLocationCode = this.clinkerLocationCodes[0];
      if (rawLocationCode) {
        // 生料区运行状态
        getAgvRunStatus(rawLocationCode)
          .then((res) => {
            this.$refs.left_form.operationalStatus = res.data.agvRunStatus;
          })
          .catch(() => {
            if (this.timer) {
              clearInterval(this.timer);
              this.timer = null;
            }
          });
      }
      if (clinkerLocationCode) {
        // 熟料区运行状态
        getAgvRunStatus(clinkerLocationCode)
          .then((res) => {
            this.$refs.right_form.operationalStatus = res.data.agvRunStatus;
          })
          .catch(() => {
            if (this.timer) {
              clearInterval(this.timer);
              this.timer = null;
            }
          });
      }
    },
    // 情空表单
    clearLeftForm() {
      if (this.deviceIsAuto) return;
      this.changeTaskId = undefined;
      this.$refs.left_form.initialForm = this.$refs.left_form.form = {
        lot: undefined,
        podCode: undefined,
        locationCode: undefined,
        // 操作类型
        agvOperateType: undefined,
      };
    },
    clearRightForm() {
      if (this.deviceIsAuto) return;
      this.$refs.right_form.initialForm = this.$refs.right_form.form = {
        lot: undefined,
        podCode: undefined,
        locationCode: undefined,
        clinkerMaterialNum: 0,
        // 操作类型
        agvOperateType: undefined,
      };
    },
    clearForm() {
      this.clearLeftForm();
      this.clearRightForm();
    },
    changeIsShow(v) {
      this.isShow = v;
    },
    changeLot() {
      if (!this.$refs.left_form.form.lot)
        return (this.changeTaskId = undefined);
      // 任务列表itemCode是否有重复项
      const isRepeat = this.taskList.filter(
        (v) => v.itemCode == this.$refs.left_form.form.lot
      );
      this.changeTaskId = isRepeat[0].id + "";
    },

    // 左侧按钮操作
    handleSubmitLeft() {
      this.currentForm = "left";
      this.clearRightForm();
    },
    // 右侧按钮操作
    handleSubmitRight() {
      this.currentForm = "right";
      this.clearLeftForm();
    },
    handleCommon(label, api, data) {
      this.$modal
        .confirm(label, {
          cancelText: "否",
          confirmText: "是",
        })
        .then((result) => {
          if (result) {
            api(data).then((res) => {
              if (res.code == 1) {
                this.$modal.showToast(res.message);
              } else {
                this.getTaskList();
                this.$modal.showToast("操作成功");
              }
            });
          }
        });
    },
    // 刀盒检验
    handleValidate(item) {
      if (!item.cutterGroupNo) return this.$modal.showToast("不存在组计划ID");
      this.$refs["validate-dialog"].groupNo = item.cutterGroupNo;
      this.$refs["validate-dialog"].openPopup();
    },
    // 加载ATP
    handleLoadATP(item) {
      if (item.disabled) return this.$modal.showToast("请勿重复点击！");
      //  禁用按钮
      this.$set(item, "disabled", true); // 5秒后解除禁用
      setTimeout(() => {
        this.$set(item, "disabled", false);
      }, 5000);

      const data = {
        deviceCode: this.deviceCode,
        cutterGroupNo: item.cutterGroupNo,
      };
      this.$modal
        .confirm("是否确认加载?", {
          cancelText: "否",
          confirmText: "是",
        })
        .then((result) => {
          if (result) {
            loadingATPFile(data).then((res) => {
              if (res.code == "Success") {
                this.getTaskList();
                this.$modal.showToast("操作成功");
              } else {
                this.$modal.showToast(res.message);
              }
            });
          }
        });
    },
    // 加载钻带
    handleLoadDrillBelt(item) {
      const data = {
        deviceCode: this.deviceCode,
        itemCode: item.itemCode,
      };
      this.handleCommon("是否确认加载?", loadingDrillFile, data);
    },
    // 开始
    handleBegin(item) {
      const data = { code: item.code, taskStatus: 40 };
      this.handleCommon(
        "钻带参数和ATP文件是否正确加载?",
        udateTaskStatus,
        data
      );
    },
    // 完成
    handleFinish(item) {
      const data = { code: item.code, taskStatus: 50 };
      this.handleCommon("是否确认完成?", udateTaskStatus, data);
    },

    // 表格单选
    radioChange: function (evt) {
      this.changeTaskId = undefined;
      for (let i = 0; i < this.taskList.length; i++) {
        if (this.taskList[i].id == evt.detail.value) {
          this.changeTaskId = evt.detail.value;
          this.$refs.left_form.form.lot = this.taskList[i].itemCode;
        }
      }
    },
  },
};
</script>
<style lang="scss" scoped>
.table-content {
  display: flex;
  flex-wrap: wrap;
  view {
    min-width: 200px;
    padding: 2px 5px;
  }
  .bold {
    font-weight: bold;
    word-break: break-all;
    color: #000;
  }
}
@media screen and (max-width: 1028px) {
  .operate-td {
    width: 140px !important;
    padding: 5px !important;
    uni-button {
      margin: 5px 0;
    }
    justify-content: center;
    align-items: center;
  }
}
.operate-td {
  width: 350px;
  padding: 0;
  uni-button {
    margin: 0 5px;
  }
  view {
    display: flex;
    flex-wrap: wrap;
  }
}

::v-deep .uni-combox__input-plac {
  font-size: 12px !important;
}

::v-deep .uni-table-scroll {
  min-height: 100px !important;
}
::v-deep uni-radio .uni-radio-input,
uni-checkbox .uni-checkbox-input {
  width: 15px !important;
  height: 15px !important;
}
/* radio 选中后的样式 */
::v-deep uni-radio .uni-radio-input.uni-radio-input-checked {
  background-color: #248067 !important;
  border-color: #248067 !important;
  background-clip: content-box !important;
  padding: 2rpx !important;
  box-sizing: border-box;
}

/* radio 选中后的图标样式*/
::v-deep uni-radio::before {
  display: none !important;
}
::v-deep uni-checkbox::before {
  display: none !important;
}
::v-deep uni-radio .uni-radio-input.uni-radio-input-checked:before {
  display: none !important;
}
// 表格头部样式
.uni-table-tr:first-child {
  background: #f8f8f9;
  .uni-table-th {
    color: #000;
  }
}
</style>
