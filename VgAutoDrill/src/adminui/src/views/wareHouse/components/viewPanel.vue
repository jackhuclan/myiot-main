<template>
  <edit-form-dialog
    v-model="showFlag"
    :title="'查看板料'"
    optType="view"
    width="90%"
  >
    <el-descriptions
      :column="4"
      size="medium"
      border
      style="margin-bottom: 10px"
      :contentStyle="content_style"
    >
      <el-descriptions-item label="库位" span="1">{{
        topForm.code
      }}</el-descriptions-item>
      <el-descriptions-item label="状态" span="1" v-if="setFormPage == 'AGV'">
        {{
          $status.deviceStatusOptions.find(
            (v) => v.value == topForm.deviceStatus
          )
            ? $status.deviceStatusOptions.find(
                (v) => v.value == topForm.deviceStatus
              ).label
            : ""
        }}
      </el-descriptions-item>
      <el-descriptions-item label="料仓" span="2">
        <div class="siloCode">
          {{ topForm.siloCode }}
        </div>
      </el-descriptions-item>
    </el-descriptions>

    <el-table
      v-if="queryParams.siloCode"
      border
      :max-height="maxHeight"
      :data="panelList"
      v-loading="loading"
      :ref="page"
    >
      <el-table-column
        label="层号"
        align="center"
        prop="floorNum"
        show-overflow-tooltip
      />

      <el-table-column label="板料编码" min-width="200" prop="panelCode">
      </el-table-column>
      <el-table-column
        label="板料类型"
        min-width="110"
        align="center"
        prop="productStatus"
      >
        <template slot-scope="scope">
          <status-tag
            :options="productStatusOptions"
            :status="scope.row.productStatus * 1"
          />
        </template>
      </el-table-column>
      <el-table-column
        label="物料编码"
        min-width="200"
        prop="itemCode"
      ></el-table-column>
      <el-table-column label="每叠块数" align="center" prop="pcs" />
      <el-table-column label="板长" align="center" prop="panelLength" />
      <el-table-column label="板宽" align="center" prop="panelWidth" />
      <el-table-column
        label="销钉偏移量"
        min-width="120px"
        align="center"
        prop="pinOffset"
      />
    </el-table>
    <el-empty v-else description="暂未绑定料仓" :image-size="100"></el-empty>
  </edit-form-dialog>
</template>

<script>
import { getCentralRackPanels } from "@/api/wareHouse/rack";
import { getLocationDetail } from "@/api/wareHouse/silo";
export default {
  name: "ViewForm",
  props: ["setFormPage"],
  data() {
    return {
      page: "changePanel",
      showFlag: false,
      dialogTitle: "",
      content_style: {
        // 居中
        height: "30px",
        "font-size": "16px",
        // 排列第二行
        "word-break": "break-all",
      },
      deviceStatus: "ONLINE",
      topForm: {},
      loading: false,
      maxHeight: 0,
      panelList: [],
      // 查询参数
      queryParams: {
        siloCode: undefined,
      },
      // 层数
      productStatusOptions: this.$status.productStatusOptions.map((v) => {
        return {
          ...v,
          name: v.label + (v.value != -1 ? "(" + v.value + ")" : ""),
        };
      }),
      detailTimer: null,
      // 原来的料仓
      oldSiloCode: undefined,
    };
  },

  watch: {
    showFlag: {
      handler(val) {
        if (val) {
          if (this.setFormPage == "AGV") {
            this.$emit("closeTimer");
          }
          this.getDevice(3000);
        } else {
          clearInterval(this.detailTimer);
          this.detailTimer = null;
          if (this.setFormPage == "AGV") {
            this.$emit("openTimer");
          } else {
            this.$emit("getRackList");
          }
        }
      },
      immediate: true,
      deep: true,
    },
  },
  methods: {
    // 获取状态
    getDevice(val) {
      if (this.detailTimer) {
        clearInterval(this.detailTimer);
        this.detailTimer = null;
      }
      this.detailTimer = setInterval(() => {
        setTimeout(() => {
          this.deviceStatus = "READY";
          // this.getDetail(this.detailId); //调用接口的方法
        }, 0);
      }, val);
    },
    // 获取料仓关联的板料
    getPanel() {
      getLocationDetail(this.topForm.code).then((res) => {
        const list = res.data.map((v) => {
          return {
            ...v,
            floorNum: v.floorNum + 1,
            inputValue: v.panelCode,
          };
        });
        this.panelList = list;
      });
    },
    // 获取板料数据
    getList(val) {
      this.loading = true;

      getCentralRackPanels(this.topForm.code).then((res) => {
        if (res.code != 1) {
          const list = res?.data.map((v) => {
            return {
              ...v,
              floorNum: v.floorNum + 1,
              inputValue: v.panelCode,
            };
          });
          this.panelList = list;
        }
        this.loading = false;
        this.maxHeight =
          window.innerHeight - 400 < 400 ? 400 : window.innerHeight - 400;
      });
    },
  },
};
</script>

<style lang="scss" scoped>
::v-deep .siloCode {
  display: flex;
  align-items: center;
  .el-button {
    .el-icon-sort {
      transform: rotate(90deg) !important;
    }
  }
}
</style>
