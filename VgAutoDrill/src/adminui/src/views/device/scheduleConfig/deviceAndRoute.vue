<template>
  <div class="app-container">
    <el-row :gutter="10" class="mb8" v-if="optType != 'view'">
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
      :data="routeList"
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
        label="路线编码"
        min-width="150"
        fixed="left"
        prop="routeCode"
        show-overflow-tooltip
      ></el-table-column>
      <el-table-column
        label="路线名称"
        prop="routeName"
        min-width="150"
        show-overflow-tooltip
      />
      <el-table-column
        label="路线说明"
        prop="routeDesc"
        min-width="150"
        show-overflow-tooltip
      />
      <el-table-column label="是否启用" align="center" prop="status">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.status == 1">是</el-tag>
          <el-tag type="danger" v-else>否</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="备注" width="150px" prop="routeRemark">
        <template slot-scope="scope">
          <tooltip :value="scope.row.routeRemark" />
        </template>
      </el-table-column>

      <el-table-column
        label="操作"
        fixed="right"
        align="center"
        class-name="small-padding fixed-width"
        v-if="optType != 'view'"
      >
        <template slot-scope="scope">
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
    <DeviceAndRouteSelect
      ref="deviceAndRouteSelect"
      @onSelected="onDeviceAndRouteSelected"
      :ids="echoIds"
    ></DeviceAndRouteSelect>
  </div>
</template>

<script>
import {
  listDeviceAndRoute,
  addDeviceAndRoute,
  delDeviceAndRoute,
  delList,
} from "@/api/device/deviceAndRoute";
// 选择
import DeviceAndRouteSelect from "@/components/deviceAndRouteSelect";
export default {
  name: "DeviceAndRoute", 
  props: ["deviceForm", "optType"],
  components: { DeviceAndRouteSelect },
  data() {
    return {
      page: "deviceAndRoute",
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
      // 表格数据
      routeList: [],
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        deviceId: undefined,
        deviceCode: undefined,
        deviceName: undefined,
        deviceTypeId: undefined,
        deviceTypeCode: undefined,
        routeId: undefined,
        routeCode: undefined,
        routeName: undefined,
      },
      isChange: 0,
    };
  },
  methods: {
    /** 根据设备查询工艺路线列表 */
    getList() {
      this.loading = true;
      listDeviceAndRoute(this.queryParams).then((res) => {
        this.routeList = res.data?.list;
        this.total = res.data?.total ? res.data.total : 0;
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
      // 需要回显的数据
      this.echoIds = [];
      listDeviceAndRoute({
        pageNum: 1,
        pageSize: 10000,
        deviceId: this.deviceForm.id,
      }).then((res) => {
        res.data.list?.forEach((v) => {
          this.echoIds.push({
            id: v.routeId,
            code: v.routeCode,
            name: v.routeName,
            vettingStatus: v.routeVettingStatus,
          });
        });
        this.$refs.deviceAndRouteSelect.queryParams.pageNum = 1;
        this.$refs.deviceAndRouteSelect.showFlag = true;
        this.$refs.deviceAndRouteSelect.showData = this.echoIds;
        this.$refs.deviceAndRouteSelect.getList();
      });
    },
    //删除
    handleDelete(row) {
      const id = row.id || this.ids;
      let label = null;
      let api = null;
      if (row.id) {
        label = `确定删除<span style="color:red"> 路线编码为${row.routeCode}</span> 的数据项？`;
        api = delDeviceAndRoute;
      } else {
        if (this.ids.length <= 0)
          return modal.msgWarning("请选择要操作的数据!");
        label = "确定删除当前操作的数据项？";
        api = delList;
      }

      this.$modal
        .confirm(label, {
          dangerouslyUseHTMLString: true, // 使用HTML片段
        })
        .then((result) => {
          if (result == "confirm") {
            api(id).then((res) => {
              if (res.code == 0) {
                this.$modal.msgSuccess("删除成功");
                this.getList();
                this.$emit("getParentList");
              } else {
                this.$modal.notifyError(res.message);
              }
            });
          }
        })
        .catch(() => {});
    },
    // 路线选择框
    onDeviceAndRouteSelected(rows) {
      if (
        this.isArrEqual(
          rows.map((v) => v.id),
          this.routeList,
          "routeId"
        )
      ) {
        return (this.$refs.deviceAndRouteSelect.showFlag = false);
      }
      const form = {
        id: 0,
        status: 0,
        deviceId: this.deviceForm.id,
        deviceCode: this.deviceForm.code,
        deviceName: this.deviceForm.name,
        deviceTypeId: this.deviceForm.deviceTypeId,
        deviceTypeCode: "agv",
        routeList: rows,
      };
      addDeviceAndRoute(form)
        .then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("操作成功");
            this.queryParams.pageNum = 1;
            this.getList();
            this.$emit("getParentList");
          } else {
            this.$modal.notifyError(res.message);
          }
        })
        .catch(() => {});
    },
  },
};
</script>
