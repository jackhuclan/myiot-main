<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="规则编码" prop="rulesCode">
        <el-input
          v-trim
          v-model="queryParams.rulesCode"
          placeholder="请输入规则编码"
          clearable
          style="width: 240px"
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="规则名称" prop="rulesName">
        <el-input
          v-trim
          v-model="queryParams.rulesName"
          placeholder="请输入规则名称"
          clearable
          style="width: 240px"
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
          :disabled="hasPermi(['masterData:autocodeRule:add'])"
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
          :disabled="hasPermi(['masterData:autocodeRule:remove'])"
          >批量删除</el-button
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
      :data="ruleList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <!-- <el-table-column type="selection" width="55" align="center" /> -->
      <el-table-column
        label="规则编码"
        min-width="250px"
        key="rulesCode"
        prop="rulesCode"
        show-overflow-tooltip
        fixed="left"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['masterData:autocodeRule:view']"
            >{{ scope.row.rulesCode }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="规则名称"
        key="rulesName"
        prop="rulesName"
        min-width="200px"
        show-overflow-tooltip
        v-if="columns[1].visible"
      />
      <el-table-column
        label="示例编码"
        key="exampleCode"
        prop="exampleCode"
        min-width="200px"
        show-overflow-tooltip
        v-if="columns[2].visible"
      >
      </el-table-column>
      <el-table-column
        label="当前编码"
        key="currentCode"
        prop="currentCode"
        min-width="200px"
        show-overflow-tooltip
        v-if="columns[3].visible"
      >
      </el-table-column>
      <el-table-column
        label="前缀"
        key="prefix"
        prop="prefix"
        min-width="100"
        show-overflow-tooltip
        v-if="columns[4].visible"
      />
      <el-table-column
        align="center"
        key="changeDate"
        label="日期构成"
        show-overflow-tooltip
        v-if="columns[5].visible"
      >
        <template slot-scope="scope">
          {{ changeDate(scope.row) }}
        </template>
      </el-table-column>
      <el-table-column
        label="后缀"
        key="suffix"
        prop="suffix"
        min-width="100"
        show-overflow-tooltip
        v-if="columns[6].visible"
      />
      <el-table-column
        label="补齐长度"
        align="center"
        key="numberLength"
        prop="numberLength"
        v-if="columns[7].visible"
      />
      <el-table-column
        label="是否补齐"
        align="center"
        key="isPadded"
        prop="isPadded"
        v-if="columns[8].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isPadded == 1">yes</el-tag>
          <el-tag v-else type="danger">no</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="备注"
        min-width="180px"
        align="center"
        key="remark"
        prop="remark"
        v-if="columns[9].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.remark" />
        </template>
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <el-button
            type="text"
            icon="el-icon-edit"
            @click="handleUpdate(scope.row)"
            :disabled="hasPermi(['masterData:autocodeRule:edit'])"
            >修改</el-button
          >
          <!-- <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['masterData:autocodeRule:remove'])"
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

    <!-- 添加或修改参数配置对话框 -->
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
            <el-form-item label="规则编码" prop="rulesCode">
              <el-input
                v-model="form.rulesCode"
                :disabled="title == '修改编码规则'"
                placeholder="请输入规则编码"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="规则名称">
              <el-input
                :disabled="title == '修改编码规则'"
                v-model="form.rulesName"
                placeholder="请输入规则名称"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="前缀" prop="prefix">
              <el-input
                v-model="form.prefix"
                placeholder="请输入"
              /> </el-form-item
          ></el-col>
          <el-col :span="8">
            <el-form-item label="日期构成">
              <el-checkbox v-model="form.hasYear" :checked="form.hasYear"
                >年份</el-checkbox
              >
              <el-checkbox v-model="form.hasMonth" :checked="form.hasMonth"
                >月份</el-checkbox
              >
              <el-checkbox v-model="form.hasDay" :checked="form.hasDay"
                >日期</el-checkbox
              >
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="后缀" prop="suffix">
              <el-input
                v-model="form.suffix"
                placeholder="请输入"
              /> </el-form-item
          ></el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="补齐长度" prop="numberLength">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :dis="optType == 'view'"
                :myNum="form.numberLength"
                @changeNum="changeNum"
                :numName="'numberLength'"
                :min="1"
                :max="10"
              /> </el-form-item
          ></el-col>
          <el-col :span="8">
            <el-form-item label="是否补齐" prop="isPadded">
              <el-radio-group v-removeAriaHidden v-model="form.isPadded">
                <el-radio :label="1">yes</el-radio>
                <el-radio :label="0">no</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item label="备注" prop="remark">
          <el-input
            v-model="form.remark"
            type="textarea"
            placeholder="请输入内容"
            :autosize="{ minRows: 2, maxRows: 10 }"
            :resize="optType == 'view' ? 'none' : ''"
          ></el-input>
        </el-form-item>
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listEncodeBuildRules,
  getEncodeBuildRules,
  delEncodeBuildRules,
  updateEncodeBuildRules,
  addEncodeBuildRules,
  delList,
} from "@/api/masterData/codeRule";

