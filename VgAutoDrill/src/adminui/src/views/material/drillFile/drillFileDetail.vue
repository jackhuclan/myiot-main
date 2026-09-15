<template>
  <el-card>
    <el-row :gutter="10" class="mb8" v-if="parentOptType != 'view'">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
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
          >批量删除</el-button
        >
      </el-col>
    </el-row>
    <el-table
      border
      :ref="page"
      v-loading="loading"
      :data="itemDrillFileDetailList"
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
      <el-table-column
        label="明细编码"
        align="center"
        prop="code"
        show-overflow-tooltip
        fixed="left"
      />
      <el-table-column
        label="直径"
        align="center"
        prop="diameter"
        show-overflow-tooltip
      />
      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        min-width="140px"
        class-name="small-padding fixed-width"
        v-if="parentOptType != 'view'"
      >
        <template slot-scope="scope">
          <el-button
            type="text"
            icon="el-icon-edit"
            @click="handleUpdate(scope.row)"
            >修改</el-button
          >
          <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
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

    <!-- 添加或修改钻带参数明细对话框 -->
    <edit-form-dialog
      v-model="open"
      :title="title"
      :optType="optType"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form ref="form" :model="form" :rules="rules" label-width="100px">
        <el-row>
          <el-col :span="12">
            <el-form-item label="明细编码" prop="code">
              <el-input v-model="form.code" placeholder="请输入明细编码" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="直径" prop="diameter">
              <el-input-number
                v-model="form.diameter"
                :precision="2"
                :step="0.1"
                :max="10"
                placeholder="请输入直径"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
  </el-card>
</template>

<script>
import {
  listItemDrillFileDetail,
  getItemDrillFileDetail,
  addItemDrillFileDetail,
  updateItemDrillFileDetail,
  delItemDrillFileDetail,
  delList,
} from "@/api/material/itemDrillFileDetail";
export default {
  name: "DrillFileDetail", 
  props: ["itemDrillFileId", "parentOptType"],
  data() {
    return {
      page: "drillFileDetail",
      // 遮罩层
      loading: true,
      optType: "",
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      //钻带参数明细表格数据
      itemDrillFileDetailList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查看弹框
      seeDetailsOpen: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        itemDrillFileId: undefined,
        code: undefined,
        diameter: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        code: [{ required: true, message: "编码不能为空", trigger: "blur" }],
        diameter: [
          { required: true, message: "直径不能为空", trigger: "blur" },
        ],
      },
    };
  },
  created() {
    this.getList();
  },
  watch: {
    itemDrillFileId(newVal) {
      this.queryParams.itemDrillFileId = newVal;
      this.getList();
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
    /** 查询ATP列表 */
    async getList() {
      this.loading = true;
      const res = await listItemDrillFileDetail({
        ...this.queryParams,
        itemDrillFileId: this.itemDrillFileId,
      });
      this.itemDrillFileDetailList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
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
        diameter: 0,
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
      this.initialForm = Object.assign({}, this.form);
      this.title = "添加钻带参数明细";
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const ItemDrillFileId = row.id || this.ids;
      getItemDrillFileDetail(ItemDrillFileId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.initialForm = Object.assign({}, res.data);
          this.open = true;
          this.optType = "edit";
          this.title = "修改钻带参数明细";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm: function () {
      this.form.itemDrillFileId = this.itemDrillFileId;
      if (this.form.id != undefined) {
        updateItemDrillFileDetail(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addItemDrillFileDetail(this.form).then((res) => {
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
          delItemDrillFileDetail,
          this.getList,
          "明细编码为" + row.code
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
  },
};
</script>
