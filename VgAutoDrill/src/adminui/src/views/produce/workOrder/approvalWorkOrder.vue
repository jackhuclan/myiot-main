<template>
  <edit-form-dialog
    v-model="open"
    title="审批"
    :submitText="'审 批'"
    :submitType="'success'"
    @submitForm="submitForm"
  >
    <el-form ref="form" :model="form" :rules="rules" label-width="100px">
      <el-row>
        <el-col :span="8">
          <el-form-item label="工单编码" prop="code">
            <el-input disabled v-model="form.code" />
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="产品编码" prop="itemCode">
            <el-input disabled v-model="form.itemCode" />
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="产品名称" prop="itemName">
            <el-input disabled v-model="form.itemName" />
          </el-form-item>
        </el-col>
      </el-row>

      <el-row>
        <el-col :span="8">
          <el-form-item label="PCS数" prop="quantityChanged">
            <el-input disabled v-model="form.quantityChanged" />
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="叠数" prop="panelCount">
            <el-input disabled v-model="form.panelCount" />
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="总组板数" prop="wadCount">
            <el-input disabled v-model="form.wadCount" />
            <span class="prompt">总组板数 = PCS数 / 叠数</span>
          </el-form-item>
        </el-col>
      </el-row>

      <el-row>
        <el-col :span="8">
          <el-form-item label="是否紧急" prop="isUrgent">
            <el-radio-group v-removeAriaHidden v-model="form.isUrgent">
              <el-radio :label="1">是</el-radio>
              <el-radio :label="0">否</el-radio>
            </el-radio-group>
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="需求日期" prop="requestDate">
            <el-date-picker
              clearable
              disabled
              v-model="form.requestDate"
              type="date"
              value-format="yyyy-MM-dd"
            >
            </el-date-picker>
          </el-form-item>
        </el-col>

        <el-col :span="8">
          <el-form-item label="工艺路线" prop="route">
            <el-select
              v-model="form.route"
              placeholder="请选择工艺路线"
              @change="changeRoute"
            >
              <el-option
                v-for="item in routeOptions"
                :key="item.value"
                :label="item.label"
                :value="item.label"
                v-optionTitle
              />
            </el-select>
            <span class="prompt">更换工艺路线，清除机台</span>
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
    <el-tabs type="border-card" @tab-click="onTabClick" v-model="tabValues">
      <el-tab-pane label="分配机台" name="分配机台">
        <FormMachineTable
          ref="FormMachineTable"
          v-if="open"
          :isApproval="true"
          :workOrderCode="form.code"
          :routeCode="form.routeCode"
          :workOrderId="form.id"
        />
      </el-tab-pane>
      <el-tab-pane label="板料信息" name="板料信息">
        <el-table v-loading="panelLoading" :data="panelList" border>
          <el-table-column
            label="板料编码"
            show-overflow-tooltip
            :min-width="flexColumnWidth('板料编码', 'panelCode', panelList)"
            prop="panelCode"
          >
          </el-table-column>

          <el-table-column
            show-overflow-tooltip
            label="物料编码"
            :min-width="flexColumnWidth('物料编码', 'itemCode', panelList)"
            prop="itemCode"
          >
          </el-table-column>
          <el-table-column
            label="板料类型"
            align="center"
            prop="productStatus"
            width="80px"
          >
            <template slot-scope="scope">
              <el-tag v-if="scope.row.productStatus == '2'">熟料</el-tag>
              <el-tag type="succes" v-else-if="scope.row.productStatus == '1'"
                >生料</el-tag
              >
              <el-tag type="info" v-else>无料</el-tag>
            </template>
          </el-table-column>
          <el-table-column
            label="每叠块数"
            show-overflow-tooltip
            align="center"
            prop="pcs"
            width="80px"
          />
          <el-table-column
            label="板宽"
            align="center"
            show-overflow-tooltip
            prop="panelWidth"
            width="100px"
          />
          <el-table-column
            label="销钉偏移量"
            width="120px"
            show-overflow-tooltip
            align="center"
            prop="pinOffset"
          />
        </el-table>

        <pagination
          v-show="panelTotal > 0"
          :total="panelTotal"
          :page.sync="panelQueryParams.pageNum"
          :limit.sync="panelQueryParams.pageSize"
          @pagination="getPanelList"
          :autoScroll="false"
        />
      </el-tab-pane>
    </el-tabs>
  </edit-form-dialog>
