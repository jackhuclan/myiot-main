<template>
  <view style="overflow-x: auto">
    <view class="uni-header">
      <view class="uni-group">
        <!-- 输入框 -->
        选择钻机:
        <combox
          style="margin-left: 10px; width: 120px"
          :candidates="['全部', '自动', '非自动']"
          placeholder="请选择"
          v-model="queryParams.isAuto"
          @clear="clearInp"
          @input="changeIsAuto"
        >
        </combox>
        <combox
          :candidates="drillList.map((v) => v.label)"
          placeholder="请选择"
          v-model="deviceCode"
          @clear="clearInp"
          @input="handleInput"
        >
        </combox>
        <!-- 搜索按钮 -->
        <button
          type="primary"
          size="mini"
          style="margin-left: 10px"
          :disabled="deviceCode === ''"
          @click="setCurrDevcie"
        >
          设定当前钻机
        </button>
      </view>
    </view>
    <view class="uni-container">
      <!-- 表格组件 -->
      <uni-table :loading="loading" border stripe emptyText="没有更多数据">
        <uni-tr>
          <!-- 表头列 -->
          <uni-th>钻机编号</uni-th>
          <uni-th>库位编号</uni-th>
          <uni-th align="center">库位类型</uni-th>
          <uni-th>lot号</uni-th>
          <uni-th>托盘号</uni-th>
          <uni-th width="130" align="center">lot是否绑定托盘</uni-th>
          <uni-th width="100" align="center">状态</uni-th>
          <uni-th width="100" align="center" class="fixed">操作</uni-th>
        </uni-tr>
        <uni-tr v-for="(item, index) in tableData" :key="index">
          <!-- 表格数据列 -->
          <uni-td>{{ item.deviceCode }}</uni-td>
          <uni-td>{{ item.locationCode }}</uni-td>
          <uni-td align="center">{{ item.locationTypeName }}</uni-td>
          <uni-td>{{ item.itemCode }}</uni-td>
          <uni-td>{{ item.podCode }}</uni-td>
          <uni-td align="center">{{ item.isBind ? "是" : "否" }}</uni-td>
          <uni-td align="center">
            {{ getTaskStatus(item.taskStatus) }}
          </uni-td>

          <uni-td class="fixed">
            <view class="uni-group" @click="toggle(item)">
              <button size="mini" type="primary">修改</button>
            </view>
          </uni-td>
        </uni-tr>
      </uni-table>
    </view>
    <!-- #ifndef H5 -->
    <fix-window />
    <!-- #endif -->
    <uni-popup
      ref="popup"
      :is-mask-click="false"
      background-color="#fff" 
    >
      <view class="popup-content">
        <uni-icons
          type="closeempty"
          class="close-fixed"
          @click="$refs.popup.close()"
        ></uni-icons>
        <view class="popup-title">
          {{ title }}
        </view>
        <uni-forms ref="baseForm" :modelValue="form">
          <uni-row>
            <uni-col :span="12">
              <uni-forms-item label="钻机编号">
                <uni-easyinput
                  v-model="form.deviceCode"
                  disabled
                  placeholder="请输入钻机编号"
                  style="width: 200px"
                />
              </uni-forms-item>
            </uni-col>
            <uni-col :span="12">
              <uni-forms-item label="库位编号">
                <uni-easyinput
                  disabled
                  v-model="form.locationCode"
                  placeholder="请输入库位编号"
                  style="width: 200px"
                />
              </uni-forms-item>
            </uni-col>
          </uni-row>
          <uni-row>
            <uni-col :span="12">
              <uni-forms-item label="库位类型">
                <uni-easyinput
                  v-model="form.locationTypeName"
                  disabled
                  placeholder="请输入库位类型"
                  style="width: 200px"
                />
              </uni-forms-item>
            </uni-col>
            <uni-col :span="12">
              <uni-forms-item label="lot号">
                <uni-easyinput
                  v-model="form.itemCode"
                  placeholder="请输入lot号"
                  style="width: 200px"
                />
              </uni-forms-item>
            </uni-col>
          </uni-row>
          <uni-row>
            <uni-col :span="12">
              <uni-forms-item label="托盘号">
                <uni-easyinput
                  v-model="form.podCode"
                  placeholder="请输入托盘号"
                  style="width: 200px"
                />
              </uni-forms-item>
            </uni-col>
            <uni-col :span="12">
              <uni-forms-item label="状态">
                <uni-data-select
                  v-model="form.taskStatus"
                  :localdata="range.filter((v) => v.value > 3)"
                >
                </uni-data-select> </uni-forms-item
            ></uni-col>
          </uni-row>
          <uni-row
            ><uni-col :span="24">
              <uni-forms-item label="lot是否绑定托盘" class="max-label">
                <uni-data-checkbox v-model="form.isBind" :localdata="sexs" />
              </uni-forms-item>
            </uni-col>
          </uni-row>
        </uni-forms>
        <view class="submit-content">
          <button size="mini" @click="submit" type="primary">确定</button>
          <button size="mini" style="margin-left: 20upx" @click="resetSubmit">
            取消
          </button>
        </view></view
      >
    </uni-popup>
  </view>
