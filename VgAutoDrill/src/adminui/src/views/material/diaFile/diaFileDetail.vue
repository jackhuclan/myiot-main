<template>
  <!-- 查看弹框 -->
  <el-card>
    <el-row :gutter="10" class="mb8" v-if="optType != 'view'">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="addConfigDetail"
          >新增</el-button
        >
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
      :data="cutterDetailList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column
        type="selection"
        width="55"
        align="center"
        v-if="optType != 'view'"
      />
      <el-table-column
        label="D直径MM"
        align="center"
        prop="d"
        show-overflow-tooltip
      >
        <template slot-scope="scope">
          <el-input
            step="0.1"
            type="number"
            placeholder="请输入内容"
            v-show="scope.row.show"
            v-model="scope.row.d"
          ></el-input>
          <span v-show="!scope.row.show">{{ scope.row.d }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="S转速KRPM"
        width="120"
        align="center"
        prop="s"
        show-overflow-tooltip
      >
        <template slot-scope="scope">
          <el-input
            step="0.1"
            type="number"
            placeholder="请输入内容"
            v-show="scope.row.show"
            v-model="scope.row.s"
          ></el-input>
          <span v-show="!scope.row.show">{{ scope.row.s }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="F(进刀速)M/MIN"
        width="120"
        align="center"
        prop="f"
        show-overflow-tooltip
      >
        <template slot-scope="scope">
          <el-input
            step="0.1"
            type="number"
            placeholder="请输入内容"
            v-show="scope.row.show"
            v-model="scope.row.f"
          ></el-input>
          <span v-show="!scope.row.show">{{ scope.row.f }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="R(退刀速)M/MIN"
        width="120"
        align="center"
        prop="r"
        show-overflow-tooltip
      >
        <template slot-scope="scope">
          <el-input
            step="0.1"
            type="number"
            placeholder="请输入内容"
            v-show="scope.row.show"
            v-model="scope.row.r"
          ></el-input>
          <span v-show="!scope.row.show">{{ scope.row.r }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="Z(深度补偿)MM"
        width="120"
        align="center"
        prop="z"
        show-overflow-tooltip
      >
        <template slot-scope="scope">
          <el-input
            step="0.1"
            type="number"
            placeholder="请输入内容"
            v-show="scope.row.show"
            v-model="scope.row.z"
          ></el-input>
          <span v-show="!scope.row.show">{{ scope.row.z }}</span>
        </template>
      </el-table-column>
      <el-table-column
        show-overflow-tooltip
        label="寿命"
        align="center"
        prop="age"
      >
        <template slot-scope="scope">
          <el-input
            type="number"
            placeholder="请输入内容"
            v-show="scope.row.show"
            v-model="scope.row.age"
          ></el-input>
          <span v-show="!scope.row.show">{{ scope.row.age }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        min-width="140px"
        class-name="small-padding fixed-width"
        v-if="optType != 'view'"
      >
        <template slot-scope="scope">
          <el-button
            v-if="!scope.row.show"
            type="text"
            icon="el-icon-edit"
            @click="() => handleConfigDetail(scope.row, 'edit')"
            >修改</el-button
          >

          <el-button
            v-if="scope.row.show"
            type="text"
            icon="el-icon-document-checked"
            @click="() => handleConfigDetail(scope.row, 'save')"
            >保存</el-button
          >

          <el-button
            @click="handleDelete(scope.row)"
            type="text"
            icon="el-icon-delete"
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
    <edit-form-dialog
      v-model="open"
      title="添加刀具明细"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form ref="form" :model="form" :rules="rules" label-width="150px">
        <el-row>
          <el-col :span="12">
            <el-form-item label="D直径MM" prop="d">
              <el-input-number
                v-model="form.d"
                :precision="2"
                :step="0.1"
                :max="10"
                placeholder="请输入D直径MM"
              ></el-input-number>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="S转速KRPM" prop="s">
              <el-input-number
                v-model="form.s"
                :precision="2"
                :step="0.1"
                :max="10"
                placeholder="请输入S转速KRPM"
              ></el-input-number>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="F(进刀速)M/MIN" prop="f">
              <el-input-number
                v-model="form.f"
                :precision="2"
                :step="0.1"
                :max="10"
                placeholder="请输入F(进刀速)M/MIN"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="R(退刀速)M/MIN" prop="r">
              <el-input-number
                v-model="form.r"
                :precision="2"
                :step="0.1"
                :max="10"
                placeholder="请输入R(退刀速)M/MIN"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="Z(深度补偿)MM" prop="z">
              <el-input-number
                v-model="form.z"
                :precision="2"
                :step="0.1"
                :max="10"
                placeholder="请输入Z(深度补偿)MM"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="寿命" prop="age">
              <el-input-number
                :precision="2"
                v-model="form.age"
                placeholder="请输入寿命"
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
  listCutterConfigDetail,
  delCutterConfigDetail,
  getCutterConfigDetail,
  updateCutterConfigDetail,
  addCutterConfigDetail,
  delList,
} from "@/api/material/configDetail";
export default {
  name: "DiaFileDetail",
  data() {
    return {
      page: "diaFileDetail",
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        masterId: undefined,
      },
      // 选中数组
      ids: [],
      multipleSelection: [],
      loading: false,
      open: false,
      rules: {
        d: [
          { required: true, message: "产品大类名称不能为空", trigger: "blur" },
        ],
        s: [
          { required: true, message: "产品大类名称不能为空", trigger: "blur" },
        ],
        f: [
          { required: true, message: "产品大类名称不能为空", trigger: "blur" },
        ],
        r: [
          { required: true, message: "产品大类名称不能为空", trigger: "blur" },
        ],
        z: [
          { required: true, message: "产品大类名称不能为空", trigger: "blur" },
        ],
        age: [
          { required: true, message: "产品大类名称不能为空", trigger: "blur" },
        ],
      },
      form: {},
      initialForm: {},
      // 刀具参数明细表格数据
      cutterDetailList: [],
      // 总条数
      total: 0,
      // 详细表查询参数
    };
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
  },
  props: ["optType"],
  methods: {
    // 查询刀具参数明细
    getList() {
      this.loading = true;
      listCutterConfigDetail(this.queryParams).then((res) => {
        this.cutterDetailList = res.data.list.map((v) => {
          return { ...v, show: false };
        });
        this.total = res.data.total;
        this.loading = false;
      });
    },
    // 表格行样式
    rowStyle({ row, rowIndex }) {
      Object.defineProperty(row, "rowIndex", {
        //给每一行添加不可枚举属性rowIndex来标识当前行
        value: rowIndex,
        writable: true,
        enumerable: false,
      });
    },
    //监听row-dblclick事件，实现选中
    rowDblclick(row, column, event) {
      if (this.optType == "view") return;
      // 获取表格对象
      let refsElTable = this.$refs[this.page];
      let findRow = this.multipleSelection.find(
        (c) => c.rowIndex == row.rowIndex
      );
      //找到选中的行
      if (findRow) {
        refsElTable.toggleRowSelection(row, false); //如过重复选中，则取消选中
        return;
      }
      refsElTable.toggleRowSelection(row, true); // 实现选中行中选中事件
    },
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.multipleSelection = selection;
      this.ids = selection.map((item) => item.id);
    },
    // 点击确定
    submitForm: function () {
      addCutterConfigDetail(this.form).then((res) => {
        if (res.code == 0) {
          this.$modal.msgSuccess("新增成功");
          this.open = false;
          this.queryParams.pageNum = 1;
          this.getList();
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 表单重置
    reset() {
      this.form = {
        masterId: this.queryParams.masterId,
        d: 0,
        s: 0,
        f: 0,
        r: 0,
        z: 0,
        age: 0,
      };
      this.resetForm("form");
    },
    // 新增明细表按钮
    addConfigDetail() {
      this.reset();
      this.initialForm = Object.assign({}, this.form);
      this.open = true;
    },
    // 点击保存
    handleConfigDetail(row, type) {
      const id = row.id || this.ids;
      if (type == "edit") {
        getCutterConfigDetail(id).then((res) => {
          if (res.code == 0) {
            this.form = res.data;
            this.initialForm = Object.assign({}, res.data);
            row.show = true;
            this.cutterDetailList = this.cutterDetailList.map((v) => {
              if (v.id != row.id) {
                return { ...v, show: false };
              }
              return { ...v };
            });
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        row.show = false;
        updateCutterConfigDetail(row).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("保存成功");
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
        this.deleteItem(row.id, delCutterConfigDetail, this.getList);
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
  },
};
</script>
