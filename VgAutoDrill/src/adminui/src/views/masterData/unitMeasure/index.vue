<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="单位编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入单位编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="单位名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入单位名称"
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
          :disabled="hasPermi(['masterData:unitMeasure:add'])"
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
          :disabled="hasPermi(['masterData:unitMeasure:remove'])"
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
          :disabled="hasPermi(['masterData:unitMeasure:import'])"
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
      :data="unitMeasureList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <!-- 点击可进行查看 -->
      <el-table-column
        label="单位编码"
        key="code"
        prop="code"
        show-overflow-tooltip
        min-width="150"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['masterData:unitMeasure:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="单位名称"
        key="name"
        prop="name"
        show-overflow-tooltip
        v-if="columns[1].visible"
        min-width="150"
      />
      <el-table-column
        label="是否主单位"
        align="center"
        key="primaryFlag"
        prop="primaryFlag"
        v-if="columns[2].visible"
        min-width="150"
      >
        <template slot-scope="scope">
          <el-tag type="danger" v-if="scope.row.primaryFlag != 'Y'">否</el-tag>
          <el-tag v-else>是</el-tag>
        </template>
      </el-table-column>

      <el-table-column
        label="是否启用"
        align="center"
        key="status"
        prop="status"
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.status == 1">是</el-tag>
          <el-tag v-else type="danger">否</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="备注"
        key="remark"
        prop="remark"
        v-if="columns[4].visible"
        min-width="180"
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
            :disabled="hasPermi(['masterData:unitMeasure:edit'])"
            >修改</el-button
          >
          <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['masterData:unitMeasure:remove'])"
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

    <!-- 添加或修改单位对话框 -->
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
          <el-col :span="10">
            <el-form-item label="单位编码" prop="code">
              <el-input
                v-model="form.code"
                placeholder="请输入单位编码"
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
            <el-form-item label="单位名称" prop="name">
              <el-input
                v-model="form.name"
                placeholder="请输入单位名称"
              /> </el-form-item
          ></el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="是否主单位" prop="primaryFlag">
              <el-radio-group
                v-removeAriaHidden
                v-model="form.primaryFlag"
                @change="handlePrimaryFlag"
              >
                <el-radio :label="'Y'">是</el-radio>
                <el-radio :label="'N'">否</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item
              label="是否启用"
              prop="status"
              v-if="title == '修改单位'"
            >
              <el-radio-group v-removeAriaHidden v-model="form.status">
                <el-radio :label="1">是 </el-radio>
                <el-radio :label="0">否 </el-radio>
              </el-radio-group>
            </el-form-item></el-col
          >
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item
              label="主单位"
              prop="primaryName"
              v-if="form.primaryFlag == 'N'"
            >
              <el-select
                v-model="primaryName"
                placeholder="请选择"
                @change="handlePrimary"
                clearable
                @clear="clearFormItem('primaryFlag')"
              >
                <el-option
                  v-for="item in measureOptions"
                  :key="item.value"
                  :label="item.name"
                  :value="item.id"
                  v-optionTitle
                >
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item
              label="换算比例"
              prop="changeRate"
              v-if="form.primaryFlag == 'N'"
            >
              <!-- 引入自定义计数器组件 -->
              <!-- <input-number
                :myNum="form.changeRate"
                @changeNum="changeNum"
                :numName="'changeRate'"
                :min="1"
              /> -->
              <el-input
                v-model="form.changeRate"
                placeholder="请输入"
              ></el-input>
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
          />
        </el-form-item>
      </el-form>
    </edit-form-dialog>
    <!-- 计量单位导入 -->
    <ImportXlsx
      ref="upload"
      @getList="getList"
      :uploadUrl="uploadUrl"
      :downloadUrl="downloadUrl"
      :fileName="fileName"
    ></ImportXlsx>
  </div>
</template>

