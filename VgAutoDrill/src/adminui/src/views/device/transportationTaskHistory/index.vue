<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="内部编号" prop="internalLotNo">
        <el-input
          v-trim
          v-model="queryParams.internalLotNo"
          placeholder="请输入内部编号"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="外部编号" prop="externalLotNo">
        <el-input
          v-trim
          v-model="queryParams.externalLotNo"
          placeholder="请输入外部编号"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <!-- <el-form-item label="物料编码" prop="itemCode">
        <el-input
          v-trim
          v-model="queryParams.itemCode"
          placeholder="请输入物料编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item> -->
      <el-form-item label="分区" prop="warehouseCode">
        <el-input
          v-trim
          v-model="queryParams.warehouseCode"
          placeholder="请输入分区"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="料仓" prop="siloCode">
        <el-input
          v-trim
          v-model="queryParams.siloCode"
          placeholder="请输入料仓"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="库位" prop="forkCode">
        <el-input
          v-trim
          v-model="queryParams.forkCode"
          placeholder="请输入库位"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="反馈信息" prop="hkResponse">
        <el-input
          v-trim
          v-model="queryParams.hkResponse"
          placeholder="请输入反馈信息"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <!--<el-form-item label="熟料备注" prop="clinkerRemark">
        <el-input
          v-trim
          v-model="queryParams.clinkerRemark"
          placeholder="请输入熟料备注"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>-->
      <el-form-item label="是否紧急" prop="isUrgent">
        <el-select
          @clear="clearQueryParams('isUrgent')"
          v-model="queryParams.isUrgent"
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option label="是" :value="1" />
          <el-option label="否" :value="0" />
        </el-select>
      </el-form-item>
      <el-form-item label="交互序列" prop="interactionSequence">
        <el-select
          v-model="queryParams.interactionSequence"
          placeholder="请选择"
          clearable
          @clear="clearQueryParams('interactionSequence')"
        >
          <el-option
            v-for="item in interaction_sequence_options"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <!-- 多选调度状态 -->
      <el-form-item label="调度状态" prop="scheduledTaskStatusList">
        <el-select
          v-model="queryParams.scheduledTaskStatusList"
          placeholder="请选择"
          multiple
          collapse-tags
          :class="
            queryParams.scheduledTaskStatusList &&
            queryParams.scheduledTaskStatusList.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in $status.schedulementOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
            v-optionTitle
          />
        </el-select>
      </el-form-item>
      <!-- 单选调度状态 -->
      <!-- <el-form-item label="调度状态" prop="scheduledTaskStatus">
        <el-select
          v-model="queryParams.scheduledTaskStatus"
          placeholder="请选择" 
        >
          <el-option
            v-for="item in $status.schedulementOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item> -->
      <el-form-item label="料仓类型" prop="transportationKind">
        <el-select
          v-model="queryParams.transportationKind"
          placeholder="请选择"
          clearable
          @clear="clearQueryParams('transportationKind')"
        >
          <el-option
            v-for="item in transportation_kind_options"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item>

      <el-form-item label="是否手动创建" prop="isManual">
        <el-select
          v-model="queryParams.isManual"
          placeholder="请选择"
          clearable
          style="width: 150px"
          @clear="clearQueryParams('isManual')"
        >
          <el-option label="是" :value="1" />
          <el-option label="否" :value="0" />
        </el-select>
      </el-form-item>
      <el-form-item label="创建时间">
        <el-date-picker
          v-model="queryParams.startTime"
          type="datetime"
          placeholder="起始时间"
          :picker-options="pickerCreateStart"
        >
        </el-date-picker>
        ↔
        <el-date-picker
          :picker-options="pickerCreateEnd"
          v-model="queryParams.endTime"
          type="datetime"
          placeholder="结束时间"
        >
        </el-date-picker>
      </el-form-item>
    </search-form>
    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="info"
          plain
          icon="el-icon-download"
          @click="handleExport"
          :disabled="hasPermi(['device:transportationTaskHistory:export'])"
          >导出</el-button
        >
      </el-col>
      <right-toolbar
        :showSearch.sync="showSearch"
        @queryTable="getList"
        :columns="columns"
        :page="page"
      ></right-toolbar>
    </el-row>

    <el-table
      border
      :ref="page"
      v-loading="loading"
      :data="transportationTaskList"
    >
      <el-table-column
        label="ID"
        key="id"
        prop="id"
        show-overflow-tooltip
        v-if="columns[0].visible"
        min-width="80"
        align="center"
        fixed="left"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['device:transportationTaskHistory:view']"
            >{{ scope.row.id }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="分区"
        key="warehouseCode"
        prop="warehouseCode"
        show-overflow-tooltip
        v-if="columns[1].visible"
        min-width="80"
      />
      <el-table-column
        label="库位"
        key="forkCode"
        prop="forkCode"
        show-overflow-tooltip
        v-if="columns[2].visible"
        min-width="150"
        fixed="left"
      >
        <template slot-scope="scope">
          <span
            >{{ scope.row.forkCode
            }}{{
              scope.row.positionCodes
                ? "（" + scope.row.positionCodes + "）"
                : ""
            }}</span
          >
        </template></el-table-column
      >
      <el-table-column
        label="交互"
        key="interactionSequence"
        prop="interactionSequence"
        align="center"
        v-if="columns[3].visible"
        min-width="80"
      >
        <template slot-scope="scope">
          <el-tag>
            {{
              interaction_sequence_options.find(
                (v) => v.value == scope.row.interactionSequence
              ) &&
              interaction_sequence_options.find(
                (v) => v.value == scope.row.interactionSequence
              ).label
            }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="调度状态"
        key="scheduledTaskStatus"
        prop="scheduledTaskStatus"
        v-if="columns[4].visible"
        align="center"
        fixed="left"
        min-width="100"
      >
        <template slot-scope="scope">
          <status-tag
            :options="$status.schedulementOptions"
            :status="scope.row.scheduledTaskStatus"
          />
        </template>
      </el-table-column>

      <el-table-column
        label="料仓类型"
        key="transportationKind"
        prop="transportationKind"
        v-if="columns[5].visible"
        min-width="80"
        fixed="left"
        align="center"
      >
        <template slot-scope="scope">
          <el-tag
            v-if="
              transportation_kind_options.find(
                (v) => v.value == scope.row.transportationKind
              )
            "
          >
            {{
              transportation_kind_options.find(
                (v) => v.value == scope.row.transportationKind
              ).label
            }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="手动创建"
        key="isManual"
        prop="isManual"
        v-if="columns[6].visible"
        min-width="80"
        align="center"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isManual == 1"> 是 </el-tag>
          <el-tag v-else type="danger"> 否 </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="创建时间"
        key="createTime"
        prop="createTime"
        show-overflow-tooltip
        v-if="columns[7].visible"
        align="center"
        min-width="160"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.createTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="等待时长(分钟)"
        key="waitTime"
        prop="waitTime"
        show-overflow-tooltip
        v-if="columns[8].visible"
        align="center"
        min-width="120"
      >
        <template slot-scope="scope">
          <!-- 等待时长（分钟）【当前时间-创建时间】,
           如果状态是完成，等待时长=开始调度时间- 创建时间；
           如果状态是取消或者终止，等待时长=取消时间/终止时间-创建时间 -->
          <!-- 已完成4 -->
          <span style="color: red" v-if="scope.row.scheduledTaskStatus == 4">
            {{ timeDifference(scope.row.createTime, scope.row.runningTime) }}
          </span>
          <!-- 已取消-2 -->
          <span
            style="color: red"
            v-else-if="scope.row.scheduledTaskStatus == -2"
          >
            {{ timeDifference(scope.row.createTime, scope.row.canceledTime) }}
          </span>
          <!-- 已终止-2 -->
          <span
            style="color: red"
            v-else-if="scope.row.scheduledTaskStatus == -1"
          >
            {{ timeDifference(scope.row.createTime, scope.row.failedTime) }}
          </span>
          <!-- 其他【当前时间-创建时间】 -->
          <span style="color: red" v-else>{{
            timeDifference(scope.row.createTime, new Date())
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="内部编号"
        key="internalLotNo"
        prop="internalLotNo"
        show-overflow-tooltip
        v-if="columns[9].visible"
        min-width="150"
      />

      <el-table-column
        label="料仓"
        key="siloCode"
        prop="siloCode"
        min-width="120"
        show-overflow-tooltip
        v-if="columns[10].visible"
      />
      <el-table-column
        label="HK反馈信息"
        key="hkResponse"
        prop="hkResponse"
        v-if="columns[11].visible"
        min-width="150"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.hkResponse" />
        </template>
      </el-table-column>

      <el-table-column
        label="备注"
        key="relatedDrillTrace"
        prop="relatedDrillTrace"
        v-if="columns[12].visible"
        min-width="150"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.relatedDrillTrace" />
        </template>
      </el-table-column>

      <el-table-column
        label="紧急"
        key="isUrgent"
        prop="isUrgent"
        align="center"
        v-if="columns[13].visible"
        min-width="60"
      >
        <template slot-scope="scope">
          <el-tag :type="scope.row.isUrgent == 0 ? '' : 'danger'">
            {{ scope.row.isUrgent == 0 ? "否" : "是" }}
          </el-tag>
        </template>
      </el-table-column>

      <el-table-column
        label="熟料"
        align="center"
        key="clinkerCount"
        prop="clinkerCount"
        v-if="columns[14].visible"
        min-width="60"
      ></el-table-column>
      <el-table-column
        label="生料"
        align="center"
        key="rawCount"
        prop="rawCount"
        min-width="60"
        v-if="columns[15].visible"
      ></el-table-column>

      <el-table-column
        label="外部编号"
        key="externalLotNo"
        prop="externalLotNo"
        show-overflow-tooltip
        v-if="columns[16].visible"
        min-width="210"
      />

      <el-table-column
        label="开始调度"
        key="runningTime"
        prop="runningTime"
        show-overflow-tooltip
        v-if="columns[17].visible"
        align="center"
        min-width="160"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.runningTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="调度完成"
        key="completedTime"
        prop="completedTime"
        show-overflow-tooltip
        v-if="columns[18].visible"
        align="center"
        min-width="160"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.completedTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="上料时长(分钟)"
        key="materialTime"
        prop="materialTime"
        show-overflow-tooltip
        v-if="columns[19].visible"
        align="center"
        min-width="120"
      >
        <template slot-scope="scope">
          <!-- 完成调度时间-开始调度时间 -->
          <span style="color: red">{{
            timeDifference(scope.row.runningTime, scope.row.completedTime)
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="执行失败"
        key="failedTime"
        prop="failedTime"
        show-overflow-tooltip
        v-if="columns[20].visible"
        align="center"
        min-width="160"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.failedTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="取消计划"
        key="canceledTime"
        prop="canceledTime"
        show-overflow-tooltip
        v-if="columns[21].visible"
        align="center"
        min-width="160"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.canceledTime) }}</span>
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
            type="text"
            icon="el-icon-document"
            @click="handleDetail(scope.row)"
            :disabled="hasPermi(['device:transportationTaskHistory:detail'])"
            >详情</el-button
          >
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

    <!-- 添加或修改料仓任务对话框 -->
    <edit-form-dialog
      v-model="open"
      :title="title"
      optType="view"
      :isFormModified="false"
      @submitForm="() => {}"
    >
      <el-form
        ref="form"
        :model="form"
        label-width="100px"
        :rules="rules"
        :disabled="optType == 'view'"
      >
        <el-row>
          <el-col :span="8">
            <el-form-item label="料仓类型" prop="transportationKind">
              <el-select
                v-model="form.transportationKind"
                placeholder="请选择"
                @change="handleSelectTransportationKind"
              >
                <el-option
                  v-for="item in transportation_kind_options"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="交互序列" prop="interactionSequence">
              <el-select
                :disabled="form.transportationKind !== 1"
                v-model="form.interactionSequence"
                placeholder="请选择"
                @change="handleSelectInteractionSequence"
              >
                <el-option
                  v-for="item in interaction_sequence_options"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="是否紧急">
              <el-radio-group
                v-removeAriaHidden
                v-model="form.isUrgent"
                :disabled="optType == 'view'"
              >
                <el-radio :label="1" style="margin-right: 15px">是</el-radio>
                <el-radio :label="0">否</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item prop="forkCode">
              <span slot="label">{{ forkCodeLabel }} </span>
              <el-input
                v-model="form.forkCode"
                placeholder=" 请选择库位 "
                disabled
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="所属分区" prop="wareHouseId">
              <el-input
                v-model="form.warehouseCode"
                placeholder=" 请选择库位 "
                disabled
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="手动创建" v-if="optType != 'add'">
              <el-radio-group
                v-removeAriaHidden
                v-model="form.isManual"
                disabled
              >
                <el-radio :label="1" style="margin-right: 15px">是</el-radio>
                <el-radio :label="0">否</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col
            :span="10"
            v-if="
              form.transportationKind != 1 &&
              form.transportationKind != undefined
            "
          >
            <el-form-item label="内部编号" prop="internalLotNo">
              <el-input
                v-model="form.internalLotNo"
                placeholder=" 请输入内部编号 "
                disabled
              />

              <span
                :style="{ display: errorVerify ? 'inline' : 'none' }"
                style="
                  position: absolute;
                  width: 100%;
                  left: 0;
                  top: 27px;
                  font-size: 12px;
                  color: #ff4949;
                "
                >{{ errorVerifyText }}</span
              >
            </el-form-item>
          </el-col>
          <el-col
            :span="10"
            v-if="form.transportationKind == 3 || form.transportationKind == 4"
          >
            <el-form-item label="外部编号" prop="externalLotNo">
              <el-input
                v-model="form.externalLotNo"
                placeholder=" 请输入外部编号 "
                disabled
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col
            :span="8"
            v-if="
              (form.interactionSequence != 0 &&
                form.interactionSequence != undefined) ||
              optType == 'view'
            "
          >
            <el-form-item label="料仓" prop="siloCode">
              <el-input
                v-model="form.siloCode"
                placeholder=" 请输入料仓编号 "
                disabled
              />
            </el-form-item>
          </el-col>
          <el-col
            :span="8"
            v-if="form.transportationKind == 3 || form.transportationKind == 4"
          >
            <el-form-item label="熟料" prop="clinkerCount">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.clinkerCount"
                @changeNum="changeNum"
                :numName="'clinkerCount'"
                :dis="optType == 'view'"
                :min="1"
                :max="1000"
              />
            </el-form-item>
          </el-col>
          <el-col
            :span="8"
            v-if="form.transportationKind == 2 && optType != 'add'"
          >
            <el-form-item label="生料" prop="rawCount">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.rawCount"
                @changeNum="changeNum"
                :numName="'rawCount'"
                :dis="true"
                :min="1"
                :max="1000"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8" v-if="optType != 'add'">
            <el-form-item label="调度状态" prop="scheduledTaskStatus">
              <el-select
                v-model="form.scheduledTaskStatus"
                placeholder="请选择"
              >
                <el-option
                  v-for="item in $status.schedulementOptions"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                  v-optionTitle
                />
              </el-select>
            </el-form-item> </el-col
        ></el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="HK反馈">
              <el-input
                type="textarea"
                v-model="form.hkResponse"
                placeholder="请输入备注"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="备注">
              <el-input
                type="textarea"
                v-model="form.relatedDrillTrace"
                placeholder="请输入备注"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item> </el-col
        ></el-row>
      </el-form>
    </edit-form-dialog>
    <my-drawer ref="MyDrawer" :detailId="detailId" isPage="schedulement" />
  </div>
</template>

<script>
import { listHistory, getHistory } from "@/api/device/transportationTask";
import drawer from "../components/siloTasksDrawer.vue";
export default {
  name: "TransportationTaskHistory",
  components: {
    MyDrawer: drawer,
  },
  data() {
    const interactionSequenceRule = (rule, value, callback) => {
      if (this.form.interactionSequence == undefined) {
        callback(new Error("请选择类型或交互序列"));
      } else {
        callback();
      }
    };
    // 内部编号
    const internalLotNoRule = (rule, value, callback) => {
      if (
        this.form.internalLotNo == undefined ||
        this.form.internalLotNo == ""
      ) {
        this.errorVerify = true;
        this.errorVerifyText = "内部编号不得为空";
        callback(new Error("  "));
      } else {
        this.errorVerify = false;
        callback();
      }
    };
    // 外部编号
    const externalLotNoRule = (rule, value, callback) => {
      if (
        this.form.externalLotNo == undefined ||
        this.form.externalLotNo == ""
      ) {
        callback(new Error("外部编号不得为空"));
      } else {
        callback();
      }
    };
    // 料仓
    const siloCodeRule = (rule, value, callback) => {
      if (this.form.siloCode == undefined || this.form.siloCode == "") {
        callback(new Error("请选择料仓"));
      } else {
        callback();
      }
    };
    // 库位
    const forkCodeRule = (rule, value, callback) => {
      if (this.form.forkCode == undefined || this.form.forkCode == "") {
        callback(new Error("请选择库位"));
      } else {
        callback();
      }
    };
    const clinkerCountRule = (rule, value, callback) => {
      if (this.form.clinkerCount == 0) {
        callback(new Error("数量不能为0"));
      } else {
        callback();
      }
    };

    return {
      page: "transportationTaskHistory",
      optType: undefined,
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 料仓任务表格数据
      transportationTaskList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: undefined,
        name: undefined,
        isUrgent: undefined,
        warehouseCode: undefined,
        internalLotNo: undefined,
        externalLotNo: undefined,
        siloCode: undefined,
        itemCode: undefined,
        forkCode: undefined,
        interactionSequence: undefined,
        scheduledTaskStatusList: this.$cache.local.get(
          "silohistory_queryParams"
        )
          ? JSON.parse(this.$cache.local.get("silohistory_queryParams"))
              .scheduledTaskStatusList
          : [],
        transportationKind: undefined,
        hkResponse: undefined,
        isManual: undefined,
        clinkerRemark: undefined,
        // 一个月前的今天
        startTime: this.$cache.local.get("silohistory_queryParams")
          ? JSON.parse(this.$cache.local.get("silohistory_queryParams"))
              .startTime
          : new Date(
              new Date().setMonth(new Date().getMonth() - 1)
            ).toLocaleString("sv-SE"),
        endTime: undefined,
      },
      //.限制结束时间必须大于等于开始时间
      pickerCreateEnd: {
        disabledDate: (time) => {
          if (this.queryParams.startTime) {
            return (
              time.getTime() <=
              new Date(this.queryParams.startTime).getTime() - 86400000
            );
          }
        },
      },
      // 限制开始日期必须小于结束时间或当前日期
      pickerCreateStart: {
        disabledDate: (time) => {
          if (this.queryParams.endTime) {
            return (
              time.getTime() > Date.now() ||
              time.getTime() >
                new Date(this.queryParams.endTime).getTime() -
                  8.64e6 /*开始日期要在选择的结束日期之前 若结束日期大于当前日期 则开始日期为小于当前日期*/
            );
          }
        },
      },
      // 交互序列
      interaction_sequence_options: [
        {
          label: "只上",
          value: 0,
        },
        {
          label: "只下",
          value: 1,
        },
      ],
      // 类型
      transportation_kind_options: [
        {
          label: "空仓",
          value: 1,
        },
        {
          label: "生料仓",
          value: 2,
        },
        {
          label: "熟料仓",
          value: 3,
        },
        {
          label: "首件",
          value: 4,
        },
      ],
      autocompleteType: null,
      detailId: undefined,
      // 表单参数
      form: {},
      initialForm: {},
      // 表单库位动态Label
      forkCodeLabel: "库位",
      // 表单校验
      errorVerify: false,
      errorVerifyText: "点击按钮选择内部编号",
      rules: {
        code: [
          {
            required: true,
            message: "料仓任务编码不能为空",
            trigger: "change",
          },
        ],
        name: [
          { required: true, message: "料仓任务名称不能为空", trigger: "blur" },
        ],
        status: [{ required: true, message: "状态不能为空", trigger: "blur" }],

        interactionSequence: [
          {
            required: true,
            validator: interactionSequenceRule,
            trigger: "change",
          },
        ],

        siloCode: [
          {
            required: true,
            validator: siloCodeRule,
            trigger: "change",
          },
        ],
        internalLotNo: [
          {
            required: true,
            validator: internalLotNoRule,
            trigger: "change",
          },
        ],
        externalLotNo: [
          {
            required: true,
            validator: externalLotNoRule,
            trigger: "change",
          },
        ],
        forkCode: [
          {
            required: true,
            validator: forkCodeRule,
            trigger: "change",
          },
        ],
        clinkerCount: [
          { required: false, validator: clinkerCountRule, trigger: "change" },
        ],
      },
      // 列信息
      columns: [
        { key: 0, label: "ID", visible: true },
        { key: 1, label: "分区", visible: true },
        { key: 2, label: "库位", visible: true },
        { key: 3, label: "交互", visible: true },
        { key: 4, label: "调度状态", visible: true },
        { key: 5, label: "料仓类型", visible: true },
        { key: 6, label: "手动创建", visible: true },
        { key: 7, label: "创建时间", visible: true },
        { key: 8, label: "等待时长", visible: true },
        { key: 9, label: "内部编号", visible: true },
        { key: 10, label: "料仓", visible: true },
        { key: 11, label: "HK反馈信息", visible: false },
        { key: 12, label: "备注", visible: true },
        { key: 13, label: "紧急", visible: true },
        { key: 14, label: "熟料", visible: true },
        { key: 15, label: "生料", visible: true },
        { key: 16, label: "外部编号", visible: true },
        { key: 17, label: "开始调度", visible: true },
        { key: 18, label: "调度完成", visible: true },
        { key: 19, label: "上料时长", visible: true },
        { key: 20, label: "执行失败", visible: true },
        { key: 21, label: "取消计划", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  computed: {
    queryParamsChange() {
      return { ...this.queryParams };
    },
  },
  watch: {
    queryParamsChange: {
      handler(val) {
        this.$cache.local.set(
          "silohistory_queryParams",
          JSON.stringify({
            startTime: val.startTime,
            scheduledTaskStatusList: val.scheduledTaskStatusList,
          })
        );
      },
      deepL: true,
      immediate: true,
    },
    // open
    open(val) {
      if (!val) {
        this.errorVerify = false;
      }
    },
  },
  methods: {
    /** 查询料仓任务列表 */
    async getList(isSearch) {
      this.loading = true;

      const res = await listHistory(this.queryParams);
      this.transportationTaskList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
    },

    //监听row-dblclick事件，实现选中
    rowDblclick(row, column, event) {
      this.handleDoubleClick(row, this, column);
    },
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.multipleSelection = selection;
      this.ids = selection.map((item) => item.id);
    },
    // 表单重置
    reset() {
      this.form = {
        code: "",
        isUrgent: 0,
        internalLotNo: "",
        externalLotNo: "",
        warehouseCode: undefined,
        forkCode: "",
        relatedDrillTrace: "",
        clinkerCount: 0,
        rawCount: 0,
        isManual: 1,
        scheduledTaskStatus: 1,
      };
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.queryParams = {
        pageNum: 1,
        pageSize: 10,
        code: undefined,
        name: undefined,
        isUrgent: undefined,
        warehouseCode: undefined,
        internalLotNo: undefined,
        externalLotNo: undefined,
        siloCode: undefined,
        itemCode: undefined,
        forkCode: undefined,
        interactionSequence: undefined,
        scheduledTaskStatusList: [],
        transportationKind: undefined,
        hkResponse: undefined,
        isManual: undefined,
        clinkerRemark: undefined,
        // 一个月前的今天
        startTime: new Date(
          new Date().setMonth(new Date().getMonth() - 1)
        ).toLocaleString("sv-SE"),
        endTime: undefined,
      };
      this.handleQuery();
    },

    // 查询明细按钮操作
    handleView(id) {
      this.reset();
      getHistory(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "查看任务" + `(ID--${id})`;
          this.optType = "view";
          this.forkCodeLabel =
            res.data.interactionSequence == 1 ? "转出库位" : "转入库位";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },

    /** 提交按钮 */
    submitForm() {},

    handleSelectInteractionSequence(val) {
      this.form.interactionSequence = val;
      this.forkCodeLabel = val == 1 ? "转出库位" : "转入库位";

      // 解决设置默认值后切换试图不更新(强制刷新)
      this.$forceUpdate();
    },
    // 选择类型带出交互序列
    handleSelectTransportationKind(val) {
      if (val == 3 || val == 4) {
        this.form.interactionSequence = 1;
        this.forkCodeLabel = "转出库位";
      } else if (val == 2) {
        this.form.interactionSequence = 0;
        this.forkCodeLabel = "转入库位";
      } else {
        this.form.interactionSequence = this.form.interactionSequence;
        this.forkCodeLabel =
          this.form.interactionSequence == 1 ? "转出库位" : "转入库位";
      }
    },

    // 点击详情
    handleDetail(row) {
      this.detailId = row.id || this.ids;
      this.$refs.MyDrawer.getDetail(this.detailId);
    },

    /** 导出按钮操作 */
    handleExport() {
      this.exportExcel(
        "/v1/TransportationTask/DownLoadHistoryList",
        "料仓历史.xlsx",
        this.queryParams
      );
    },
  },
};
</script>
