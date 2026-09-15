<template>
  <el-dialog
    v-if="panelList.length > 0 && open"
    :title="code"
    :visible.sync="open"
    width="90%"
    append-to-body
    :close-on-click-modal="false"
    v-dialogClose
    v-dialogDrag
    custom-class="drill_dialog"
  >
    <div class="wrapper" v-if="panelList.length > 0">
      <div v-for="(item, index) in panelList" :key="index">
        <el-table
          border
          :data="item.panelDetailDtos"
          :header-cell-class-name="headerCellClassName"
        >
          <el-table-column align="center" width="65px" :resizable="false">
            <template slot="header">
              <div class="elHeadCon">
                <div class="headerCon1">
                  {{ types.find((v) => v.value == item.layer).label }}
                </div>
                <span class="headerCon2">{{
                  item.layer == 0 ? "轴号" : "层号"
                }}</span>
              </div>
            </template>
            <template slot-scope="scope">
              {{ scope.row.splindleIndex }}
            </template>
          </el-table-column>

          <el-table-column label="板料编码" min-width="160px" prop="panelCode">
          </el-table-column>
          <el-table-column
            label="物料"
            prop="itemCode"
            min-width="150px"
            show-overflow-tooltip
          >
          </el-table-column>
        </el-table>
      </div>
    </div>
    <el-empty description="暂无数据" v-else></el-empty>
  </el-dialog>
</template>

<script>
import { getPanelList, loadPanelDetailData } from "@/api/device/device";
export default {
  props: ["code"],
  data() {
    return {
      open: false,
      panelList: [],
      types: [
        {
          label: "生料仓",
          value: 0,
        },
        {
          label: "钻机",
          value: 1,
        },
        {
          label: "熟料仓",
          value: 2,
        },
      ],
      type: null,
    };
  },

  watch: {
    open(val) {
      if (!val) {
        this.$emit("changeSetInterval");
        // this.getList(this.code);
        document
          .querySelectorAll(".wrapper input[type=checkbox]")
          .forEach((v) => {
            v.checked = false;
          });
      }
    },
    panelList(val) {
      if (val.length <= 0 && !this.open) {
        this.$modal
          .confirm("暂无数据,请加载初始数据！")
          .then((result) => {
            if (result == "confirm") {
              loadPanelDetailData({ deviceCode: this.code })
                .then((res) => {
                  if (res.code == 0) {
                    setTimeout(() => {
                      this.getList(this.code);
                    }, 500);
                  } else {
                    this.$modal.notifyError(res.message);
                  }
                })
                .catch(() => {});
            }
          })

          .catch(() => {});
      } else {
        this.open = true;
      }
    },
  },
  methods: {
    headerCellClassName({ row, column, rowIndex, columnIndex }) {
      if (rowIndex == 0 && columnIndex == 0) {
        return "elChgTbeClmn";
      }
    },
    getList(deviceCode) {
      getPanelList({ deviceCode }).then((res) => {
        this.panelList = res.data;
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.wrapper {
  display: flex;
  overflow-x: auto;
  overflow-y: hidden;
  padding: 0 2px;
  > div {
    flex: 1;
    &:not(&:first-child) {
      padding-left: 10px;
    }
  }
}
::v-deep .drill_dialog .el-dialog__body {
  padding: 0px 20px 10px 20px !important;
}
::v-deep .elChgTbeClmn {
  padding: 0px !important;
  .cell {
    padding: 0px !important;
  }
}
.elHeadCon {
  position: relative;
  width: 100%;
  height: 40px;
  box-sizing: border-box;
  line-height: 60px;
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
  left: 5px;
  bottom: 0;
  color: #d4515e;
}
.headerCon2 {
  position: absolute;
  right: 5px;
  top: 0;
}
</style>
