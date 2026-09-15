<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="设备编码" prop="deviceCode">
        <el-input
          v-trim
          v-model="queryParams.deviceCode"
          placeholder="请输入设备编码"
          clearable
          @keyup.enter.native="handleQuery"
        /> </el-form-item
      ><el-form-item label="开始日期">
        <el-date-picker
          v-model="queryParams.queryStartTime"
          type="date"
          placeholder="开始日期"
        >
        </el-date-picker>
      </el-form-item>
      <el-form-item label="结束日期">
        <el-date-picker
          v-model="queryParams.queryEndTime"
          type="date"
          placeholder="结束日期"
        >
        </el-date-picker>
      </el-form-item>
      <el-form-item label="班次" prop="sailings">
        <el-select
          v-model="queryParams.sailings"
          placeholder="请选择"
          style="width: 150px"
          @clear="clearQueryParams('sailings')"
          clearable
        >
          <el-option label="白班" :value="0" />
          <!-- <el-option label="中班" :value="1" /> -->
          <el-option label="晚班" :value="2" />
        </el-select>
      </el-form-item>
    </search-form>
    <el-collapse v-model="activeNames" class="my-collapse">
      <el-collapse-item name="设备稼动率">
        <span class="collapse-title" slot="title">设备稼动率</span>
        <div>
          <el-table style="width: 100%" :ref="page" :data="analysisList" border>
            <el-table-column
              show-overflow-tooltip
              label="设备编码"
              prop="deviceCode"
              key="deviceCode"
              fixed="left"
              min-width="120"
            >
              <template slot-scope="scope">
                <span
                  class="click_code"
                  :data-id="scope.row.id"
                  v-isGetSelection="['masterData:client:view']"
                  >{{ scope.row.deviceCode }}</span
                >
              </template>
            </el-table-column>
            <el-table-column
              show-overflow-tooltip
              label="工艺路线"
              prop="routeCode"
              key="routeCode"
            />
            <el-table-column
              label="班次"
              align="center"
              key="sailings"
              min-width="150"
              prop="sailings"
            >
              <template slot-scope="scope">
                <span v-if="scope.row.sailings == 2"
                  >{{
                    parseTime(scope.row.createTime, "{y}-{m}-{d} ")
                  }}-晚班</span
                >
                <span v-else-if="scope.row.sailings === 0"
                  >{{
                    parseTime(scope.row.createTime, "{y}-{m}-{d} ")
                  }}-白班</span
                >
              </template>
            </el-table-column>

            <el-table-column
              label="实际稼动率(%)"
              min-width="120"
              align="center"
              key="duty"
              prop="duty"
            >
              <template slot-scope="scope">
                <span :style="{ color: getDutyColor(scope.row.duty) }">{{
                  scope.row.duty
                }}</span>
              </template>
            </el-table-column>
            <el-table-column
              label="开机时长(分钟)"
              min-width="120"
              align="center"
              key="openTime"
              prop="openTime"
            >
            </el-table-column>

            <el-table-column
              label="待机时长(分钟)"
              align="center"
              min-width="120"
              key="waitTime"
              prop="waitTime"
            >
            </el-table-column>

            <el-table-column
              label="异常时长(分钟)"
              align="center"
              min-width="120"
              key="errorTime"
              prop="errorTime"
            >
              <template slot-scope="scope">
                <span style="color: red">{{ scope.row.errorTime }}</span>
              </template>
            </el-table-column>
            <el-table-column label="生产日期"> </el-table-column>
          </el-table>

          <pagination
            v-show="total > 0"
            :total="total"
            :page.sync="queryParams.pageNum"
            :layout="'total,sizes, prev, pager, next'"
            :limit.sync="queryParams.pageSize"
            @pagination="getList"
          />
        </div>
      </el-collapse-item>
      <el-collapse-item name="设备异常">
        <span class="collapse-title" slot="title">设备异常</span>
        <div>
          <el-table
            style="width: 100%"
            :ref="page"
            :data="analysisDetailList"
            border
          >
            <el-table-column
              min-width="120"
              show-overflow-tooltip
              label="设备编码"
              prop="deviceId"
              fixed="left"
              align="center"
              key="deviceId"
            >
            </el-table-column>

            <el-table-column
              label="异常原因"
              min-width="120"
              show-overflow-tooltip
              prop="reasonDesc"
              key="reasonDesc"
            >
              <template slot-scope="scope">
                <tooltip :value="scope.row.reasonDesc" />
              </template>
            </el-table-column>
            <el-table-column
              label="异常开始时间"
              prop="startTime"
              min-width="180"
              align="center"
              key="startTime"
            >
            </el-table-column>
            <el-table-column
              label="异常结束时间"
              key="endTime"
              prop="endTime"
              min-width="180"
              align="center"
            />
            <el-table-column
              label="间隔时长(分钟)"
              min-width="120"
              align="center"
              key="intervalTime"
              prop="intervalTime"
            />
            <el-table-column
              min-width="150"
              key="dateSailings"
              prop="dateSailings"
              label="班次"
              align="center"
            >
            </el-table-column>
          </el-table>

          <pagination
            v-show="analysisDetailListTotal > 0"
            :total="analysisDetailListTotal"
            :layout="'total, prev, pager, next'"
            :page.sync="queryParams.pageNum"
            :limit.sync="queryParams.pageSize"
            @pagination="getList"
          />
        </div>
      </el-collapse-item>
      <el-collapse-item name="设备调度">
        <span class="collapse-title" slot="title">设备调度</span>
        <schedulementTable ref="schedulementTable" />
      </el-collapse-item>
    </el-collapse>
  </div>