export default {
  name: "AutoCodeRule", 
  data() {
    return {
      page: "autocodeRule",
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
      // 字典表格数据
      ruleList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 日期范围
      dateRange: [],
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        rulesCode: undefined,
        rulesName: undefined,
        prefix: undefined,
        suffix: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        rulesCode: [
          { required: true, message: "规则编码不能为空", trigger: "blur" },
        ],
        rulesName: [
          { required: true, message: "规则名称不能为空", trigger: "blur" },
        ],
      },
      // 列信息
      columns: [
        { key: 0, label: "规则编码", visible: true },
        { key: 1, label: "规则名称", visible: true },
        { key: 2, label: "示例编码", visible: true },
        { key: 3, label: "当前编码", visible: true },
        { key: 4, label: "前缀", visible: true },
        { key: 5, label: "日期构成", visible: true },
        { key: 6, label: "后缀", visible: true },
        { key: 7, label: "补齐长度", visible: true },
        { key: 8, label: "是否补齐", visible: true },
        { key: 9, label: "备注", visible: true },
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
    changeDate(row) {
      let str = "";
      if (row.hasYear && row.hasMonth && row.hasDay) {
        str = "年/月/日";
      } else if (row.hasYear && row.hasMonth) {
        str = "年/月";
      } else if (row.hasMonth && row.hasDay) {
        str = "月/日";
      } else if (row.hasYear && row.hasDay) {
        str = "年/日";
      } else if (row.hasYear) {
        str = "年";
      } else if (row.hasMonth) {
        str = "月";
      } else if (row.hasDay) {
        str = "日";
      } else {
        str = "";
      }
      return str;
    },
    /** 查询编码列表 */
    getList(isSearch) {
      this.loading = true;
      listEncodeBuildRules(this.queryParams).then((res) => {
        this.ruleList = res.data.list;
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
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.dateRange = [];
      this.handleQuery();
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.open = true;
      this.initialForm = Object.assign({}, this.form);
      this.title = "添加编码规则";
      this.optType = "add";
    },

    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const ruleId = row.id || this.ids;
      getEncodeBuildRules(ruleId).then((res) => {
        if (res.code == 0) {
          this.form = {
            ...res.data,
            hasDay: Boolean(res.data.hasDay),
            hasMonth: Boolean(res.data.hasMonth),
            hasYear: Boolean(res.data.hasYear),
          };
          this.initialForm = Object.assign({}, res.data);
          this.open = true;
          this.title = "修改编码规则";
          this.optType = "edit";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 点击查看
    handleView(id) {
      this.reset();
      getEncodeBuildRules(id).then((res) => {
        if (res.code == 0) {
          this.form = {
            ...res.data,
            hasDay: Boolean(res.data.hasDay),
            hasMonth: Boolean(res.data.hasMonth),
            hasYear: Boolean(res.data.hasYear),
          };
          this.title = "查看编码规则";
          this.optType = "view";
          this.open = true;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm: function () {
      if (this.form.id != undefined) {
        updateEncodeBuildRules(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addEncodeBuildRules(this.form).then((res) => {
          if (res.code == 0) {
            this.queryParams.pageNum = 1;
            this.$modal.msgSuccess("新增成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.id,
          delEncodeBuildRules,
          this.getList,
          "规则编码为" + row.subjectCode
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
  },
};
</script>
<style lang="scss" scoped>
::v-deep .el-checkbox {
  margin-right: 10px !important;
}
</style>
