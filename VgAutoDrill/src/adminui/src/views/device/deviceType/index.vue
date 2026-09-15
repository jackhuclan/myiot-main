<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="类型编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入类型编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="类型名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入类型名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="状态" prop="status">
        <el-select
          @clear="clearQueryParams('status')"
          v-model="queryParams.status"
          placeholder="类型状态"
          clearable
          style="width: 120px"
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
          type="info"
          plain
          icon="el-icon-sort"
          @click="toggleExpandAll"
          >展开/折叠</el-button
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
      v-if="refreshTable"
      v-loading="loading"
      :data="deviceTypeList"
      row-key="id"
      :default-expand-all="isExpandAll"
      :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
      border
    >
      <el-table-column
        key="code"
        prop="code"
        label="类型编码"
        min-width="150px"
        show-overflow-tooltip
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span v-if="scope.row.parentId == 0">{{ scope.row.code }}</span>
          <span
            v-else
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['device:deviceType:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        key="name"
        prop="name"
        label="类型名称"
        align="center"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[1].visible"
      ></el-table-column>
      <el-table-column
        key="status"
        prop="status"
        label="状态"
        align="center"
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
        v-if="columns[3].visible"
        width="180px"
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
            @click.native.stop="handleUpdate(scope.row)"
            :disabled="hasPermi(['device:deviceType:edit'])"
            v-if="scope.row.parentId != 0"
            >修改</el-button
          >
          <!-- <el-button
            v-if="scope.row.parentId != 0"
            type="text"
            icon="el-icon-delete"
            @click.native.stop="handleDelete(scope.row)"
            :disabled="hasPermi(['device:deviceType:remove'])"
            >删除</el-button
          > -->
        </template>
      </el-table-column>
    </el-table>
    <!-- 添加或修改类型对话框 -->
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
            <el-form-item label="类型编码" prop="code">
              <el-input
                disabled
                v-model="form.code"
                placeholder="请输入类型编码"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="类型名称" prop="name">
              <el-input
                disabled
                v-model="form.name"
                placeholder="请输入类型名称"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="上级类型" prop="parentId">
              <treeselect
                :class="$store.getters.size + '-treeselect'"
                :disabled="optType == 'view'"
                noOptionsText="暂无数据"
                noChildrenText="暂无数据"
                noResultsText="暂无数据"
                :show-count="true"
                v-model="form.parentId"
                :normalizer="normalizer"
                :options="deviceTypeOptions"
                placeholder="选择上级类型"
              />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="12">
            <el-form-item label="类型状态">
              <el-radio-group v-removeAriaHidden v-model="form.status">
                <el-radio
                  v-for="dict in dict.type.sys_normal_disable"
                  :key="dict.value"
                  :label="dict.value"
                  >{{ dict.label }}</el-radio
                >
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import { treeselect } from "@/api/device/device";
import {
  addDeviceType,
  delDeviceType,
  getDeviceType,
  listDeviceType,
  updateDeviceType,
} from "@/api/device/deviceType";

export default {
  name: "Device",
  dicts: ["sys_normal_disable"],
  data() {
    // 自定义校验规则
    const parentIdRule = (rule, value, callback) => {
      // if (this.form.parentId == 1) {
      //   callback(new Error("根分类不可选，请重新选择"));
      // } else
      if (!this.form.parentId) {
        callback(new Error("请选择所属分类"));
      } else {
        callback();
      }
    };
    return {
      page: "deviceType",
      optType: "",
      // 遮罩层
      loading: true,
      // 显示搜索条件
      showSearch: true,
      // 表格树数据
      deviceTypeList: [],
      // 类型树选项
      deviceTypeOptions: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      total: 0,
      // 是否展开，默认全部展开
      isExpandAll: true,
      // 重新渲染表格状态
      refreshTable: true,
      // 查询参数
      queryParams: {
        pageNum: 0,
        pageSize: 10000,
        name: undefined,
        code: undefined,
        status: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        parentId: [
          { required: true, validator: parentIdRule, trigger: "change" },
        ],
        name: [
          { required: true, message: "类型名称不能为空", trigger: "blur" },
        ],
        code: [
          { required: true, message: "类型编码不能为空", trigger: "blur" },
        ],
      },
      // 列信息
      columns: [
        { key: 0, label: "类型编码", visible: true },
        { key: 1, label: "类型名称", visible: true },
        { key: 2, label: "状态", visible: true },
        { key: 3, label: "创建时间", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    // this.getTreeselect();
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
    /** 查询类型列表 */
    getList(isSearch) {
      this.loading = true;
      listDeviceType(this.queryParams).then((res) => {
        this.total = res.data.total;
        this.deviceTypeList = this.handleTree(res.data.list, "id");
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
      });
    },
    // 获取类型树
    getTreeselect() {
      treeselect().then((res) => {
        this.deviceTypeOptions = res.data;
      });
    },
    /** 转换类型数据结构 */
    normalizer(node) {
      if (node.children && !node.children.length) {
        delete node.children;
      }
      return {
        id: node.id,
        label: node.label,
        children: node.children,
      };
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
    handleAdd(row) {
      this.reset();
      if (row.id != undefined) {
        this.form.parentId = row.id;
      }
      this.initialForm = Object.assign({}, this.form);
      this.optType = "add";
      this.open = true;
      this.title = "添加类型";
    },
    /** 展开/折叠操作 */
    toggleExpandAll() {
      this.refreshTable = false;
      this.isExpandAll = !this.isExpandAll;
      this.$nextTick(() => {
        this.refreshTable = true;
      });
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      this.getTreeselect();
      getDeviceType(row.id).then((res) => {
        if (res.code == 0) {
          this.optType = "edit";
          this.form = res.data;
          this.initialForm = Object.assign({}, res.data);
          this.open = true;
          this.title = "修改类型";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 查看编码操作 */
    handleView(id) {
      this.reset();
      this.getTreeselect();
      getDeviceType(id).then((res) => {
        if (res.code == 0) {
          this.optType = "view";
          this.form = res.data;
          this.open = true;
          this.title = "查看类型";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm: function () {
      if (this.form.id != undefined) {
        updateDeviceType(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addDeviceType(this.form).then((res) => {
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
          delDeviceType,
          this.getList,
          "类型编码为" + row.name
        );
      }
    },
  },
};
</script>
