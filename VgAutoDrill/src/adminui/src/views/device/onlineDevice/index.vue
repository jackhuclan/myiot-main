<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="是否刷新">
        <!-- <el-checkbox v-model="queryParams.checked"></el-checkbox> -->
        <el-switch v-model="queryParams.checked" />
      </el-form-item>
      <el-form-item label="频率" prop="refresh">
        <el-select
          v-model="queryParams.refresh"
          placeholder="请选择"
          @change="handleRefreshSelect"
          style="width: 120px"
        >
          <el-option :label="'3秒'" :value="3" />
          <el-option :label="'5秒'" :value="5" />
          <el-option :label="'10秒'" :value="10" />
        </el-select>
      </el-form-item>
      <el-form-item label="设备编码" prop="deviceId">
        <el-input
          v-trim
          v-model="queryParams.deviceId"
          placeholder="请输入"
          clearable
          @input="handleInput"
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="设备类别" prop="requestDeviceKindList">
        <el-select
          v-model="queryParams.requestDeviceKindList"
          placeholder="请选择"
          multiple
          collapse-tags
          :class="
            queryParams.requestDeviceKindList &&
            queryParams.requestDeviceKindList.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in $status.deviceKinds"
            :key="item.value"
            :label="item.label"
            :value="item.value"
            v-optionTitle
          />
        </el-select>
      </el-form-item>
      <el-form-item label="是否自动" prop="isAuto">
        <el-select
          @clear="clearQueryParams('isAuto')"
          v-model="queryParams.isAuto"
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option label="自动" :value="true" />
          <el-option label="非自动" :value="false" />
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

    <el-table :ref="page" :data="schedulementList" border>
      <el-table-column
        label="设备"
        key="deviceId"
        prop="deviceId"
        v-if="columns[0].visible"
        min-width="150px"
      >
        <template slot-scope="scope">
          <el-tooltip
            v-if="scope.row.descriptor.hostAddress"
            v-delTabIndex
            effect="dark"
            :content="scope.row.descriptor.hostAddress"
            placement="bottom"
            popper-class="device_tooltip"
          >
            <span
              class="click_code remark"
              :data-id="scope.row.descriptor.hostAddress"
              v-isGetSelection
              >{{ scope.row.deviceId }}</span
            >
          </el-tooltip>
          <span v-else class="click_code remark">{{ scope.row.deviceId }}</span>
        </template>
      </el-table-column>
      <el-table-column
        key="deviceKind"
        label="设备类别"
        show-overflow-tooltip
        v-if="columns[1].visible"
        min-width="120px"
      >
        <template slot-scope="scope">
          {{
            scope.row.descriptor &&
            $status.deviceKinds.find(
              (v) => v.value == scope.row.descriptor.deviceKind
            ) &&
            $status.deviceKinds.find(
              (v) => v.value == scope.row.descriptor.deviceKind
            ).label
          }}
        </template>
      </el-table-column>
      <el-table-column
        label="是否自动"
        align="center"
        key="IsAuto"
        min-width="100px"
        v-if="columns[6].visible"
      >
        <template slot-scope="scope">
          <el-tag
            v-if="
              scope.row.descriptor &&
              scope.row.descriptor.extra.IsAuto == 'True'
            "
            >自动</el-tag
          >
          <el-tag
            v-else-if="
              scope.row.descriptor &&
              scope.row.descriptor.extra.IsAuto == 'False'
            "
            type="danger"
            >非自动</el-tag
          >
          <span v-else></span>
        </template>
      </el-table-column>
      <el-table-column
        min-width="150px"
        label="对接设备"
        key="targetDevice"
        prop="targetDevice"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />
      <el-table-column
        min-width="150px"
        label="工艺路线"
        key="routeCode"
        prop="routeCode"
        show-overflow-tooltip
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">
          <span v-if="scope.row.productId == 'drill'">
            {{ scope.row.routeCode }}
          </span>
          <span v-if="scope.row.productId == 'agv'">
            {{ scope.row.routeCode }}
            【{{ scope.row.routeName }}】
          </span>
          <span v-else></span>
        </template>
      </el-table-column>

      <el-table-column
        show-overflow-tooltip
        label="登录时间"
        align="center"
        key="loginTime"
        min-width="180"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.loginTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        min-width="150px"
        label="空闲"
        key="notActive"
        prop="notActive"
        show-overflow-tooltip
        align="center"
        v-if="columns[5].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.notActive && scope.row.productId == 'drill'"
            >是</el-tag
          >
          <el-tag
            v-else-if="!scope.row.notActive && scope.row.productId == 'drill'"
            type="danger"
            >否</el-tag
          >
          <span v-else></span>
        </template>
      </el-table-column>

      <el-table-column
        label="状态"
        align="center"
        key="status"
        prop="status"
        show-overflow-tooltip
        min-width="100px"
        v-if="columns[7].visible"
      />
      <el-table-column
        fixed="right"
        label="操作"
        align="center"
        min-width="140px"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <el-button
            type="text"
            icon="el-icon-document"
            @click="handleDetail(scope.row)"
            :disabled="hasPermi(['device:onlineDevice:detail'])"
            >详情</el-button
          >
          <el-dropdown
            v-if="scope.row.productId != 'agv'"
            trigger="click"
            @command="(command) => handleCommand(command, scope.row)"
          >
            <span :class="'el-dropdown-link ' + $store.getters.size">
              <i class="el-icon-d-arrow-right el-icon--right"></i>更多
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                command="handleUnlockIt"
                icon="el-icon-cpu"
                :disabled="
                  hasPermi(
                    ['device:onlineDevice:unlockIt'] ||
                      !scope.row.notActive ||
                      !scope.row.routingKey
                  )
                "
                >解除锁定</el-dropdown-item
              >
              <el-dropdown-item
                command="handleResetSignal"
                icon="el-icon-s-data"
                :disabled="
                  hasPermi(['device:onlineDevice:resetSignal']) ||
                  !scope.row.notActive
                "
                >重置信号</el-dropdown-item
              >
            </el-dropdown-menu>
          </el-dropdown>
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
    <!-- 从右侧弹出抽屉 -->
    <el-drawer
      v-if="detailOpen"
      :title="'详细信息'"
      :visible.sync="detailOpen"
      direction="rtl"
      size="600px"
    >
      <div slot="title" class="drawer_title">
        <span>设备--{{ deviceInfo.deviceId }}</span>
        <i
          v-if="isRefresh"
          class="el-icon-success"
          style="font-size: 20px; color: #67c23c"
        />
        <i v-else class="el-icon-info" style="font-size: 20px" />
      </div>
      <el-collapse accordion>
        <el-collapse-item name="descriptor">
          <span class="collapse-title" slot="title">配置参数</span>
          <json-view
            v-if="deviceInfo.descriptor"
            :data="deviceInfo.descriptor"
            :deep="1"
          />
          <span v-else>暂无数据</span>
        </el-collapse-item>
        <el-collapse-item name="properties">
          <span class="collapse-title" slot="title">动态属性</span>
          <json-view
            v-if="deviceInfo.properties"
            :data="deviceInfo.properties"
            :deep="1"
          />
          <span v-else>暂无数据</span>
        </el-collapse-item>
        <el-collapse-item
          name="payloadPanels"
          v-if="deviceInfo.payloadPanels && deviceInfo.payloadPanels.length > 0"
        >
          <span class="collapse-title" slot="title">板料信息</span>
          <el-table border :data="deviceInfo.payloadPanels">
            <el-table-column
              prop="position"
              align="center"
              width="60"
              label="轴位置"
            >
              <template slot-scope="scope">
                {{ scope.row.position }}
              </template>
            </el-table-column>
            <el-table-column
              prop="layer"
              align="center"
              width="60"
              label="层位置"
            >
              <template slot-scope="scope">
                {{ scope.row.layer + 1 }}
              </template>
            </el-table-column>
            <el-table-column
              show-overflow-tooltip
              prop="panelCode"
              label="板料"
            />
            <el-table-column
              show-overflow-tooltip
              prop="siloCode"
              label="料仓"
            />
            <el-table-column
              prop="itemCode"
              label="物料"
              show-overflow-tooltip
            />
            <el-table-column
              prop="panelWidth"
              align="center"
              label="板宽"
              width="60"
            />
            <el-table-column
              prop="productStatus"
              align="center"
              show-overflow-tooltip
              label="产品"
              width="60"
            />
          </el-table>
        </el-collapse-item>
        <el-collapse-item
          name="cutter"
          v-if="
            deviceInfo.payloadCutterTrays &&
            deviceInfo.payloadCutterTrays.length > 0
          "
        >
          <span class="collapse-title" slot="title">刀具信息</span>
          <el-table border :data="deviceInfo.payloadCutterTrays">
            <el-table-column prop="z" align="center" label="层数">
              <template slot-scope="scope">{{ scope.row.z }}</template>
            </el-table-column>
            <el-table-column prop="indexOnLayer" align="center" label="序号">
              <template slot-scope="scope">{{
                scope.row.indexOnLayer
              }}</template>
            </el-table-column>
            <el-table-column
              prop="siloCode"
              align="center"
              show-overflow-tooltip
              label="料仓"
            >
            </el-table-column>
            <el-table-column
              prop="trayCode"
              align="center"
              show-overflow-tooltip
              label="刀盘"
            >
            </el-table-column>
            <el-table-column
              prop="itemCode"
              label="物料"
              show-overflow-tooltip
            />

            <el-table-column prop="status" align="center" label="状态" />
          </el-table>
        </el-collapse-item>
      </el-collapse>
    </el-drawer>
  </div>
