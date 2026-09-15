<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="网关名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
    </search-form>

    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['device:deviceGateway:add'])"
          >新增</el-button
        >
      </el-col>
      <right-toolbar
        :showSearch.sync="showSearch"
        @queryTable="getList"
        :columns="columns"
        :page="page"
      ></right-toolbar>
    </el-row>

    <el-table :ref="page" v-loading="loading" :data="deviceGatewayList" border>
      <el-table-column
        label="网关名称"
        key="name"
        prop="name"
        show-overflow-tooltip
        v-if="columns[0].visible"
        min-width="120px"
      />
      <el-table-column
        label="应用名称"
        key="appName"
        prop="appName"
        show-overflow-tooltip
        v-if="columns[1].visible"
        min-width="120px"
      />
      <el-table-column
        label="当前版本"
        align="center"
        key="cVersion"
        prop="cVersion"
        show-overflow-tooltip
        min-width="80"
        v-if="columns[2].visible"
      />
      <el-table-column
        min-width="200px"
        label="网关地址"
        key="visitWebsite"
        prop="visitWebsite"
        show-overflow-tooltip
        v-if="columns[3].visible"
      />
      <el-table-column
        label="可用版本"
        key="aVersion"
        prop="aVersion"
        align="center"
        min-width="150px"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <el-select
            @visible-change="visibleChange($event, scope.row)"
            v-model="scope.row.aVersion"
            placeholder="请选择"
            :loading="scope.row.loading"
          >
            <el-option
              v-for="item in options"
              :key="item.cVersion"
              :label="item.cVersion"
              :value="item.cVersion"
            >
            </el-option>
          </el-select>
        </template>
      </el-table-column>
      <el-table-column
        label="附加参数"
        key="parameters"
        prop="parameters"
        min-width="180"
        v-if="columns[5].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.parameters" />
        </template>
      </el-table-column>
      <el-table-column
        show-overflow-tooltip
        label="安装时间"
        key="setupTime"
        align="center"
        min-width="180"
        v-if="columns[6].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.setupTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        min-width="220px"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <el-button
            type="text"
            @click="handleCheck(scope.row)"
            :disabled="hasPermi(['device:deviceGateway:check'])"
            icon="el-icon-refresh"
          >
            检查更新</el-button
          >
          <el-button
            type="text"
            :disabled="hasPermi(['device:deviceGateway:install'])"
            @click="handleInstall(scope.row)"
          >
            <svg-icon icon-class="install" style="margin-right: 5px" />
            安装
          </el-button>

          <el-dropdown
            trigger="click"
            @command="(command) => handleCommand(command, scope.row)"
          >
            <span :class="'el-dropdown-link ' + $store.getters.size">
              <i class="el-icon-d-arrow-right el-icon--right"></i>更多
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                command="handleUpdate"
                :disabled="hasPermi(['device:deviceGateway:edit'])"
                icon="el-icon-edit"
                >修改</el-dropdown-item
              >
              <el-dropdown-item
                command="handleDelete"
                :disabled="hasPermi(['device:deviceGateway:remove'])"
                icon="el-icon-delete"
                >删除</el-dropdown-item
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
    <!-- 修改参数弹框 -->
    <edit-form-dialog
      v-model="open"
      :title="title"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form ref="form" :model="form" label-width="100px" :rules="rules">
        <el-row>
          <el-col :span="12">
            <el-form-item label="网关名称" prop="name">
              <el-input
                v-model="form.name"
                placeholder="请输入网关名称"
              ></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="应用名称" prop="appName">
              <el-select
                v-model="form.appName"
                placeholder="应用名称"
                clearable
              >
                <!-- <el-option label="agv" value="agv" /> -->
                <el-option label="drill" value="drill" />
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="网关IP" prop="ip">
              <el-input
                v-model="form.ip"
                placeholder="请输入网关IP"
                @input="changeIp"
              ></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="网关地址" prop="visitWebsite">
              <el-input
                disabled
                v-model="form.visitWebsite"
                placeholder="请输入网关地址"
              ></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item label="附加参数" prop="parameters">
          <!-- <b-code-editor
            v-if="open"
            ref="editor"
            v-model="form.parameters"
            :indent-unit="4"
        /> -->
          <vue-json-editor
            v-model="form.parameters"
            :showBtns="false"
            :mode="'code'"
            style="height: 500px"
            lang="zh"
            @json-save="onJsonSave"
            @json-change="onJsonChange"
            @has-error="onError"
          />
        </el-form-item>
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listDeviceGateway,
  addDeviceGateway,
  updateDeviceGateway,
  delDeviceGateway,
  getDeviceGateway,
  getDeviceGatewayVerson,
  installPackages,
} from "@/api/device/deviceGateway";
import vueJsonEditor from "vue-json-editor-fix-cn";

