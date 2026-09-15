<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="设备" prop="deviceCode">
        <el-input
          v-trim
          v-model="queryParams.deviceCode"
          placeholder="请输入设备"
          clearable
          @keyup.enter.native="handleQuery"
        />
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
          :disabled="hasPermi(['utilizationRate:maintenance:add'])"
          >新增</el-button
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
      :data="maintenanceList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="记录Id"
        key="id"
        prop="id"
        width="80"
        fixed="left"
        align="center"
        v-if="columns[0].visible"
      >
      </el-table-column>
      <el-table-column
        label="设备编码"
        key="deviceCode"
        min-width="180"
        prop="deviceCode"
        show-overflow-tooltip
        v-if="columns[1].visible"
      >
      </el-table-column>
      <el-table-column
        label="保养项目"
        key="maintenanceTypeName"
        prop="maintenanceTypeName"
        min-width="180"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />
      <el-table-column
        label="开始时间"
        prop="startTime"
        min-width="180"
        align="center"
        v-if="columns[3].visible"
        key="startTime"
        ><template slot-scope="scope">
          <span>{{
            parseTime(scope.row.startTime, "{y}-{m}-{d} {h}:{i}:{s}")
          }}</span></template
        >
      </el-table-column>
      <el-table-column
        label="结束时间"
        prop="endTime"
        min-width="180"
        align="center"
        v-if="columns[4].visible"
        key="endTime"
        ><template slot-scope="scope">
          <span>{{
            parseTime(scope.row.endTime, "{y}-{m}-{d} {h}:{i}:{s}")
          }}</span></template
        >
      </el-table-column>
      <el-table-column
        label="保养人员"
        key="maintenancePersonName"
        prop="maintenancePersonName"
        align="center"
        show-overflow-tooltip
        v-if="columns[5].visible"
      />

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
            :disabled="hasPermi(['utilizationRate:maintenance:edit'])"
            >修改</el-button
          >
          <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['utilizationRate:maintenance:delete'])"
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

    <!-- 添加或修改保养记录对话框 -->
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
            <el-form-item label="设备编码" prop="deviceCode">
              <el-autocomplete
                v-model="form.deviceCode"
                :fetch-suggestions="querySearchAsync"
                placeholder="请输入内容"
                @select="handleSelect"
                popper-class="query-suggestion"
                :popper-append-to-body="false"
                :debounce="0"
              >
                <el-button
                  v-debounce
                  slot="append"
                  @click="handleSelectDevice"
                  icon="el-icon-search"
                ></el-button
              ></el-autocomplete>

              <deviceSelect ref="deviceSelect" @onSelected="onDeviceSelected">
              </deviceSelect>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="保养项目" prop="maintenanceTypeName">
              <el-select
                ref="select1"
                v-model="form.maintenanceTypeName"
                placeholder="请选择"
                clearable
                @select="handleMaintenanceType"
                @clear="handleClear"
              >
                <el-option
                  v-for="item in maintenanceTypeOptions"
                  :key="item.id"
                  :label="item.label"
                  :value="item.label"
                  v-optionTitle
                >
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="保养人员" prop="maintenancePersonName">
              <el-input
                v-model="form.maintenancePersonName"
                placeholder="请选择保养人员"
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
        </el-row>
        <el-row>
          <el-col>
            <el-form-item label="保养说明" prop="remark">
              <el-input
                type="textarea"
                v-model="form.remark"
                placeholder="请输入保养说明"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              /> </el-form-item
          ></el-col>
        </el-row>
        <el-row>
          <el-col :span="7">
            <el-form-item label="计算类型" prop="countType">
              <el-radio-group
                v-removeAriaHidden
                v-model="form.countType"
                @input="handleInput"
              >
                <el-radio :label="0">按次</el-radio>
                <el-radio :label="1">按耗时</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="开始时间" prop="startTime">
              <el-date-picker
                clearable
                v-model="form.startTime"
                type="datetime"
                placeholder="开始时间"
              >
              </el-date-picker>
            </el-form-item>
          </el-col>
          <el-col :span="9" v-if="form.countType != 0 || optType == 'view'">
            <el-form-item
              label="结束时间"
              prop="endTime"
              :rules="
                form.countType == 0
                  ? rules.endTime
                  : [
                      {
                        required: true,
                        message: '结束时间不能为空',
                        trigger: 'blur',
                      },
                    ]
              "
            >
              <div style="display: flex">
                <el-tooltip content="快捷设置">
                  <el-button
                    @click="handleQuickSettings"
                    icon="el-icon-s-tools"
                    size="mini"
                  ></el-button
                ></el-tooltip>

                <el-date-picker
                  clearable
                  :disabled="form.countType == 0"
                  v-model="form.endTime"
                  type="datetime"
                  placeholder="结束时间"
                >
                </el-date-picker>
              </div>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
  </div>
</template>
  
  <script>
