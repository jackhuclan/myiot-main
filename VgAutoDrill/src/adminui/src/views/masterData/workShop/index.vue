<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="车间编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入车间编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="车间名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入车间名称"
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
          :disabled="hasPermi(['masterData:workshop:add'])"
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
          :disabled="hasPermi(['masterData:workshop:remove'])"
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
      :data="workShopList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <!-- 点击可进行查看 -->
      <el-table-column
        label="车间编码"
        show-overflow-tooltip
        key="code"
        prop="code"
        min-width="150"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['masterData:workshop:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="车间名称"
        key="name"
        prop="name"
        show-overflow-tooltip
        min-width="150"
        v-if="columns[1].visible"
      />
      <el-table-column
        label="负责人"
        key="charge"
        prop="charge"
        show-overflow-tooltip
        min-width="100"
        v-if="columns[2].visible"
      />
      <el-table-column
        label="状态"
        align="center"
        key="status"
        prop="status"
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">
          <dict-tag
            :options="dict.type.sys_normal_disable"
            :value="scope.row.status"
          />
        </template>
      </el-table-column>
      <el-table-column
        label="备注"
        min-width="150"
        key="remark"
        prop="remark"
        v-if="columns[4].visible"
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
            :disabled="hasPermi(['masterData:workshop:edit'])"
            >修改</el-button
          >
          <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['masterData:workshop:remove'])"
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

    <!-- 添加或修改车间对话框 -->
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
          <el-col :span="10">
            <el-form-item label="车间编码" prop="code">
              <el-input
                v-model="form.code"
                placeholder="请输入车间编码"
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
            <el-form-item label="车间名称" prop="name">
              <el-input v-model="form.name" placeholder="请输入车间名称" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="8">
            <el-form-item label="负责人" prop="charge">
              <el-input v-model="form.charge" placeholder="请选择负责人">
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
          <el-col :span="4">
            <el-form-item></el-form-item>
          </el-col>
          <el-col :span="12" v-if="title == '修改车间'">
            <el-form-item label="状态" prop="status">
              <el-radio-group v-removeAriaHidden v-model="form.status">
                <el-radio :label="1">正常</el-radio>
                <el-radio :label="0">停用</el-radio>
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
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listWorkshop,
  getWorkshop,
  delWorkshop,
  addWorkshop,
  updateWorkshop,
  delList,
} from "@/api/masterData/workShop";
// 负责人选择
import UserSingleSelect from "@/components/userSelect";
export default {
  name: "Workshop",
  dicts: ["sys_normal_disable"],
  components: { UserSingleSelect },
  data() {
    return {
      page: "workShop",
      autoGenFlag: false,
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
      // 车间表格数据
      workShopList: [],
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
        charge: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        code: [
          { required: true, message: "车间编码不能为空", trigger: "change" },
        ],
        name: [
          { required: true, message: "车间名称不能为空", trigger: "blur" },
        ],
        status: [
          { required: true, message: "是否启用不能为空", trigger: "blur" },
        ],
      },
      // 列信息
      columns: [
        { key: 0, label: "车间编码", visible: true },
        { key: 1, label: "车间名称", visible: true },
        { key: 2, label: "负责人", visible: true },
        { key: 3, label: "状态", visible: true },
        { key: 4, label: "备注", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  watch: {
    "form.charge": {
      handler(val) {
        if (val == "") {
          this.form.charge = "";
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
    /** 查询车间列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listWorkshop(this.queryParams);
      this.workShopList = res.data.list;
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
        charge: "",
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
      this.open = true;
      this.initialForm = Object.assign({}, this.form);
      this.title = "添加车间";
      this.optType = "add";
    },
    // 查询明细按钮操作
    handleView(id) {
      this.reset();
      getWorkshop(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "查看车间";
          this.optType = "view";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const workShopId = row.id || this.ids;
      getWorkshop(workShopId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "修改车间";
          this.enCode = res.data.code;
          this.optType = "edit";
          this.initialForm = Object.assign({}, res.data);
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm() {
      if (this.form.id != null) {
        updateWorkshop(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addWorkshop(this.form).then((res) => {
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
          delWorkshop,
          this.getList,
          "车间编码为" + row.code
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    //点击负责人选择按钮
    handleUserSelect() {
      this.$refs.userSelect.showFlag = true;
      this.$refs.userSelect.selectedId = this.form.charge
        ? this.form.charge
        : undefined;
      this.$refs.userSelect.getTreeselect();
      this.$refs.userSelect.getList();
    },
    //负责人返回
    onUserSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "charge", obj.realName);
      }
    },
    //自动生成物料编码
    async handleAutoGenChange(autoGenFlag) {
      if (autoGenFlag) {
        this.getEncode({
          rulesCode: "WORKSHOP_CODE",
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
