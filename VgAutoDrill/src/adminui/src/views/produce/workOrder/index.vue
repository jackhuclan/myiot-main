<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="排序方式" prop="queryOrderBy">
        <el-select
          @change="handleQueryOrderBy"
          v-model="queryParams.queryOrderBy"
          placeholder="请选择"
          style="width: 150px"
        >
          <el-option
            v-for="item in $status.queryOrderByOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="工单编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入工单编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>

      <el-form-item label="来源类型" prop="orderSource">
        <el-select
          v-model="queryParams.orderSource"
          placeholder="请选择"
          clearable
          @clear="clearQueryParams('orderSource')"
          style="width: 150px"
        >
          <el-option :value="'客户订单'">客户订单</el-option>
          <el-option :value="'库存需求'">库存需求</el-option>
        </el-select>
      </el-form-item>

      <el-form-item label="产品编码" prop="itemCode">
        <el-input
          v-trim
          v-model="queryParams.itemCode"
          placeholder="请输入产品编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>

      <el-form-item label="客户名称" prop="clientName">
        <el-input
          v-trim
          v-model="queryParams.clientName"
          placeholder="请输入客户名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <!-- 单选工单状态 -->
      <el-form-item label="工单状态" prop="manuOrderStatusList">
        <el-select
          v-model="queryParams.manuOrderStatusList"
          placeholder="请选择"
          multiple
          collapse-tags
          @change="handleselectManuOrderStatusList"
          :class="
            queryParams.manuOrderStatusList &&
            queryParams.manuOrderStatusList.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in $status.workOrderOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
            v-optionTitle
          />
        </el-select>
      </el-form-item>
      <!-- 单选工单状态 -->
      <!-- <el-form-item label="工单状态" prop="manuOrderStatus">
        <el-select
          v-model="queryParams.manuOrderStatus"
          placeholder="请选择"
          @change="handleselectManuOrderStatus"
          style="width: 150px"
        >
          <el-option
            v-for="item in $status.workOrderOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item> -->

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
      <el-form-item label="是否外部工单" prop="isExternal">
        <el-select
          v-model="queryParams.isExternal"
          placeholder="请选择"
          clearable
          @clear="clearQueryParams('isExternal')"
          style="width: 150px"
        >
          <el-option label="是" :value="1" />
          <el-option label="否" :value="0" />
        </el-select>
      </el-form-item>
      <el-form-item label="层数" prop="layerNumList">
        <el-select
          v-model="queryParams.layerNumList"
          placeholder="请选择"
          multiple
          collapse-tags
          style="width: 150px"
        >
          <el-option
            v-for="item in layerNumOptions"
            :key="item.value"
            :label="item.label + '层'"
            :value="item.value"
          />
        </el-select>
      </el-form-item>

      <el-form-item label="需求日期" prop="requestDate">
        <el-date-picker
          clearable
          v-model="queryParams.requestDate"
          type="date"
          value-format="yyyy-MM-dd"
          placeholder="请选择需求日期"
          style="width: 180px"
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
          :disabled="hasPermi(['produce:workorder:add'])"
          >新增</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="warning"
          plain
          @click="handleProdTaskAll"
          icon="el-icon-s-finance"
          :disabled="hasPermi(['produce:workorder:generatetasks'])"
          >批量生成任务
        </el-button></el-col
      >
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-delete"
          @click="handleDelete"
          :disabled="hasPermi(['produce:workorder:remove'])"
          >批量删除</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="info"
          plain
          icon="el-icon-upload2"
          @click="handleImport"
          :disabled="hasPermi(['produce:workorder:import'])"
          >导入</el-button
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
      :data="workOrderList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />

      <el-table-column
        label="工单编码"
        key="code"
        prop="code"
        show-overflow-tooltip
        fixed="left"
        min-width="200px"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['produce:workorder:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="创建时间"
        key="createTime"
        fixed="left"
        align="center"
        width="180"
        v-if="columns[1].visible"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.createTime, "{y}-{m}-{d} {h}:{i}:{s}")
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="产品编码"
        key="itemCode"
        prop="itemCode"
        show-overflow-tooltip
        min-width="200px"
        v-if="columns[2].visible"
      />
      <el-table-column
        label="工艺路线"
        key="routeCode"
        prop="routeCode"
        show-overflow-tooltip
        min-width="100px"
        v-if="columns[3].visible"
      />
      <el-table-column
        label="是否紧急"
        align="center"
        key="isUrgent"
        prop="isUrgent"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isUrgent == 1" type="danger"> 是 </el-tag>
          <el-tag v-else>否</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="状态"
        align="center"
        key="manuOrderStatus"
        prop="manuOrderStatus"
        v-if="columns[5].visible"
      >
        <template slot-scope="scope">
          <status-tag
            :options="$status.workOrderOptions"
            :status="scope.row.manuOrderStatus"
          />
        </template>
      </el-table-column>
      <el-table-column
        label="层数"
        align="center"
        key="layerNum"
        prop="layerNum"
        show-overflow-tooltip
        v-if="columns[6].visible"
      />
      <el-table-column
        label="叠数"
        align="center"
        key="panelCount"
        prop="panelCount"
        v-if="columns[7].visible"
        show-overflow-tooltip
      />
      <el-table-column
        label="总组板数"
        align="center"
        key="wadCount"
        prop="wadCount"
        v-if="columns[8].visible"
        show-overflow-tooltip
      />
      <el-table-column
        label="是否外部工单"
        align="center"
        key="isExternal"
        prop="isExternal"
        v-if="columns[9].visible"
        min-width="120"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isExternal == 1" type="danger"> 是 </el-tag>
          <el-tag v-else>否</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="标记颜色"
        align="center"
        key="color"
        prop="color"
        v-if="columns[10].visible"
      >
        <template slot-scope="scope">
          <div
            :style="{
              margin: '0 auto',
              width: '20px',
              height: '20px',
              background: scope.row.remarkColor
                ? scope.row.remarkColor
                : workOrderColor,
            }"
          ></div>
        </template>
      </el-table-column>

      <el-table-column
        label="单位"
        align="center"
        key="unitOfMeasure"
        prop="unitOfMeasure"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[11].visible"
      />
      <el-table-column
        label="调整后PCS"
        align="center"
        min-width="120px"
        key="quantityChanged"
        prop="quantityChanged"
        v-if="columns[12].visible"
      />
      <el-table-column
        label="已排产PCS"
        align="center"
        key="quantityScheduled"
        prop="quantityScheduled"
        min-width="120px"
        v-if="columns[13].visible"
      />
      <el-table-column
        label="已生产PCS"
        align="center"
        key="quantityProduced"
        prop="quantityProduced"
        min-width="120px"
        v-if="columns[14].visible"
      />

      <el-table-column
        label="客户名称"
        key="clientName"
        prop="clientName"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[15].visible"
      />
      <el-table-column
        key="requestDate"
        label="需求日期"
        align="center"
        width="100"
        v-if="columns[16].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.requestDate, "{y}-{m}-{d}") }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="预计完成时长"
        align="center"
        min-width="100px"
        key="estimatedTime"
        v-if="columns[17].visible"
      >
        <template slot-scope="scope">
          {{ scope.row.estimatedTime
          }}{{ scope.row.estimatedTime ? "(分钟)" : "" }}
        </template>
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        min-width="220px"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <el-button
            type="text"
            @click="handleApproval(scope.row)"
            icon="el-icon-check"
            :disabled="
              hasPermi(['produce:workorder:commit']) ||
              scope.row.manuOrderStatus != 0
            "
            >审批</el-button
          >
          <el-button
            type="text"
            @click="handleProdTask(scope.row)"
            icon="el-icon-s-finance"
            :disabled="
              hasPermi(['produce:workorder:generatetasks']) ||
              scope.row.manuOrderStatus != 1
            "
          >
            生成任务
          </el-button>

          <el-dropdown
            trigger="click"
            @command="(command) => handleCommand(command, scope.row)"
          >
            <span :class="'el-dropdown-link ' + $store.getters.size">
              <i class="el-icon-d-arrow-right el-icon--right"></i>更多
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                command="handleConfig"
                icon="el-icon-s-cooperation"
                :disabled="
                  hasPermi(['produce:workorder:config']) ||
                  (scope.row.manuOrderStatus != 0 &&
                    scope.row.manuOrderStatus != 1)
                "
                >调整路线</el-dropdown-item
              >
              <el-dropdown-item
                icon="el-icon-edit"
                command="handleSelectColor"
                :disabled="hasPermi(['produce:workorder:remarkcolor'])"
                >标记颜色</el-dropdown-item
              >
              <el-dropdown-item
                command="handleSubmitTask"
                icon="el-icon-check"
                :disabled="
                  hasPermi(['produce:workorder:commit']) ||
                  scope.row.manuOrderStatus == 0
                "
                >提交任务</el-dropdown-item
              >
              <el-dropdown-item
                command="handleCancelTask"
                icon="el-icon-refresh-left"
                :disabled="
                  hasPermi(['produce:workorder:revoke']) ||
                  scope.row.manuOrderStatus == 0
                "
                >撤回任务</el-dropdown-item
              >
              <el-dropdown-item
                icon="el-icon-edit"
                command="handleUpdate"
                :disabled="
                  hasPermi(['produce:workorder:edit']) ||
                  scope.row.manuOrderStatus != 0
                "
                >修改</el-dropdown-item
              >

              <el-dropdown-item
                :command="composeValue('MoveIn', scope.row)"
                :disabled="hasPermi(['produce:workorder:setMoveInTime'])"
              >
                <svg-icon icon-class="Move-In" />
                MoveIn
              </el-dropdown-item>
              <el-dropdown-item
                :command="composeValue('MoveOut', scope.row)"
                :disabled="hasPermi(['produce:workorder:setMoveOutTime'])"
              >
                <svg-icon icon-class="Move-Out" />
                MoveOut
              </el-dropdown-item>
              <el-dropdown-item
                :command="composeValue('TrackIn', scope.row)"
                :disabled="hasPermi(['produce:workorder:setTrackInTime'])"
              >
                <svg-icon
                  style="transform: rotateY(180deg); /* 水平镜像翻转 */"
                  icon-class="Track-In"
                />
                TrackIn
              </el-dropdown-item>
              <el-dropdown-item
                :command="composeValue('TrackOut', scope.row)"
                :disabled="hasPermi(['produce:workorder:setTrackOutTime'])"
              >
                <svg-icon icon-class="Track-Out" />
                TrackOut
              </el-dropdown-item>
              <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="
                  hasPermi(['produce:workorder:remove']) ||
                  scope.row.manuOrderStatus != 0
                "
                >删除</el-dropdown-item
              >
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
    <!-- 添加或修改生产工单对话框 -->
    <edit-form-dialog
      v-model="open"
      :title="title"
      :optType="optType"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form ref="form" :model="form" label-width="100px" :rules="rules">
        <el-row>
          <el-col :span="10">
            <el-form-item label="工单编码" prop="code">
              <el-input
                v-model="form.code"
                placeholder="请输入工单编码"
                :disabled="autoGenFlag || optType != 'add'"
              />
            </el-form-item>
          </el-col>
          <el-col :span="4">
            <el-form-item label-width="80">
              <el-switch
                v-model="autoGenFlag"
                active-color="#13ce66"
                active-text="自动生成"
                @change="handleAutoGenChange(autoGenFlag)"
                :disabled="optType != 'add'"
              >
              </el-switch>
            </el-form-item>
          </el-col>
          <el-col :span="10">
            <el-form-item label="工单名称" prop="name">
              <el-input
                v-model="form.name"
                :disabled="optType == 'view'"
                placeholder="请输入工单名称"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="产品编码" prop="itemCode">
              <el-autocomplete
                :disabled="optType == 'view'"
                v-model="form.itemCode"
                :fetch-suggestions="querySearchAsync"
                placeholder="请输入内容"
                @select="onProductSelected"
                popper-class="el-autocomplete-suggestion"
                :popper-append-to-body="false"
                :debounce="0"
              >
                <el-button
                  :disabled="optType == 'view'"
                  v-debounce
                  slot="append"
                  @click="handleSelectProduct"
                  icon="el-icon-search"
                ></el-button>
              </el-autocomplete>
              <ProductSelect
                ref="ProductSelect"
                @onSelected="onProductSelected"
              >
              </ProductSelect>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="工艺路线" prop="route">
              <el-input
                v-if="optType == 'view'"
                v-model="form.route"
                disabled
                :placeholder="form.routeId != null ? form.route : '暂未配置'"
              />
              <el-select
                v-else
                v-model="form.route"
                placeholder="工艺路线"
                @change="selectRoute"
                :popper-append-to-body="false"
                popper-class="hide-select"
              >
                <el-option
                  v-for="item in routeOptions"
                  :key="item.id"
                  :label="item.label"
                  :value="item.label"
                  :title="item.label"
                />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="需求日期" prop="requestDate">
              <el-date-picker
                :disabled="optType == 'view'"
                :picker-options="pickerOptions"
                clearable
                v-model="form.requestDate"
                type="date"
                value-format="yyyy-MM-dd"
                style="width: 206px"
                placeholder="请选择需求日期"
              >
              </el-date-picker>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="计划PCS" prop="quantity">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :dis="optType == 'view'"
                :myNum="form.quantity"
                @changeNum="changeNumQuantity"
                :numName="'quantity'"
                :min="0"
                :max="1000"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="调整后PCS" prop="quantityChanged">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.quantityChanged"
                @changeNum="changeNum"
                :numName="'quantityChanged'"
                :dis="optType == 'view'"
                :min="0"
                :max="1000"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="SpecGroup" prop="specGroup">
              <el-input
                v-model="form.specGroup"
                placeholder="请输入SpecGroup"
                :disabled="optType == 'view'"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="叠数" prop="panelCount">
              <el-input
                disabled
                v-model="form.panelCount"
                placeholder="请选择产品"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="总组板数" prop="wadCount">
              <el-input v-model="form.wadCount" disabled />
              <span class="prompt">总组板数 = PCS数 / 叠数</span>
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="24">
            <el-form-item label="转换前路径" prop="beforeDrillFilePath">
              <el-input
                :disabled="optType == 'view'"
                v-model="form.beforeDrillFilePath"
                type="textarea"
                placeholder="请输入文件路径"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="转换后路径" prop="afterDrillFilePath">
              <el-input
                :disabled="optType == 'view'"
                v-model="form.afterDrillFilePath"
                type="textarea"
                placeholder="请输入文件路径"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>
        </el-row>

        <el-collapse v-model="activeNames" @change="handleChange">
          <el-collapse-item name="params">
            <span class="collapse-title" slot="title">更多参数</span>
            <el-row>
              <el-col :span="8">
                <el-form-item label="来源类型" prop="orderSource">
                  <el-select
                    v-model="form.orderSource"
                    :disabled="optType == 'view'"
                  >
                    <el-option :value="'客户订单'">客户订单</el-option>
                    <el-option :value="'库存需求'">库存需求</el-option>
                  </el-select>
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item label="来源单据" prop="sourceCode">
                  <el-input
                    :disabled="optType == 'view'"
                    v-model="form.sourceCode"
                    placeholder="请输入来源单据"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item label="是否紧急" prop="a">
                  <el-radio-group
                    v-removeAriaHidden
                    v-model="form.isUrgent"
                    :disabled="optType == 'view'"
                  >
                    <el-radio :label="1">是</el-radio>
                    <el-radio :label="0">否</el-radio>
                  </el-radio-group>
                </el-form-item>
              </el-col>
            </el-row>
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
                <el-form-item label="批次号" prop="batchCode">
                  <el-input
                    :disabled="optType == 'view'"
                    v-model="form.batchCode"
                    placeholder="请输入批次号"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item label="孔数" prop="drillCount">
                  <!-- 引入自定义计数器组件 -->
                  <input-number
                    :myNum="form.drillCount"
                    @changeNum="changeNum"
                    :dis="optType == 'view'"
                    :numName="'drillCount'"
                    :min="0"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item label="层数" prop="layerNum">
                  <!-- 引入自定义计数器组件 -->
                  <el-input v-model="form.layerNum" disabled />
                </el-form-item>
              </el-col>
            </el-row>

            <el-row>
              <el-col :span="8">
                <el-form-item label="Move In" prop="moveInTime">
                  <el-input v-model="form.moveInTime" disabled />
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item label="Move Out" prop="moveOutTime">
                  <el-input v-model="form.moveOutTime" disabled />
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item label="标记颜色" prop="remarkColor">
                  <div :style="{ display: 'flex' }">
                    <div
                      :style="{
                        background: form.remarkColor,
                        marginRight: '3px',
                      }"
                      :class="$store.getters.size + '-remarkColor'"
                    ></div>
                    <el-button
                      :disabled="optType == 'view'"
                      v-debounce
                      @click="handleSelectColor"
                      icon="el-icon-search"
                    ></el-button>
                  </div>
                </el-form-item>
              </el-col>
            </el-row>
            <el-row>
              <el-col :span="8">
                <el-form-item label="Track In" prop="trackInTime">
                  <el-input v-model="form.trackInTime" disabled />
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item label="Track Out" prop="trackOutTime">
                  <el-input v-model="form.trackOutTime" disabled />
                </el-form-item>
              </el-col>
            </el-row>

            <el-row>
              <el-col :span="8">
                <el-form-item label="客户编码" prop="clientCode">
                  <el-input
                    :disabled="optType == 'view'"
                    v-model="form.clientCode"
                    placeholder="请输入客户编码"
                  >
                    <el-button
                      v-debounce
                      :disabled="optType == 'view'"
                      slot="append"
                      @click="handleSelectClient"
                      icon="el-icon-search"
                    ></el-button>
                  </el-input>
                  <ClientSelect
                    ref="clientSelect"
                    @onSelected="onClientSelected"
                  >
                  </ClientSelect>
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item label="客户名称" prop="clientName">
                  <el-input
                    :disabled="optType == 'view'"
                    v-model="form.clientName"
                    placeholder="请输入客户名称"
                  />
                </el-form-item>
              </el-col>
            </el-row>
          </el-collapse-item>
          <el-collapse-item v-if="optType == 'view'" name="task">
            <span class="collapse-title" slot="title">任务信息</span>
            <el-table v-loading="taskLoading" :data="taskList" border>
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
                  />
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
              <el-table-column
                label="排产数量"
                align="center"
                prop="quantity"
              />
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
                      {{
                        parseTime(scope.row.startTime, "{m}-{d} {h}:{i}:{s}")
                      }}
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
                        parseTime(
                          scope.row.realStartTime,
                          "{m}-{d} {h}:{i}:{s}"
                        )
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
                      {{
                        parseTime(scope.row.createTime, "{m}-{d} {h}:{i}:{s}")
                      }}
                    </div>
                  </el-tooltip>
                </template>
              </el-table-column>
            </el-table>

            <pagination
              v-show="taskTotal > 0"
              :total="taskTotal"
              :page.sync="taskQueryParams.pageNum"
              :limit.sync="taskQueryParams.pageSize"
              @pagination="getTaskList"
              :autoScroll="false"
            />
          </el-collapse-item>
          <el-collapse-item v-if="optType == 'view'" name="panel">
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
                  <el-tag
                    type="succes"
                    v-else-if="scope.row.productStatus == '1'"
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
              v-show="panelTotal > 0"
              :total="panelTotal"
              :page.sync="panelQueryParams.pageNum"
              :limit.sync="panelQueryParams.pageSize"
              @pagination="getPanelList"
              :autoScroll="false"
            />
          </el-collapse-item>
        </el-collapse>
      </el-form>
    </edit-form-dialog>
    <!-- 生产工单导入 -->
    <ImportXlsx
      ref="upload"
      @getList="getList"
      :uploadUrl="uploadUrl"
      :downloadUrl="downloadUrl"
      :fileName="fileName"
    ></ImportXlsx>
    <!-- 分配工艺路线 -->
    <RouteSelect ref="routeSelect" @onSelected="onRouteSelected" />
    <!-- 公共查看弹框 -->
    <ViewDialog ref="ViewDialog" />
    <!-- 添加生产任务对话框 -->
    <ProdTaskDialog ref="ProdTaskDialog" @parentGetList="getList" />
    <!-- 审批工单对话框 -->
    <ApprovalWorkOrder ref="ApprovalWorkOrder" @parentGetList="getList" />
    <!-- 颜色 -->
    <ColorSelect ref="ColorSelect" @onSelected="onColorSelected"> </ColorSelect>
  </div>
