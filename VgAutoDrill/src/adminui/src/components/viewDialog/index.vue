<template>
  <edit-form-dialog
    v-model="open"
    :title="title"
    :optType="'view'"
    @submitForm="submitForm"
  >
    <el-form ref="form" :model="form" label-width="100px">
      <el-row>
        <el-col :span="8">
          <el-form-item label="工单编码" prop="code">
            <el-input
              v-model="form.code"
              disabled
              placeholder="请输入工单编码"
            />
          </el-form-item>
        </el-col>
        <el-col :span="4">
          <el-form-item label-width="80">
            <el-switch
              v-model="autoGenFlag"
              active-color="#13ce66"
              active-text="自动生成"
              disabled
            >
            </el-switch>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="工单名称" prop="name">
            <el-input
              v-model="form.name"
              disabled
              placeholder="请输入工单名称"
            />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="8">
          <el-form-item label="来源类型" prop="orderSource">
            <el-select disabled v-model="form.orderSource">
              <el-option :value="'客户订单'">客户订单</el-option>
              <el-option :value="'库存需求'">库存需求</el-option>
            </el-select>
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="来源单据" prop="sourceCode">
            <el-input
              disabled
              v-model="form.sourceCode"
              placeholder="请输入来源单据"
            />
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="是否紧急" prop="isUrgent">
            <el-radio-group v-model="form.isUrgent" disabled>
              <el-radio :label="1">是</el-radio>
              <el-radio :label="0">否</el-radio>
            </el-radio-group>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="8">
          <el-form-item label="产品编码" prop="itemCode">
            <el-input
              v-model="form.itemCode"
              disabled
              placeholder="请输入产品编码"
            >
            </el-input>
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="产品名称" prop="itemName">
            <el-input
              v-model="form.itemName"
              disabled
              placeholder="请输入产品名称"
            />
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="单位" prop="unitOfMeasure">
            <el-input
              v-model="form.unitOfMeasure"
              disabled
              placeholder="请输入单位"
            />
          </el-form-item>
        </el-col>
      </el-row>

      <el-row>
        <el-col :span="8">
          <el-form-item label="规格型号" prop="specification">
            <el-input
              disabled
              v-model="form.specification"
              placeholder="请输入规格型号"
            />
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="叠板层数" prop="panelCount">
            <!-- 引入自定义计数器组件 -->
            <input-number
              :dis="true"
              :myNum="form.panelCount"
              @changeNum="changeNum"
              :numName="'panelCount'"
              :min="0"
            />
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="工艺路线" prop="route">
            <el-input
              disabled
              v-model="form.route"
              :placeholder="form.routeId != null ? '' : '暂未配置'"
            />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="8">
          <el-form-item label="批次号" prop="batchCode">
            <el-input
              v-model="form.batchCode"
              disabled
              placeholder="请输入批次号"
            />
          </el-form-item>
        </el-col>

        <el-col :span="8">
          <el-form-item label="生产数量" prop="quantity">
            <!-- 引入自定义计数器组件 -->
            <input-number
              :myNum="form.quantity"
              @changeNum="changeNum"
              :numName="'quantity'"
              :dis="true"
              :min="0"
            />
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="调整数量" prop="quantityChanged">
            <!-- 引入自定义计数器组件 -->
            <input-number
              :myNum="form.quantityChanged"
              @changeNum="changeNum"
              :numName="'quantityChanged'"
              :dis="true"
              :min="0"
            />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="8">
          <el-form-item label="已排产数量" prop="quantityScheduled">
            <!-- 引入自定义计数器组件 -->
            <input-number
              :myNum="form.quantityScheduled"
              @changeNum="changeNum"
              :numName="'quantityScheduled'"
              :dis="true"
              :min="0"
            />
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="已生产数量" prop="quantityProduced">
            <!-- 引入自定义计数器组件 -->
            <input-number
              :myNum="form.quantityProduced"
              @changeNum="changeNum"
              :numName="'quantityProduced'"
              :dis="true"
              :min="0"
            />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="8">
          <el-form-item label="需求日期" prop="requestDate">
            <el-date-picker
              clearable
              disabled
              v-model="form.requestDate"
              type="date"
              value-format="yyyy-MM-dd"
              placeholder="请选择需求日期"
            >
            </el-date-picker>
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="客户编码" prop="clientCode">
            <el-input
              v-model="form.clientCode"
              disabled
              placeholder="请输入客户编码"
            >
            </el-input>
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="客户名称" prop="clientName">
            <el-input
              v-model="form.clientName"
              disabled
              placeholder="请输入客户名称"
            />
          </el-form-item>
        </el-col>
      </el-row>
      <el-collapse accordion v-model="activeNames">
        <el-collapse-item name="params">
          <span class="collapse-title" slot="title">更多参数</span>
          <el-row>
            <el-col :span="8">
              <el-form-item label="产品名称" prop="itemName">
                <el-input
                  v-model="form.itemName"
                  placeholder="请选择产品"
                  disabled
                />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="单位" prop="unitOfMeasure">
                <el-input
                  v-model="form.unitOfMeasure"
                  placeholder="请选择产品"
                  disabled
                />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="规格型号" prop="specification">
                <el-input
                  disabled
                  v-model="form.specification"
                  placeholder="请选择产品"
                />
              </el-form-item>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="8">
              <el-form-item label="待排产叠数" prop="wadCount">
                <el-input v-model="form.wadCount" disabled />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="孔数" prop="drillCount">
                <!-- 引入自定义计数器组件 -->
                <input-number
                  :myNum="form.drillCount"
                  @changeNum="changeNum"
                  :dis="true"
                  :numName="'drillCount'"
                  :min="0"
                />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="叠板层数" prop="panelCount">
                <el-input
                  disabled
                  v-model="form.panelCount"
                  placeholder="请选择产品"
                />
              </el-form-item>
            </el-col>
          </el-row>

          <el-row>
            <el-col :span="8">
              <el-form-item label="SpecCroup" prop="specCroup">
                <el-input v-model="form.specCroup" disabled />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="MoveInTime" prop="moveInTime">
                <el-input v-model="form.moveInTime" disabled />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item
                label-width="120px"
                label="MoveOutTime"
                prop="moveOutTime"
              >
                <el-input v-model="form.moveOutTime" disabled />
              </el-form-item>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="8">
              <el-form-item label="TrackInTime" prop="trackInTime">
                <el-input v-model="form.trackInTime" disabled />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item
                label-width="120px"
                label="TrackOutTime"
                prop="trackOutTime"
              >
                <el-input v-model="form.trackOutTime" disabled />
              </el-form-item>
            </el-col>
          </el-row>
        </el-collapse-item>
        <el-collapse-item name="task">
          <span class="collapse-title" slot="title">任务信息</span>
          <el-table v-loading="loading" :data="taskList" border>
            <el-table-column
              label="任务编码"
              fixed="left"
              min-width="200px"
              prop="code"
              show-overflow-tooltip
            />
            <el-table-column label="是否紧急" align="center" prop="isUrgent">
              <template slot-scope="scope">
                <el-tag v-if="scope.row.isUrgent == 1" type="danger">
                  是
                </el-tag>
                <el-tag v-else>否</el-tag>
              </template>
            </el-table-column>
            <el-table-column
              label="任务状态"
              min-width="100px"
              align="center"
              prop="taskStatus"
            >
              <template slot-scope="scope">
                <status-tag
                  :options="$status.taskOptions"
                  :status="scope.row.taskStatus"
                ></status-tag>
              </template>
            </el-table-column>
            <el-table-column
              label="工序编码"
              min-width="150px"
              prop="processCode"
              show-overflow-tooltip
            />

            <el-table-column
              label="工作站编码"
              min-width="150px"
              prop="workStationCode"
              show-overflow-tooltip
            />
            <el-table-column
              label="工艺路线"
              min-width="150px"
              prop="routeCode"
              show-overflow-tooltip
            />
            <el-table-column label="排产数量" align="center" prop="quantity" />
            <el-table-column
              label="领取数量"
              align="center"
              prop="nowWadCount"
            />
            <el-table-column
              label="计划开始"
              min-width="150px"
              align="center"
              prop="startTime"
            >
              <template slot-scope="scope">
                <el-tooltip class="item" effect="dark" placement="top">
                  <div slot="content">
                    {{ parseTime(scope.row.startTime) }}
                  </div>
                  <div>
                    {{ parseTime(scope.row.startTime, "{m}-{d} {h}:{i}:{s}") }}
                  </div>
                </el-tooltip>
              </template>
            </el-table-column>
            <el-table-column
              label="实际开始"
              min-width="150px"
              align="center"
              prop="realStartTime"
            >
              <template slot-scope="scope">
                <el-tooltip class="item" effect="dark" placement="top">
                  <div slot="content">
                    {{ parseTime(scope.row.realStartTime) }}
                  </div>
                  <div>
                    {{
                      parseTime(scope.row.realStartTime, "{m}-{d} {h}:{i}:{s}")
                    }}
                  </div>
                </el-tooltip>
              </template>
            </el-table-column>
            <el-table-column
              label="计划完成"
              min-width="150px"
              align="center"
              prop="endTime"
            >
              <template slot-scope="scope">
                <el-tooltip class="item" effect="dark" placement="top">
                  <div slot="content">
                    {{ parseTime(scope.row.endTime) }}
                  </div>
                  <div>
                    {{ parseTime(scope.row.endTime, "{m}-{d} {h}:{i}:{s}") }}
                  </div>
                </el-tooltip>
              </template>
            </el-table-column>

            <el-table-column
              label="实际完成"
              min-width="150px"
              align="center"
              prop="realEndTime"
            >
              <template slot-scope="scope">
                <el-tooltip class="item" effect="dark" placement="top">
                  <div slot="content">
                    {{ parseTime(scope.row.realEndTime) }}
                  </div>
                  <div>
                    {{
                      parseTime(scope.row.realEndTime, "{m}-{d} {h}:{i}:{s}")
                    }}
                  </div>
                </el-tooltip>
              </template>
            </el-table-column>

            <el-table-column
              label="创建时间"
              align="center"
              prop="createTime"
              width="180"
            >
              <template slot-scope="scope">
                <el-tooltip class="item" effect="dark" placement="top">
                  <div slot="content">
                    {{ parseTime(scope.row.createTime) }}
                  </div>
                  <div>
                    {{ parseTime(scope.row.createTime, "{m}-{d} {h}:{i}:{s}") }}
                  </div>
                </el-tooltip>
              </template>
            </el-table-column>
          </el-table>

          <pagination
            v-show="total > 0"
            :total="total"
            :page.sync="queryParams.pageNum"
            :limit.sync="queryParams.pageSize"
            @pagination="getList"
          />
        </el-collapse-item>
        <el-collapse-item name="panel">
          <span class="collapse-title" slot="title">板料信息</span>
          <el-table v-loading="panelLoading" :data="panelList" border>
            <el-table-column
              label="板料编码"
              show-overflow-tooltip
              min-width="200"
              prop="panelCode"
            />

            <el-table-column
              label="板料类型"
              align="center"
              prop="productStatus"
            >
              <template slot-scope="scope">
                <el-tag v-if="scope.row.productStatus == '2'">熟料</el-tag>
                <el-tag type="succes" v-else-if="scope.row.productStatus == '1'"
                  >生料</el-tag
                >
                <el-tag type="info" v-else>无料</el-tag>
              </template>
            </el-table-column>
            <el-table-column label="物料编码" min-width="200" prop="itemCode">
            </el-table-column>
            <el-table-column label="每叠块数" align="center" prop="pcs" />
            <el-table-column label="板宽" align="center" prop="panelWidth" />
            <el-table-column
              label="销钉偏移量"
              min-width="120px"
              align="center"
              prop="pinOffset"
            />
          </el-table>

          <pagination
            v-show="total > 0"
            :total="total"
            :page.sync="panelQueryParams.pageNum"
            :limit.sync="panelQueryParams.pageSize"
            @pagination="getPanelList"
          />
        </el-collapse-item>
      </el-collapse>
    </el-form>
  </edit-form-dialog>
