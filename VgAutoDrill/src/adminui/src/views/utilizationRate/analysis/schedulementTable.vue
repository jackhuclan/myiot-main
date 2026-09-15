<template>
  <div>
    <el-table
      border
      :key="tableKey"
      :data="schedulementList"
      :ref="page"
      @sort-change="sortChange"
    >
      <el-table-column
        label="记录Id"
        align="center"
        key="id"
        prop="id"
        show-overflow-tooltip
        fixed="left"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['device:schedulement:detail']"
            >{{ scope.row.id }}</span
          ></template
        ></el-table-column
      >
      <el-table-column
        min-width="120px"
        label="发起设备"
        key="sourceDeviceId"
        prop="sourceDeviceId"
        show-overflow-tooltip
        fixed="left"
      >
        <template slot-scope="scope">
          <span v-if="scope.row.subDeviceCode">{{
            scope.row.subDeviceCode
          }}</span>
          <span v-else>{{ scope.row.sourceDeviceId }}</span></template
        >
      </el-table-column>

      <el-table-column
        min-width="120px"
        label="调度设备"
        key="requireDeviceId"
        prop="requireDeviceId"
        fixed="left"
        show-overflow-tooltip
      />
      <el-table-column
        label="交互方式"
        min-width="150px"
        key="interactionSequence"
        prop="interactionSequence"
        show-overflow-tooltip
        align="center"
      >
        <template slot-scope="scope">
          <span :style="{ color: scope.row.isUrgent > 0 ? 'red' : '' }">
            {{ scope.row.isUrgent > 0 ? "紧急 ---" : "" }}
            {{
              $status.interactionSequenceOptions.find(
                (v) => v.value == scope.row.interactionSequence
              )
                ? $status.interactionSequenceOptions.find(
                    (v) => v.value == scope.row.interactionSequence
                  ).label
                : ""
            }}
            {{
              scope.row.requestInteractionBehaviorName
                ? "--- " + scope.row.requestInteractionBehaviorName
                : ""
            }}
          </span>
        </template></el-table-column
      >

      <el-table-column
        label="调度状态"
        align="center"
        key="scheduledTaskStatus"
        prop="scheduledTaskStatus"
        min-width="120px"
      >
        <template slot-scope="scope">
          <status-tag
            :options="$status.schedulementOptions"
            :status="scope.row.scheduledTaskStatus"
          />
        </template>
      </el-table-column>

      <el-table-column
        label="任务编码"
        key="taskId"
        prop="taskId"
        min-width="150px"
        show-overflow-tooltip
      />

      <el-table-column
        label="产品编码"
        key="itemCode"
        prop="itemCode"
        min-width="150px"
        show-overflow-tooltip
      >
        <template slot-scope="scope">
          {{
            scope.row.requestDeviceKind != 2 && scope.row.requestDeviceKind != 3
              ? scope.row.requestSummaryInfo
              : scope.row.itemCode
          }}
        </template></el-table-column
      >
      <el-table-column
        label="工艺路线"
        key="routeCode"
        prop="routeCode"
        min-width="100px"
        show-overflow-tooltip
      />
      <!-- 接口暂无提供 -->
      <!-- <el-table-column
          label="班次"
          min-width="150"
          align="center"
          key="sailings"
          prop="sailings"
          show-overflow-tooltip
          v-if="columns[8].visible"
        >
          <template slot-scope="scope">
            <span v-if="scope.row.sailings == 1"
              >{{ parseTime(scope.row.createTime, "{y}-{m}-{d} ") }}-中班</span
            >
            <span v-if="scope.row.sailings == 2"
              >{{ parseTime(scope.row.createTime, "{y}-{m}-{d} ") }}-晚班</span
            >
            <span v-else-if="scope.row.sailings === 0"
              >{{ parseTime(scope.row.createTime, "{y}-{m}-{d} ") }}-白班</span
            >
          </template>
        </el-table-column>
        <el-table-column
          label="等待时长(分钟)"
          align="center"
          sortable
          min-width="140"
          key="waitTime"
          prop="waitTime"
          v-if="columns[7].visible"
        >
        </el-table-column> -->

      <el-table-column
        label="需求备注"
        key="remark"
        prop="remark"
        min-width="180px"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.remark" />
        </template>
      </el-table-column>
      <el-table-column
        label="呼叫时间"
        align="center"
        key="createTime"
        prop="createTime"
        min-width="180"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.createTime, "{m}-{d} {h}:{i}:{s}")
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="分配时间"
        align="center"
        key="allocateTime"
        prop="allocateTime"
        min-width="180"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.allocateTime, "{m}-{d} {h}:{i}:{s}")
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="开始时间"
        align="center"
        key="runningTime"
        prop="runningTime"
        min-width="180"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.runningTime, "{m}-{d} {h}:{i}:{s}")
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="完成时间"
        align="center"
        key="completedTime"
        prop="completedTime"
        min-width="180"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.completedTime, "{m}-{d} {h}:{i}:{s}")
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="失败时间"
        align="center"
        key="failedTime"
        prop="failedTime"
        min-width="180"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.failedTime, "{m}-{d} {h}:{i}:{s}")
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="取消时间"
        align="center"
        key="canceledTime"
        prop="canceledTime"
        min-width="180"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.canceledTime, "{m}-{d} {h}:{i}:{s}")
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="取消原因"
        key="cancelReason"
        prop="cancelReason"
        min-width="180px"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.cancelReason" />
        </template>
      </el-table-column>

      <el-table-column
        label="板料检验是否OK"
        key="isBarcodeOk"
        align="center"
        min-width="150px"
        prop="isBarcodeOk"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isBarcodeOk">是</el-tag>
          <el-tag type="danger" v-else-if="scope.row.isBarcodeOk == 0"
            >否</el-tag
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

    <my-drawer ref="MyDrawer" :detailId="detailId" isPage="schedulement" />
  </div>