</template>

<script>
import {
  getSummaryList,
  getRateReasonDetails,
} from "@/api/device/deviceRecords";
import { listRoute } from "@/api/produce/route";
import schedulementTable from "./schedulementTable.vue";
export default {
  // 稼动率分析
  name: "Analysis",
  components: { schedulementTable },
  data() {
    return {
      page: "analysis",
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      analysisDetailListTotal: 0,
      // 事件表格数据
      selectedAnalysisId: null,
      routeList: [],
      analysisList: [],
      activeNames: ["设备稼动率"],
      analysisDetailList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        deviceCode: undefined,
        // 默认当前日期前一天
        queryStartTime: this.getNextDate(new Date(), -1),
        queryEndTime: undefined,
        // 数据日期倒序
        queryOrderBy: 2,
        routeCodeList: [],
        sailings: undefined,
      },
    };
  },
  activated() {
    this.getList();
  },

  methods: {
    // 查询所有已审批工艺路线
    getRouteList() {
      listRoute({ pageNum: 1, pageSize: 1000, vettingStatus: 1 }).then(
        (res) => {
          this.routeList = res.data.list;
        }
      );
    },
    /** 查询列表 */
    async getList(isSearch) {
      const res = await getSummaryList(this.queryParams);
      this.analysisList = res.data.list;
      this.total = res.data.total;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
    },

    /** 搜索按钮操作 */
    handleQuery() {
      if (
        this.queryParams.deviceCode != undefined &&
        this.queryParams.deviceCode != "" &&
        this.queryParams.sailings != undefined &&
        this.queryParams.queryStartTime != undefined &&
        this.queryParams.queryEndTime != undefined
      ) {
        this.queryParams.pageNum = 1;
        this.getList("search");
      } else {
        this.$modal.msgError("请确保搜索条件全部填写!");
      }
    },
    /** 重置按钮操作 */
    resetQuery() {
      // 默认当前日期前一天
      this.queryParams.queryStartTime = this.getNextDate(new Date(), -1);
      this.queryParams.queryEndTime = undefined;
      this.handleQuery();
    },

    // 点击查看
    handleView(id) {
      this.activeNames.push("设备异常", "设备调度");
      this.selectedAnalysisId = id;
      const deviceCode = this.analysisList.find((v) => v.id == id)?.deviceCode;
      const createTime = this.analysisList.find((v) => v.id == id)?.createTime;
      const routeCode = this.analysisList.find((v) => v.id == id)?.routeCode;
      getRateReasonDetails(id).then((res) => {
        this.analysisDetailList = res.data;
        this.analysisDetailListTotal = res.data?.length;
      });
      this.$refs.schedulementTable.queryParams.sourceDeviceId = deviceCode;
      this.$refs.schedulementTable.queryParams.startTime = createTime;
      this.$refs.schedulementTable.queryParams.routeCodeList = [routeCode];
      this.$refs.schedulementTable.changeSetInterval();
    },
  },
};
</script>
<style lang="scss" scoped>
.collapse-title {
  flex: 1 0 90%;
  order: 1;
  font-size: 16px;
  font-weight: bold;
}

.my-collapse {
  .el-collapse-item {
    width: calc(100%);
  }

  ::v-deep .el-collapse-item__header {
    flex: 1 0 auto;
    order: -1;
    padding-left: 4px;
  }
  ::v-deep .el-collapse-item__content {
    background-color: rgb(240, 242, 245);
    padding-bottom: 0 !important;
  }
}
</style>