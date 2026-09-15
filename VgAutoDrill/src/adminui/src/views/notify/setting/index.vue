<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="通知类型编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入通知类型编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="通知类型名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入通知类型名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
    </search-form>

    <el-row :gutter="10" class="mb8">
      <!-- <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['notify:setting:add'])"
          >新增</el-button
        >
      </el-col> -->
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
      :data="NotifySettingList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="通知类型编码"
        min-width="200px"
        fixed="left"
        key="code"
        prop="code"
        show-overflow-tooltip
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['notify:setting:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="通知类型名称"
        min-width="150px"
        key="name"
        prop="name"
        show-overflow-tooltip
        v-if="columns[1].visible"
      />

      <el-table-column
        label="发送频率(分钟)"
        align="center"
        key="sendFrequency"
        prop="sendFrequency"
        v-if="columns[2].visible"
        min-width="150px"
      />
      <el-table-column
        label="重复发送"
        align="center"
        key="isRepeatSend"
        prop="isRepeatSend"
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isRepeatSend == 1">是</el-tag>
          <el-tag type="danger" v-else>否</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="创建时间"
        align="center"
        key="createTime"
        prop="createTime"
        width="180"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.createTime) }}</span>
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
            :disabled="hasPermi(['notify:setting:edit'])"
            >修改</el-button
          >
          <!-- <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['notify:setting:remove'])"
            >删除</el-button
          > -->
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

    <!-- 添加或修改通知类型对话框 -->
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
        :rules="rules"
        label-width="100px"
        :disabled="optType == 'view'"
      >
        <el-row>
          <el-col :span="12">
            <el-form-item label="类型名称" prop="name">
              <el-input
                v-model="form.name"
                placeholder="请输入类型名称"
                :disabled="title == '修改类型'"
              /> </el-form-item
          ></el-col>
          <el-col :span="12">
            <el-form-item label="类型编码" prop="code">
              <el-input
                v-model="form.code"
                placeholder="请输入类型编码"
                :disabled="title == '修改类型'"
              /> </el-form-item
          ></el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="发送频率" prop="code">
              <el-select
                v-model="frequency"
                placeholder="请选择"
                @change="handleFrequency"
              >
                <el-option
                  v-for="item in options"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                >
                </el-option>
              </el-select> </el-form-item
          ></el-col>
          <el-col :span="12">
            <el-form-item label="是否重复发送" prop="isRepeatSend">
              <el-radio-group v-removeAriaHidden v-model="form.isRepeatSend">
                <el-radio :label="1">是</el-radio>
                <el-radio :label="0">否</el-radio>
              </el-radio-group>
            </el-form-item></el-col
          >
        </el-row>
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listNotifySetting,
  getNotifySetting,
  delNotifySetting,
  addNotifySetting,
  updateNotifySetting,
} from "@/api/notify/notifySetting";

export default {
  name: "NotifySetting", 
  data() {
    return {
      page: "notifySetting",
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
      // 通知类型表格数据
      NotifySettingList: [],
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
        notifyDesc: undefined,
        notifyWays: undefined,
        notifyParams: undefined,
        status: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        name: [
          { required: true, message: "通知类型名称不能为空", trigger: "blur" },
        ],
        code: [
          { required: true, message: "通知类型编码不能为空", trigger: "blur" },
        ],
      },
      options: [
        {
          value: 5,
          label: "5分钟",
        },
        {
          value: 10,
          label: "10分钟",
        },
      ],
      frequency: "",
      // 列信息
      columns: [
        { key: 0, label: "通知类型编码", visible: true },
        { key: 1, label: "通知类型名称", visible: true },
        { key: 2, label: "发送频率(分钟)", visible: true },
        { key: 3, label: "是否重复发送", visible: true },
        { key: 4, label: "创建时间", visible: true },
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
    /** 查询通知类型列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listNotifySetting(this.queryParams);
      this.NotifySettingList = res.data.list;
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
        id: undefined,
        name: undefined,
        code: undefined,
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
      this.handleQuery();
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.optType = "add";
      this.initialForm = Object.assign({}, this.form);
      this.open = true;
      this.frequency = undefined;
      this.title = "添加通知类型";
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const NotifySettingId = row.id || this.ids;
      getNotifySetting(NotifySettingId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.initialForm = Object.assign({}, res.data);
          this.frequency = this.options.find(
            (v) => v.value == res.data.sendFrequency
          )?.label;
          this.optType = "edit";
          this.open = true;
          this.title = "修改通知类型";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 点击编码查看操作 */
    handleView(id) {
      this.reset();
      getNotifySetting(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.frequency = this.options.find(
            (v) => v.value == res.data.sendFrequency
          )?.label;
          this.optType = "view";
          this.open = true;
          this.title = "查看通知类型";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm: function () {
      if (this.form.id != undefined) {
        updateNotifySetting(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addNotifySetting(this.form).then((res) => {
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
    // 删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(row.id, delNotifySetting, this.getList);
      } else {
        // this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 选择发送频率
    handleFrequency(val) {
      this.form.sendFrequency = val;
    },
  },
};
</script>