import { listDevice } from "@/api/device/device";
import {
  listMaintenanceRecords,
  addMaintenanceRecords,
  updateMaintenanceRecords,
  delMaintenanceRecords,
  getMaintenanceRecords,
} from "@/api/utilizationRate/maintenance";
import UserSingleSelect from "@/components/userSelect";
import deviceSelect from "@/components/deviceSelect";
export default {
  name: "Maintenance",
  components: { deviceSelect, UserSingleSelect },
  data() {
    const checkDeviceCode = async (rule, value, callback) => {
      const res = await listDevice({
        pageNum: 1,
        pageSize: 1000,
        name: undefined,
        code: value,
        deviceKindList: [2, 3],
      });
      if (!value) {
        return callback(new Error("设备编码不能为空"));
      } else if (res.data.list.length == 0) {
        return callback(new Error("设备编码不存在,请重新输入或选择"));
      }
      callback();
    };
    return {
      page: "maintenance",
      optType: "",
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 保养记录表格数据
      maintenanceList: [],
      // 弹出层标题
      title: "",
      dataTitle: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        deviceCode: undefined,
      },
      //保养项目列表
      maintenanceTypeOptions: [
        {
          label: "更换压力脚",
          value: 1,
        },
      ],
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        endTime: [
          { required: false, message: "结束时间不能为空", trigger: "blur" },
        ],
        countType: [
          { required: true, message: "计算类型不能为空", trigger: "blur" },
        ],
        deviceCode: [
          { required: true, validator: checkDeviceCode, trigger: "change" },
        ],
        startTime: [
          { required: true, message: "开始时间不能为空", trigger: "blur" },
        ],
        maintenanceTypeName: [
          { required: true, message: "保养项目不能为空", trigger: "blur" },
        ],
      },
      // 列信息，
      columns: [
        { key: 0, label: "记录Id", visible: true },
        { key: 1, label: "设备编码", visible: true },
        { key: 2, label: "保养项目", visible: true },
        { key: 3, label: "开始时间", visible: true },
        { key: 4, label: "结束时间", visible: true },
        { key: 5, label: "保养人员", visible: true },
      ],
    };
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
    /** 查询保养记录列表 */
    getList(isSearch) {
      this.loading = true;
      listMaintenanceRecords(this.queryParams).then((res) => {
        this.maintenanceList = res.data.list;
        this.total = res.data.total;
      });

      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch(
          "搜索成功，共" + this.maintenanceList.length + "条数据！"
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
        deviceCode: "",
        startTime: new Date().toLocaleString(),
        endTime: null,
        maintenanceTypeName: undefined,
        remark: "",
      };
      this.autoGenFlag = false;
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      if (this.queryParams.deviceCode) {
        this.maintenanceList = this.maintenanceList.filter((v) => {
          if (v.deviceCode.includes(this.queryParams.deviceCode)) {
            return v;
          }
        });
      } else {
        this.getList("search");
      }
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.handleQuery();
    },

    handleMaintenanceType(val) {
      this.form.maintenanceTypeId = val.value;
    },
    handleClear() {
      this.form.maintenanceTypeName = undefined;
    },
    handleInput(val) {
      if (val == 0) {
        this.form.endTime = this.form.startTime;
      } else {
        this.form.endTime = null;
      }
    },
    // 快捷设置结束时间
    handleQuickSettings() {
      this.form.endTime = new Date().toLocaleString();
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.open = true;
      this.title = "添加保养记录信息";
      this.initialForm = Object.assign({}, this.form);
      this.optType = "add";
    },

    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const id = row.id || this.ids;
      getMaintenanceRecords(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.initialForm = Object.assign({}, { ...row });
          this.open = true;
          this.title = "修改保养记录信息";
          this.optType = "edit";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm() {
      if (this.form.id != null) {
        updateMaintenanceRecords(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addMaintenanceRecords(this.form).then((res) => {
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
      this.open = false;
    },
    //删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.id,
          delMaintenanceRecords,
          this.getList,
          "记录Id为" + row.id
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 操作输入物料编码
    querySearchAsync(queryString, cb) {
      if (queryString === "" || !queryString) {
        let arr = [];
        cb(arr);
      } else {
        clearTimeout(this.timeout);
        this.timeout = setTimeout(() => {
          listDevice({
            pageNum: 1,
            pageSize: 1000,
            name: undefined,
            code: queryString,
            // 仅查询钻机
            deviceKindList: [2, 3],
          }).then((res) => {
            if (res.code === 0) {
              let arr = res.data.list.map((v) => {
                return {
                  value: v.code,
                  ...v,
                };
              });
              const showSuggestion =
                document.querySelector(".query-suggestion");
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
    handleSelect(obj) {
      this.$set(this.form, "deviceCode", obj.code);
    },
    //设备选择弹出框
    handleSelectDevice() {
      this.$refs.deviceSelect.showFlag = true;
      this.$refs.deviceSelect.showLeft = false;
      this.$refs.deviceSelect.title = "设备选择";
      // 仅查询钻机
      this.$refs.deviceSelect.queryParams.deviceKindList = [2, 3];
      this.$refs.deviceSelect.getTreeselect();
      this.$refs.deviceSelect.getList();
    },
    onDeviceSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "deviceCode", obj.code);
      }
    },
    //点击保养人选择按钮
    handleUserSelect() {
      this.$refs.userSelect.showFlag = true;
      this.$refs.userSelect.selectedId = this.form.maintenancePersonName
        ? this.form.maintenancePersonName
        : undefined;
      this.$refs.userSelect.getList();
      this.$refs.userSelect.getTreeselect();
    },
    //保养人返回
    onUserSelected(row) {
      if (row != null && row != undefined) {
        this.$set(this.form, "maintenancePersonName", row.realName);
        this.$set(this.form, "maintainId", row.id);
      }
    },
  },
};
</script>
  