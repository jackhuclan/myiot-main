<template>
  <el-dialog
    :title="'异常处理(记录Id-' + id + ')'"
    :visible.sync="open"
    width="90%"
    append-to-body
    :close-on-click-modal="false"
    v-dialogClose
    v-dialogDrag
    custom-class="drill_dialog"
    :before-close="cancel"
  >
    <div
      class="wrapper"
      v-if="[...this.leftPanelList, ...this.rightPanelList].length > 0"
    >
      <div
        v-for="(item, index) in [...this.leftPanelList, ...this.rightPanelList]"
        :key="index"
        :class="'item-' + (index + 1)"
      >
        <el-row :gutter="10" class="mb8" v-if="index != 2">
          <el-col :span="1.5">
            <el-button
              type="primary"
              v-debounce
              plain
              icon="el-icon-refresh"
              @click="handleSynchronousData(item)"
              >同步</el-button
            >
          </el-col>
          <el-col :span="1.5">
            <el-button
              v-debounce
              type="success"
              plain
              icon="el-icon-finished"
              @click="hanldeIssued(item)"
              >下发</el-button
            >
          </el-col>
        </el-row>

        <el-table border :data="item.panelDetailDtos">
          <el-table-column
            label="层号"
            align="center"
            :prop="item.layer == -1 ? 'floorNum' : 'splindleIndex'"
            class-name="elChgTbeClmn"
            width="50px"
            :resizable="false"
          >
            <template slot="header">
              <div class="elHeadCon">
                <div class="headerCon1">
                  {{ types.find((v) => v.value == item.layer).label }}
                </div>
                <div class="headerCon2">层号</div>
              </div>
            </template>
            <template slot-scope="scope">
              {{
                item.layer == -1 ? scope.row.floorNum : scope.row.splindleIndex
              }}
            </template>
          </el-table-column>
          <el-table-column
            label="板料编码"
            min-width="150px"
            show-overflow-tooltip
            prop="panelCode"
          >
          </el-table-column>
          <el-table-column
            label="物料"
            prop="itemCode"
            min-width="150px"
            show-overflow-tooltip
          >
          </el-table-column>
          <el-table-column
            v-if="item.layer == -1"
            label="板料类型"
            min-width="120"
            align="center"
            prop="productStatus"
          >
            <template slot-scope="scope">
              <status-tag
                :options="$status.productStatusOptions"
                :status="scope.row.productStatus * 1"
              />
            </template>
          </el-table-column>
          <el-table-column
            label="操作"
            align="center"
            fixed="right"
            class-name="small-padding fixed-width"
          >
            <template slot-scope="scope">
              <el-button
                style="position: relative"
                type="text"
                :disabled="!scope.row.panelCode"
                icon="el-icon-sort"
              >
                移动
                <el-cascader
                  v-if="scope.row.panelCode"
                  :key="cascaderKey"
                  class="cascader"
                  v-model="value"
                  @visible-change="
                    (event) => visibleChange(event, scope.row, item.layer)
                  "
                  :options="options"
                  @change="handleChange"
                  ref="cascaderHandle"
                ></el-cascader>
              </el-button>
            </template>
          </el-table-column>
        </el-table>
      </div>
    </div>
    <el-empty description="暂无数据" v-else></el-empty>
  </el-dialog>
</template>

<script>
import { getPanelList } from "@/api/device/device";
import {
  movePanel,
  synchronousPanelData,
  getSiloDetailsByLocation,
  allotsPanelData,
} from "@/api/device/schedulement";

