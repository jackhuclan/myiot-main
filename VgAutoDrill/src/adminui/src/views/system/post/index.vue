<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="岗位名称" prop="positionName">
        <el-input
          v-trim
          v-model="queryParams.positionName"
          placeholder="请输入岗位名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="状态" prop="status">
        <el-select
          @clear="clearQueryParams('status')"
          v-model="queryParams.status"
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option
            v-for="dict in dict.type.sys_normal_disable"
            :key="dict.value"
            :label="dict.label"
            :value="dict.value"
          />
        </el-select>
      </el-form-item>
    </search-form>

    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['system:post:add'])"
          >新增</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          type="danger"
          plain
          icon="el-icon-delete"
          @click="handleDelete"
          :disabled="hasPermi(['system:post:remove'])"
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
      :data="postList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="岗位名称"
        align="center"
        key="positionName"
        prop="positionName"
        min-width="150px"
        show-overflow-tooltip
        v-if="columns[0].visible"
      />
      <el-table-column
        label="岗位排序"
        align="center"
        key="sort"
        prop="sort"
        v-if="columns[1].visible"
      />
      <el-table-column
        label="状态"
        align="center"
        key="status"
        prop="status"
        v-if="columns[2].visible"
      >
        <template slot-scope="scope">
          <dict-tag
            :options="dict.type.sys_normal_disable"
            :value="scope.row.status"
          />
        </template>
      </el-table-column>
      <el-table-column
        label="创建时间"
        align="center"
        key="createTime"
        prop="createTime"
        width="180"
        v-if="columns[3].visible"
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
            :disabled="hasPermi(['system:post:edit'])"
            >修改</el-button
          >
          <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['system:post:remove'])"
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

    <!-- 添加或修改岗位对话框 -->
    <edit-form-dialog
      v-model="open"
      :optType="optType"
      :title="title"
      width="650px"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form ref="form" :model="form" :rules="rules" label-width="100px">
        <el-row>
          <el-col :span="12">
            <el-form-item label="岗位名称" prop="positionName">
              <el-input
                v-model="form.positionName"
                placeholder="请输入岗位名称"
              /> </el-form-item
          ></el-col>
          <el-col :span="12">
            <el-form-item label="岗位顺序" prop="sort">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.sort"
                @changeNum="changeNum"
                :numName="'sort'"
                :min="1"
              />
            </el-form-item>
          </el-col>
        </el-row>

        <el-form-item label="岗位状态" prop="status">
          <el-radio-group v-removeAriaHidden v-model="form.status">
            <el-radio
              v-for="dict in dict.type.sys_normal_disable"
              :key="dict.value"
              :label="dict.value"
              >{{ dict.label }}</el-radio
            >
          </el-radio-group>
        </el-form-item>
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listPost,
  getPost,
  delPost,
  addPost,
  updatePost,
  delList,
} from "@/api/system/post";

export default {
  name: "Post",
  dicts: ["sys_normal_disable"],
  data() {
    return {
      page: "post",
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
      // 岗位表格数据
      postList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        positionName: undefined,
        status: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        positionName: [
          { required: true, message: "岗位名称不能为空", trigger: "blur" },
        ],
        sort: [
          { required: true, message: "岗位顺序不能为空", trigger: "change" },
        ],
      },
      // 列信息
      columns: [
        { key: 0, label: `岗位名称`, visible: true },
        { key: 1, label: `岗位排序`, visible: true },
        { key: 3, label: `状态`, visible: true },
        { key: 4, label: `创建时间`, visible: true },
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
    /** 查询岗位列表 */
    getList(isSearch) {
      this.loading = true;
      listPost(this.queryParams).then((res) => {
        this.postList = res.data.list;
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
        positionName: "",
        sort: 1,
        status: "1",
        remark: "",
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
      this.open = true;
      this.optType = "add";
      this.title = "添加岗位";
      this.initialForm = Object.assign({}, this.form);
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const postId = row.id || this.ids;
      getPost(postId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.initialForm = Object.assign({}, res.data);
          this.open = true;
          this.optType = "edit";
          this.title = "修改岗位";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm: function () {
      if (this.form.id != undefined) {
        updatePost(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addPost(this.form).then((res) => {
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
        this.deleteItem(
          row.id,
          delPost,
          this.getList,
          "岗位名称为" + row.positionName
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    /** 导出按钮操作 */
    handleExport() {
      this.download(
        "system/post/export",
        {
          ...this.queryParams,
        },
        `post_${new Date().getTime()}.xlsx`
      );
    },
  },
};
</script>
