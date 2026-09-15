<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="设备编码" prop="alarmName">
        <el-input
          v-trim
          v-model="queryParams.alarmName"
          placeholder="请输入设备编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="文件名" prop="alarmName">
        <el-input
          v-trim
          v-model="queryParams.alarmName"
          placeholder="请输入文件名"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="开始时间">
        <el-date-picker
          v-model="queryParams.queryStartTime"
          type="datetime"
          placeholder="开始时间"
        >
        </el-date-picker>
      </el-form-item>
      <el-form-item label="结束时间">
        <el-date-picker
          v-model="queryParams.queryEndTime"
          type="datetime"
          placeholder="结束时间"
        >
        </el-date-picker>
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
          :disabled="hasPermi(['alarm:brokenKnives:export'])"
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
      border
      :ref="page"
      v-loading="loading"
      :data="brokenKnivesInfoList"
    >
      <el-table-column
        label="设备编码"
        key="deviceCode"
        prop="deviceCode"
        min-width="150px"
        fixed="left"
        v-if="columns[0].visible"
        show-overflow-tooltip
      >
      </el-table-column>
      <el-table-column
        label="日期"
        key="date"
        prop="date"
        min-width="120px"
        v-if="columns[1].visible"
      />
      <el-table-column
        label="断刀时间"
        key="time"
        min-width="120px"
        prop="time"
        v-if="columns[2].visible"
      />
      <el-table-column
        label="钻带文件"
        key="file"
        prop="file"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[3].visible"
      />
      <el-table-column
        show-overflow-tooltip
        label="断刀次数"
        key="brokenKnivesNum"
        prop="brokenKnivesNum"
        v-if="columns[4].visible"
      >
      </el-table-column>
      <el-table-column
        label="断刀轴号"
        key="shaftNumber"
        prop="shaftNumber"
        show-overflow-tooltip
        v-if="columns[5].visible"
      />
      <el-table-column
        label="刀具号"
        key="cutterNumber"
        prop="cutterNumber"
        show-overflow-tooltip
        v-if="columns[6].visible"
      />

      <el-table-column
        label="刀径"
        key="knifeDiameter"
        prop="knifeDiameter"
        show-overflow-tooltip
        v-if="columns[7].visible"
      />

      <el-table-column
        label="孔号"
        show-overflow-tooltip
        align="center"
        key="holeNumber"
        prop="holeNumber"
        v-if="columns[8].visible"
      >
      </el-table-column>
      <el-table-column
        label="断刀寿命"
        key="brokenToolLife"
        prop="brokenToolLife"
        show-overflow-tooltip
        v-if="columns[9].visible"
      />
      <el-table-column
        label="X坐标"
        key="xCoordinates"
        prop="xCoordinates"
        show-overflow-tooltip
        v-if="columns[10].visible"
      />
      <el-table-column
        label="Y坐标"
        key="yCoordinates"
        prop="yCoordinates"
        show-overflow-tooltip
        v-if="columns[11].visible"
      />
      <el-table-column
        label="程序块"
        key="blocks"
        prop="blocks"
        show-overflow-tooltip
        v-if="columns[12].visible"
      />
      <el-table-column
        label="程序阶级"
        key="proceduralClass"
        prop="proceduralClass"
        show-overflow-tooltip
        v-if="columns[13].visible"
      />
    </el-table>

    <pagination
      v-show="total > 0"
      :total="total"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
    />
  </div>
</template>
  
  <script>
export default {
  name: "BrokenKnivesInfo",
  data() {
    return {
      page: "brokenKnivesInfo",
      // 遮罩层
      loading: true,
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 事件表格数据
      brokenKnivesInfoList: [],
      // 告警设置查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: undefined,
        name: undefined,
        queryStartTime: undefined,
        queryEndTime: undefined,
      },

      // 列信息
      columns: [
        { key: 0, label: "设备编码", visible: true },
        { key: 1, label: "日期", visible: true },
        { key: 2, label: "断刀时间", visible: true },
        { key: 3, label: "钻带文件", visible: true },
        { key: 4, label: "断刀次数", visible: true },
        { key: 5, label: "断刀轴号", visible: true },
        { key: 6, label: "刀具号", visible: true },
        { key: 7, label: "刀径", visible: true },
        { key: 8, label: "孔号", visible: true },
        { key: 9, label: "断刀寿命", visible: true },
        { key: 10, label: "X坐标", visible: true },
        { key: 11, label: "Y坐标", visible: true },
        { key: 12, label: "程序块", visible: true },
        { key: 13, label: "程序阶级", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },

  methods: {
    /** 查询事件列表 */
    async getList(isSearch) {
      //   this.loading = true;
      //   const res = await listAlarmSetting(this.queryParams);
      this.brokenKnivesInfoList = [
        {
          deviceCode: "设备编号-01",
          date: "日期-01",
          time: "断刀时间-01",
          file: "钻带文件-01",
          brokenKnivesNum: "断刀次数-01",
          shaftNumber: "断刀轴号-01",
          cutterNumber: "刀具号-01",
          knifeDiameter: "刀径-01",
          holeNumber: "孔号-01",
          brokenToolLife: "断刀寿命-01",
          xCoordinates: "X坐标-01",
          yCoordinates: "Y坐标-01",
          blocks: "程序块-01",
          proceduralClass: "程序阶级-01",
        },
        {
          deviceCode: "设备编号-02",
          date: "日期-02",
          time: "断刀时间-02",
          file: "钻带文件-02",
          brokenKnivesNum: "断刀次数-02",
          shaftNumber: "断刀轴号-02",
          cutterNumber: "刀具号-02",
          knifeDiameter: "刀径-02",
          holeNumber: "孔号-02",
          brokenToolLife: "断刀寿命-02",
          xCoordinates: "X坐标-02",
          yCoordinates: "Y坐标-02",
          blocks: "程序块-02",
          proceduralClass: "程序阶级-02",
        },
      ];
      this.total = this.brokenKnivesInfoList.length
      this.loading = false;
      //   // 只有搜索状态下进行提示
      //   if (isSearch == "search") {
      //     this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      //   }
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;

      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.queryParams.queryEndTime = undefined;
      this.queryParams.queryStartTime = undefined;
      this.handleQuery();
    },
    /** 导出按钮操作 */
    handleExport() {
      this.exportExcel(
        "/v1/AlarmEvent/DownLoadList",
        "断刀信息.xlsx",
        this.queryParams
      );
    },
  },
};
</script>
  