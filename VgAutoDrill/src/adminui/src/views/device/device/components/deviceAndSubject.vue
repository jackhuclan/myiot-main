<template>
  <el-dialog
    :visible.sync="open"
    width="960px"
    append-to-body
    :close-on-click-modal="false"
    v-dialogClose
    v-dialogDrag
    @close="closeDialog"
  >
    <el-tabs type="border-card">
      <el-tab-pane label="点检项目">
        <div class="app-container">
          <el-row :gutter="10" class="mb8">
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
            :data="checksubjectList"
            @selection-change="handleSelectionChange"
            @row-dblclick="rowDblclick"
            :row-style="rowStyle"
            width="100%"
          >
            <el-table-column type="selection" width="55" align="center" />
            <el-table-column
              label="项目编码"
              prop="subjectCode"
              show-overflow-tooltip
            />
            <el-table-column
              label="项目名称"
              prop="subjectName"
              show-overflow-tooltip
            />
            <el-table-column
              label="标准"
              width="200px"
              prop="standard"
              show-overflow-tooltip
            />
            <el-table-column
              label="项目内容"
              width="200px"
              prop="subjectContent"
              show-overflow-tooltip
            />

            <el-table-column label="操作" align="center" fixed="right">
              <template slot-scope="scope">
                <el-button
                  v-debounce
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
        </div>
        <SubjectSelect
          ref="subjectSelect"
          @onSelected="onSubjectSelected"
        ></SubjectSelect>
      </el-tab-pane>
    </el-tabs>
  </el-dialog>
</template>

<script>
import SubjectSelect from "@/components/subjectSelect/checkbox";
import {
  addDeviceAndSubject,
  listDeviceAndSubject,
  delDeviceAndSubject,
  delList,
} from "@/api/device/deviceAndSubject";
export default {
  name: "Checksubject",
  props: ["deviceId"],
  components: { SubjectSelect },
  data() {
    return {
      page: "checksubject",
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 需要回显的数据
      echoIds: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 点检项目表格数据
      checksubjectList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        deviceId: undefined,
        subjectId: undefined,
      },
      // 表单参数
      form: {},
    };
  },
  methods: {
    /** 查询点检项目列表 */
    getList() {
      this.loading = true;
      this.multipleSelection = [];
      listDeviceAndSubject(this.queryParams).then((response) => {
        this.checksubjectList = response.data.list;
        this.total = response.data.total;
        this.loading = false;
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
    /** 新增按钮操作 */
    handleAdd() {
      //需要回显的数据
      this.echoIds = [];
      listDeviceAndSubject({
        pageNum: 1,
        pageSize: 10000,
        deviceId: this.deviceId,
      }).then((res) => {
        res.data.list?.forEach((v) => {
          this.echoIds.push(v.subjectId);
        });
        this.$refs.subjectSelect.queryParams.pageNum = 1;
        this.$refs.subjectSelect.showFlag = true;
        this.$refs.subjectSelect.showData = this.echoIds;
        this.$refs.subjectSelect.getList();
      });
    },
    onSubjectSelected(rows) {
      if (this.isArrEqual(rows, this.checksubjectList, "subjectId")) {
        return (this.$refs.subjectSelect.showFlag = false);
      }
      addDeviceAndSubject({
        deviceId: this.deviceId,
        subjectIds: rows,
      }).then((response) => {
        if (response.code == 0) {
          this.queryParams.pageNum = 1;
          this.getList();
          this.$modal.msgSuccess("操作成功");
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    //删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.id,
          delDeviceAndSubject,
          this.getList,
          "项目编码为" + row.subjectId
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 关闭点检清空回显
    closeDialog() {
      // this.arr = [];
      this.open = false;
    },
  },
};
</script>