export default {
  name: "DeviceGateway", 
  components: { vueJsonEditor },
  data() {
    const parametersRules = (rule, value, callback) => {
      if (!this.hasJsonFlag) {
        callback();
      } else {
        callback(new Error("格式不正确"));
      }
    };
    return {
      page: "onlineDevice",
      optType: undefined,
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 设备表格数据
      deviceGatewayList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: undefined,
        name: undefined,
        cVersion: undefined,
        aVersion: undefined,
        visitWebsite: undefined,
        setupTime: undefined,
        parameters: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      hasJsonFlag: false,
      // 表单校验
      rules: {
        name: [
          { required: true, message: "网关名称不能为空", trigger: "blur" },
        ],
        appName: [
          { required: true, message: "应用名称不能为空", trigger: "blur" },
        ],
        ip: [{ required: true, message: "网关IP不能为空", trigger: "blur" }],
        visitWebsite: [
          {
            type: "url",
            pattern: /http(s)?:\/\/([\w-]+\.)+[\w-]+(\/[\w- .\/?%&=]*)?/,
            required: true,
            message: "不合法,请重输网关IP",
            trigger: "change",
          },
        ],
        parameters: [
          {
            validator: parametersRules,
            trigger: "change",
          },
        ],
      },
      options: [],
      value: "",
      // 列信息
      columns: [
        { key: 0, label: "网关名称", visible: true },
        { key: 1, label: "应用名称", visible: true },
        { key: 2, label: "当前版本", visible: true },
        { key: 3, label: "网关地址", visible: true },
        { key: 4, label: "可用版本", visible: true },
        { key: 5, label: "附加参数", visible: true },
        { key: 6, label: "安装时间", visible: true },
      ],
      visitWebsite: "",
    };
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      const parameters = JSON.stringify(this.form.parameters);
      const initialFormParameters = JSON.stringify(this.initialForm.parameters);
      const isParameters = parameters !== initialFormParameters;
      const form = { ...this.form };
      delete form.parameters;
      const initialForm = { ...this.initialForm };
      delete initialForm.parameters;
      const isForm = JSON.stringify(form) !== JSON.stringify(initialForm);
      return isParameters || isForm || this.hasJsonFlag;
    },
  },
  methods: {
    onJsonChange(value) {
      // 实时保存
      this.onJsonSave(value);
    },
    onJsonSave(value) {
      this.hasJsonFlag = false;
      this.form.parameters = value;
    },
    onError(value) {
      this.hasJsonFlag = true;
    },
    /** 查询设备列表 */
    async getList(isSearch) {
      this.queryParams.name = this.queryParams.name?.trim();
      this.loading = true;
      listDeviceGateway(this.queryParams).then((res) => {
        this.deviceGatewayList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
      });
    },
    // 表单重置
    reset() {
      this.form = {
        visitWebsite: "",
        name: "",
        appName: "",
        ip: undefined,
        parameters: {},
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
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.ids = selection.map((item) => item.id);
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleCheck":
          this.handleCheck(row);
          break;
        case "handleUpdate":
          this.handleUpdate(row);
          break;

        case "handleDelete":
          this.handleDelete(row);
          break;
        default:
          break;
      }
    },
    //点击安装
    handleInstall(row) {
      const aVersion = row.aVersion ? row.aVersion : row.cVersion;
      if (!aVersion)
        return this.$modal.msgError("当前安装版本为空，请重新选择");

      this.$modal
        .confirm("请确定是否安装版本为" + aVersion + "的应用？")
        .then((res) => {
          if (res == "confirm") {
            const taskLoading = this.$loading({
              lock: true,
              text: "正在安装" + aVersion + "版本,请稍候...",
              spinner: "el-icon-loading",
              background: "rgba(0, 0, 0, 0.7)",
            });
            installPackages({
              id: row.id,
              aVersion,
            })
              .then((res) => {
                if (res.code == 0) {
                  taskLoading.close();
                  this.$modal.msgSuccess("安装成功");
                  this.getList();
                } else {
                  taskLoading.close();
                  this.$modal.notifyError(res.message);
                }
              })
              .catch(() => {
                taskLoading.close();
              });
          }
        })
        .catch(() => {});
    },
    //  检查更新
    handleCheck(row) {
      this.options = [];
      getDeviceGatewayVerson({
        id: row.id,
      }).then((res) => {
        if (res.code == 0) {
          this.options = res.data.list;
          this.$modal.msgSuccess("操作成功");
        } else {
          this.options = [];
          this.$modal.notifyError(res.message);
        }
      });
    },
    visibleChange(v, row) {
      if (v) {
        row.loading = true;
        this.options = [];
        getDeviceGatewayVerson({
          id: row.id,
        }).then((res) => {
          if (res.code == 0) {
            this.options = res.data.list;
          } else {
            this.options = [];
            this.$modal.notifyError(res.message);
          }
          row.loading = false;
        });
      }
    },
    changeIp(val) {
      if (val) {
        this.form.visitWebsite = "http://" + val + ":5257";
      } else {
        if (this.optType == "edit") {
          this.form.visitWebsite = this.visitWebsite;
          this.form.ip = undefined;
        } else {
          this.form.visitWebsite = "";
          this.form.ip = undefined;
        }
      }
    },
    //修改
    handleUpdate(row) {
      this.reset();
      const id = row.id || this.ids;
      getDeviceGateway(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          // this.form.parameters =
          //   res.data.parameters != undefined ? res.data.parameters : "{}";
          this.form.parameters =
            res.data.parameters != undefined
              ? JSON.parse(res.data.parameters)
              : {};
          this.visitWebsite = res.data.visitWebsite;
          const reg = new RegExp(/\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}/);
          if (res.data.visitWebsite.match(reg) != null) {
            this.form.ip = res.data.visitWebsite.match(reg)[0];
          }
          this.initialForm = Object.assign({}, res.data);

          this.optType = "edit";
          this.open = true;
          this.title = "修改网关";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 点击新增
    handleAdd() {
      this.reset();
      this.open = true;
      this.title = "添加网关";
      this.initialForm = Object.assign({}, this.form);
      this.optType = "add";
    },
    // 点击删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.id,
          delDeviceGateway,
          this.getList,
          "网关名称为" + row.name
        );
      } else {
        // this.deleteItem(this.ids, delList, this.getList);
      }
    },
    submitForm() {
      if (this.form.id != undefined) {
        updateDeviceGateway({
          ...this.form,
          parameters: JSON.stringify(this.form.parameters),
        }).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addDeviceGateway({
          ...this.form,
          parameters: JSON.stringify(this.form.parameters),
        }).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("新增成功");
            this.open = false;
            this.queryParams.pageNum = 1;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
  },
};
</script>
<style lang="scss" scoped>
// json数据元素
::v-deep .jv-container .jv-code {
  padding: 0 30px !important;
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

::v-deep .jsoneditor-vue {
  height: 100% !important;
}
/* jsoneditor右上角默认有一个链接,加css去掉了 */
::v-deep .jsoneditor-poweredBy {
  display: none !important;
}
</style>