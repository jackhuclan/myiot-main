<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="料仓号" prop="siloCode">
        <el-input
          v-trim
          v-model="queryParams.siloCode"
          placeholder="请输入料仓号"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="位置" prop="location">
        <el-input
          v-trim
          v-model="queryParams.location"
          placeholder="请输入位置"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="ID" prop="id">
        <el-input
          v-trim
          v-model="queryParams.id"
          oninput="value=value.replace(/\D/g,'')"
          @clear="clearQueryParams('id')"
          placeholder="请输入内容纯数字"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>

      <el-form-item label="生料" prop="undrilledItem">
        <el-input
          v-trim
          v-model="queryParams.undrilledItem"
          placeholder="请输入生料"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="熟料" prop="drilledItem">
        <el-input
          v-trim
          v-model="queryParams.drilledItem"
          placeholder="请输入熟料"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="异常" prop="isWarning">
        <el-select
          style="width: 150px"
          v-model="queryParams.isWarning"
          placeholder="请选择"
          @clear="clearQueryParams('isWarning')"
          clearable
        >
          <el-option label="是" :value="true" />
          <el-option label="否" :value="false" />
        </el-select>
      </el-form-item>
      <el-form-item label="存在多个熟料" prop="hasMultipleDrilled">
        <el-select
          style="width: 150px"
          v-model="queryParams.hasMultipleDrilled"
          placeholder="请选择"
          @clear="clearQueryParams('hasMultipleDrilled')"
          clearable
        >
          <el-option label="是" :value="true" />
          <el-option label="否" :value="false" />
        </el-select>
      </el-form-item>
      <el-form-item label="主题" prop="subject">
        <el-input
          v-trim
          v-model="queryParams.subject"
          placeholder="请输入主题"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="创建时间">
        <el-date-picker
          v-model="queryParams.startTime"
          type="datetime"
          placeholder="起始时间"
          :picker-options="pickerCreateStart"
        ></el-date-picker
        >↔
        <el-date-picker
          :picker-options="pickerCreateEnd"
          v-model="queryParams.endTime"
          type="datetime"
          placeholder="结束时间"
        ></el-date-picker>
      </el-form-item>
    </search-form>
    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="info"
          plain
          icon="el-icon-download"
          @click="handleExport"
          :disabled="hasPermi(['device:siloPanelTrace:export'])"
          >导出</el-button
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
      :ref="page"
      v-loading="loading"
      :data="siloPanelTraceList"
      border
      :row-style="tableRowStyle"
    >
      <el-table-column
        label="ID"
        show-overflow-tooltip
        key="id"
        prop="id"
        fixed="left"
        align="center"
        width="100"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <el-tooltip content="点击展示板料信息">
            <el-button
              v-if="scope.row.id"
              type="text"
              @click="handleViewPanek(scope.row.id)"
              :disabled="hasPermi(['device:siloPanelTrace:panel'])"
              class="table-btn"
              >{{ scope.row.id }}</el-button
            >
          </el-tooltip>
        </template>
      </el-table-column>
      <el-table-column
        label="料仓号"
        show-overflow-tooltip
        key="siloCode"
        prop="siloCode"
        min-width="120"
        fixed="left"
        v-if="columns[1].visible"
      ></el-table-column>
      <el-table-column
        label="位置"
        show-overflow-tooltip
        key="location"
        prop="location"
        min-width="120"
        fixed="left"
        v-if="columns[2].visible"
      />
      <el-table-column
        label="主题"
        show-overflow-tooltip
        key="subject"
        prop="subject"
        min-width="210"
        v-if="columns[3].visible"
      />
      <el-table-column
        label="生料"
        show-overflow-tooltip
        key="undrilledItem"
        prop="undrilledItem"
        min-width="160"
        v-if="columns[4].visible"
      />
      <el-table-column
        label="熟料"
        show-overflow-tooltip
        key="drilledItem"
        prop="drilledItem"
        min-width="160"
        v-if="columns[5].visible"
      />
      <el-table-column
        label="汇总信息"
        key="siloSummary"
        prop="siloSummary"
        min-width="180"
        v-if="columns[6].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.siloSummary" />
        </template>
      </el-table-column>
      <el-table-column
        label="异常"
        key="isWarning"
        prop="isWarning"
        width="100"
        align="center"
        v-if="columns[7].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="!scope.row.isWarning">否</el-tag>
          <el-tag v-else type="danger">是</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="异常信息"
        key="warningDescription"
        prop="warningDescription"
        min-width="180"
        v-if="columns[8].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.warningDescription" />
        </template>
      </el-table-column>
      <el-table-column
        label="上次记录"
        show-overflow-tooltip
        key="referenceRecordId"
        prop="referenceRecordId"
        align="center"
        width="80"
        v-if="columns[9].visible"
      >
        <template slot-scope="scope">
          <el-tooltip content="点击展示板料信息">
            <el-button
              v-if="scope.row.id"
              type="text"
              @click="handleViewPanek(scope.row.referenceRecordId)"
              :disabled="hasPermi(['device:siloPanelTrace:panel'])"
              class="table-btn"
              >{{ scope.row.referenceRecordId }}</el-button
            >
          </el-tooltip>
        </template>
      </el-table-column>
      <el-table-column
        label="调度记录"
        key="scheduleId"
        prop="scheduleId"
        width="80"
        align="center"
        v-if="columns[10].visible"
      >
        <template slot-scope="scope">
          <el-tooltip content="点击展示详细">
            <el-button
              v-if="scope.row.scheduleId"
              type="text"
              @click="handleViewScheduling(scope.row.scheduleId)"
              :disabled="hasPermi(['device:siloPanelTrace:schedule'])"
              class="table-btn"
              >{{ scope.row.scheduleId }}</el-button
            >
          </el-tooltip>
        </template>
      </el-table-column>
      <el-table-column
        label="料仓任务"
        key="transportationTaskId"
        prop="transportationTaskId"
        width="80"
        align="center"
        v-if="columns[11].visible"
      >
        <template slot-scope="scope">
          <el-tooltip content="点击展示详细">
            <el-button
              v-if="scope.row.transportationTaskId"
              type="text"
              @click="handleViewSilo(scope.row.transportationTaskId)"
              :disabled="hasPermi(['device:siloPanelTrace:transportationTask'])"
              class="table-btn"
              >{{ scope.row.transportationTaskId }}</el-button
            >
          </el-tooltip>
        </template>
      </el-table-column>
      <el-table-column
        label="创建时间"
        align="center"
        key="createTime"
        prop="createTime"
        width="160"
        v-if="columns[12].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.createTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="创建人"
        align="center"
        key="creatorName"
        prop="creatorName"
        width="120"
        v-if="columns[13].visible"
      ></el-table-column>
    </el-table>

    <pagination
      v-show="total > 0"
      :total="total"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
    />

    <silo-drawer ref="silo_drawer" :detailId="siloId" />
    <schedulement-drawer
      ref="schedulement_drawer"
      :detailId="recordId"
      isPage="schedulement"
    />
    <!-- 添加或修改料仓任务对话框 -->
    <edit-form-dialog
      v-model="open"
      :title="title"
      optType="view"
      @submitForm="() => {}"
    >
      <el-form ref="form" :model="form" label-width="100px" disabled>
        <el-row>
          <el-col :span="8">
            <el-form-item label="料仓号" prop="siloCode">
              <el-input v-model="form.siloCode" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="创建人" prop="creatorName">
              <el-input v-model="form.creatorName" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="创建时间" prop="createTime">
              <el-date-picker
                style="width: 205px"
                v-model="form.createTime"
                type="datetime"
                placeholder="创建时间"
              ></el-date-picker>
            </el-form-item>
          </el-col>
        </el-row>

        <el-form-item label="异常" prop="isWarning">
          <el-radio-group v-removeAriaHidden v-model="form.isWarning">
            <el-radio :label="true">是</el-radio>
            <el-radio :label="false">否</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="异常信息" prop="warningDescription">
          <el-input
            type="textarea"
            v-model="form.warningDescription"
            :autosize="{ minRows: 2, maxRows: 10 }"
            resize="none"
          />
        </el-form-item>
        <el-form-item label="汇总信息" prop="siloSummary">
          <el-input
            type="textarea"
            v-model="form.siloSummary"
            :autosize="{ minRows: 2, maxRows: 10 }"
            resize="none"
          />
        </el-form-item>
      </el-form>
      <el-table border :data="payloadPanels" v-loading="detailLoading">
        <el-table-column
          label="层号"
          width="50"
          align="center"
          prop="floorNum"
          show-overflow-tooltip
        />

        <el-table-column
          label="板料编码"
          min-width="220"
          show-overflow-tooltip
          prop="panelCode"
        ></el-table-column>
        <el-table-column
          label="板料类型"
          min-width="110"
          align="center"
          show-overflow-tooltip
          prop="productStatus"
        >
          <template slot-scope="scope">
            <status-tag
              :options="productStatusOptions"
              :status="scope.row.productStatus * 1"
            />
          </template>
        </el-table-column>
        <el-table-column
          label="物料编码"
          min-width="200"
          show-overflow-tooltip
          prop="itemCode"
        ></el-table-column>
        <el-table-column label="每叠块数" align="center" prop="pcs" />
        <el-table-column label="板长" align="center" prop="panelLength" />
        <el-table-column label="板宽" align="center" prop="panelWidth" />
      </el-table>
      <pagination
        v-show="detailTotal > 0"
        :total="detailTotal"
        :page.sync="detailQueryParams.pageNum"
        :limit.sync="detailQueryParams.pageSize"
        @pagination="getDetailList"
      />
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listSiloPanelTrace,
  listSiloPanelTraceDetail,
  getSiloPanelTrace,
} from "@/api/wareHouse/siloPanelTrace";
import SchedulementDrawer from "../components/drawer.vue";
import SiloDrawer from "../components/siloTasksDrawer.vue"; // 物料产品选择

