<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="设备编码" prop="deviceCode">
        <el-input
          v-trim
          v-model="queryParams.deviceCode"
          placeholder="请输入设备编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="设备名称" prop="deviceName">
        <el-input
          v-trim
          v-model="queryParams.deviceName"
          placeholder="请输入设备名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="维护状态" prop="maintainStatus">
        <el-select
          @clear="clearQueryParams('maintainStatus')"
          v-model="queryParams.maintainStatus"
          placeholder="请选择"
          clearable
          style="width: 120px"
        >
          <el-option
            v-for="item in [
              { label: '默认', value: '-1' },
              { label: '已处理', value: '1' },
              { label: '未处理', value: '0' },
            ]"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
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
          :disabled="hasPermi(['device:repair:add'])"
          >新增</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-delete"
          @click="handleDelete"
          :disabled="hasPermi(['device:repair:remove'])"
          >批量删除</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="info"
          plain
          icon="el-icon-download"
          @click="handleExport"
          :disabled="hasPermi(['device:repair:export'])"
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
      :ref="page"
      v-loading="loading"
      :data="repairList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
      border
    >
      <el-table-column type="selection" width="55" align="center" />

      <el-table-column
        label="设备编码"
        key="deviceCode"
        prop="deviceCode"
        show-overflow-tooltip
        v-if="columns[0].visible"
        min-width="150px"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['device:repair:view']"
            >{{ scope.row.deviceCode }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="设备名称"
        key="deviceName"
        prop="deviceName"
        show-overflow-tooltip
        v-if="columns[1].visible"
        min-width="150px"
      />

      <el-table-column
        label="维护人员"
        key="maintainPerson"
        prop="maintainPerson"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[2].visible"
      />

      <el-table-column
        label="维护状态"
        align="center"
        key="maintainStatus"
        prop="maintainStatus"
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">
          <el-tag type="danger" v-if="scope.row.maintainStatus == 0">
            未处理
          </el-tag>
          <el-tag v-else-if="scope.row.maintainStatus == 1"> 已处理 </el-tag>
          <el-tag type="info" v-else> 默认 </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="维护时间"
        align="center"
        key="maintainTime"
        prop="maintainTime"
        min-width="180"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.maintainTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="备注"
        min-width="150"
        key="remark"
        prop="remark"
        v-if="columns[5].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.remark" />
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
            :disabled="hasPermi(['device:repair:edit'])"
            >修改</el-button
          >
          <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['device:repair:remove'])"
            >删除</el-button
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
    <!-- 添加或修改设备维护对话框 -->
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
          <el-col :span="12">
            <el-form-item label="设备编码" prop="deviceCode">
              <el-input
                v-model="form.deviceCode"
                placeholder="请选择设备"
                disabled
              >
                <el-button
                  v-if="!form.id"
                  :disabled="optType != 'add'"
                  v-debounce
                  @click="handleDeviceSelectAdd"
                  slot="append"
                  icon="el-icon-search"
                ></el-button>
              </el-input>
            </el-form-item>

            <DeviceSelect
              ref="deviceSelcet"
              @onSelected="onDeviceSelectAdd"
            ></DeviceSelect>
          </el-col>
          <el-col :span="12">
            <el-form-item label="设备名称" prop="deviceName">
              <el-input
                v-model="form.deviceName"
                placeholder="请选择设备"
                disabled
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="维护日期" prop="maintainTime">
              <el-date-picker
                clearable
                v-model="form.maintainTime"
                type="datetime"
                placeholder="选择日期时间"
                style="width: 100%"
              >
              </el-date-picker>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="维护人员" prop="maintainPerson">
              <el-input
                v-model="form.maintainPerson"
                placeholder="请选择维护人员"
              >
                <el-button
                  v-debounce
                  slot="append"
                  @click="handleUserSelect"
                  icon="el-icon-search"
                ></el-button>
              </el-input>
            </el-form-item>
            <UserSingleSelect
              ref="userSelect"
              @onSelected="onUserSelected"
            ></UserSingleSelect>
          </el-col>
          <el-col :span="10">
            <el-form-item label="维护状态" v-if="form.id != null">
              <el-radio-group v-removeAriaHidden v-model="form.maintainStatus">
                <el-radio :label="'-1'">默认</el-radio>
                <el-radio :label="'1'">已处理</el-radio>
                <el-radio :label="'0'">未处理</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="备注" prop="remark">
              <el-input
                v-model="form.remark"
                type="textarea"
                placeholder="请输入内容"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <el-divider v-if="form.id != null" content-position="center"
        >维护明细</el-divider
      >
      <el-card shadow="always" v-if="form.id != null && open" class="box-card">
        <RepairDetail
          :parentOptType="optType"
          ref="line"
          :masterId="form.id"
          :deviceId="form.deviceId"
        ></RepairDetail>
      </el-card>
    </edit-form-dialog>
  </div>
