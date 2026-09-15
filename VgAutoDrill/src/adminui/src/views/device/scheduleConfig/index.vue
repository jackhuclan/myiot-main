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
        />
      </el-form-item>
      <el-form-item label="设备名称" prop="deviceName">
        <el-input
          v-trim
          v-model="queryParams.deviceName"
          placeholder="请输入设备名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>

      <el-form-item label="工艺路线" prop="routeCode">
        <el-select
          v-model="queryParams.routeCode"
          placeholder="请选择"
          @clear="clearQueryParams('routecode')"
          clearable
        >
          <el-option
            v-for="item in routeQueryList"
            :key="item.id"
            :label="item.label"
            :value="item.code"
            v-optionTitle
          />
        </el-select>
      </el-form-item>
      <el-form-item label="设备类别" prop="requestDeviceKindList">
        <el-select
          v-model="queryParams.requestDeviceKindList"
          placeholder="请选择"
          multiple
          collapse-tags
          :popper-append-to-body="false"
          popper-class="hide-select"
          :class="
            queryParams.requestDeviceKindList &&
            queryParams.requestDeviceKindList.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in $status.agvKindList"
            :key="item.value"
            :label="item.label"
            :value="item.value"
            v-optionTitle
          />
        </el-select>
      </el-form-item>
 
    </search-form>
    <el-row :gutter="10" class="mb8">
      <right-toolbar
        :showSearch.sync="showSearch"
        @queryTable="getList"
        :columns="columns"
        :page="page"
      ></right-toolbar>
    </el-row>

    <el-table :ref="page" v-loading="loading" :data="scheduleConfigList" border>
      <el-table-column
        label="设备编码"
        show-overflow-tooltip
        key="code"
        prop="code"
        min-width="120"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          {{ scope.row.code }}
        </template>
      </el-table-column>
      <el-table-column
        label="设备名称"
        show-overflow-tooltip
        key="name"
        prop="name"
        min-width="120"
        v-if="columns[1].visible"
      />
      <el-table-column
        label="设备类别"
        align="center"
        key="deviceKind"
        min-width="120"
        v-if="columns[2].visible"
      >
        <template slot-scope="scope">
          {{
            scope.row.deviceKind &&
            $status.agvKindList.find((v) => v.value == scope.row.deviceKind) &&
            $status.agvKindList.find((v) => v.value == scope.row.deviceKind)
              .label
          }}
        </template>
      </el-table-column>

       
      <el-table-column
        label="工艺路线"
        show-overflow-tooltip
        key="routeNameDescription"
        prop="routeNameDescription"
        v-if="columns[3].visible"
      />
      <el-table-column
        label="创建时间"
        align="center"
        key="createTime"
        prop="createTime"
        width="180"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.createTime) }}</span>
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
            icon="el-icon-cooperation"
            @click="handleUpdate(scope.row)"
            :disabled="hasPermi(['device:scheduleConfig:config'])"
            >配置路线</el-button
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
    <!-- 添加或修改工艺路线对话框 -->
    <el-dialog
      title="配置路线"
      :visible.sync="open"
      width="960px"
      append-to-body
      :close-on-click-modal="false"
      v-dialogClose
      v-dialogDrag
    >
      <el-form
        ref="form"
        :model="form"
        label-width="100px"
        :rules="rules"
        :disabled="optType == 'view'"
      >
        <el-row>
          <el-col :span="12">
            <el-form-item label="设备编码" prop="code">
              <el-input v-model="form.code" disabled placeholder="设备编码" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="设备名称" prop="name">
              <el-input v-model="form.name" disabled placeholder="设备名称" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <el-tabs type="border-card" v-if="form.id != null && open">
        <!-- 关联工艺路线 -->
        <el-tab-pane label="关联工艺路线">
          <DeviceAndRoute
            @getParentList="getList"
            ref="deviceAndRoute"
            :deviceForm="form"
            :optType="optType"
          />
        </el-tab-pane>
      </el-tabs>
    </el-dialog>
  </div>
</template>

<script>
import { getDeviceAndRouteList, getDevice } from "@/api/device/device";
import { getDropSelectDatas } from "@/api/produce/route";
import DeviceAndRoute from "./deviceAndRoute.vue";
export default {
  name: "ScheduleConfig",
  components: { DeviceAndRoute },
  data() {
    return {
      page: "device",
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
      // 设备表格数据
      scheduleConfigList: [],
      routeQueryList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 设备列表查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        deviceCode: undefined,
        deviceName: undefined, 
        deviceTypeId: undefined,
        deviceTypeCode: "agv",
        routeCode: undefined,
        requestDeviceKindList: [],
      },
      // 表单参数
      form: {},
      maintainPeriodDays: "",
      maintainPeriodDaysOptions: [
        {
          value: 1,
          label: "一天",
        },
        {
          value: 2,
          label: "两天",
        },
        {
          value: 3,
          label: "三天",
        },
        {
          value: 4,
          label: "四天",
        },
        {
          value: 5,
          label: "五天",
        },
      ],
      // 表单校验
      rules: {
        name: [
          { required: true, message: "设备名称不能为空", trigger: "blur" },
        ],
        code: [
          { required: true, message: "设备顺序不能为空", trigger: "blur" },
        ],
        status: [
          { required: true, message: "设备状态不能为空", trigger: "blur" },
        ],
      },
      // 设备状态
      formDeviceStatus: "",
      // 列信息
      columns: [
        { key: 0, label: "设备编码", visible: true },
        { key: 1, label: "设备名称", visible: true },
        { key: 2, label: "设备类别", visible: true },
        { key: 3, label: "工艺路线", visible: true },
        { key: 4, label: "创建时间", visible: true },
      ],
    };
  },

  activated() {
    this.getList();
    this.getRouteList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  methods: {
    /** 查询设备列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await getDeviceAndRouteList(this.queryParams);
      this.scheduleConfigList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
    },
    // 查询所有已审批工艺路线
    getRouteList() {
      getDropSelectDatas({ pageNum: 1, pageSize: 1000, vettingStatus: 1 }).then(
        (res) => {
          this.routeQueryList = res.data?.map((v) => {
            return { ...v, label: `${v.code}(${v.name})` };
          });
        }
      );
    },
    // 表单重置
    reset() {
      this.form = {
        id: undefined,
        status: undefined,
        name: undefined,
        code: undefined,
        parentId: undefined,
        ancestors: undefined,
        deviceTypeId: undefined,
        deviceTypeCode: undefined,
        deviceVendorId: undefined,
        deviceBrand: undefined,
        deviceSpec: undefined,
        workStationId: undefined,
        maintainPeriodDays: undefined,
        parameters: undefined,
        deviceStatus: undefined,
      };
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

    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      getDevice(row.id).then((res) => {
        this.form = res.data;
        this.open = true;
        this.$nextTick(() => {
          this.$refs.deviceAndRoute.queryParams.deviceId = res.data.id;
          this.$refs.deviceAndRoute.getList();
        });
      });
    },
  },
};
</script>