<script>
import {
  listUnitMeasure,
  listPrimaryUnitmeasure,
  getUnitMeasure,
  delUnitMeasure,
  addUnitMeasure,
  updateUnitMeasure,
  delList,
} from "@/api/masterData/unitMeasure";
export default {
  name: "UnitMeasure",
  data() {
    // 自定义校验
    const changeRateRule = (rule, value, callback) => {
      // 判断输入框输入的值是否是非数字支持小数
      var reg = /^\d+(?=\.{0,1}\d+$|$)/;
      if (value != "") {
        if (!reg.test(value)) {
          callback(new Error("不正确的输入，请输入数字！"));
          return false;
        } else if (value * 1 == NaN || value <= 0) {
          callback(new Error("不得为0"));
          return false;
        }
      }

      callback();
    };
    return {
      page: "unitMeasure",
      // 自动生成编码
      autoGenFlag: false,
      enCode: "",
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
      // 单位表格数据
      unitMeasureList: [],
      //主单位列表
      measureOptions: [],
      // 主单位
      primaryName: "",
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: null,
        name: null,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        code: [
          { required: true, message: "单位编码不能为空", trigger: "change" },
        ],
        name: [
          { required: true, message: "单位名称不能为空", trigger: "change" },
        ],
        primaryFlag: [
          { required: true, message: "是否是主单位不能为空", trigger: "blur" },
        ],
        status: [
          { required: true, message: "是否启用不能为空", trigger: "blur" },
        ],
        changeRate: [
          { required: true, validator: changeRateRule, trigger: "change" },
        ],
      },
      // 计量单位导入的参数
      // 导入的url
      uploadUrl: "/v1/UnitMeasure/UploadList",
      // 下载的url
      downloadUrl: "/v1/UnitMeasure/DownLoad",
      // 导入的模板下载名
      fileName: "计量单位模板.xlsx",
      // 列信息
      columns: [
        { key: 0, label: "单位编码", visible: true },
        { key: 1, label: "单位名称", visible: true },
        { key: 2, label: "是否是主单位", visible: true },
        { key: 3, label: "是否启用", visible: true },
        { key: 4, label: "备注", visible: true },
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
    /** 查询单位列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listUnitMeasure(this.queryParams);
      this.unitMeasureList = res.data.list;
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
        code: "",
        name: "",
        primaryFlag: "Y",
        remark: "",
        primaryId: 0,
        changeRate: 1,
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

    // 查询明细按钮操作
    handleView(id) {
      const item = this.unitMeasureList.find((v) => (v.id = id));
      this.reset();
      // 非主单位获取主单位
      if (item.primaryFlag == "N") {
        listPrimaryUnitmeasure(id).then((res) => {
          this.measureOptions = res.data;
          this.primaryName = this.unitMeasureList.filter(
            (v) => v.id == item.primaryId
          )[0]?.name;
        });
      }

      getUnitMeasure(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "查看单位";
          this.optType = "view";
          this.autoGenFlag = false;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },

    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      // 主单位为空
      this.primaryName = "";
      // 查询主单位
      listPrimaryUnitmeasure().then((res) => {
        this.measureOptions = res.data;
        this.open = true;
        this.initialForm = Object.assign({}, this.form);
        this.title = "添加单位";
        this.optType = "add";
        this.autoGenFlag = false;
      });
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      // 非主单位获取主单位
      if (row.primaryFlag == "N") {
        listPrimaryUnitmeasure(row.id).then((res) => {
          this.measureOptions = res.data;
          this.primaryName = this.unitMeasureList.filter(
            (v) => v.id == row.primaryId
          )[0]?.name;
        });
      }
      const measureId = row.id || this.ids;
      getUnitMeasure(measureId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.form.changeRate =
            this.form.changeRate <= 0 ? 1 : this.form.changeRate;
          this.enCode = res.data.code;
          this.initialForm = Object.assign({}, res.data);
          this.open = true;
          this.title = "修改单位";
          this.optType = "edit";
          this.autoGenFlag = false;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm() {
      this.form.changeRate =
        this.form.changeRate <= 0 ? 1 : this.form.changeRate;
      if (this.form.id != null) {
        updateUnitMeasure(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addUnitMeasure(this.form).then((res) => {
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
          delUnitMeasure,
          this.getList,
          "单位编码为" + row.code
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 选择主单位
    handlePrimary(val) {
      this.form.primaryId = val;
    },
    clearFormItem() {
      this.form.primaryId = undefined;
    },
    // 是否为主单位
    handlePrimaryFlag(val) {
      if (val == "Y" && this.optType == "add") {
        this.form.primaryId = 0;
      }
    },
    /** 导入按钮操作 */
    handleImport() {
      this.$refs.upload.title = "计量单位导入";
      this.$refs.upload.open = true;
    },
    //自动生成编码
    handleAutoGenChange(autoGenFlag) {
      if (autoGenFlag) {
        this.getEncode({
          rulesCode: "UNITMEASURE_CODE",
          buildCount: 1,
        }).then((response) => {
          const code = response.data[0];
          this.form.code = code;
        });
      } else {
        if (this.optType == "edit") return (this.form.code = this.enCode);
        this.form.code = "";
      }
    },
  },
};
</script>
