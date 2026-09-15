<template>
  <div class="app-container">
    <el-row :gutter="10" v-if="parentOptType != 'view'" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['produce:routeAndProcess:add'])"
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
          :disabled="hasPermi(['produce:routeAndProcess:remove'])"
          >批量删除</el-button
        >
      </el-col>
    </el-row>

    <el-table
      border
      :ref="page"
      v-loading="loading"
      :data="routeAndProcessList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column
        type="selection"
        width="55"
        align="center"
        v-if="parentOptType != 'view'"
      />
      <el-table-column label="序号" prop="orderNum" />
      <el-table-column
        label="工序编码"
        prop="processCode"
        min-width="100px"
        show-overflow-tooltip
      />
      <el-table-column
        label="工序名称"
        prop="processName"
        min-width="100px"
        show-overflow-tooltip
      />
      <el-table-column label="自检数量" align="center" prop="selfCheckNum" />
      <el-table-column
        label="耗时(分钟)"
        width="120px"
        align="center"
        prop="requiredTime"
      />
      <el-table-column
        label="是否手动检查"
        width="150px"
        align="center"
        prop="isManualCheck"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isManualCheck == '1'">是</el-tag>
          <el-tag v-else type="danger">否</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="关键工序" align="center" prop="keyFlag">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.keyFlag == '1'">是</el-tag>
          <el-tag v-else type="danger">否</el-tag>
        </template>
      </el-table-column>

      <el-table-column
        label="甘特图显示颜色"
        align="center"
        prop="color"
        width="150px"
      >
        <template slot-scope="scope">
          <el-color-picker v-model="scope.row.color" disabled></el-color-picker>
        </template>
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        min-width="140px"
        v-if="parentOptType != 'view'"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <el-button
            type="text"
            icon="el-icon-edit"
            @click="handleUpdate(scope.row)"
            :disabled="hasPermi(['produce:routeAndProcess:edit'])"
            >修改</el-button
          >
          <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['produce:routeAndProcess:remove'])"
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
      :autoScroll="false"
    />

    <!-- 添加或修改工艺组成对话框 -->
    <edit-form-dialog
      v-model="open"
      :title="title"
      :optType="optType"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form ref="form" label-width="100px" :model="form" :rules="rules">
        <el-row>
          <el-col :span="8">
            <el-form-item label="工序" prop="processName">
              <el-select
                v-model="form.processName"
                placeholder="请选择工序"
                @change="handleChange"
                :disabled="title == '修改工艺组成'"
              >
                <el-option
                  v-for="item in processOptions"
                  :key="item.id"
                  :label="item.name"
                  :value="item.id"
                  :title="item.name"
                ></el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="序号" prop="orderNum">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.orderNum"
                @changeNum="changeNum"
                :numName="'orderNum'"
                :min="0"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="自检数量" prop="selfCheckNum">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.selfCheckNum"
                @changeNum="changeNum"
                :numName="'selfCheckNum'"
                :min="0"
              />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="8">
            <el-form-item label="本工序耗时" prop="requiredTime">
              <el-input
                :style="{ width: '200px' }"
                v-model.number="form.requiredTime"
                placeholder="请输入耗时(分钟)"
              /> </el-form-item
          ></el-col>
          <el-col :span="8">
            <el-form-item
              label="是否关键工序"
              label-width="150px"
              prop="keyFlag"
            >
              <el-tooltip effect="dark" placement="top">
                <div slot="content">
                  是：整个工单的生产进度将根据当前工序的生产报工数量进行更新
                  <br />
                  每个工艺流程只能有一个关键工序
                </div>
                <el-radio-group v-removeAriaHidden v-model="form.keyFlag">
                  <el-radio :label="'1'"> 是 </el-radio>
                  <el-radio :label="'0'"> 否 </el-radio>
                </el-radio-group>
              </el-tooltip>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="是否首检" prop="isManualCheck">
              <el-radio-group v-removeAriaHidden v-model="form.isManualCheck">
                <el-radio :label="'1'"> 是 </el-radio>
                <el-radio :label="'0'"> 否 </el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="甘特图颜色" label-width="100px " prop="color">
              <el-input
                placeholder="请输入颜色编码"
                v-model="form.color"
                maxlength="7"
                class="color-input"
              >
                <template slot="prepend"
                  ><el-color-picker v-model="form.color"></el-color-picker>
                </template>
              </el-input>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>

      <el-tabs type="border-card" v-if="form.id != null">
        <!-- 关联设备 -->
        <el-tab-pane label="关联工作站">
          <RouteAndProcessAndWorkStation
            v-if="form.id != null"
            :optType="optType"
            :processCode="form.processCode"
            :routeAndProcessId="form.id"
          ></RouteAndProcessAndWorkStation>
        </el-tab-pane>
      </el-tabs>
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listRouteAndProcess,
  getRouteAndProcess,
  delRouteAndProcess,
  addRouteAndProcess,
  updateRouteAndProcess,
  listAllProcess,
  delList,
} from "@/api/produce/routeAndProcess";
// 工艺关联工作站
import RouteAndProcessAndWorkStation from "./routeAndProcessAndWorkStation";

