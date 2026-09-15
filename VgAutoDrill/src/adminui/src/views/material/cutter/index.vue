<template>
  <div class="app-container">
    <el-form
      v-affix
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      v-show="showSearch"
    >
      <el-form-item label="刀具名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入刀具名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="刀具编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入刀具编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="状态" prop="status">
        <el-select
          v-model="queryParams.status"
          placeholder="刀具状态"
          clearable
          @clear="clearQueryParams('status')"
        >
          <el-option
            v-for="dict in dict.type.sys_normal_disable"
            :key="dict.value"
            :label="dict.label"
            :value="dict.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item>
        <el-button
          v-debounce
          type="primary"
          icon="el-icon-search"
          @click="handleQuery"
          >搜索</el-button
        >
        <el-button v-debounce icon="el-icon-refresh" @click="resetQuery"
          >重置</el-button
        >
      </el-form-item>
    </el-form>

    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          v-hasPermi="['material:cutter:add']"
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
          v-hasPermi="['material:cutter:remove']"
          >删除</el-button
        >
      </el-col>
      <right-toolbar
        :showSearch.sync="showSearch"
        @queryTable="getList"
      ></right-toolbar>
    </el-row>

    <el-table
      v-loading="loading"
      :data="cutterList"
      @selection-change="handleSelectionChange"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="刀具编码"
        show-overflow-tooltip
        align="center"
        prop="code"
      />
      <el-table-column
        show-overflow-tooltip
        label="刀具名称"
        align="center"
        prop="name"
      />
      <el-table-column label="最高库存" align="center" prop="maxStock" />
      <el-table-column label="现有库存" align="center" prop="currentStock" />
      <el-table-column label="最低库存" align="center" prop="minStock" />
      <el-table-column
        label="规格型号"
        align="center"
        prop="specification"
        show-overflow-tooltip
      />
      <el-table-column
        label="单位"
        align="center"
        prop="unitOfMeasure"
        show-overflow-tooltip
      />
      <el-table-column label="刀片寿命" align="center" prop="life" />
      <el-table-column
        label="预计加工(件)"
        width="160"
        align="center"
        prop="estimatedPieces"
      />
      <el-table-column label="入库日期" align="center" width="180">
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.inboundDate) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="用途位置"
        show-overflow-tooltip
        align="center"
        prop="purpose"
      />
      <el-table-column label="状态" align="center" prop="status">
        <template slot-scope="scope">
          <dict-tag
            :options="dict.type.sys_normal_disable"
            :value="scope.row.status"
          />
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
            v-hasPermi="['material:cutter:edit']"
            >修改</el-button
          >
          <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            v-hasPermi="['material:cutter:remove']"
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

    <!-- 添加或修改刀具对话框 -->
    <el-dialog
      :title="title"
      :visible.sync="open"
      width="960px"
      append-to-body
      :close-on-click-modal="false"
      v-dialogClose
      v-dialogDrag
    >
      <el-form ref="form" :model="form" :rules="rules" label-width="100px">
        <el-row>
          <el-col :span="8">
            <el-form-item label="刀具名称" prop="name">
              <el-input
                v-model="form.name"
                placeholder="请输入刀具名称"
              /> </el-form-item
          ></el-col>
          <el-col :span="8">
            <el-form-item label="刀具编码" prop="code">
              <el-input
                v-model="form.code"
                placeholder="请输入刀具编码"
              /> </el-form-item
          ></el-col>
          <el-col :span="8"
            ><el-form-item label="刀片寿命" prop="life">
              <el-input
                type="number"
                v-model="form.life"
                placeholder="请输入刀片寿命"
              /> </el-form-item
          ></el-col>
        </el-row>
        <el-row>
          <el-col :span="8"
            ><el-form-item label="最高库存" prop="maxStock">
              <el-input
                type="number"
                v-model="form.maxStock"
                placeholder="请输入最高库存"
              /> </el-form-item
          ></el-col>
          <el-col :span="8"
            ><el-form-item label="现有库存" prop="currentStock">
              <el-input
                type="number"
                v-model="form.currentStock"
                placeholder="请输入现有库存"
              /> </el-form-item
          ></el-col>
          <el-col :span="8">
            <el-form-item label="最低库存" prop="minStock">
              <el-input
                type="number"
                v-model="form.minStock"
                placeholder="请输入最低库存"
              /> </el-form-item
          ></el-col>
        </el-row>

        <el-row>
          <el-col :span="8">
            <el-form-item label="规格型号" prop="specification">
              <el-input
                v-model="form.specification"
                placeholder="请输入规格型号"
              /> </el-form-item
          ></el-col>
          <el-col :span="8"
            ><el-form-item label="单位" prop="unitOfMeasure">
              <el-input v-model="form.unitOfMeasure" placeholder="请输入单位" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="预计加工件数" prop="estimatedPieces">
              <el-input
                type="number"
                v-model="form.estimatedPieces"
                placeholder="请输入预计加工件数"
              /> </el-form-item
          ></el-col>
        </el-row>

        <el-row>
          <el-col :span="8">
            <el-form-item label="用途位置" prop="purpose">
              <el-input
                v-model="form.purpose"
                placeholder="请输入用途位置"
              /> </el-form-item
          ></el-col>
          <el-col :span="8">
            <el-form-item label="入库日期" prop="inboundDate">
              <el-date-picker
                v-model="form.inboundDate"
                type="datetime"
                placeholder="选择日期时间"
              >
              </el-date-picker>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item
              label="刀具状态"
              v-if="title == '修改刀具'"
              prop="status"
            >
              <el-radio-group v-removeAriaHidden v-model="form.status">
                <el-radio
                  v-for="dict in dict.type.sys_normal_disable"
                  :key="dict.value"
                  :label="dict.value * 1"
                  >{{ dict.label }}</el-radio
                >
              </el-radio-group>
            </el-form-item></el-col
          >
        </el-row>
      </el-form>

      <div slot="footer" class="dialog-footer">
        <el-button type="primary" @click="submitForm">确 定</el-button>
        <el-button @click="open = false">取 消</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import {
  listCutter,
  getCutter,
  delCutter,
  addCutter,
  updateCutter,
  delList,
} from "@/api/material/cutter";