</template>
  
  <script>
import { listSchedulement } from "@/api/device/schedulement";
import drawer from "@/views/device/components/drawer.vue";
import { listRoute } from "@/api/produce/route";
export default {
  name: "Schedulement",
  components: { MyDrawer: drawer },
  data() {
    return {
      tableKey: Math.random(),
      page: "schedulement",
      initWidth: 0,
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 设备表格数据
      schedulementList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        sourceDeviceId: undefined,
        requireDeviceId: undefined,
        id: undefined,
        taskId: undefined,
        itemCode: undefined,
        scheduledTaskStatusList: [],
        requestDeviceKindList: [],
        routeCodeList: [],
        interactionSequenceList: [],
        startTime: undefined,
        endTime: undefined,
        runningStartTime: undefined,
        runningEndTime: undefined,
        isBarcodeOk: undefined,
      },
      // 工艺路线
      routeOptions: [],
      rememberChecked: null,
      // 表单参数
      form: {},
      // 表单校验
      rules: {
        name: [
          { required: true, message: "设备名称不能为空", trigger: "blur" },
        ],
        code: [
          { required: true, message: "设备顺序不能为空", trigger: "blur" },
        ],
      },
      // 定时刷新数据
      timer: null,
      detailId: null,

      // 异常处理
      abnormalId: null,
      requireDeviceId: null,
      sourceDeviceId: null,
    };
  },
  computed: {
    // 拿出要监听的属性
    listenChange() {
      const { refresh, checked } = this.queryParams;
      return { refresh, checked };
    },
  },
  watch: {
    listenChange: {
      // 开启深度监听
      handler(val, old) {
        const { refresh, checked } = val;
        if (checked) {
          if (this.timer) {
            clearInterval(this.timer);
            this.timer = null;
          } else {
            this.changeSetInterval(refresh);
          }
        } else {
          clearInterval(this.timer);
          this.timer = null;
        }
      },
      immediate: true,
      deep: true,
    },
  },
  activated() {
    this.queryParams.checked = true;
    this.getRoute();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  // keep-alive 特有钩子函数 关闭定时器
  deactivated() {
    clearInterval(this.timer);
    this.timer = null;
  },
  methods: {
    // 定时器
    changeSetInterval() {
      this.handleQuery();
      if (this.timer == null) {
        this.timer = setInterval(() => {
          setTimeout(() => { 
            this.handleQuery(); //调用接口的方法
          }, 0);
        }, 5000);
      } else {
        clearInterval(this.timer);
        this.timer = null;
      }
    },

    /** 查询设备列表 */
    async getList(isSearch) {
      this.loading = true;
      listSchedulement(this.queryParams)
        .then((res) => {
          this.schedulementList = res.data.list;
          this.total = res.data.total;
          this.loading = false;
        })
        .catch((error) => {});
    },
    // 查询工艺路线
    async getRoute() {
      const res = await listRoute({
        vettingStatus: 1,
        pageNum: 1,
        pageSize: 100,
      });
      this.routeOptions = res.data.list;
    },

    // 等待时长排序
    sortChange(info) {
      // ascending升序
      // descending降序
      // this.queryParams.order = info.order;
      // this.queryParams.pageNum++;
      // this.getList("search");

      console.log("接口暂时未支持等待时长排序");
    },

    /** 搜索按钮操作 */
    handleQuery() {
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.handleQuery();
    },

    // 点击详情
    handleView(id) {
      this.detailId = id;
      this.$refs.MyDrawer.getDetail(this.detailId);
      clearInterval(this.timer);
      this.timer = null;
    },
  },
};
</script>
  
  <style lang="scss" scoped>
// 时间日期组件
::v-deep .el-date-editor {
  width: 180px !important;
  .el-input__inner {
    width: 180px !important;
    padding: 0 25px 0 25px !important;
  }
}
</style>
  