export default {
  name: "RouteAndProcess",
  // dicts: ['mes_link_type','sys_yes_no'],
  components: { RouteAndProcessAndWorkStation },
  data() {
    return {
      page: "routeAndProcess",
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
      // 工艺组成表格数据
      routeAndProcessList: [],
      //工序选项
      processOptions: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        routeId: undefined,
        processId: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        processName: [
          { required: true, message: "工艺路线不能为空", trigger: "blur" },
        ],
        isManualCheck: [
          {
            required: true,
            message: "请指定当前工序是否需要手动检查",
            trigger: "blur",
          },
        ],
        color: [
          {
            required: true,
            message: "请输入甘特图颜色",
            trigger: "blur",
          },
        ],
        keyFlag: [
          {
            required: true,
            message: "请指定当前工序是否关键工序",
            trigger: "blur",
          },
        ],
        orderNum: [
          { required: true, message: "序号不能为空", trigger: "blur" },
        ],
        selfCheckNum: [
          {
            required: true,
            message: "请输入自检数量",
            trigger: "blur",
          },
        ],
        requiredTime: [
          {
            required: true,
            message: "请输入所需耗时(分钟)",
            trigger: "blur",
          },
          { type: "number", message: "请输入数字" },
        ],
      },
      // 原来的序号
      oldNum: null,
    };
  },
  props: ["routeId", "parentOptType"],
  created() {
    this.getList();
    this.getProcess();
  },
  watch: {
    "form.requiredTime": {
      handler(val) {
        if (!val) {
          this.form.requiredTime = undefined;
        }
      },
      deep: true,
    },
    open(val) {
      if (!val) {
        this.$emit("getList");
      }
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
    /** 查询工艺组成列表 */
    async getList() {
      this.loading = true;
      this.queryParams.routeId = this.routeId;
      const res = await listRouteAndProcess(this.queryParams);
      this.routeAndProcessList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
    },
    // //查询工序信息
    async getProcess() {
      // 工序状态为正常的
      const res = await listAllProcess({ status: 1 });

      this.processOptions = res.data.list;
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
        routeId: this.routeId,
        processId: undefined,
        orderNum: 1,
        color: "",
        keyFlag: "",
        requiredTime: undefined,
        isManualCheck: "",
        selfCheckNum: 0,
        processName: undefined,
      };
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList();
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.resetForm("queryForm");
      this.handleQuery();
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.open = true;
      this.optType = "add";
      this.title = "添加工艺组成";
      this.initialForm = Object.assign({}, this.form);
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const recordId = row.id || this.ids;
      getRouteAndProcess(recordId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.form.selfCheckNum = res.data.selfCheckNum
            ? res.data.selfCheckNum
            : 0;
          this.oldNum = res.data.orderNum;
          this.open = true;
          this.optType = "edit";
          this.title = "修改工艺组成";
          this.initialForm = Object.assign({}, res.data);
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm() {
      if (this.form.id != null) {
        updateRouteAndProcess(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addRouteAndProcess(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("新增成功");
            this.open = false;
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
        this.deleteItem(
          row.id,
          delRouteAndProcess,
          this.getList,
          "工序编码为" + row.processCode
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 工序下拉框
    handleChange(obj) {
      this.form.processId = obj;
    },
  },
};
</script>