</template>

<script>
import { listDrill } from "../../api/home.js";
import { queryByDeviceCode, update } from "../../api/setting.js";

export default {
  // 数据属性
  data() {
    return {
      // 搜索值
      searchVal: "",
      // 表格数据
      tableData: [],
      drillList: [],
      deviceCode: "",
      // 加载状态
      loading: false,
      title: "",
      form: {},
      // 单选数据源
      sexs: [
        {
          text: "是",
          value: 1,
        },
        {
          text: "否",
          value: 0,
        },
      ],
      queryParams: { isAuto: "非自动" },

      range: [
        { value: 1, text: "任务创建" },
        {
          value: 2,
          text: "任务下发",
        },
        {
          value: 3,
          text: "任务回调",
        },
        {
          value: 4,
          text: "任务完成",
        },
        {
          value: 5,
          text: "任务异常",
        },
      ],
    };
  },

  onShow() {
    this.deviceCode = uni.getStorageSync("uni-deviceCode")
      ? uni.getStorageSync("uni-deviceCode")
      : "";
    this.getDrillList();
    this.getData();
  },
  computed: {
    getTaskStatus() {
      return (status) => {
        return this.range.find((v) => v.value == status)?.text
          ? this.range.find((v) => v.value == status)?.text
          : "";
      };
    },
  },
  // 方法
  methods: {
    // 搜索函数
    setCurrDevcie() {
      const item = this.drillList.find((v) => v.label == this.deviceCode);
      if (!item) return;
      this.$store.dispatch("changeIsAuto", item.isAuto);
      this.$store.dispatch("changeCode", this.deviceCode);
      this.$store.dispatch("changeRawLocationCodes", item.rawLocationCodes);
      this.$store.dispatch(
        "changeClinkerLocationCodes",
        item.clinkerLocationCodes
      );
    },
    // 修改
    toggle(item) {
      this.title = "修改信息";
      this.form = { ...item, isBind: item.isBind ? 1 : 0 };
      this.$refs.popup.open();
    },
    // 确定
    submit() {
      update({ ...this.form, isBind: this.form.isBind ? true : false }).then(
        (res) => {
          if (res.code == 0) {
            this.getData();
            this.$modal.showToast("操作成功");
            this.$refs.popup.close();
          } else {
            this.$modal.showToast(res.message);
          }
        }
      );
    },
    // 取消
    resetSubmit() {
      this.$refs.popup.close();
    },
    // 获取当前钻机列表
    getDrillList() {
      listDrill(this.queryParams).then((res) => {
        this.drillList = res.data.list.map((v) => {
          return {
            ...v,
            label: v.code,
            value: v.code,
          };
        });
      });
    },
    changeIsAuto() {
      this.getDrillList();
    },
    // 情空钻机
    clearInp() {},

    handleInput(v) {
      this.getData();
    },
    // 获取数据函数
    getData() {
      this.loading = true;
      queryByDeviceCode(this.deviceCode).then((res) => {
        this.tableData = res.data;
        this.loading = false;
      });
    },
  },
};
</script>
<style lang="scss" scoped>
.uni-container .uni-table-body-wrapper {
  position: relative;
}

.uni-container .uni-table-body {
  display: flex;
}
.fixed {
  position: sticky;
  right: 0;
  z-index: 1;
  border-left: 1px #ebeef5 solid;
  background-color: #ffffff;
}
.popup-content {
  padding: 10px 20px 20px 20px;
  width: 600px;
  height: calc(100vh / 2);
  position: relative;
}
.close-fixed {
  position: absolute;
  right: 10px;
  top: 10px;
}
.popup-title {
  font-size: 18px;
  font-weight: bold;
  margin-bottom: 10px;
}
.max-label {
  ::v-deep .uni-forms-item__label {
    width: 120px !important;
  }
}
::v-deep .uni-forms-item__label {
  padding-right: 10px; /* 标签与输入框间距 */
  text-align: right !important; /* 标签文本右对齐 */
  justify-content: flex-end !important;
}
::v-deep .uni-forms-item__content {
  display: flex;
  align-items: center;
}
.submit-content {
  position: absolute;
  bottom: 10px;
  right: 20px;
}
::v-deep .uni-input-input {
  color: #4e4b4b !important;
}
</style>
