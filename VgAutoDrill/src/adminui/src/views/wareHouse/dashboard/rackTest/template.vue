<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="库位" prop="code" style="padding-left: 10px">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入库位"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="工艺路线" prop="routeCodes">
        <el-select
          v-model="queryParams.routeCodes"
          placeholder="请选择"
          multiple
          collapse-tags
          :class="
            queryParams.routeCodes && queryParams.routeCodes.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in routeQueryList"
            :key="item.id"
            :label="item.label"
            :value="item.code"
            v-optionTitle
          >
          </el-option>
        </el-select>
      </el-form-item>
    </search-form>
    <el-row
      v-if="rackList.length > 0"
      :style="{
        backgroundColor: '#fff',
        padding: showSearch ? '0 0 10px 10px' : '10px',
      }"
    >
      <el-col :span="5">
        <el-button
          type="info"
          plain
          icon="el-icon-sort"
          @click="toggleExpandAll"
          >展开/折叠</el-button
        >
      </el-col>
      <right-toolbar
        :showSearch.sync="showSearch"
        @queryTable="getList"
        :page="page"
      ></right-toolbar>
    </el-row>
    <el-collapse
      v-model="activeNames"
      class="my-collapse"
      v-if="rackList.length > 0"
    >
      <el-collapse-item
        :name="rack.wareHouseCode"
        v-for="rack in rackList"
        :key="rack.wareHouseCode"
      >
        <div class="collapse-title" slot="title">
          <span class="maintitle">{{ rack.wareHouseCode }}</span>
          <span class="subtitle">展示信息xxxx</span>
        </div>
        <div style="margin: 0 4px 5px 4px; padding: 4px; background: #fff">
          暂无信息
        </div>
        <div class="rack_box">
          <div
            v-for="(item, i) in rack.racks"
            :key="item.id + '' + i"
            class="rack_box_item"
          >
            <div
              class="item"
              :style="{
                borderColor: item.status == 1 ? '#1890ff' : '#ff4949',
              }"
            >
              <li class="rack_li">
                <span
                  class="rack_title"
                  :style="{ color: item.status == 1 ? '#1890ff' : '#ff4949' }"
                >
                  {{ item.code
                  }}{{ item.positionCode ? "(" + item.positionCode + ")" : "" }}
                  {{ item.routeCode ? "(" + item.routeCode + ")" : "" }}
                </span>
              </li>
              <!-- <li class="rack_li" v-if="item.readyOrOnline != undefined">
                <span class="status"
                  >{{ item.readyOrOnline ? "REDAY" : "ONLINE" }}
                </span>
                <el-switch
                  v-hasPermi="['warehouse:rackDashboard:changeStatus']"
                  class="rack_switch"
                  :value="Boolean(item.readyOrOnline)"
                  active-text="REDAY"
                  inactive-text="ONLINE"
                  active-color="#13ce66"
                  inactive-color="#409EFF"
                  @change="(event) => hanldeCommand(event, item)"
                >
                </el-switch>
              </li> -->
              <!-- <div class="times">
                <li class="msg">异常时长：</li>
                <li class="msg">手动时长：</li>
                <li class="msg">无任务时长：</li>
              </div> -->
              <!-- <li class="msg">正在执行:</li> -->
              <li class="rack_li">
                <el-button
                  type="primary"
                  icon="el-icon-paperclip"
                  plain
                  @click="handleView(item)"
                  :disabled="hasPermi([`dashboard:${formName}:change`])"
                  >操作板料</el-button
                >
                <el-button
                  v-debounce
                  type="danger"
                  plain
                  icon="el-icon-upload2"
                  @click="hanldeCommand('ResetStatusCommand', item)"
                  :disabled="hasPermi([`dashboard:${formName}:report`])"
                  >重新上报</el-button
                >
              </li>
              <li
                style="
                  display: flex;
                  flex-wrap: wrap;
                  align-items: center;
                  border-bottom: solid 1px #ccc;
                  padding: 5px;
                "
              >
                <BreathingLight
                  label="是否预约:"
                  color="green"
                  :status="item.appointed"
                />
              </li>
              <li
                v-if="item.appointedMessage"
                style="
                  display: flex;
                  flex-wrap: wrap;
                  align-items: center;
                  padding: 5px;
                  word-break: break-all;
                "
              >
                预约信息：{{ item.appointedMessage }}
              </li>
              <li class="rack-clock">
                <svg-icon icon-class="clock" />
                <count-down :endTime="item.endTime" class="count-down" />
              </li>

              <el-table
                :data="item.siloInfos"
                style="width: 100%; margin-bottom: 10px"
                size="mini"
              >
                <el-table-column
                  prop="itemCode"
                  label="产品编码"
                  min-width="100"
                  show-overflow-tooltip
                  ><template slot-scope="scope">
                    <el-tooltip
                      effect="dark"
                      placement="bottom"
                      content="点击查看库存"
                    >
                      <span
                        class="one-line-red"
                        @click="openItemSearchDialog(scope.row.itemCode)"
                        >{{ scope.row.itemCode }}</span
                      >
                    </el-tooltip>
                  </template>
                </el-table-column>
                <el-table-column
                  prop="siloCode"
                  label="料仓"
                  min-width="100"
                  show-overflow-tooltip
                >
                </el-table-column>
                <el-table-column
                  prop="siloCount"
                  label="数量"
                  align="center"
                  show-overflow-tooltip
                  width="50"
                >
                </el-table-column>
                <el-table-column
                  prop="productStatus"
                  label="状态"
                  show-overflow-tooltip
                  width="60"
                  align="center"
                >
                </el-table-column>
              </el-table>
              <div class="rack-status">
                <el-switch
                  :disabled="hasPermi([`dashboard:${formName}:disabled`])"
                  class="rack_dis_switch"
                  :value="Boolean(item.status)"
                  active-text="启用"
                  inactive-text="禁用"
                  inactive-color="#ff4949"
                  @change="(event) => handleEnabledOrDisabled(event, item)"
                >
                </el-switch>
              </div>
            </div>
          </div>
        </div>
      </el-collapse-item>
      <ItemSearchDialog ref="ItemSearchDialog" />
    </el-collapse>
    <el-empty v-else description="暂无数据"></el-empty>

    <setForm ref="setForm" @getRackList="getList"> </setForm>
  </div>