</template>
<script>
import {
  listDeviceMaintain,
  getDeviceMaintain,
  delDeviceMaintain,
  delList,
  addDeviceMaintain,
  updateDeviceMaintain,
} from "@/api/device/deviceMaintain";
import RepairDetail from "./repairDetail";
import DeviceSelect from "@/components/deviceSelect";
import UserSingleSelect from "@/components/userSelect";
export default {
  name: "Repair",
  components: { RepairDetail, DeviceSelect, UserSingleSelect },
  data() {
    return {
      page: "repair",
      optType: "",
      autoGenFlag: false,
      // 遮罩层
      loading: true,
      // 选中数组s
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 维护记录表格数据
      repairList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        deviceId: undefined,
        deviceName: undefined,
        deviceCode: undefined,
        maintainId: undefined,
        maintainPerson: undefined,
        maintainStatus: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        deviceCode: [
          { required: true, message: "设备编码不能为空", trigger: "change" },
        ],
        deviceName: [
          { required: true, message: "设备名称不能为空", trigger: "change" },
        ],
        maintainTime: [
          { required: true, message: "维护日期不能为空", trigger: "change" },
        ],
        maintainPerson: [
          { required: true, message: "维护人员不能为空", trigger: "change" },
        ],
      },
      // 列信息
      columns: [
        { key: 0, label: "设备编码", visible: true },
        { key: 1, label: "设备名称", visible: true },
        { key: 2, label: "维护人员", visible: true },
        { key: 3, label: "维护状态", visible: true },
        { key: 4, label: "维护时间", visible: true },
        { key: 5, label: "备注", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  watch: {
    "form.maintainTime": {
      handler(val) {
        if (val) {
          this.form.maintainTime = new Date(val).toLocaleString("sv-SE");
        }
      },
      deep: true,
    },
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
  },
  methods: {
    /** 查询维护记录列表 */
    getList(isSearch) {
      this.loading = true;
      listDeviceMaintain(this.queryParams).then((res) => {
        this.repairList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
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
        deviceId: undefined,
        maintainTime: null,
        maintainPerson: "",
        remark: "",
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
      this.handleQuery();
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.optType = "add";
      this.open = true;
      this.title = "添加维护记录";
      this.initialForm = Object.assign({}, this.form);
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const subjectId = row.id || this.ids;
      getDeviceMaintain(subjectId).then((res) => {
        if (res.code == 0) {
          this.optType = "edit";
          this.form = res.data;
          this.initialForm = Object.assign({}, res.data);
          this.title = "修改维护记录";
          this.open = true;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 点击编码操作 */
    handleView(id) {
      this.reset();
      getDeviceMaintain(id).then((res) => {
        if (res.code == 0) {
          this.optType = "view";
          this.form = res.data;
          this.title = "查看维护记录";
          this.open = true;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm() {
      if (this.form.id != null) {
        updateDeviceMaintain(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addDeviceMaintain(this.form).then((res) => {
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
    //删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.id,
          delDeviceMaintain,
          this.getList,
          "设备编码为" + row.deviceCode
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    /** 导出按钮操作 */
    handleExport() {
      this.exportExcel("/v1/DeviceMaintain/DownLoadList", "设备维护记录.xlsx");
    },
    //点击维护人选择按钮
    handleUserSelect() {
      this.$refs.userSelect.showFlag = true;
      this.$refs.userSelect.selectedId = this.form.maintainPerson
        ? this.form.maintainPerson
        : undefined;
      this.$refs.userSelect.getList();
      this.$refs.userSelect.getTreeselect();
    },
    //维护人返回
    onUserSelected(row) {
      if (row != null && row != undefined) {
        this.$set(this.form, "maintainPerson", row.realName);
        this.$set(this.form, "maintainId", row.id);
      }
    },
    //设备资源选择弹出
    handleDeviceSelectAdd() {
      this.$refs.deviceSelcet.showFlag = true;
      this.$refs.deviceSelcet.selectedDeviceCode = this.form.deviceCode
        ? this.form.deviceCode
        : undefined;
      this.$refs.deviceSelcet.getList();
      this.$refs.deviceSelcet.getTreeselect();
    },
    //设备资源选择回调
    onDeviceSelectAdd(row) {
      if (row != null && row != undefined) {
        this.$set(this.form, "deviceId", row.id);
        this.$set(this.form, "deviceCode", row.code);
        this.$set(this.form, "deviceName", row.name);
      }
    },
  },
};
</script>