</template>

<script>
import {
  listWorkOrder,
  getWorkOrder,
  addWorkOrder,
  updateWorkOrder,
  updateWorkOrderRoute,
  delWorkOrder,
  delList,
  commitTask,
  robackTask,
  bulkAddDrillTaskByMO,
  remarkWorkOrderColor,
  getWorkOrderAndPanelList,
  clearWorkStation,
  setMoveInTime,
  setMoveOutTime,
  setTrackInTime,
  setTrackOutTime,
  verifyWIPItemNum,
} from "@/api/produce/workOrder";
import { listItem } from "@/api/masterData/item";

import { listAllProcess, listTask } from "@/api/produce/task";
// 物料产品选择
import ProductSelect from "@/components/itemSelect";
// 工艺路线选择
import RouteSelect from "@/components/routeSelect";
// 颜色选择
import ColorSelect from "@/components/colorSelect";
// 客户选择
import ClientSelect from "@/components/clientSelect";
// 公用查看弹框
import ViewDialog from "@/components/viewDialog";
// 生成任务对话框
import ProdTaskDialog from "./productionTaskDialog.vue";
// 审批对话框
import ApprovalWorkOrder from "./approvalWorkOrder.vue";
import { MessageBox } from "element-ui";
export default {
  name: "Workorder",
  components: {
    ProductSelect,
    ClientSelect,
    ColorSelect,
    RouteSelect,
    ViewDialog,
    ProdTaskDialog,
    ApprovalWorkOrder,
  },

  data() {
    // 自定义校验规则
    const checkpanelCount = (rule, value, callback) => {
      if (!value || value <= 0) {
        return callback(new Error("叠数不能为0"));
      }
      callback();
    };
    const checkQuantityProduced = (rule, value, callback) => {
      if (value > this.form.quantityChanged) {
        return callback(new Error("已生产数量不得大于调整数量"));
      } else if (value > this.form.quantityScheduled) {
        return callback(new Error("已生产数量不得大于已排产数量"));
      }
      callback();
    };
    const checkQuantityScheduled = (rule, value, callback) => {
      if (value > this.form.quantityChanged) {
        return callback(new Error("已排产数量不得大于调整数量"));
      }
      callback();
    };
    const checkQuantityChanged = (rule, value, callback) => {
      if (value < this.form.quantity) {
        return callback(new Error("调整数量不得小于生产数量"));
      } else if (value <= 0) {
        return callback(new Error("调整数量不得为0"));
      }
      callback();
    };
    const checkQuantity = (rule, value, callback) => {
      if (!value || value <= 0) {
        return callback(new Error("生产数量不能为0"));
      }
      callback();
    };
    const checkItemCode = async (rule, value, callback) => {
      const res = await listItem({
        pageNum: 1,
        pageSize: 1000,
        name: undefined,
        code: value,
        // 只允许选择产品物料
        itemOrProduct: "2",
        status: 1,
      });
      if (!value && this.optType != "view") {
        return callback(new Error("产品编码不能为空"));
      } else if (res.data.list.length == 0) {
        return callback(new Error("产品编码不存在,请重新输入或选择"));
      }
      callback();
    };
    const checkBeforeDrillFilePath = (rule, value, callback) => {
      //查找"_convduo"
      let patt = new RegExp("_convduo", "i");
      // 是否包含
      let result = patt.test(value);
      if (result) {
        return callback(new Error("当前路径不允许包含'_convduo'字段"));
      }
      callback();
    };
    return {
      timeout: null,
      page: "workOrder",
      //自动生成编码
      autoGenFlag: false,
      enCode: "",
      optType: undefined,
      // 遮罩层
      loading: true,
      // 显示搜索条件
      showSearch: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 生产工单表格数据
      workOrderList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      total: 0,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        sourceCode: undefined,
        itemId: undefined,
        itemCode: undefined,
        itemName: undefined,
        itemTypeId: undefined,
        batchCode: undefined,
        status: undefined,
        code: undefined,
        name: undefined,
        orderSource: undefined,
        clientName: undefined,
        clientCode: undefined,
        requestDate: undefined,
        manuOrderStatusList: [],
        queryOrderBy: 2,
        isUrgent: undefined,
        isExternal: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      layerNumOptions: Array.from({ length: 12 }, (v, i) => {
        return { label: i + 1, value: i + 1 };
      }),
      // 表单校验
      rules: {
        code: [
          { required: true, message: "工单编码不能为空", trigger: "change" },
        ],
        name: [
          { required: true, message: "工单名称不能为空", trigger: "change" },
        ],
        orderSource: [
          { required: true, message: "来源类型不能为空", trigger: "change" },
        ],
        itemCode: [
          { required: true, validator: checkItemCode, trigger: "change" },
        ],
        itemName: [
          { required: true, message: "产品名称不能为空", trigger: "change" },
        ],
        unitOfMeasure: [
          { required: true, message: "单位不能为空", trigger: "change" },
        ],
        quantity: [
          { required: true, validator: checkQuantity, trigger: "change" },
        ],
        quantityChanged: [
          {
            required: true,
            validator: checkQuantityChanged,
            trigger: "change",
          },
        ],
        quantityProduced: [
          {
            required: true,
            validator: checkQuantityProduced,
            trigger: "change",
          },
        ],
        quantityScheduled: [
          {
            required: true,
            validator: checkQuantityScheduled,
            trigger: "change",
          },
        ],
        requestDate: [
          { required: true, message: "需求日期不能为空", trigger: "change" },
        ],
        panelCount: [
          { required: true, validator: checkpanelCount, trigger: "change" },
        ],
        route: [
          { required: true, message: "工艺路线不能为空", trigger: "change" },
        ],
        isUrgent: [
          { required: true, message: "订单状态不能为空", trigger: "change" },
        ],
        // 转换前路径校验
        beforeDrillFilePath: [
          {
            required: false,
            validator: checkBeforeDrillFilePath,
            trigger: "change",
          },
        ],
      },
      // 生产工单导入参数
      // 导入的url
      uploadUrl: "/v1/WorkOrder/UploadList",
      downloadUrl: "/v1/WorkOrder/DownLoad",
      // 导入的模板下载名
      fileName: "生产工单模板.xlsx",
      activeNames: [],
      // 工艺路线
      route: undefined,
      routeOptions: [],
      taskList: [],
      panelList: [],
      taskLoading: true,
      panelLoading: true,
      activeNames: [],
      taskTotal: 0,
      panelTotal: 0,
      // 查询参数
      taskQueryParams: {
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
      // 列信息
      columns: [
        { key: 0, label: "工单编码", visible: true },
        { key: 1, label: "创建时间", visible: true },
        { key: 2, label: "产品编码", visible: true },
        { key: 3, label: "工艺路线", visible: true },
        { key: 4, label: "是否紧急", visible: true },
        { key: 5, label: "状态", visible: true },
        { key: 6, label: "层数", visible: true },
        { key: 7, label: "叠数", visible: true },
        { key: 8, label: "总组板数", visible: true },
        { key: 9, label: "是否外部工单", visible: true },
        { key: 10, label: "标记颜色", visible: true },
        { key: 11, label: "单位", visible: true },
        { key: 12, label: "调整后PCS", visible: true },
        { key: 13, label: "已排产PCS", visible: true },
        { key: 14, label: "已生产PCS", visible: true },
        { key: 15, label: "客户名称", visible: true },
        { key: 16, label: "需求日期", visible: true },
        { key: 17, label: "预计完成时长", visible: true },
      ],
      //标记颜色默认色
      workOrderColor: "#ff5722",
      pickerOptions: {
        disabledDate(time) {
          return time.getTime() < Date.now() - 24 * 60 * 60 * 1000;
        },
      },
    };
  },
  watch: {
    open(val) {
      if (!val) {
        this.activeNames = [];
      }
    },
    "form.itemCode": {
      handler(val) {
        if (val == "") {
          this.form.itemCode = undefined;
          this.form.itemId = undefined;
          this.form.itemTypeId = undefined;
          this.form.itemName = undefined;
          this.form.unitOfMeasure = undefined;
          this.form.specification = undefined;
          this.form.panelCount = 0;
          this.form.routeId = undefined;
          this.form.routeName = undefined;
          this.form.routeCode = undefined;
          this.form.route = undefined;
          this.form.dispenseMachines = 0;
          this.form.specGroup = undefined;
          this.form.beforeDrillFilePath = undefined;
          this.form.afterDrillFilePath = undefined;
          this.computeWadCount();
        }
      },
      deep: true,
    },
    "form.clientCode": {
      handler(val) {
        if (val == "") {
          this.form.clientCode = undefined;
          this.form.clientName = undefined;
          this.form.clientId = undefined;
        }
      },
      deep: true,
    },
  },

  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
  },
  methods: {
    /** 查询生产工单列表 */
    getList(isSearch) {
      const local_arr = this.$cache.local.getJSON("workOrder_statusList")
        ? JSON.parse(this.$cache.local.getJSON("workOrder_statusList"))
        : [];
      this.queryParams.manuOrderStatusList = Array.isArray(local_arr)
        ? local_arr
        : [local_arr];
      this.queryParams.queryOrderBy =
        this.$cache.local.get("workOrder_QueryOrderBy") != undefined &&
        this.$cache.local.get("workOrder_QueryOrderBy") != "0"
          ? this.$cache.local.get("workOrder_QueryOrderBy") * 1
          : 2;

      this.loading = true;
      listWorkOrder(this.queryParams).then((res) => {
        this.workOrderList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
      });
    },
    // 查看工单下任务
    getTaskList() {
      this.taskLoading = true;
      this.taskQueryParams.workOrderId = this.form.id;
      listTask(this.taskQueryParams).then((res) => {
        this.taskList = res.data.list;
        this.taskTotal = res.data.total;
        this.taskLoading = false;
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
    // 计算待排产叠数
    computeWadCount() {
      if (this.form.quantityChanged == 0 || this.form.panelCount == 0) {
        // 叠数计算
        this.form.wadCount = 0;
      } else {
        // 叠数计算
        this.form.wadCount = Math.ceil(
          this.form.quantityChanged / this.form.panelCount
        );
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
        name: "",
        code: "",
        orderSource: "客户订单",
        sourceCode: "",
        itemId: undefined,
        itemName: undefined,
        itemCode: undefined,
        itemTypeId: undefined,
        batchCode: undefined,
        specification: undefined,
        unitOfMeasure: undefined,
        panelCount: 1,
        wadCount: 0,
        drillCount: 10001,
        isUrgent: 0,
        remarkColor: "#20b2aa",
        dispenseMachines: 0,
        routeId: undefined,
        routeName: undefined,
        routeCode: undefined,
        route: undefined,
        quantity: 0,
        quantityChanged: 0,
        quantityProduced: 0,
        quantityScheduled: 0,
        clientId: undefined,
        clientName: undefined,
        clientCode: undefined,
        requestDate: null,
        panelCount: 0,
        wadCount: 0,
        drillCount: 10000,
        isUrgent: 0,
        remarkColor: this.workOrderColor,
        beforeDrillFilePath: "",
        afterDrillFilePath: "",
      };
      this.autoGenFlag = false;
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.$cache.local.remove("workOrder_statusList");
      this.$cache.local.remove("workOrder_QueryOrderBy");
      this.handleQuery();
    },

    // 多选工单状态
    handleselectManuOrderStatusList(val) {
      this.$cache.local.setJSON(
        "workOrder_statusList",
        JSON.stringify(Array.isArray(val) ? val : [val])
      );
    },
    // 单选工单状态
    handleselectManuOrderStatus(val) {
      this.$cache.local.setJSON("workOrder_status", val);
    },
    // 监听排序方式选择
    handleQueryOrderBy(val) {
      this.$cache.local.set("workOrder_QueryOrderBy", val);
    },
    // 点击查看
    handleView(id) {
      this.reset();
      getWorkOrder(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.form.remarkColor = res.data.remarkColor
            ? res.data.remarkColor
            : this.workOrderColor;
          this.open = true;
          this.title = "查看工单信息";
          this.optType = "view";
          this.autoGenFlag = false;
          // this.$refs.ViewDialog.open = true;
          // this.$refs.ViewDialog.form = res.data;
          // this.$refs.ViewDialog.title = "查看工单信息";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 新增按钮操作 */
    handleAdd(row) {
      this.reset();
      this.routeOptions = [];
      this.activeNames = ["open"];
      this.open = true;
      this.title = "添加生产工单";
      this.optType = "add";
      this.autoGenFlag = false;
      this.initialForm = Object.assign({}, this.form);
    },

    //批量生成任务
    handleProdTaskAll() {
      if (this.ids.length <= 0)
        return this.$modal.msgWarning("请选择要操作的数据!");
      const flag = this.multipleSelection.some(
        (item) => item.manuOrderStatus != 1
      );
      if (flag) return this.$modal.notifyError("请选择已审批的工单！");
      const list = this.multipleSelection.map((v) => {
        return {
          workOrderId: v.id,
          workOrderCode: v.code,
          panelCount: v.panelCount,
          wadCount: v.wadCount,
          shaftCount: v.drillTaskShaftCount ? v.drillTaskShaftCount : 0,
          isUrgent: v.isUrgent,
          requestDate: v.requestDate,
        };
      });
      const loading = this.$loading({
        lock: true,
        text: "正在生成任务......",
        spinner: "el-icon-loading",
        background: "rgba(0, 0, 0, 0.7)",
      });
      bulkAddDrillTaskByMO(list).then((res) => {
        if (res.code == 0) {
          this.$modal.msgSuccess("操作成功");
          this.getList();
          loading.close();
        } else {
          this.$modal.notifyError(res.message);
          loading.close();
        }
      });
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      this.activeNames = [];
      const id = row.id || this.ids;
      getWorkOrder(id).then((res) => {
        if (res.code == 0) {
          this.form = {
            ...res.data,
            remarkColor: res.data.remarkColor
              ? res.data.remarkColor
              : this.workOrderColor,
            requestDate: new Date(res.data.requestDate).toLocaleDateString(
              "sv-SE"
            ),
            batchCode: res.data.batchCode ? res.data.batchCode : "",
          };

          this.enCode = res.data.code;
          this.enName = res.data.name;
          this.open = true;
          this.title = "修改生产工单";
          this.optType = "edit";
          this.autoGenFlag = false;
          listAllProcess({ itemId: res.data.itemId }).then((response) => {
            this.routeOptions = response.data.routeInfos.map((v) => {
              return {
                label: v.code + " " + v.name,
                value: v.id,
                code: v.code,
                name: v.name,
              };
            });
          });
          this.initialForm = Object.assign(
            {},
            {
              ...res.data,
              requestDate: new Date(res.data.requestDate).toLocaleDateString(
                "sv-SE"
              ),
              remarkColor: res.data.remarkColor
                ? res.data.remarkColor
                : "#ff5722",
              batchCode: res.data.batchCode ? res.data.batchCode : "",
            }
          );
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },

    // 点击审批
    handleApproval(row) {
      this.reset();
      const id = row.id || this.ids;
      this.$refs.ApprovalWorkOrder.changeForm(id);
    },
    // 点击生成任务按钮
    handleProdTask(row) {
      this.reset();
      const id = row.id || this.ids;
      this.$refs.ProdTaskDialog.changeForm(id);
    },
    // 提交任务
    handleSubmitTask(row) {
      commitTask({ workOrderCode: row.code }).then((res) => {
        if (res.code == 0) {
          this.$modal.msgSuccess("操作成功");
          this.getList();
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 撤回任务
    handleCancelTask(row) {
      robackTask({ workOrderCode: row.code }).then((res) => {
        if (res.code == 0) {
          this.$modal.msgSuccess("操作成功");
          this.getList();
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // MoveIn/Out;TarckIn/Out
    handleSet(val) {
      let api = null;
      this.$modal
        .confirm("确定对当前操作的数据项执行" + val.name + "？")
        .then((result) => {
          if (result == "confirm") {
            switch (val.name) {
              case "MoveIn":
                api = setMoveInTime;
                break;
              case "MoveOut":
                api = setMoveOutTime;
                break;
              case "TrackIn":
                api = setTrackInTime;
                break;
              case "TrackOut":
                api = setTrackOutTime;
                break;
              default:
                break;
            }
            api(val.row.code).then((res) => {
              if (res.code == 0) {
                this.$modal.msgSuccess(val.name + " 操作执行成功");
              } else {
                this.$modal.notifyError(res.message);
              }
            });
          }
        })
        .catch(() => {});
    },
    composeValue(name, row) {
      return {
        name,
        row,
      };
    },
    // 更多操作触发
    handleCommand(command, row) {
      if (typeof command == "object") {
        return this.handleSet(command);
      }
      switch (command) {
        case "handleConfig":
          this.handleConfig(row);
          break;
        case "handleSubmitTask":
          this.handleSubmitTask(row);
          break;
        case "handleCancelTask":
          this.handleCancelTask(row);
          break;

        case "handleUpdate":
          this.handleUpdate(row);
          break;
        case "handleDelete":
          this.handleDelete(row);
          break;
        case "handleSelectColor":
          this.handleSelectColor(row);
          break;
        default:
          break;
      }
    },
    // 新增 修改
    changeDate() {
      if (this.form.id != null) {
        updateWorkOrder(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addWorkOrder(this.form).then((res) => {
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
    /** 提交按钮 */
    submitForm() {
      this.form = { ...this.form, isAddWorkOrder: 0 };
      verifyWIPItemNum(this.form).then((flag) => {
        if (flag.code == 1) {
          // 只提示无操作
          // MessageBox.confirm(flag.message, "系统提示", {
          //   showCancelButton: false,
          //   showConfirmButton: false,
          //   type: "warning",
          // })
          //   .then((confirm) => {})
          //   .catch(() => {});

          // 提示并且用户可进行确认取消操作
          this.$modal
            .confirm(flag.message)
            .then((confirm) => {
              this.changeDate();
            })
            .catch(() => {});
        } else {
          this.changeDate();
        }
      });
    },
    // 删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.id,
          delWorkOrder,
          this.getList,
          "工单编号为" + row.code
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },

    // 点击配置
    handleConfig(row) {
      const id = row.id || this.ids;
      this.reset();
      getWorkOrder(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.$refs.routeSelect.showFlag = true;
          this.$refs.routeSelect.selectedRouteId = row.routeId
            ? row.routeId
            : undefined;
          this.$refs.routeSelect.getList(row.itemId);
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    //工艺路线选择
    onRouteSelected(obj) {
      if (obj != undefined && obj != null) {
        updateWorkOrderRoute({
          id: this.form.id,
          routeId: obj.id,
          routeCode: obj.code,
          routeName: obj.name,
        }).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("配置成功");
            this.getList();
            clearWorkStation(this.form.code);
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 选择工艺路线
    selectRoute(val) {
      this.form.routeId = this.routeOptions.find((v) => v.label == val).value;
      this.form.routeName = this.routeOptions.find((v) => v.label == val).name;
      this.form.routeCode = this.routeOptions.find((v) => v.label == val).code;
      // 解决设置默认值后切换试图不更新(强制刷新)
      this.$forceUpdate();
    },
    //物料选择弹出框
    handleSelectProduct() {
      this.$refs.ProductSelect.showFlag = true;
      this.$refs.ProductSelect.title = "产品选择";
      this.$refs.ProductSelect.selectedItemCode = this.form.itemCode
        ? this.form.itemCode
        : undefined;
      this.$refs.ProductSelect.getList();
    },
    onProductSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "itemCode", obj.code);
        this.$set(this.form, "itemId", obj.id);
        this.$set(this.form, "itemTypeId", obj.itemTypeId);
        this.$set(this.form, "itemName", obj.name);
        this.$set(this.form, "unitOfMeasure", obj.unitOfMeasure);
        this.$set(this.form, "specification", obj.specification);
        this.$set(this.form, "panelCount", obj.panelCount);
        this.$set(this.form, "layerNum", obj.layerNum);
        this.$set(this.form, "specGroup", obj.specGroup);
        this.$set(this.form, "beforeDrillFilePath", obj.beforeDrillFilePath);
        this.$set(this.form, "afterDrillFilePath", obj.afterDrillFilePath);
        this.$set(
          this.form,
          "dispenseMachines",
          obj.dispenseMachines != null ? obj.dispenseMachines : 0
        );
        listAllProcess({ itemId: obj.id }).then((response) => {
          this.routeOptions = response.data.routeInfos.map((v) => {
            return {
              label: v.code + " " + v.name,
              value: v.id,
              code: v.code,
              name: v.name,
            };
          });
          this.form.routeId = this.routeOptions[0]?.value;
          this.form.routeName = this.routeOptions[0]?.name;
          this.form.routeCode = this.routeOptions[0]?.code;
          this.form.route = this.routeOptions[0]?.label;
        });
        this.computeWadCount();
      }
    },
    //客户选择弹出框
    handleSelectClient() {
      this.$refs.clientSelect.showFlag = true;
      this.$refs.clientSelect.selectedClientId = this.form.clientId
        ? this.form.clientId
        : undefined;
      this.$refs.clientSelect.getList();
    },
    onClientSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "clientId", obj.id);
        this.$set(this.form, "clientCode", obj.code);
        this.$set(this.form, "clientName", obj.name);
      }
    },
    // 颜色选择弹出框
    handleSelectColor(row) {
      const remarkColor = row.id ? row.remarkColor : this.form.remarkColor;
      if (row.id) {
        this.optType = "color";
        this.reset();
        getWorkOrder(row.id).then((res) => {
          if (res.code == 0) {
            this.form = res.data;
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }

      this.$refs.ColorSelect.showFlag = true;
      this.$refs.ColorSelect.selectedColorId = remarkColor
        ? remarkColor
        : undefined;
      this.$refs.ColorSelect.getList();
    },
    onColorSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "remarkColor", obj.value);
        if (this.optType == "color") {
          remarkWorkOrderColor({
            id: this.form.id,
            remarkColor: this.form.remarkColor,
          }).then((res) => {
            if (res.code == 0) {
              this.$modal.msgSuccess("标记成功");
              this.getList();
            } else {
              this.$modal.notifyError(res.message);
            }
          });
        }
      }
    },
    //点击生产数量计数器
    changeNumQuantity(params) {
      this.form[params.str] = params.value;
      this.form.quantityChanged = this.form.quantity;
      this.computeWadCount();
    },
    // 点击计数器
    changeNum(params) {
      this.form[params.str] = params.value;
      this.computeWadCount();
    },
    /** 导入按钮操作 */
    handleImport() {
      this.$refs.upload.title = "工单导入";
      this.$refs.upload.open = true;
    },
    //自动生成码
    handleAutoGenChange(autoGenFlag) {
      if (autoGenFlag) {
        this.getEncode({
          rulesCode: "WORKORDER_CODE",
          buildCount: 1,
        }).then((response) => {
          const code = response.data[0];
          this.form.code = code;
          this.form.name = code;
        });
      } else {
        if (this.optType == "edit") {
          this.form.code = this.enCode;
          this.form.name = this.enName;
          return;
        }
        this.form.code = "";
        this.form.name = "";
      }
    },
    querySearchAsync(queryString, cb) {
      if (queryString === "" || !queryString) {
        let arr = [];
        cb(arr);
      } else {
        clearTimeout(this.timeout);
        this.timeout = setTimeout(() => {
          listItem({
            pageNum: 1,
            pageSize: 1000,
            name: undefined,
            code: queryString,
            // 只允许选择产品物料
            itemOrProduct: "2",
            status: 1,
          }).then((res) => {
            if (res.code === 0) {
              let arr = res.data.list.map((v) => {
                return {
                  value: v.code,
                  ...v,
                };
              });
              const showSuggestion = document.querySelector(
                ".el-autocomplete-suggestion"
              );
              cb(arr);
              if (arr.length > 0) {
                showSuggestion.style.display = "block";
              }
            } else {
              this.$modal.notifyError(res.message);
            }
          });
        }, 500);
      }
    },
    // 点击折叠面板
    handleChange(val) {
      let id = this.activeNames.pop();
      if (id) {
        if (id == "task") {
          this.getTaskList();
        } else if (id == "panel") {
          this.getPanelList();
        }
        this.$nextTick(() => {
          // 结构渲染完毕后执行
          this.activeNames.push(id); //添加对应的name值
        });
      }
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
}
::v-deep .el-collapse-item__wrap {
  border: none;
}
::v-deep .el-autocomplete-suggestion {
  width: auto !important;
  display: none;
}
::v-deep .el-autocomplete-suggestion.is-loading {
  width: 150px !important;
}
</style>