</template>

<script>
import countDown from "@/components/countDown"; //引入路径，可更改
import {
  unBind,
  enableRack,
  disableRack,
  getFullDatas,
} from "@/api/wareHouse/rack";
import setForm from "../../components/changePanel.vue";
import { mapState } from "vuex";
import { getDropSelectDatas } from "@/api/produce/route";
import { allotsDeviceCommand } from "@/api/device/device";
export default {
  name: "RackDashboard",
  props: {
    deviceKinds: {
      default: [],
      type: Array,
    },
    formName: {
      default: "",
      type: String,
    },
  },
  components: { countDown, setForm },
  data() {
    return {
      page: "rack",
      open: false,
      showSearch: true,
      title: "",
      activeNames: [],
      loading: false,
      rackList: [],
      routeQueryList: [],
      // 板料信息、
      panelList: [],
      total: 0,
      form: {},
      rules: {},
      // 查询参数
      queryParams: {
        code: undefined,
        routeCodes: [],
      },
      maxHeight: 0,
      itemList: [],
      timer: null,
    };
  },
  activated() {
    this.queryParams = localStorage.getItem(this.formName + "_db_searchForm")
      ? JSON.parse(localStorage.getItem(this.formName + "_db_searchForm"))
      : {
          code: undefined,
          routeCodes: [],
        };
    // 折叠面板当前展开  默认展开第一个
    this.activeNames = localStorage.getItem(this.formName + "_activeNames")
      ? JSON.parse(localStorage.getItem(this.formName + "_activeNames"))
      : [this.rackList[0]?.wareHouseCode];
    this.getRouteList();
    this.getList();
  },
  deactivated() {
    this.closeTimer();
  },

  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
    ...mapState({
      warehouse_dashboard_interval: (state) =>
        state.personalized.warehouse_dashboard_interval * 1000,
    }),
  },
  watch: {
    activeNames(val) {
      localStorage.setItem(this.formName + "_activeNames", JSON.stringify(val));
    },
    warehouse_dashboard_interval: {
      handler(val) {
        if (this.timer) {
          clearInterval(this.timer);
          this.timer = null;
        }
        // this.timer = setInterval(() => {
        //   setTimeout(() => {
        //     this.getList(); //调用接口的方法
        //   }, 0);
        // }, val);
      },
      deepL: true,
      immediate: true,
    },
    queryParams: {
      handler(val) {
        localStorage.setItem(
          this.formName + "_db_searchForm",
          JSON.stringify(val)
        );
      },
      deep: true,
    },
  },
  methods: {
    // 开启定时器
    openTimer() {
      if (this.timer) {
        clearInterval(this.timer);
        this.timer = null;
      }
      // 每隔5秒自动刷新
      // this.timer = setInterval(() => {
      //   setTimeout(() => {
      this.getList(); //调用接口的方法
      //   }, 0);
      // }, this.warehouse_dashboard_interval);
    },
    // 关闭定时器
    closeTimer() {
      clearInterval(this.timer);
      this.timer = null;
    },
    getList() {
      this.queryParams.deviceKinds = this.deviceKinds;
      //获取库位视图数据
      getFullDatas(this.queryParams).then((res) => {
        if (res?.code == 0) {
          this.rackList = res?.data.map((v) => {
            return {
              ...v,
              racks: v.racks.map((rack, i) => {
                return {
                  ...rack,
                  siloStatus: 1,
                  endTime: `2024/07/11 14:${i < 0 ? "9" + i : i}:00`,
                };
              }),
            };
          });
        } else {
          this.rackList = [];
        }
      });
    },
    // 查询工艺路线
    getRouteList() {
      getDropSelectDatas({ vettingStatus: 1, pageNum: 1, pageSize: 100 }).then(
        (res) => {
          this.routeQueryList = res.data?.map((v) => {
            return { ...v, label: `${v.code}(${v.name})` };
          });
        }
      );
    },
    // 展开折叠
    toggleExpandAll() {
      if (this.activeNames.length == this.rackList.length) {
        this.activeNames = [];
      } else {
        this.activeNames = [];
        // 注意：由于每点开一个的单独面板 activeName都会发生变化，所以点击全部展开的时候要将activeName置空
        for (const collapseTitleData of this.rackList) {
          this.activeNames.push(collapseTitleData.wareHouseCode);
        }
      }
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.handleQuery();
    },
    handleReadyOrOnline(value, item) {
      item.ReadyOrOnline = value;
    },
    // 启用禁用
    handleEnabledOrDisabled(value, item) {
      let text = value ? "启用" : "禁用";
      let api = value ? enableRack : disableRack;
      const that = this;
      this.$modal
        .confirm(`确定${text}<span style="color:red"> ${item.code} </span>?`, {
          dangerouslyUseHTMLString: true, // 使用HTML片段
        })
        .then(function () {
          api({ code: item.code }).then((result) => {
            if (result.code == 0) {
              that.$modal.msgSuccess(text + "成功");
              that.getList();
              item.status = item.status === 0 ? 1 : 0;
            } else {
              item.status = item.status === 0 ? 0 : 1;
              this.$modal.notifyError(result.message);
            }
          });
        })
        .catch(function () {
          item.status = item.status === 0 ? 0 : 1;
        });
    },

    // 绑定料仓
    handleView(row) {
      // this.$refs.setForm.open = true;
      // this.$refs.setForm.form = { ...row };
      // this.$refs.setForm.title = `绑定料仓---(库位：${row.code})`;
      // this.$refs.setForm.queryParams.siloCode = row.siloCode;
      // this.$refs.setForm.getList();
      this.$refs.setForm.showFlag = true;
      this.$refs.setForm.topForm = { ...row };
      this.$refs.setForm.oldSiloCode = row.siloCode;
      this.$refs.setForm.dialogTitle = `操作板料`;
      this.$refs.setForm.queryParams.siloCode = row.siloCode;
      this.$refs.setForm.getList();
    },
    async hanldeCommand(val, item) {
      let command = "";
      let label = "";
      if (typeof val != "string") {
        command = val ? "ResetStatusCommand" : "SetOnlineCommand";
        label = `<span style="color:red"> ${item.code}</span> 当前状态为${
          item.readyOrOnline ? "REDAY" : "ONLINE"
        },确定修改状态？`;
      } else {
        command = val;
        label = `确定对<span style="color:red"> ${item.code}</span> 执行重新上报操作？`;
      }
      const res = await this.$modal
        .confirm(label, {
          dangerouslyUseHTMLString: true, // 使用HTML片段
        })
        .catch(() => {});
      if (!res) return;

      allotsDeviceCommand({
        locationCode: item.code,
        command,
      }).then((res) => {
        if (res.code != 0) {
          this.$modal.notifyError(res.message);
        } else {
          this.$modal.msgSuccess("设置成功");
          item.readyOrOnline = val ? 1 : 0;
          this.getList();
        }
      });
    },
    // 点击产品编码打开弹框
    openItemSearchDialog(code) {
      this.$refs.ItemSearchDialog.open = true;
      this.$refs.ItemSearchDialog.title = `查看库存--(${code})`;
      this.$refs.ItemSearchDialog.queryParams.itemCode = code;
      this.$refs.ItemSearchDialog.getList();
    },
  },
};
</script>
<style lang="scss" scoped>
.app-container {
  background-color: #f0f2f5;
}
.rack_box {
  display: flex;
  flex-wrap: wrap;
  width: 100%;
  font-size: 14px;
}
.rack_box_item {
  overflow: hidden;
  color: #303133;
  transition: 0.3s;
  flex-shrink: 0;
  width: calc(100% / 4);
  padding: 4px;
  position: relative;

  @media screen and (min-width: 1500px) and (max-width: 1920px) {
    width: calc(100% / 4);
  }
  @media screen and (min-width: 1050px) and (max-width: 1500px) {
    width: calc(100% / 3);
  }
  @media screen and (min-width: 768px) and (max-width: 1050px) {
    width: calc(100% / 2);
  }
  @media screen and (max-width: 768px) {
    width: calc(100% / 2);
  }
}
.item {
  background-color: #fff;
  border-radius: 4px;
  border: solid 1px transparent;
}
.times {
  padding: 5px;
  border-bottom: solid 1px #ccc;
  color: red;
  .msg {
    border-bottom: none;
  }
}
.msg {
  padding: 5px;
  // margin-bottom: 5px;
  display: flex;
  word-break: break-all;
  border-bottom: solid 1px #ccc;
}
.rack_li {
  .status {
    font-size: 15px;
    padding: 0px 6px;
    letter-spacing: 1px;
  }
  &.silocode {
    justify-content: flex-start;
  }
  > span {
    padding: 0 2px;
  }
  .el-button {
    padding: 3px 6px !important;
  }
  padding: 5px;
  border-bottom: solid 1px #ccc;
  display: flex;
  align-items: center;
  justify-content: space-between;
  .rack_title {
    min-width: 30px;
    font-size: 18px;
    display: block;
    word-break: break-all;
    width: calc(100% - 50px);
  }
}
.rack-clock {
  padding: 5px;
  border-top: solid 1px #ccc;
  word-break: break-all;
}
.rack-status {
  position: absolute;
  right: 10px;
  top: 14px;
  color: #409eff !important;
}

.count-down {
  margin-left: 5px;
  font-size: 14px;
}
// ::v-deep .el-dialog__body {
//   padding-top: 10px !important;
// }

.my-collapse {
  .collapse-title {
    margin-left: 20px;
    display: flex;
    align-content: center;
  }
  .maintitle {
    font-size: 16px;
    font-weight: bold;
    color: #000;
  }
  .subtitle {
    margin-left: 10px;
  }
  ::v-deep .el-collapse-item__header {
    position: relative;
  }
  ::v-deep .el-collapse-item__arrow {
    position: absolute;
    left: 4px;
  }
  ::v-deep .el-collapse-item__content {
    background-color: rgb(240, 242, 245);
    padding-bottom: 0;
    padding-top: 10px;
  }
}
</style>