</template>

<script>
import {
  listOnlineDevice,
  cleanLockerData,
  resetAGVSignal,
  getOnlineDeviceInfo,
} from "@/api/device/onlineDevice";
import { mapState } from "vuex";
export default {
  name: "OnlineDevice",
  data() {
    return {
      page: "onlineDevice",
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 设备表格数据
      schedulementList: [],
      deviceInfo: {},
      deviceId: undefined,
      // 重新渲染表格状态
      refreshTable: true,
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 是否显示详情弹框
      detailOpen: false,
      isRefresh: false,
      // 查询参数
      queryParams: this.$cache.local.get("onlineDevice_queryParams")
        ? JSON.parse(this.$cache.local.get("onlineDevice_queryParams"))
        : {
            pageNum: 1,
            pageSize: 10,
            deviceId: undefined,
            status: undefined,
            targetDevice: undefined,
            requestDeviceKindList: [],
            refresh: 10,
            isAuto: undefined,
            checked: false,
          },
      openChecked: true,
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
      // 列信息
      columns: [
        { key: 0, label: "设备", visible: true },
        { key: 1, label: "设备类别", visible: true },
        { key: 2, label: "对接设备", visible: true },
        { key: 3, label: "工艺路线", visible: true },
        { key: 4, label: "登录时间", visible: true },
        { key: 5, label: "空闲", visible: true },
        { key: 6, label: "自动/非自动", visible: true },
        { key: 7, label: "状态", visible: true },
      ],
    };
  },
  computed: {
    // 拿出要监听的属性
    listenChange() {
      const { refresh, checked } = this.queryParams;
      return { refresh, checked };
    },
    ...mapState({
      onlineDevice_detailInterval: (state) =>
        state.personalized.onlineDevice_detailInterval * 1000,
    }),
    queryParamsChange() {
      return { ...this.queryParams };
    },
  },
  watch: {
    queryParamsChange: {
      handler(val) {
        this.$cache.local.set(
          "onlineDevice_queryParams",
          JSON.stringify({ ...val })
        );
      },
      deepL: true,
      immediate: true,
    },
    onlineDevice_detailInterval: {
      handler(val) {
        if (this.detailOpen) {
          this.getDetatilSetInterval(val);
        }
      },
      deepL: true,
      immediate: true,
    },
    detailOpen(val) {
      if (val) {
        this.openChecked = this.queryParams.checked;
        this.getDetatilSetInterval(this.onlineDevice_detailInterval);
        this.queryParams.checked = false;
      } else {
        clearInterval(this.detailTimer);
        this.detailTimer = null;
        this.isRefresh = false;
        this.queryParams.checked = this.openChecked;
      }
    },
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
  directives: {
    delTabIndex: {
      bind(el, binding) {
        // el为绑定的元素，binding为绑定给指令的对象
        el.__vueSetTimeoutIndex__ = setTimeout(() => {
          // 清除当前tabIndex
          el.removeAttribute("tabindex");
          clearTimeout(el.__vueSetTimeoutIndex__);
        }, 0);
      },
      unbind(el) {
        clearTimeout(el.__vueSetTimeoutIndex__);
      },
    },
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  // keep-alive 特有钩子函数 关闭定时器
  deactivated() {
    clearInterval(this.timer);
    this.timer = null;
    this.queryParams.checked = false;
  },
  methods: {
    // 定时器
    changeSetInterval(refresh) {
      this.timer = setInterval(() => {
        setTimeout(() => {
          this.input_trim();
          this.handleQuery(); //调用接口的方法
        }, 0);
      }, refresh * 1000);
    },
    // 去空格
    input_trim() {
      this.queryParams.code = this.queryParams.code?.trim();
    },
    /** 查询设备列表 */
    async getList(isSearch) {
      this.queryParams.deviceId = this.queryParams.deviceId?.trim();
      this.loading = true;
      listOnlineDevice(this.queryParams).then((res) => {
        this.schedulementList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
      });
    },
    // 详情数据获取定时器
    getDetatilSetInterval(val) {
      if (this.detailTimer) {
        clearInterval(this.detailTimer);
        this.detailTimer = null;
      }
      this.detailTimer = setInterval(() => {
        setTimeout(() => {
          this.isRefresh = true;
          this.getDeviceDetatil(); //调用接口的方法
        }, 0);
      }, val);
    },
    // 获取设备详细信息
    getDeviceDetatil() {
      getOnlineDeviceInfo(this.deviceId).then((res) => {
        if (res.code == 0) {
          this.isRefresh = true;
          this.deviceInfo = res.data;
        } else {
          this.$modal.notifyError(res.message);
        }
        setTimeout(() => {
          this.isRefresh = false;
        }, 1500);
      });
    },
    // 表单重置
    reset() {
      this.form = {
        id: undefined,
        name: undefined,
        code: undefined,
      };
      this.resetForm("form");
    },
    // 解决v-model不回显 监听不到变化
    handleInput(val) {
      this.queryParams.deviceId = val;
      this.$forceUpdate();
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
        deviceId: undefined,
        status: undefined,
        targetDevice: undefined,
        requestDeviceKindList: [],
        refresh: 10,
        isAuto: undefined,
        checked: false,
      };
      this.handleQuery();
    },
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.ids = selection.map((item) => item.id);
    },
    // 切换刷新频率
    handleRefreshSelect(refresh) {
      clearInterval(this.timer);
      this.timer = null;
    },
    // 解除锁定
    handleUnlockIt(row) {
      cleanLockerData(row).then((res) => {
        if (res == "") {
          this.$modal.msgSuccess("已下发");
        } else {
          this.$modal.msgError(res);
        }
      });
    },

    // 重置信号
    handleResetSignal(row) {
      resetAGVSignal(row).then((res) => {
        if (res == "") {
          this.$modal.msgSuccess("已下发");
        } else {
          this.$modal.msgError(res);
        }
      });
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleUnlockIt":
          this.handleUnlockIt(row);
          break;
        case "handleResetSignal":
          this.handleResetSignal(row);
          break;
        default:
          break;
      }
    },
    // 点击详情
    handleDetail(row) {
      this.deviceId = row.deviceId;
      this.detailOpen = true;
      this.getDeviceDetatil();
    },
    // 点击跳转设备控制台
    handleView(url) {
      // http://10.50.114.08:8004
      // http    10.50.114.08    8004
      const http = url.split("//")[0];
      const hostName = url.split("//")[1].split(":")[0];
      const post = url.split(hostName)[1];
      let arr = hostName.split(".");
      arr[arr.length - 1] =
        Number(arr[arr.length - 1]) && !isNaN(arr[arr.length - 1])
          ? arr[arr.length - 1] * 1 + ""
          : arr[arr.length - 1];
      const newUrl = `${http}//${arr.join(".")}${post}`;
      // 跳转链接
      window.open(newUrl, "_blank");
    },
  },
};
</script>
<style lang="scss" scoped>
// json插件样式
::v-deep .json-item {
  padding-left: 1rem !important;
  display: block !important;
}
::v-deep .json-key {
  display: inline !important;
  white-space: normal !important;
  word-break: break-all !important;
}
::v-deep .json-value {
  display: inline !important;
}

.collapse-title {
  flex: 1 0 99%;
  order: 1;
}
::v-deep .el-drawer__body {
  overflow-x: hidden;
}
::v-deep .el-drawer__header {
  height: 40px;
  padding: 0 10px;
  margin: 0;
  .drawer_title {
    height: 20px;
    padding: 0;
    margin: 0;
    display: flex;
    align-items: center;
    span {
      margin-right: 5px;
    }
  }
}
.el-collapse-item__header {
  flex: 1 0 auto;
  order: -1;
}

::v-deep .el-collapse {
  margin: 0 10px;
}
::v-deep .el-collapse {
  border: none;
}
// 自定义表格tooltip
.remark {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  color: #1890ff;
}
</style>
