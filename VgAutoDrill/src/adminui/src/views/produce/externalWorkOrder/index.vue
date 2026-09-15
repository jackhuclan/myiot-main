<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="外部工单编码" prop="sourceCode">
        <el-input
          v-trim
          v-model="queryParams.sourceCode"
          placeholder="请输入外部工单编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="内部工单编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入内部工单编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
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
      <el-form-item label="工艺路线" prop="routeCode">
        <el-select
          v-model="queryParams.routeCode"
          placeholder="请选择"
          @clear="clearQueryParams('routeCode')"
          clearable
        >
          <el-option
            v-for="item in routeQueryList"
            :key="item.id"
            :label="item.label"
            :value="item.code"
            v-optionTitle
          ></el-option>
        </el-select>
      </el-form-item>

      <el-form-item label="工序组" prop="specGroup">
        <el-input
          v-trim
          v-model="queryParams.specGroup"
          placeholder="请输入工序组"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="非法数据" prop="isErrorData">
        <el-select
          v-model="queryParams.isErrorData"
          placeholder="请选择"
          @clear="clearQueryParams('isErrorData')"
          clearable
        >
          <el-option label="是" :value="1"></el-option>
          <el-option label="否" :value="0"></el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="处理结果" prop="externalStatus">
        <el-select
          v-model="queryParams.externalStatus"
          placeholder="请选择"
          @clear="clearQueryParams('externalStatus')"
          clearable
        >
          <el-option label="未处理" :value="0"></el-option>
          <el-option label="处理成功" :value="1"></el-option>
          <el-option label="处理失败" :value="2"></el-option>
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
          type="danger"
          plain
          icon="el-icon-delete"
          @click="handleDelete"
          :disabled="hasPermi(['produce:externalWorkOrder:remove'])"
          >批量删除</el-button
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
      :data="externalWorkOrderList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />

      <el-table-column
        label="外部工单"
        key="sourceCode"
        prop="sourceCode"
        show-overflow-tooltip
        fixed="left"
        min-width="180px"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['produce:externalWorkOrder:view']"
            >{{ scope.row.sourceCode }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="处理结果"
        fixed="left"
        key="remark"
        prop="remark"
        min-width="150px"
        v-if="columns[1].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.remark" />
        </template>
      </el-table-column>

      <el-table-column
        label="内部工单"
        key="code"
        prop="code"
        show-overflow-tooltip
        min-width="180px"
        v-if="columns[2].visible"
      >
      </el-table-column>
      <el-table-column
        label="内部工单状态"
        key="innerOrderStatus"
        prop="innerOrderStatus"
        show-overflow-tooltip
        min-width="100px"
        v-if="columns[30].visible"
        align="center"
      >
        <template slot-scope="scope">
          <status-tag
            :options="$status.workOrderOptions"
            :status="scope.row.innerOrderStatus"
          ></status-tag>
        </template>
      </el-table-column>

      <el-table-column
        label="配刀相关"
        key="cutterInfo"
        prop="cutterInfo"
        show-overflow-tooltip
        min-width="180px"
        v-if="columns[3].visible"
      >
      </el-table-column>
      <el-table-column
        label="产品编码"
        v-if="columns[4].visible"
        key="itemCode"
        prop="itemCode"
        show-overflow-tooltip
        min-width="200px"
      />

      <el-table-column
        label="产品名称"
        v-if="columns[5].visible"
        key="itemName"
        prop="itemName"
        show-overflow-tooltip
        min-width="200px"
      />

      <el-table-column
        label="工艺路线"
        v-if="columns[6].visible"
        key="routeCode"
        prop="routeCode"
        show-overflow-tooltip
        min-width="100px"
      >
      </el-table-column>

      <el-table-column
        label="工序组"
        v-if="columns[7].visible"
        key="specGroup"
        prop="specGroup"
        show-overflow-tooltip
        min-width="100px"
      />

      <el-table-column
        label="单位"
        align="center"
        key="unitOfMeasure"
        prop="unitOfMeasure"
        show-overflow-tooltip
        min-width="100px"
        v-if="columns[8].visible"
      />
      <el-table-column
        label="库存数量"
        align="center"
        key="lotStockNum"
        prop="lotStockNum"
        v-if="columns[9].visible"
      />
      <el-table-column
        label="生产数量"
        align="center"
        key="quantity"
        prop="quantity"
        v-if="columns[10].visible"
      />

      <el-table-column
        label="层数"
        align="center"
        key="layerNum"
        prop="layerNum"
        min-width="60px"
        v-if="columns[11].visible"
      />

      <el-table-column
        label="叠数"
        align="center"
        key="panelCount"
        prop="panelCount"
        min-width="60px"
        v-if="columns[12].visible"
      />
      <el-table-column
        label="创建时间"
        align="center"
        prop="createTime"
        width="180"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.createTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="待排产叠数"
        align="center"
        min-width="100px"
        key="wadCount"
        prop="wadCount"
        v-if="columns[13].visible"
      />

      <el-table-column
        label="板料长度"
        align="center"
        key="panelLength"
        prop="panelLength"
        min-width="80px"
        v-if="columns[14].visible"
      />

      <el-table-column
        label="转换前路径"
        align="center"
        key="beforeDrillFilePath"
        prop="beforeDrillFilePath"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[15].visible"
      />

      <el-table-column
        label="转换后路径"
        align="center"
        key="afterDrillFilePath"
        prop="afterDrillFilePath"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[16].visible"
      />
      <el-table-column
        label="是否暂停"
        align="center"
        width="100"
        key="isHold"
        prop="isHold"
        v-if="columns[17].visible"
      ></el-table-column>

      <el-table-column
        label="是否在钻孔计划区"
        align="center"
        min-width="150px"
        key="isInPlanWarehouse"
        prop="isInPlanWarehouse"
        v-if="columns[18].visible"
      ></el-table-column>

      <el-table-column
        label="是否在钻孔计划区备注"
        align="center"
        min-width="240px"
        key="isInPlanWarehouseRemark"
        prop="isInPlanWarehouseRemark"
        v-if="columns[19].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.isInPlanWarehouseRemark" /> </template
      ></el-table-column>

      <el-table-column
        label="需求日期"
        align="center"
        key="requestDate"
        width="100"
        v-if="columns[20].visible"
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
        v-if="columns[29].visible"
      >
        <template slot-scope="scope">
          {{ scope.row.estimatedTime
          }}{{ scope.row.estimatedTime ? "(分钟)" : "" }}
        </template>
      </el-table-column>
      <el-table-column
        key="moveInTime"
        label="MoveInTime"
        align="center"
        width="180"
        v-if="columns[21].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.moveInTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        key="moveOutTime"
        label="MoveOutTime"
        align="center"
        width="180"
        v-if="columns[22].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.moveOutTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        key="trackInTime"
        label="TrackInTime"
        align="center"
        width="180"
        v-if="columns[23].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.trackInTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        key="trackOutTime"
        label="TrackOutTime"
        align="center"
        width="180"
        v-if="columns[24].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.trackOutTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        key="signMoveInTime"
        label="SignMoveInTime"
        align="center"
        width="180"
        v-if="columns[25].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.signMoveInTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        key="signMoveOutTime"
        label="SignMoveOutTime"
        align="center"
        width="180"
        v-if="columns[26].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.signMoveOutTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        key="signTrackInTime"
        label="SignTrackInTime"
        align="center"
        width="180"
        v-if="columns[27].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.signTrackInTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        key="signTrackOutTime"
        label="SignTrackOutTime"
        align="center"
        width="180"
        v-if="columns[28].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.signTrackOutTime) }}</span>
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
            icon="el-icon-edit"
            @click="handleUpdate(scope.row)"
            :disabled="hasPermi(['produce:externalWorkOrder:edit'])"
            >修改</el-button
          >
          <el-button
            v-if="!scope.row.kwHoldEnable && !scope.row.kwAGVStockInEnable"
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['produce:externalWorkOrder:remove'])"
            >删除</el-button
          >
          <el-dropdown
            v-else
            trigger="click"
            @command="(command) => handleCommand(command, scope.row)"
          >
            <span :class="'el-dropdown-link ' + $store.getters.size">
              <i class="el-icon-d-arrow-right el-icon--right"></i>更多
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                v-if="scope.row.kwHoldEnable"
                command="handleKwHoldEnable"
                icon="el-icon-video-pause"
                :disabled="hasPermi(['produce:externalWorkOrder:kwHold'])"
                >Mes暂停</el-dropdown-item
              >
              <el-dropdown-item
                v-if="scope.row.kwAGVStockInEnable"
                command="handleKwAGVStockInEnable"
                icon="el-icon-refresh"
                :disabled="hasPermi(['produce:externalWorkOrder:kwAGVStockIn'])"
                >Mes转仓</el-dropdown-item
              >
              <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="hasPermi(['produce:externalWorkOrder:remove'])"
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

    <!-- 添加或修改客户对话框 -->

    <edit-form-dialog
      v-model="open"
      :title="title"
      :optType="optType"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form ref="form" :model="form" label-width="100px" :rules="rules">
        <el-row>
          <el-col :span="8">
            <el-form-item
              label="外部工单编码"
              prop="sourceCode"
              label-width="120px"
            >
              <el-input
                disabled
                v-model="form.sourceCode"
                placeholder="请输入外部工单编码"
              />
            </el-form-item>
          </el-col>

          <el-col :span="8">
            <el-form-item label="内部工单编码" prop="code" label-width="120px">
              <el-input
                disabled
                v-model="form.code"
                placeholder="请输入内部工单编码"
              />
            </el-form-item>
          </el-col>

          <el-col :span="8">
            <el-form-item label="生产数量" prop="quantity">
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
                ></el-button
              ></el-autocomplete>

              <ProductSelect
                ref="ProductSelect"
                @onSelected="onProductSelected"
              >
              </ProductSelect>
            </el-form-item>
          </el-col>

          <el-col :span="8">
            <el-form-item label="层数" prop="layerNum">
              <el-input v-model="form.layerNum" disabled />
            </el-form-item>
          </el-col>

          <el-col :span="8">
            <el-form-item label="单位" prop="unitOfMeasure">
              <el-input v-model="form.unitOfMeasure" disabled />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="8">
            <el-form-item label="叠数" prop="panelCount">
              <el-input v-model="form.panelCount" disabled />
            </el-form-item>
          </el-col>

          <el-col :span="8">
            <el-form-item label="待排产叠数" prop="wadCount">
              <el-input v-model="form.wadCount" disabled />
            </el-form-item>
          </el-col>

          <el-col :span="8">
            <el-form-item label="板料长度" prop="panelLength">
              <input-number
                :dis="optType == 'view'"
                :myNum="form.panelLength"
                @changeNum="changeNumQuantity"
                :numName="'panelLength'"
                :min="0"
                :max="1000"
              />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="8">
            <el-form-item label="工艺路线" prop="routeCode">
              <el-select
                v-model="form.routeCode"
                placeholder="工艺路线"
                @change="selectRoute"
                :popper-append-to-body="false"
                :disabled="optType == 'view'"
                popper-class="hide-select"
              >
                <el-option
                  v-for="item in routeOptions"
                  :key="item.id"
                  :label="item.label"
                  :value="item.code"
                  v-optionTitle
                />
              </el-select>
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

        <el-row v-if="optType == 'view'">
          <el-form-item label="创建时间" prop="createTime">
            <el-date-picker
              disabled
              v-model="form.createTime"
              type="datetime"
              placeholder="请选择需求日期"
            >
            </el-date-picker>
          </el-form-item>
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

        <el-row>
          <el-col :span="24">
            <el-form-item label="处理结果" prop="remark">
              <el-input
                :disabled="optType == 'view'"
                v-model="form.remark"
                type="textarea"
                placeholder="请输入处理结果"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import { listItem } from "@/api/masterData/item";