export default {
  name: "SiloPanelTrace",
  components: { SchedulementDrawer, SiloDrawer },

  data() {
    return {
      page: "siloPanelTrace",
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      // 显示搜索条件
      showSearch: true,
      // 主表格数据
      siloPanelTraceList: [],
      //表格行背景色
      tableRowStyle({ row, rowIndex }) {
        // 有异常时背景色高亮
        if (row.isWarning) {
          return { background: "#fef0f0" };
        }
        return "";
      },
      // 总条数
      total: 0,
      // 主表格查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        id: undefined,
        subject: undefined,
        code: undefined,
        siloCode: undefined,
        location: undefined,
        undrilledItem: undefined,
        drilledItem: undefined,
        isWarning: undefined,
        hasMultipleDrilled: undefined,
        // 7天前
        startTime: this.$cache.local.get("siloPanelTrace_queryParams")
          ? JSON.parse(this.$cache.local.get("siloPanelTrace_queryParams"))
              .startTime
          : new Date(
              new Date().setDate(new Date().getDate() - 7)
            ).toLocaleString("sv-SE"),
        endTime: undefined,
      },
      form: {},
      open: false,
      title: "",
      // 板料信息
      payloadPanels: [],
      detailLoading: false,
      detailTotal: 0,
      // 板料信息查询条件
      detailQueryParams: {
        pageNum: 1,
        pageSize: 30,
        masterId: undefined,
        locationCode: undefined,
        siloCode: undefined,
        itemCode: undefined,
        panelCode: undefined,
        productStatus: undefined,
      },
      // 层数
      productStatusOptions: this.$status.productStatusOptions.map((v) => {
        return {
          ...v,
          name: v.label + (v.value != -1 ? "(" + v.value + ")" : ""),
        };
      }),
      //.限制结束时间必须大于等于开始时间
      pickerCreateEnd: {
        disabledDate: (time) => {
          if (this.queryParams.startTime) {
            return (
              time.getTime() <=
              new Date(this.queryParams.startTime).getTime() - 86400000
            );
          }
        },
      },
      // 限制开始日期必须小于结束时间或当前日期
      pickerCreateStart: {
        disabledDate: (time) => {
          if (this.queryParams.endTime) {
            return (
              time.getTime() > Date.now() ||
              time.getTime() >
                new Date(this.queryParams.endTime).getTime() -
                  8.64e6 /*开始日期要在选择的结束日期之前 若结束日期大于当前日期 则开始日期为小于当前日期*/
            );
          }
        },
      },
      // 列信息
      columns: [
        { key: 0, label: "ID", visible: true },
        { key: 1, label: "料仓号", visible: true },
        { key: 2, label: "位置", visible: true },
        { key: 3, label: "主题", visible: true },
        { key: 4, label: "生料", visible: true },
        { key: 5, label: "熟料", visible: true },
        { key: 6, label: "汇总信息", visible: true },
        { key: 7, label: "是否有异常", visible: true },
        { key: 8, label: "异常信息", visible: true },
        { key: 9, label: "上次记录", visible: true },
        { key: 10, label: "调度记录", visible: true },
        { key: 11, label: "料仓任务", visible: true },
        { key: 12, label: "创建时间", visible: true },
        { key: 13, label: "创建人", visible: true },
      ],
      // 调度记录id
      recordId: null,
      //料仓任务id
      siloId: null,
    };
  },
  computed: {
    queryParamsChange() {
      return { ...this.queryParams };
    },
  },
  watch: {
    queryParamsChange: {
      handler(val) {
        this.$cache.local.set(
          "siloPanelTrace_queryParams",
          JSON.stringify({
            startTime: val.startTime,
          })
        );
      },
      deepL: true,
      immediate: true,
    },
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  methods: {
    /** 查询设备列表 */
    async getList(isSearch) {
      if (!this.queryParams.startTime && !this.queryParams.endTime)
        return this.$modal.msgWarning("请填写创建时间的起始或结束再进行搜索!");
      this.loading = true;
      const res = await listSiloPanelTrace(this.queryParams);
      if (res.code == 1) return;
      this.siloPanelTraceList = res.data?.list;
      this.total = res.data?.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
    },
    // 表单重置
    reset() {
      this.form = {};
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.queryParams = {
        pageNum: 1,
        pageSize: 10,
        id: undefined,
        subject: undefined,
        code: undefined,
        siloCode: undefined,
        location: undefined,
        undrilledItem: undefined,
        drilledItem: undefined,
        isWarning: undefined,
        hasMultipleDrilled: undefined,
        // 7天前
        startTime: new Date(
          new Date().setDate(new Date().getDate() - 7)
        ).toLocaleString("sv-SE"),
        endTime: undefined,
      };
      this.handleQuery();
    },
    // 板料信息
    handleViewPanek(id) {
      getSiloPanelTrace(id).then((res) => {
        if (res.code == 0) {
          this.title = "板料信息(ID-" + id + ",位置-" + res.data.location + ")";
          this.detailQueryParams.masterId = id;
          this.detailQueryParams.pageNum = 1;
          this.open = true;
          this.getDetailList();
          this.form = res.data;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 获取料仓板料追溯详细信息
    getDetailList() {
      this.detailLoading = true;
      listSiloPanelTraceDetail(this.detailQueryParams).then((res) => {
        if (res.code == 1) return;
        this.payloadPanels = res.data?.list.map((v) => {
          return { ...v, floorNum: v.floorNum + 1 };
        });
        this.detailTotal = res.data?.total;
        this.detailLoading = false;
      });
    },
    // 调度记录
    handleViewScheduling(id) {
      this.recordId = id;
      this.$refs.schedulement_drawer.getDetail(this.recordId);
    },
    // 料仓任务
    handleViewSilo(id) {
      this.siloId = id;
      this.$refs.silo_drawer.getDetail(this.siloId);
    },
    /** 导出按钮操作 */
    handleExport() {
      this.exportExcel(
        "/v1/SiloPanelTrace/DownLoadList",
        "料仓板料追溯.xlsx",
        this.queryParams
      );
    },
  },
};
</script>
<style lang="scss" scoped>
.table-btn {
  user-select: unset;
}
.table-btn.is-disabled {
  color: #606266;
}
</style>
