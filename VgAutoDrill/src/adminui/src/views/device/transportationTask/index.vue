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
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['device:transportationTask:add'])"
          >新增</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-close"
          @click="handleBatchCancel"
          :disabled="hasPermi(['device:transportationTask:cancel'])"
          >批量取消</el-button
        >
      </el-col>
      <!-- <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-delete"
          @click="handleDelete"
          :disabled="hasPermi(['device:transportationTask:remove'])"
          >批量删除</el-button
        >
      </el-col> -->
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="info"
          plain
          icon="el-icon-download"
          @click="handleExport"
          :disabled="hasPermi(['device:transportationTask:export'])"
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
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
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
            v-isGetSelection="['device:transportationTask:view']"
            >{{ scope.row.id }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="分区"
        key="warehouseCode"
        prop="warehouseCode"
        v-if="columns[1].visible"
        :min-width="
          flexColumnWidth('分区', 'warehouseCode', transportationTaskList)
        "
      />
      <el-table-column
        label="库位"
        key="forkCode"
        prop="forkCode"
        v-if="columns[2].visible"
        :min-width="
          parseInt(
            flexColumnWidth('库位', 'forkCode', transportationTaskList)
          ) +
          parseInt(
            flexColumnWidth('库位', 'positionCodes', transportationTaskList)
          ) +
          'px'
        "
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
      >
        <template slot-scope="scope">
          <el-tooltip effect="dark" placement="bottom" content="点击查看库存">
            <span
              class="one-line-red"
              @click="openItemSearchDialog(scope.row.internalLotNo)"
              >{{ scope.row.internalLotNo }}</span
            >
          </el-tooltip>
        </template></el-table-column
      >

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
        :min-width="
          flexColumnWidth('备注', 'relatedDrillTrace', transportationTaskList)
        "
      >
        <!-- <template slot-scope="scope"> 
          <tooltip :value="scope.row.relatedDrillTrace" />
        </template> -->
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
        min-width="140px"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <el-button
            type="text"
            icon="el-icon-document"
            @click="handleDetail(scope.row)"
            :disabled="hasPermi(['device:transportationTask:detail'])"
            >详情</el-button
          >

          <el-dropdown
            trigger="click"
            @command="(command) => handleCommand(command, scope.row)"
          >
            <span :class="'el-dropdown-link ' + $store.getters.size">
              <i class="el-icon-d-arrow-right el-icon--right"></i>更多
            </span>
            <el-dropdown-menu slot="dropdown">
              <!--               v-if="
                  scope.row.scheduledTaskStatus == 1 ||
                  scope.row.scheduledTaskStatus == 2 ||
                  scope.row.scheduledTaskStatus == 3
                " -->
              <el-dropdown-item
                command="handleCancel"
                icon="el-icon-close"
                :disabled="
                  hasPermi(['device:transportationTask:cancel']) ||
                  (scope.row.scheduledTaskStatus != 1 &&
                    scope.row.scheduledTaskStatus != 2 &&
                    scope.row.scheduledTaskStatus != 3)
                "
                >取消</el-dropdown-item
              >

              <el-dropdown-item
                command="handleUpdate"
                icon="el-icon-edit"
                :disabled="
                  hasPermi(['device:transportationTask:edit']) ||
                  scope.row.scheduledTaskStatus == 3
                "
                >修改</el-dropdown-item
              >
              <!-- <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="hasPermi(['device:transportationTask:remove'])"
                >删除</el-dropdown-item
              > -->
            </el-dropdown-menu>
          </el-dropdown>
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
      :optType="optType"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
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
              <el-autocomplete
                :disabled="optType == 'view'"
                v-model="form.forkCode"
                :fetch-suggestions="querySearchAsync"
                @focus="handleFocus('forkCode')"
                placeholder="请输入内容"
                @select="onRackSelected"
                popper-class="el-autocomplete-suggestion"
                :popper-append-to-body="false"
                :debounce="0"
              >
                <el-button
                  :disabled="optType == 'view'"
                  v-debounce
                  slot="append"
                  @click="handleSelectRackCode"
                  icon="el-icon-search"
                ></el-button>
              </el-autocomplete>
              <rackManageSelect ref="RackSelect" @onSelected="onRackSelected">
              </rackManageSelect>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="所属分区" prop="wareHouseId">
              <treeselect
                :class="$store.getters.size + '-treeselect'"
                disabled
                v-model="form.warehouseCode"
                :options="wareHouseTypeOptions"
                @select="wareHouseTypeSelect"
                @open="changeTreeselectOpen"
                :normalizer="normalizer"
                :show-count="true"
                noOptionsText="暂无数据"
                noChildrenText="暂无数据"
                noResultsText="暂无数据"
                placeholder="请选择库位"
              >
              </treeselect>
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
              <span slot="label">
                内部编号
                <el-tooltip content="点击按钮选择内部编号" placement="top">
                  <i class="el-icon-question"></i>
                </el-tooltip>
              </span>
              <el-input
                @focus="handleFocusErrorVerify"
                @blur="errorVerify = false"
                readonly
                v-model="form.internalLotNo"
                placeholder="请选择内部编号"
                :disabled="optType == 'view'"
                ><el-button
                  :disabled="optType == 'view'"
                  v-debounce
                  slot="append"
                  @click="handleSelectProduct"
                  icon="el-icon-search"
                ></el-button
              ></el-input>
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
              <ProductSelect
                ref="ProductSelect"
                @onSelected="onProductSelected"
              >
              </ProductSelect>
            </el-form-item>
          </el-col>
          <el-col
            :span="10"
            v-if="form.transportationKind == 3 || form.transportationKind == 4"
          >
            <el-form-item label="外部编号" prop="externalLotNo">
              <el-input
                v-model="form.externalLotNo"
                :placeholder="autoGenFlag ? '请选择内部编号' : '请输入外部编号'"
                :disabled="autoGenFlag || optType != 'add'"
              />
            </el-form-item>
          </el-col>
          <el-col
            :span="4"
            v-if="form.transportationKind == 3 || form.transportationKind == 4"
          >
            <el-form-item label-width="80">
              <el-switch
                v-model="autoGenFlag"
                active-color="#13ce66"
                active-text="自动生成"
                @change="handleAutoGenChange(autoGenFlag)"
              >
              </el-switch>
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
              <el-autocomplete
                :disabled="optType == 'view'"
                v-model="form.siloCode"
                :fetch-suggestions="querySearchAsync"
                @focus="handleFocus('siloCode')"
                placeholder="请输入内容"
                @select="onSiloManageSelected"
                popper-class="el-autocomplete-suggestion"
                :popper-append-to-body="false"
                :debounce="0"
              >
                <el-button
                  :disabled="optType == 'view'"
                  v-debounce
                  slot="append"
                  @click="handleSelectSiloManage"
                  icon="el-icon-search"
                ></el-button>
              </el-autocomplete>
              <siloManageSelect
                ref="siloManageSelect"
                @onSelected="onSiloManageSelected"
              >
              </siloManageSelect>
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
          <!--<el-col :span="24">
            <el-form-item label="熟料备注">
              <el-input
                type="textarea"
                v-model="form.clinkerRemark"
                placeholder="请输入熟料备注"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>-->
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
    <my-drawer ref="MyDrawer" :detailId="detailId" />
    <ItemSearchDialog ref="ItemSearchDialog" />
  </div>
</template>

<script>
import {
  listTransportationTask,
  getTransportationTask,
  delTransportationTask,
  addTransportationTask,
  updateTransportationTask,
  delList,
  cancelSingle,
  bulkCancel,
} from "@/api/device/transportationTask";
// 料仓选择
import siloManageSelect from "@/components/siloManageSelect";
// 库位选择
import rackManageSelect from "@/components/rackManageSelect";
import { listPartition, treeselect } from "@/api/wareHouse/partition";
import { listRack } from "@/api/wareHouse/rack";
import { listSilo } from "@/api/wareHouse/silo";
import drawer from "../components/siloTasksDrawer.vue"; // 物料产品选择
import ProductSelect from "@/components/itemSelect";
export default {
  name: "TransportationTask",
  components: {
    siloManageSelect,
    rackManageSelect,
    MyDrawer: drawer,
    ProductSelect,
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
      page: "transportationTask",
      //自动生成编码
      autoGenFlag: true,
      enCode: "",
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
      // 分区下拉选项
      wareHouseTypeOptions: [],
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
        scheduledTaskStatusList: this.$cache.local.get("silotask_queryParams")
          ? JSON.parse(this.$cache.local.get("silotask_queryParams"))
              .scheduledTaskStatusList
          : [],
        transportationKind: undefined,
        hkResponse: undefined,
        isManual: undefined,
        clinkerRemark: undefined,
        // 一个月前的今天
        startTime: this.$cache.local.get("silotask_queryParams")
          ? JSON.parse(this.$cache.local.get("silotask_queryParams")).startTime
          : new Date(
              new Date().setDate(new Date().getDate() - 1)
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
    this.getTreeselect();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
    queryParamsChange() {
      return { ...this.queryParams };
    },
  },
  watch: {
    queryParamsChange: {
      handler(val) {
        this.$cache.local.set(
          "silotask_queryParams",
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
    // 随机数
    RandomNum(Min, Max) {
      let num = Min + Math.round(Math.random() * (Max - Min));
      return num;
    },
    /** 查询料仓任务列表 */
    async getList(isSearch) {
      this.loading = true;

      const res = await listTransportationTask(this.queryParams);
      this.transportationTaskList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
    },
    /** 查询 分区下拉树结构 */
    getTreeselect() {
      treeselect().then((res) => {
        this.wareHouseTypeOptions = res.data;
      });
    },
    normalizer(node) {
      if (node.children && !node.children.length) {
        delete node.children;
      }

      return {
        id: node.code,

        label: node.label,

        isDisabled: node.status == 0,

        children: node.children,
      };
    },
    // 选择归属分区时同时保存分区code
    wareHouseTypeSelect(val) {
      this.form.warehouseCode = val.code;
    },
    changeTreeselectOpen() {
      const doms = document.querySelectorAll(".el-dialog .el-select");
      doms.forEach((v, i) => {
        this.$refs[`select` + (i + 1)]?.blur();
      });
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
          new Date().setDate(new Date().getDate() - 1)
        ).toLocaleString("sv-SE"),
        endTime: undefined,
      };
      this.handleQuery();
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.getTreeselect();
      this.open = true;
      this.initialForm = Object.assign({}, this.form);
      this.title = "添加任务";
      this.optType = "add";
    },
    // 查询明细按钮操作
    handleView(id) {
      this.reset();
      this.getTreeselect();
      getTransportationTask(id).then((res) => {
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
    // 点击产品编码打开弹框
    openItemSearchDialog(code) {
      this.$refs.ItemSearchDialog.open = true;
      this.$refs.ItemSearchDialog.title = `查看库存--(${code})`;
      this.$refs.ItemSearchDialog.queryParams.itemCode = code;
      this.$refs.ItemSearchDialog.getList();
    },
    // 取消操作
    handleCancel(row) {
      this.$modal
        .confirm(
          `确定取消ID为<span style="color:red"> ${row.id}</span> 的数据项？`,
          {
            dangerouslyUseHTMLString: true, // 使用HTML片段
          }
        )
        .then((result) => {
          if (result == "confirm") {
            cancelSingle(row.id).then((res) => {
              if (res.code == 0) {
                this.$modal.msgSuccess("取消成功");
                this.getList();
              } else {
                this.$modal.notifyError(res.message);
              }
            });
          }
        })
        .catch(() => {});
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      this.getTreeselect();
      const transportationTaskId = row.id || this.ids;
      getTransportationTask(transportationTaskId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.initialForm = Object.assign({}, res.data);
          this.open = true;
          this.title = "修改任务" + `(ID--${transportationTaskId})`;
          this.optType = "edit";
          this.forkCodeLabel =
            res.data.interactionSequence == 1 ? "转出库位" : "转入库位";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm() {
      let siloCode = this.form.siloCode;
      let externalLotNo = this.form.externalLotNo;
      let internalLotNo = this.form.internalLotNo;
      let clinkerCount = this.form.clinkerCount;
      // 只上是没有料仓编号熟料为0
      if (this.form.interactionSequence == 0) {
        siloCode = undefined;
        clinkerCount = 0;
      }
      // 空仓时没有外部编号和内部编号
      if (this.form.transportationKind == 1) {
        externalLotNo = undefined;
        internalLotNo = undefined;
        // 生料仓 有内部编号无外部编号
      } else if (this.form.transportationKind == 2) {
        externalLotNo = undefined;
      }

      const form = {
        ...this.form,
        siloCode,
        internalLotNo,
        externalLotNo,
        clinkerCount,
        startLocationCode:
          this.forkCodeLabel == "转出库位" ? this.form.forkCode : undefined,
        endlocationcode:
          this.forkCodeLabel == "转入库位" ? this.form.forkCode : undefined,
      };
      if (this.form.id != null) {
        updateTransportationTask(form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addTransportationTask(form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("新增成功");
            this.open = false;
            this.queryParams.pageNum = 1;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },

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
    // 批量取消
    async handleBatchCancel() {
      if (this.ids.length <= 0)
        return this.$modal.msgWarning("请选择要操作的数据!");
      const results = await this.$modal
        .confirm("确认执行取消？")
        .catch(() => {});
      if (results == "confirm") {
        bulkCancel(this.ids).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("取消成功");
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(row.id, delTransportationTask, this.getList);
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 点击详情
    handleDetail(row) {
      this.detailId = row.id || this.ids;
      this.$refs.MyDrawer.getDetail(this.detailId);
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleUpdate":
          this.handleUpdate(row);
          break;
        case "handleCancel":
          this.handleCancel(row);
          break;
        case "handleDelete":
          this.handleDelete(row);
          break;
        default:
          break;
      }
    },
    //物料选择弹出框
    handleSelectProduct() {
      this.$refs.ProductSelect.showFlag = true;
      this.$refs.ProductSelect.title = "内部编号";
      this.$refs.ProductSelect.selectedItemCode = this.form.internalLotNo
        ? this.form.internalLotNo
        : undefined;
      this.$refs.ProductSelect.getList();
    },
    onProductSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "internalLotNo", obj.code);
        if (this.autoGenFlag) {
          this.form.externalLotNo =
            obj.code + "@" + this.RandomNum(1001, 399999);
        } else {
          if (this.optType == "edit")
            return (this.form.externalLotNo = this.enCode);
          this.form.externalLotNo = "";
        }
      }
    },
    // 选择库位
    handleSelectRackCode() {
      this.$refs.RackSelect.showFlag = true;
      this.$refs.RackSelect.selectedRackId = this.form.forkCode
        ? this.form.forkCode
        : undefined;
      this.$refs.RackSelect.queryParams.pageNum = 1;
      // 限制只能选择板料插齿类型的库位
      this.$refs.RackSelect.queryParams.deviceKinds = [15];
      this.$refs.RackSelect.getList();
    },
    onRackSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "forkCode", obj.code);
        this.$set(this.form, "warehouseCode", obj.wareHouseCode);
      }
    },
    //选择料仓
    handleSelectSiloManage() {
      this.$refs.siloManageSelect.showFlag = true;
      this.$refs.siloManageSelect.selectedSiloId = this.form.siloCode
        ? this.form.siloCode
        : undefined;
      this.$refs.siloManageSelect.getList();
    },
    onSiloManageSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "siloCode", obj.code);
      }
    },
    handleFocusErrorVerify() {
      this.errorVerify = true;
      this.errorVerifyText = "点击按钮选择内部编号";
    },
    handleFocus(val) {
      this.autocompleteType = val;
    },
    querySearchAsync(queryString, cb) {
      let api = null;
      switch (this.autocompleteType) {
        case "warehouseCode":
          api = listPartition;
          break;
        case "forkCode":
          api = listRack;
          break;
        case "siloCode":
          api = listSilo;
          break;
      }
      if (queryString === "" || !queryString) {
        let arr = [];
        cb(arr);
      } else {
        clearTimeout(this.timeout);
        this.timeout = setTimeout(() => {
          api({
            pageNum: 1,
            pageSize: 1000,
            code: queryString,
          }).then((res) => {
            if (res.code === 0) {
              let arr = res.data.list.map((v) => {
                return {
                  value: v.code,
                  ...v,
                };
              });
              cb(arr);
            } else {
              this.$modal.notifyError(res.message);
            }
          });
        }, 500);
      }
    },
    /** 导出按钮操作 */
    handleExport() {
      this.exportExcel(
        "/v1/TransportationTask/DownLoadList",
        "料仓任务.xlsx",
        this.queryParams
      );
    },
    //自动生成编码
    handleAutoGenChange(autoGenFlag) {
      if (autoGenFlag) {
        // this.getEncode({
        //   rulesCode: "UNITMEASURE_CODE",
        //   buildCount: 1,
        // }).then((response) => {
        //   const code = response.data[0];
        //   this.form.code = code;
        // });
        if (this.form.internalLotNo) {
          this.form.externalLotNo =
            this.form.internalLotNo + "@" + this.RandomNum(1001, 399999);
        }
      } else {
        if (this.optType == "edit") return (this.form.code = this.enCode);
        this.form.code = "";
      }
    },
  },
};
</script>