</template>

<script>
import { listTask } from "@/api/produce/task";
export default {
  data() {
    return {
      autoGenFlag: false,
      activeNames: [],
      // 遮罩层
      loading: true,
      // 总条数
      total: 0,
      panelLoading: true,
      panelTotal: 0,
      // 生产报工记录表格数据
      taskList: [],
      // 板料信息
      panelList: [],
      // 弹出层标题
      title: "查看工单",
      // 是否显示弹出层
      open: false,
      // 表单参数
      form: {},
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        workOrderCode: undefined,
        workOrderId: undefined,
      },
      panelQueryParams: {
        pageNum: 1,
        pageSize: 10,
        workOrderCode: undefined,
        workOrderId: undefined,
      },
    };
  },
  watch: {
    activeNames(val) {
      if (val == "task") {
        this.getList();
      } else if (val == "panel") {
        this.getPanelList();
      }
    },
    open(val) {
      if (!val) {
        this.activeNames = [];
      }
    },
  },
  methods: {
    submitForm() {},
    getList() {
      this.loading = true;
      this.queryParams.workOrderId = this.form.id;
      listTask(this.queryParams).then((res) => {
        this.taskList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
      });
    },
    // 查看工单关联的板料信息
    getPanelList() {
      this.panelLoading = true;
      this.panelQueryParams.workOrderId = this.form.id;
      listTask(this.panelQueryParams).then((res) => {
        this.panelList = res.data.list;
        this.panelTotal = res.data.total;
        this.panelLoading = false;
      });
    },
  },
};
</script>

<style scoped>
.popconfirm {
  margin: 0 5px;
}
.collapse-title {
  flex: 1 0 90%;
  order: 1;
}

.el-collapse-item__header {
  flex: 1 0 auto;
  order: -1;
}
::v-deep .el-collapse {
  width: 920px;
  padding-left: 20px;
  border-bottom: none !important;
}
::v-deep .el-collapse-item__wrap {
  border: none;
}
</style>