export default {
  props: ["sourceDeviceId", "id", "requireDeviceId"],
  data() {
    return {
      open: false,
      // 左侧数据
      leftPanelList: [],
      // 右侧数据
      rightPanelList: [],
      value: [],
      options: [],
      types: [
        {
          label: "AGV",
          value: -1,
        },
        {
          label: "钻机",
          value: 1,
        },
        {
          label: "生料仓",
          value: 0,
        },
        {
          label: "熟料仓",
          value: 2,
        },
      ],
      type: null,
      sourceDeviceKind: null,
      siloCode: null,
      startData: {},
      targetData: {},
      cascaderKey: 1,
      isMove: false,
    };
  },
  watch: {
    open(val) {
      if (!val) {
        this.$emit("changeSetInterval");
        this.isMove = false;
      }
    },
    //切换数据源
    changeMenu() {
      ++this.cascaderKey;
    },
  },
  methods: {
    changeSynchronousData(deviceCode, requireDeviceId) {
      Promise.all([
        synchronousPanelData({ deviceCode: deviceCode }),
        synchronousPanelData({ deviceCode: requireDeviceId }),
      ])
        .then((results) => {
          // 处理所有Promise都成功的情况
          if (results[0].code != 0 || results[1].code != 0)
            return this.$modal.notifyError(
              results[0].message || results[1].message
            );
        })
        .catch((error) => {
          // 处理任一Promise失败的情况
        });
    },
    getLeftPanelList(location) {
      getSiloDetailsByLocation({ location }).then((res) => {
        if (res.code != 0) return this.$modal.notifyError(res.message);
        this.leftPanelList = [{ layer: -1, panelDetailDtos: res.data }];
        this.siloCode = res.data[0]?.siloCode;
      });
    },
    getRightPanelList(deviceCode) {
      getPanelList({ deviceCode }).then((res) => {
        if (res.code != 0) return this.$modal.notifyError(res.message);
        this.rightPanelList = res.data.filter((v) => v.layer != 1);
      });
    },
    getList(deviceCode, requireDeviceId) {
      this.getLeftPanelList(requireDeviceId);
      this.getRightPanelList(deviceCode);
    },
    visibleChange(flag, row, layer) {
      if (flag) {
        if (layer == -1) {
          this.options = [
            {
              value: 1,
              label: "生料仓",
              children: [...this.leftPanelList, ...this.rightPanelList]
                .find((v) => v.layer == 1)
                ?.panelDetailDtos.map((v) => {
                  return {
                    value: v.splindleIndex,
                    label: v.splindleIndex + "层",
                  };
                }),
            },
            {
              value: 2,
              label: "熟料仓",
              children: [...this.leftPanelList, ...this.rightPanelList]
                .find((v) => v.layer == 2)
                ?.panelDetailDtos.map((v) => {
                  return {
                    value: v.splindleIndex,
                    label: v.splindleIndex + "层",
                  };
                }),
            },
          ];
          this.startData = {
            type: -1,
            locationCode: this.siloCode,
            locationIndex: row.floorNum,
            // 调度设备
            relatedDeviceKind: 6,
          };
        } else {
          this.options = [
            {
              value: -1,
              label: "AGV",
              children: [...this.leftPanelList, ...this.rightPanelList]
                .find((v) => v.layer == -1)
                ?.panelDetailDtos.map((v) => {
                  return {
                    value: v.floorNum,
                    label: v.floorNum + "层",
                  };
                }),
            },
          ];
          this.startData = {
            locationCode: this.sourceDeviceId,
            layer: row.layer,
            locationIndex: row.splindleIndex,
            // 发起设备
            relatedDeviceKind: this.sourceDeviceKind,
          };
        }
      } else {
        this.value = [];
      }
    },
    handleChange(value) {
      this.$refs.cascaderHandle.dropDownVisible = false; //监听值发生变化就关闭它
      this.value = [];

      if (this.startData.type == -1) {
        this.targetData = {
          locationCode: this.sourceDeviceId,
          layer: value[0],
          locationIndex: value[1],
          // 发起设备
          relatedDeviceKind: this.sourceDeviceKind,
        };
      } else {
        this.targetData = {
          locationCode: this.siloCode,
          locationIndex: value[1],
          relatedDeviceKind: 6,
        };
      }
      delete this.startData.type;
      movePanel({
        startData: this.startData,
        targetData: this.targetData,
      }).then((res) => {
        if (res.code == 0) {
          this.$modal.msgSuccess("移动成功");
          this.isMove = true;
          this.getList(this.sourceDeviceId, this.requireDeviceId);
        } else {
          this.isMove = false;
          if (res.code != 0) return this.$modal.notifyError(res.message);
        }
      });
    },
    // 同步
    handleSynchronousData(item) {
      synchronousPanelData({
        deviceCode:
          item.layer == -1 ? this.requireDeviceId : this.sourceDeviceId,
      }).then((res) => {
        if (res.code != 0) return this.$modal.notifyError(res.message);
        item.layer == -1
          ? this.getLeftPanelList(this.requireDeviceId)
          : this.getRightPanelList(this.sourceDeviceId);

        this.$modal.msgSuccess("同步成功");
      });
    },
    hanldeIssued(item) {
      const layers = this.rightPanelList.map((v) => v.layer);
      allotsPanelData({
        deviceCode:
          item.layer == -1 ? this.requireDeviceId : this.sourceDeviceId,
        layers: item.layer == -1 ? undefined : layers,
      }).then((res) => {
        if (res.code != 0) return this.$modal.notifyError(res.message);
        item.layer == -1
          ? this.getLeftPanelList(this.requireDeviceId)
          : this.getRightPanelList(this.sourceDeviceId);
        this.isMove = false;
        this.$modal.msgSuccess("下发成功");
      });
    },
    //弹框关闭时
    async cancel() {
      if (this.isMove) {
        const results = await this.$modal
          .confirm("数据已被处理但未下发 是否要关闭？", {
            confirmButtonText: "是",
            cancelButtonText: "否",
          })
          .catch(() => {});
        if (results == "confirm") {
          this.open = false;
        }
      } else {
        this.open = false;
      }
    },
  },
};
</script>

<style lang="scss" scoped>
.wrapper {
  display: grid;
  grid-template-columns: 1fr 1fr;
  overflow-x: auto;
  .item-1 {
    grid-row: 1 / 3; /* 合并占据 1到2 行，不包括3 */
    grid-column: 1 / 2; /* 合并占据 1到1 列，不包括2 */
    margin-left: 10px;
  }
  .item-2,
  .item-3 {
    margin-left: 10px;

    margin-right: 10px;
  }
}
.cascader {
  position: absolute;
  left: 0;
  top: 0;
  opacity: 0;
  width: 100% !important;
}
::v-deep .drill_dialog .el-dialog__body {
  padding: 0px 0px 10px 0px !important;
}
::v-deep .el-icon-sort {
  transform: rotate(90deg);
}
/* 如果单元格的padding */
::v-deep .el-table__header .elChgTbeClmn .cell {
  padding: 0px !important;
}
::v-deep .elChgTbeClmn.is-center.is-leaf.el-table__cell {
  padding: 0px !important;
}
.elHeadCon {
  position: relative;
  width: 100%;
  height: 40px;
  box-sizing: border-box;
  background: linear-gradient(
    to bottom right,
    transparent 0%,
    transparent calc(50% - 1px),
    #dfe6ec 50%,
    transparent calc(50% + 1px),
    transparent 100%
  );
}

.headerCon1 {
  position: absolute;
  left: 2px;
  top: 0;
  color: #d4515e;
}
.headerCon2 {
  position: absolute;
  right: 0;
  bottom: 0px;
}
::v-deep .el-radio {
  display: block !important;
  margin: 5px 0 5px 7px !important;
}
</style>