import { listAllProcess } from "@/api/produce/task";
// 物料产品选择
import ProductSelect from "@/components/itemSelect";
// 工艺路线选择
import RouteSelect from "@/components/routeSelect";
import {
  delExternalWorkOrder,
  getExternalWorkOrder,
  listExternalWorkOrder,
  updateExternalWorkOrder,
  kwAGVStockIn,
  kwHoldLot,
  delList,
} from "@/api/produce/externalWorkOrder";
import { getDropSelectDatas } from "@/api/produce/route";
export default {
  name: "ExternalWorkOrder",
  components: {
    ProductSelect,
    RouteSelect,
  },
  data() {
    // 自定义校验规则
    const checkpanelCount = (rule, value, callback) => {
      if (!value || value <= 0) {
        return callback(new Error("叠数不能为0"));
      }
      callback();
    };

    const checkQuantity = (rule, value, callback) => {
      if (!value || value <= 0) {
        return callback(new Error("生产数量不能为0"));
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
    return {
      timeout: null,
      page: "externalWorkOrder",
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
      externalWorkOrderList: [],
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
        specGroup: undefined,
        code: undefined,
        itemCode: undefined,
        routeCode: undefined,
        requestDate: undefined,
        isErrorData: undefined,
        externalStatus: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      routeQueryList: [],
      // 表单校验
      rules: {
        code: [
          { required: true, message: "工单编码不能为空", trigger: "change" },
        ],
        name: [
          { required: true, message: "工单名称不能为空", trigger: "change" },
        ],
        itemCode: [
          { required: true, validator: checkItemCode, trigger: "change" },
        ],

        quantity: [
          { required: true, validator: checkQuantity, trigger: "change" },
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
        // 转换前路径校验
        beforeDrillFilePath: [
          {
            required: false,
            validator: checkBeforeDrillFilePath,
            trigger: "change",
          },
        ],
      },
      // 工艺路线
      routeOptions: [],
      // 列信息
      columns: [
        { key: 0, label: "外部工单", visible: true },
        { key: 1, label: "处理结果", visible: true },
        { key: 2, label: "内部工单", visible: true },
        { key: 3, label: "配刀相关", visible: true },
        { key: 4, label: "产品编码", visible: true },
        { key: 5, label: "产品名称", visible: true },
        { key: 6, label: "工艺路线", visible: true },
        { key: 7, label: "工序组", visible: true },
        { key: 8, label: "单位", visible: true },
        { key: 9, label: "库存数量", visible: true },
        { key: 10, label: "生产数量", visible: true },
        { key: 11, label: "层数", visible: true },
        { key: 12, label: "叠数", visible: true },
        { key: 13, label: "待排产叠数", visible: true },
        { key: 14, label: "板料长度", visible: true },
        { key: 15, label: "转换前路径", visible: true },
        { key: 16, label: "转换后路径", visible: true },
        { key: 17, label: "是否暂停", visible: true },
        { key: 18, label: "是否在钻孔计划区", visible: true },
        { key: 19, label: "是否在钻孔计划区备注", visible: true },
        { key: 20, label: "需求日期", visible: true },
        { key: 21, label: "MoveInTime", visible: true },
        { key: 22, label: "MoveOutTime", visible: true },
        { key: 23, label: "TrackInTime", visible: true },
        { key: 24, label: "TrackOutTime", visible: true },
        { key: 25, label: "SignMoveInTime", visible: true },
        { key: 26, label: "SignMoveOutTime", visible: true },
        { key: 27, label: "SignTrackInTime", visible: true },
        { key: 28, label: "SignTrackOutTime", visible: true },
        { key: 29, label: "预计完成时长", visible: true },
        { key: 30, label: "内部工单状态", visible: true },
      ],
      pickerOptions: {
        disabledDate(time) {
          return time.getTime() < Date.now() - 24 * 60 * 60 * 1000;
        },
      },
    };
  },

  watch: {
    "form.itemCode": {
      handler(val) {
        if (val == "") {
          this.form.itemCode = undefined;
          this.form.itemName = undefined;
          this.form.routeName = undefined;
          this.form.routeCode = undefined;
          this.form.dispenseMachines = 0;
          this.computeWadCount();
        }
      },
      deep: true,
    },
  },

  activated() {
    this.getList();
    this.getRouteList();
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
    // 查询工艺路线
    getRouteList() {
      getDropSelectDatas({
        vettingStatus: 1,
        pageNum: 1,
        pageSize: 100,
      }).then((res) => {
        this.routeQueryList = res.data?.map((v) => {
          return { ...v, label: `${v.code}(${v.name})` };
        });
      });
    },

    /** 查询生产工单列表 */
    getList(isSearch) {
      this.loading = true;
      listExternalWorkOrder(this.queryParams).then((res) => {
        this.externalWorkOrderList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
      });
    },
    // 计算待排产叠数
    computeWadCount() {
      if (this.form.quantity == 0 || this.form.panelCount == 0) {
        // 叠数计算
        this.form.wadCount = 0;
      } else {
        // 叠数计算
        this.form.wadCount = Math.ceil(
          this.form.quantity / this.form.panelCount
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
        code: "",
        orderSource: "",
        sourceCode: "",
        itemName: "",
        itemCode: "",
        incodeNumber: "",
        panelLength: undefined,
        batchCode: "",
        specification: "",
        unitOfMeasure: "",
        quantity: undefined,
        wadCount: undefined,
        clientName: "",
        clientCode: "",
        requestDate: null,
        panelCount: undefined,
        layerNum: undefined,
        drillCount: undefined,
        productCategoryCode: "",
        routeCode: "",
        routeName: "",
        specGroup: "",
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
      getExternalWorkOrder(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "查看外部工单";
          this.optType = "view";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 新增按钮操作 */
    handleAdd(row) {
      this.reset();
      this.routeOptions = [];
      this.open = true;
      this.title = "添加外部工单";
      this.optType = "add";
      this.autoGenFlag = false;
      this.initialForm = Object.assign({}, this.form);
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const id = row.id || this.ids;
      getExternalWorkOrder(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "修改外部工单";
          this.optType = "edit";

          listAllProcess({ itemCode: res.data.itemCode }).then((response) => {
            this.routeOptions = response.data.routeInfos?.map((v) => {
              return { ...v, label: v.code + "---" + v.name };
            });
          });
          this.initialForm = Object.assign({}, res.data);
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },

    /** 提交按钮 */
    submitForm() {
      delete this.form.panels;
      this.form.deviceCodes = [];
      updateExternalWorkOrder(this.form).then((res) => {
        if (res.code == 0) {
          this.$modal.msgSuccess("修改成功");
          this.open = false;
          this.getList();
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // hold暂停
    handleKwHoldEnable(row) {
      this.$modal
        .confirm(
          `确定处理<span style="color:red">外部工单编码为 ${row.code}</span> 的数据项？`,
          {
            dangerouslyUseHTMLString: true, // 使用HTML片段
          }
        )
        .then((result) => {
          if (result == "confirm") {
            kwHoldLot(row.code).then((res) => {
              if (res.code == 0) {
                this.$modal.msgSuccess("操作成功");
                this.getList();
              } else {
                this.$modal.notifyError(res.message);
              }
            });
          }
        })
        .catch(() => {});
    },
    // Mes转仓
    handleKwAGVStockInEnable(row) {
      this.$modal
        .confirm(
          `确定处理<span style="color:red">外部工单编码为 ${row.code}</span> 的数据项？`,
          {
            dangerouslyUseHTMLString: true, // 使用HTML片段
          }
        )
        .then((result) => {
          if (result == "confirm") {
            kwAGVStockIn(row.code).then((res) => {
              if (res.code == 0) {
                this.$modal.msgSuccess("操作成功");
                this.getList();
              } else {
                this.$modal.notifyError(res.message);
              }
            });
          }
        })
        .catch(() => {});
    },
    // 删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.id,
          delExternalWorkOrder,
          this.getList,
          "外部工单编号为" + row.sourceCode
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleDelete":
          this.handleDelete(row);
          break;
        case "handleKwAGVStockInEnable":
          this.handleKwAGVStockInEnable(row);
          break;

        case "handleKwHoldEnable":
          this.handleKwHoldEnable(row);
          break;

        default:
          break;
      }
    },
    // 选择工艺路线
    selectRoute(val) {
      this.form.routeName = this.routeOptions.find((v) => v.code == val).name;
      this.form.routeCode = val;
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
        this.$set(this.form, "itemName", obj.name);
        listAllProcess({ itemCode: obj.code }).then((response) => {
          this.routeOptions = response.data.routeInfos?.map((v) => {
            return { ...v, label: v.code + "---" + v.name };
          });
          this.form.routeName = this.routeOptions[0]?.name;
          this.form.routeCode = this.routeOptions[0]?.code;
        });
        this.computeWadCount();
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
  },
};
</script>