export default {
  name: "Cutter",
  dicts: ["sys_normal_disable"],
  data() {
    return {
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 刀具表格数据
      cutterList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 0,
        pageSize: 0,
        name: undefined,
        code: undefined,
        status: undefined,
      },
      // 表单参数
      form: {},
      // 表单校验
      rules: {
        name: [
          { required: true, message: "刀具名称不能为空", trigger: "blur" },
        ],
        specification: [
          { required: true, message: "规格型号不能为空", trigger: "blur" },
        ],
        status: [
          { required: true, message: "刀具状态不能为空", trigger: "blur" },
        ],
        code: [
          { required: true, message: "刀具编码不能为空", trigger: "blur" },
        ],
        maxStock: [
          { required: true, message: "最高库存不能为空", trigger: "blur" },
        ],
        currentStock: [
          { required: true, message: "现有库存不能为空", trigger: "blur" },
        ],
        minStock: [
          { required: true, message: "最低库存不能为空", trigger: "blur" },
        ],
        inboundDate: [
          { required: true, message: "入库日期不能为空", trigger: "blur" },
        ],
        unitOfMeasure: [
          { required: true, message: "单位不能为空", trigger: "blur" },
        ],
        life: [
          { required: true, message: "刀片寿命不能为空", trigger: "blur" },
        ],
        estimatedPieces: [
          { required: true, message: "预计加工件数不能为空", trigger: "blur" },
        ],
        purpose: [
          { required: true, message: "用途位置不能为空", trigger: "blur" },
        ],
        cutterStatus: [
          { required: true, message: "刀具不能为空", trigger: "blur" },
        ],
      },
      // 时间组件
      pickerOptions: {
        shortcuts: [
          {
            text: "今天",
            onClick(picker) {
              picker.$emit("pick", new Date());
            },
          },
          {
            text: "昨天",
            onClick(picker) {
              const date = new Date();
              date.setTime(date.getTime() - 3600 * 1000 * 24);
              picker.$emit("pick", date);
            },
          },
          {
            text: "一周前",
            onClick(picker) {
              const date = new Date();
              date.setTime(date.getTime() - 3600 * 1000 * 24 * 7);
              picker.$emit("pick", date);
            },
          },
        ],
      },
    };
  },
  created() {
    this.getList();
  },
  methods: {
    /** 查询刀具列表 */
    async getList() {
      this.loading = true;
      const res = await listCutter(this.queryParams);
      this.cutterList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
    },
    // 表单重置
    reset() {
      this.form = {
        code: undefined,
        name: undefined,
        maxStock: undefined,
        currentStock: undefined,
        minStock: undefined,
        inboundDate: undefined,
        specification: undefined,
        unitOfMeasure: undefined,
        life: undefined,
        estimatedPieces: undefined,
        purpose: undefined,
        cutterStatus: undefined,
        isDeleted: undefined,
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
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.ids = selection.map((item) => item.id);
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.open = true;
      this.title = "添加刀具";
    },
    /** 修改按钮操作 */
    async handleUpdate(row) {
      this.reset();
      const cutterId = row.id || this.ids;
      const res = await getCutter(cutterId);
      this.form = res.data;
      this.open = true;
      this.title = "修改刀具";
    },
    /** 提交按钮 */
    submitForm: function () {
      this.$refs["form"].validate(async (valid) => {
        if (valid) {
          if (this.form.id != undefined) {
            await updateCutter(this.form);
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            await addCutter(this.form);
            this.$modal.msgSuccess("新增成功");
            this.open = false;
            this.getList();
          }
        }
      });
    },
    // 删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(row.id, delCutter, this.getList);
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
  },
};
</script>
