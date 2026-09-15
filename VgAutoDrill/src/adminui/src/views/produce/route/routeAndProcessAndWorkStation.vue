<template>
  <div class="app-container">
    <el-row :gutter="10" v-if="optType != 'view'" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['produce:routeProcessAndWorkStation:add'])"
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
          :disabled="hasPermi(['produce:routeProcessAndWorkStation:remove'])"
          >批量删除</el-button
        >
      </el-col>
    </el-row>

    <el-table
      border
      :ref="page"
      v-loading="loading"
      :data="routeProcessAndWorkStationList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="工作站编码"
        prop="workStationCode"
        min-width="100px"
        show-overflow-tooltip
      />
      <el-table-column
        label="工作站名称"
        prop="workStationName"
        min-width="100px"
        show-overflow-tooltip
      />
      <el-table-column
        label="操作"
        align="center"
        v-if="optType != 'view'"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <el-button
            v-debounce
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['produce:routeProcessAndWorkStation:remove'])"
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
    <RouteProcessAndWorkStationSelect
      ref="workStationSelect"
      @onSelected="onWorkStationSelected"
    >
    </RouteProcessAndWorkStationSelect>
  </div>
</template>

<script>
import {
  listRouteProcessAndWorkStation,
  getRouteProcessAndWorkStation,
  delRouteProcessAndWorkStation,
  addRouteProcessAndWorkStation,
  delList,
  updateRouteProcessAndWorkStation,
  exsitWorkStation,
} from "@/api/produce/routeProcessAndWorkStation";
import RouteProcessAndWorkStationSelect from "@/components/routeProcessAndWorkStationSelect";
export default {
  name: "RouteProcessAndWorkStation",
  components: { RouteProcessAndWorkStationSelect },
  data() {
    return {
      page: "routeProcessAndWorkStation",
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
      // 工艺关联设备表格数据
      routeProcessAndWorkStationList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        workStationId: undefined,
        routeAndProcessId: undefined,
      },
      // 表单参数
      form: {},
      // 表单校验
      rules: {
        routeId: [
          { required: true, message: "工艺路线ID不能为空", trigger: "blur" },
        ],
        processId: [
          { required: true, message: "工序ID不能为空", trigger: "blur" },
        ],
      },
      isChange: false,
    };
  },
  props: ["routeAndProcessId", "optType", "processCode"],
  created() {
    this.getList();
  },
  methods: {
    /** 查询工艺组成列表 */
    async getList() {
      this.loading = true;
      this.queryParams.routeAndProcessId = this.routeAndProcessId;
      const res = await listRouteProcessAndWorkStation(this.queryParams);
      this.routeProcessAndWorkStationList = res.data.list;
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
        routeAndProcessId: this.routeAndProcessId,
        workStationId: null,
        orderNum: 1,
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
      //需要回显的数据
      this.echoIds = [];
      listRouteProcessAndWorkStation({
        pageNum: 1,
        pageSize: 10000,
        routeAndProcessId: this.routeAndProcessId,
      }).then((res) => {
        res.data.list?.forEach((v) => {
          this.echoIds.push(v.workStationId);
        });
        this.$refs.workStationSelect.queryParams.pageNum = 1;
        this.$refs.workStationSelect.queryParams.processCode = this.processCode;
        this.$refs.workStationSelect.showFlag = true;
        this.$refs.workStationSelect.showData = this.echoIds;
        this.$refs.workStationSelect.getList();
      });
    },
    //删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.id,
          delRouteProcessAndWorkStation,
          this.getList,
          "工作站编码为" + row.workStationCode
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    //工作站弹出框
    handleSelectWorkStation() {
      this.$refs.workStationSelect.showFlag = true;
    },
    onWorkStationSelected(rows) {
      if (
        this.isArrEqual(
          rows,
          this.routeProcessAndWorkStationList,
          "workStationId"
        )
      ) {
        return (this.$refs.workStationSelect.showFlag = false);
      }
      let workStationIds = [];
      rows.forEach((v) => {
        workStationIds.push({
          workStationId: v,
          orderNum: 0,
        });
      });
      const data = {
        workStationIds,
        routeAndProcessId: this.routeAndProcessId,
      };
      exsitWorkStation(data).then((res) => {
        if (res.code == 0) {
          this.handleAddWorkStation(data);
        } else {
          this.$notify.error({
            title: "提示",
            message: res.message,
          });
          //   this.$confirm(
          //     `<span style="color:red"> ${res.message}</span> ` +
          //       "是否继续添加？",
          //     {
          //       confirmButtonText: "继续",
          //       dangerouslyUseHTMLString: true, // 使用HTML片段
          //     }
          //   )
          //     .then(() => {
          //       this.handleAddWorkStation(data);
          //     })
          //     .catch(() => {});
        }
      });
    },
    // 添加工作站
    handleAddWorkStation(query) {
      addRouteProcessAndWorkStation(query)
        .then((res) => {
          if (res.code == 0) {
            this.queryParams.pageNum = 1;
            this.$modal.msgSuccess("操作成功");
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        })
        .catch(() => {});
      this.$refs.workStationSelect.showFlag = false;
    },
  },
};
</script>
