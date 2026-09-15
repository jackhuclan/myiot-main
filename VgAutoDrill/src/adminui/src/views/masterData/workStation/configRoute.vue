<template>
  <el-dialog
    title="配置路线"
    :visible.sync="open"
    width="960px"
    append-to-body
    :close-on-click-modal="false"
    v-dialogClose
    v-dialogDrag
  >
    <el-form ref="form" :model="form" disabled label-width="100px">
      <el-row>
        <el-col :span="8">
          <el-form-item label="工作站编码" prop="code">
            <el-input v-model="form.code" placeholder="请输入工作站编码" />
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="工作站名称" prop="name">
            <el-input v-model="form.name" placeholder="请输入工作站名称" />
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="当前工序" prop="processName">
            <el-input v-model="form.processName" placeholder="暂无配置工序" />
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
    <el-tabs type="border-card" v-if="form.id != null && form.processId">
      <el-tab-pane label="关联工艺路线">
        <div class="app-container">
          <el-row :gutter="10" class="mb8" v-if="optType != 'view'">
            <el-col :span="1.5">
              <el-button
                v-debounce
                type="primary"
                plain
                icon="el-icon-plus"
                @click="handleAdd"
                :disabled="hasPermi(['masterData:workstation:addRoute'])"
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
                :disabled="hasPermi(['masterData:workstation:removeRoute'])"
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
              prop="code"
              show-overflow-tooltip
            ></el-table-column>
            <el-table-column
              label="路线名称"
              prop="name"
              min-width="150"
              show-overflow-tooltip
            />
            <el-table-column
              label="路线说明"
              prop="routeDesc"
              min-width="150"
              show-overflow-tooltip
            />
            <el-table-column
              label="审批状态"
              align="center"
              prop="vettingStatus"
            >
              <template slot-scope="scope">
                <el-tag v-if="scope.row.vettingStatus == 1">已审批</el-tag>
                <el-tag type="danger" v-else>未审批</el-tag>
              </template>
            </el-table-column>
            <el-table-column label="是否启用" align="center" prop="status">
              <template slot-scope="scope">
                <el-tag v-if="scope.row.status == 1">是</el-tag>
                <el-tag type="danger" v-else>否</el-tag>
              </template>
            </el-table-column>
            <el-table-column label="备注" width="150px" prop="routeRemark" />
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
                  type="text"
                  icon="el-icon-delete"
                  @click="handleDelete(scope.row)"
                  :disabled="hasPermi(['masterData:workstation:removeRoute'])"
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
          <workStationAndRouteSelect
            ref="workStationAndRouteSelect"
            @onSelected="onWorkStationAndRouteSelected"
            :ids="echoIds"
          />
        </div>
      </el-tab-pane>
    </el-tabs>
  </el-dialog>
</template>

<script>
import {
  getRoutesByWorkStation,
  getRoutesByProcess,
  addDataByWorkStation,
} from "@/api/masterData/workStation";
import {
  delRouteProcessAndWorkStation,
  delList,
  exsitWorkStationByRoute,
} from "@/api/produce/routeProcessAndWorkStation";
import workStationAndRouteSelect from "@/components/workStationAndRouteSelect";
export default {
  name: "ProrouteAndRoute", 
  components: { workStationAndRouteSelect },
  data() {
    return {
      page: "workStationAndRoute",
      //自动生成编码
      autoGenFlag: false,
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
        pageNum: 0,
        pageSize: 0,
        workStationId: 0,
        routeAndProcessId: 0,
      },
      open: false,
      form: {},
      optType: "",
    };
  },
  methods: {
    /** 根据工序编码获取工艺路线结果 */
    async getList() {
      this.loading = true;
      const res = await getRoutesByWorkStation(this.queryParams);
      this.routeList = res.data.list;
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
      this.ids = selection.map((item) => item.routeProcessAndWorkStationId);
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
      getRoutesByWorkStation({
        pageNum: 1,
        pageSize: 10000,
        workStationId: this.queryParams.workStationId,
      }).then((res) => {
        res.data.list?.forEach((v) => {
          this.echoIds.push(v);
        });
        this.$refs.workStationAndRouteSelect.queryParams.pageNum = 1;
        this.$refs.workStationAndRouteSelect.queryParams.processCode =
          this.processCode;
        this.$refs.workStationAndRouteSelect.showFlag = true;
        this.$refs.workStationAndRouteSelect.showData = this.echoIds;
        this.$refs.workStationAndRouteSelect.getList();
      });
    },
    onWorkStationAndRouteSelected(rows) {
      if (
        this.isArrEqual(
          rows.map((v) => v.id),
          this.routeList,
          "id"
        )
      ) {
        return (this.$refs.workStationAndRouteSelect.showFlag = false);
      }

      const data = {
        routeInfos: rows,
        workStationId: this.queryParams.workStationId,
      };
      exsitWorkStationByRoute(data).then((res) => {
        if (res.code == 0) {
          addDataByWorkStation(data).then((res_add) => {
            if (res_add.code == 0) {
              this.$modal.msgSuccess("新增成功");
              this.queryParams.pageNum = 1;
              this.getList();
              this.$refs.workStationAndRouteSelect.showFlag = false;
            } else {
              this.$modal.notifyError(res_add.message);
            }
          });
        } else {
          this.$notify.error({
            title: "提示",
            message: res.message,
          });
        }
      });
    },
    // 删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.routeProcessAndWorkStationId,
          delRouteProcessAndWorkStation,
          this.getList,
          "路线编码为" + row.code
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
  },
};
</script>

<style lang="scss" scoped></style>