</template>

<script>
import FormMachineTable from "../drillWorkOrder/formMachineTable.vue";
import {
  commitWorkOrder,
  getWorkOrderAndPanelList,
  updateWorkOrderRoute,
  getWorkOrder,
  clearWorkStation,
} from "@/api/produce/workOrder";
import { listAllProcess } from "@/api/produce/task";

export default {
  components: { FormMachineTable },
  data() {
    return {
      open: false,
      tabValues: "分配机台",
      panelList: [],
      panelLoading: true,
      panelTotal: 0,
      panelQueryParams: {
        pageNum: 1,
        pageSize: 10,
        workOrderCode: undefined,
        workOrderId: undefined,
      },
      form: {},
      rules: {
        isUrgent: [
          { required: true, message: "订单状态不能为空", trigger: "change" },
        ],
      },
      routeOptions: [],
      productCategoryId: null,
    };
  },
  watch: {
    open: {
      handler(val) {
        if (!val) {
          this.$emit("parentGetList");
        }
      },
      deep: true,
    },
  },
  methods: {
    changeForm(id, isCall) {
      getWorkOrder(id).then((res) => {
        if (res.code == 0) {
          this.form = {
            ...res.data,
            shaftCount: 5,
            workOrderId: res.data.id,
            workOrderCode: res.data.code,
            workOrderName: res.data.name,
            quantity: res.data.quantityChanged,
            usableCount: res.data.wadCount,
            dispenseMachines:
              res.data.dispenseMachines != null ? res.data.dispenseMachines : 0,
          };
          this.tabValues = "分配机台";
          this.open = true;
          if (isCall != "false") {
            this.getList(res.data.id);
            this.getRouteList();
          }
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    getList(id) {
      this.$nextTick(() => {
        this.$refs.FormMachineTable.getList(id);
      });
    },
    // 获取工艺路线
    getRouteList() {
      const itemId = this.form.itemId;
      listAllProcess({ itemId }).then((res) => {
        this.routeOptions = res.data?.routeInfos.map((v) => {
          return {
            label: v.code + " " + v.name,
            value: v.id,
            code: v.code,
            name: v.name,
          };
        });
      });
    },
    // 查看工单关联的板料信息
    getPanelList() {
      this.panelLoading = true;
      this.panelQueryParams.workOrderCode = this.form.code;
      getWorkOrderAndPanelList(this.panelQueryParams).then((res) => {
        this.panelList = res.data.list;
        this.panelTotal = res.data.total;
        this.panelLoading = false;
      });
    },
    // 点击tab
    onTabClick(val) {
      if (val.label == "板料信息") {
        this.getPanelList();
      }
    },

    // 修改工艺路线
    changeRoute(val) {
      this.$modal
        .confirm("确定修改工艺路线？")
        .then(() => {
          this.form.routeId = this.routeOptions.find(
            (v) => v.label == val
          )?.value;
          this.form.routeName = this.routeOptions.find(
            (v) => v.label == val
          )?.name;
          this.form.routeCode = this.routeOptions.find(
            (v) => v.label == val
          )?.code;
          updateWorkOrderRoute({
            id: this.form.id,
            routeId: this.form.routeId,
            routeCode: this.form.routeCode,
            routeName: this.form.routeName,
          }).then((res) => {
            if (res.code == 0) {
              this.$modal.msgSuccess("配置成功");
              if (
                this.$refs.FormMachineTable.workOrderAndWorkStation.length > 0
              ) {
                clearWorkStation(this.form.code).then(() => {
                  this.getList(this.form.id);
                });
              }
            } else {
              this.$modal.notifyError(res.message);
            }
            this.changeForm(this.form.id, "false");
          });
        })
        .catch(() => {
          this.changeForm(this.form.id, "false");
        });
    },
    submitForm: function () {
      if (
        !this.$refs.FormMachineTable.workOrderAndWorkStation ||
        !this.$refs.FormMachineTable.workOrderAndWorkStation.length
      )
        return this.$modal.notifyError("缺少机台");
      commitWorkOrder({
        id: this.form.id,
        isUrgent: this.form.isUrgent,
      }).then((res) => {
        if (res.code == 0) {
          this.$modal.msgSuccess("审批成功");
          this.open = false;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
  },
};
</script>

<style lang="scss" scoped></style>
