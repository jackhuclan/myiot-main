<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="物料名称" prop="itemName">
        <el-input
          v-trim
          v-model="queryParams.itemName"
          placeholder="请输入物料名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="物料编码" prop="itemCode">
        <el-input
          v-trim
          v-model="queryParams.itemCode"
          placeholder="请输入物料编码"
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
          :disabled="hasPermi(['material:atpFile:add'])"
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
          :disabled="hasPermi(['material:atpFile:remove'])"
          >批量删除</el-button
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
      :data="itemAtpFileList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="物料编码"
        min-width="160"
        key="itemCode"
        prop="itemCode"
        fixed="left"
        show-overflow-tooltip
        v-if="columns[0].visible"
      />
      <el-table-column
        label="物料名称"
        min-width="160"
        key="itemName"
        prop="itemName"
        show-overflow-tooltip
        v-if="columns[1].visible"
      />
      <el-table-column
        label="钻带文件"
        min-width="160"
        key="itemDrillFilePath"
        prop="itemDrillFilePath"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />
      <el-table-column
        label="钻带文件名称"
        key="itemDrillFileName"
        prop="itemDrillFileName"
        min-width="160"
        show-overflow-tooltip
        v-if="columns[3].visible"
      />
      <el-table-column
        label="刀具参数"
        min-width="160"
        key="diaFilePath"
        prop="diaFilePath"
        show-overflow-tooltip
        v-if="columns[4].visible"
      />
      <el-table-column
        label="刀具参数名称"
        min-width="160"
        key="diaFileName"
        prop="diaFileName"
        show-overflow-tooltip
        v-if="columns[5].visible"
      />

      <el-table-column
        label="刀盘数量"
        align="center"
        key="diskCount"
        prop="diskCount"
        v-if="columns[6].visible"
      />
      <el-table-column
        label="单刀盘列数"
        align="center"
        key="columnsLimit"
        prop="columnsLimit"
        width="120"
        v-if="columns[7].visible"
      />
      <el-table-column
        label="单刀盘行数"
        align="center"
        key="rowsLimit"
        prop="rowsLimit"
        width="120"
        v-if="columns[8].visible"
      />
      <el-table-column
        label="自动生成"
        align="center"
        key="isGenerated"
        prop="isGenerated"
        v-if="columns[9].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isGenerated == 1">是</el-tag>
          <el-tag v-else type="danger">否</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="排刀文件"
        min-width="160"
        key="atpFilePath"
        prop="atpFilePath"
        show-overflow-tooltip
        v-if="columns[10].visible"
      >
        <template slot-scope="scope">
          <span>{{
            scope.row.atpFilePath != null ? scope.row.atpFilePath : "暂未生成"
          }}</span>
        </template>
      </el-table-column>

      <el-table-column
        label="排刀文件生成时间"
        align="center"
        key="generateTime"
        width="180"
        v-if="columns[11].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.generateTime) }}</span>
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
            icon="el-icon-edit"
            @click="handleUpdate(scope.row)"
            :disabled="hasPermi(['material:atpFile:edit'])"
            >修改</el-button
          >
          <el-dropdown
            trigger="click"
            @command="(command) => handleCommand(command, scope.row)"
          >
            <span :class="'el-dropdown-link ' + $store.getters.size">
              <i class="el-icon-d-arrow-right el-icon--right"></i>更多
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                command="handleRowknives"
                icon="el-icon-s-grid"
                :disabled="hasPermi(['material:atpFile:rowknives'])"
                >排刀</el-dropdown-item
              >
              <el-dropdown-item
                command="downLoadATP"
                icon="el-icon-download"
                :disabled="hasPermi(['material:atpFile:download'])"
                >下载文件</el-dropdown-item
              >
              <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="hasPermi(['material:atpFile:remove'])"
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

    <!-- 添加或修改排刀文件对话框 -->
    <edit-form-dialog
      v-model="open"
      :optType="optType"
      :title="title"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form
        ref="form"
        :model="form"
        :rules="rules"
        label-width="100px"
        v-loading="loading"
      >
        <el-row>
          <el-col :span="12">
            <el-form-item label="钻带文件" prop="itemDrillFilePath">
              <UpLoadFile
                ref="itemDrillFileUpload"
                :onChange="fileChangeItemDrillFilePath"
                :fileList="drillFileList"
                :btnText="'选择钻带文件'"
                :onRemove="onRemoveDrillFileList"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="刀具文件" prop="diaFilePath">
              <UpLoadFile
                ref="diaFileUpload"
                :onChange="fileChangeDiaFilePath"
                :onRemove="onRemoveDiaFileList"
                :fileList="diaFileList"
                :btnText="'选择刀具文件'"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="物料编码" prop="itemCode">
              <el-input v-model="form.itemCode" placeholder="请输入物料编码">
                <el-button
                  v-debounce
                  slot="append"
                  @click="handleSelectProduct"
                  icon="el-icon-search"
                ></el-button>
              </el-input>
              <ItemSelect ref="ItemSelect" @onSelected="onItemSelected">
              </ItemSelect>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="物料名称" prop="itemName">
              <el-input
                v-model="form.itemName"
                readonly
                placeholder="请输入物料名称"
              />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="8">
            <el-form-item label="刀盘数量" prop="diskCount">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.diskCount"
                @changeNum="changeNum"
                :numName="'diskCount'"
                :min="1"
                :max="50"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="单刀盘列数" prop="columnsLimit">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.columnsLimit"
                @changeNum="changeNum"
                :numName="'columnsLimit'"
                :min="1"
                :max="50"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="单刀盘行数" prop="rowsLimit">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.rowsLimit"
                @changeNum="changeNum"
                :numName="'rowsLimit'"
                :min="1"
                :max="50"
              /> </el-form-item
          ></el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
    <!-- 点击刀具打开弹框 -->
    <!-- <my-dialog ref="myDialog" @getList="getList" :atpForm="form"></my-dialog> -->
  </div>
</template>

<script>
// 物料选择
import ItemSelect from "@/components/itemSelect";
import {
  listItemAtpFile,
  getItemAtpFile,
  delItemAtpFile,
  addItemAtpFile,
  updateItemAtpFile,
  upLoad,
  downLoadATPFile,
  delList,
} from "@/api/material/atpFile";
import dialog from "./diaFilePathDialog";
export default {
  name: "AtpFile", 
  components: { "my-dialog": dialog, ItemSelect },
  data() {
    // 自定义校验规则
    const drillFileRole = (rule, value, callback) => {
      if (this.drillFileList.length <= 0) {
        callback(new Error("未选择文件"));
      } else {
        callback();
      }
    };
    const diaFileRule = (rule, value, callback) => {
      if (this.diaFileList.length <= 0) {
        callback(new Error("未选择文件"));
      } else {
        callback();
      }
    };
    return {
      page: "atpFile",
      // 遮罩层
      loading: true,
      optType: "",
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // ATP表格数据
      itemAtpFileList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        itemId: undefined,
        atpFilePath: undefined,
        atpParameters: undefined,
        itemDrillFileId: undefined,
        cutterConfigMasterId: undefined,
        diskCount: undefined,
        columnsLimit: undefined,
        rowsLimit: undefined,
        isGenerated: undefined,
        generateTime: undefined,
        itemName: undefined,
        itemCode: undefined,
      },
      // 修改刀盘等数量时作比较的参数
      diskCount: undefined,
      columnsLimit: undefined,
      rowsLimit: undefined,
      itemAtpFileId: undefined,
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        itemCode: [
          { required: true, message: "物料编码不能为空", trigger: "change" },
        ],
        itemName: [
          { required: true, message: "物料名称不能为空", trigger: "change" },
        ],
        atpFilePath: [
          { required: true, message: "ATP文件路径不能为空", trigger: "blur" },
        ],
        itemDrillFilePath: [
          { required: true, validator: drillFileRole, trigger: "chnage" },
        ],
        columnsLimit: [
          { required: true, message: "单刀盘列数不能为空", trigger: "change" },
        ],
        rowsLimit: [
          { required: true, message: "单刀盘行数不能为空", trigger: "change" },
        ],
        diskCount: [
          { required: true, message: "刀盘数量不能为空", trigger: "change" },
        ],
        isGenerated: [
          { required: true, message: "是否生成不能为空", trigger: "blur" },
        ],
        diaFilePath: [
          { required: true, validator: diaFileRule, trigger: "change" },
        ],
      },
      // 下载文件展示loading
      downloadLoadingInstance: undefined,
      // 刀具文件
      diaFileList: [],
      // 钻带文件
      drillFileList: [],
      // 列信息
      columns: [
        { key: 0, label: "物料编码", visible: true },
        { key: 1, label: "物料名称", visible: true },
        { key: 2, label: "钻带文件", visible: true },
        { key: 3, label: "钻带文件名称", visible: true },
        { key: 4, label: "刀具参数", visible: true },
        { key: 5, label: "刀具参数名称", visible: true },
        { key: 6, label: "刀盘数量", visible: true },
        { key: 7, label: "单刀盘列数", visible: true },
        { key: 8, label: "单刀盘行数", visible: true },
        { key: 9, label: "自动生成", visible: true },
        { key: 10, label: "排刀文件", visible: true },
        { key: 11, label: "排刀文件生成时间", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  watch: {
    drillFileList: {
      handler(val) {
        if (val.length > 0) {
          if (val[0].raw) {
            this.upLoadFile(val[0].raw, upLoad).then((res) => {
              this.form.itemDrillFilePath = res;
              this.form.itemDrillFileName = val[0].name;
            });
          }
        } else {
          this.form.itemDrillFilePath = undefined;
          this.form.itemDrillFileName = undefined;
        }
      },
      deep: true,
    },
    diaFileList: {
      handler(val) {
        if (val.length > 0) {
          if (val[0].raw) {
            this.upLoadFile(val[0].raw, upLoad).then((res) => {
              this.form.diaFilePath = res;
              this.form.diaFileName = val[0].name;
            });
          }
        } else {
          this.form.diaFilePath = undefined;
          this.form.diaFileName = undefined;
        }
      },
      deep: true,
    },
    "form.itemCode": {
      handler(val) {
        if (val == "") {
          this.form.itemId = undefined;
          this.form.itemName = undefined;
          this.form.itemCode = undefined;
          this.form.itemTypeId = undefined;
        }
      },
      deep: true,
    },
    open(val) {
      if (!val) {
        this.onRemoveDrillFileList();
        this.onRemoveDiaFileList();
      }
    },
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
  },
  methods: {
    /** 查询ATP列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listItemAtpFile(this.queryParams);
      this.itemAtpFileList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
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
        itemId: undefined,
        itemCode: undefined,
        itemName: undefined,
        atpFilePath: undefined,
        atpParameters: undefined,
        itemDrillFileId: undefined,
        itemDrillFilePath: undefined,
        diaFileName: undefined,
        diaFilePath: undefined,
        cutterConfigMasterId: undefined,
        diskCount: 6,
        columnsLimit: 5,
        rowsLimit: 10,
        isGenerated: undefined,
        generateTime: undefined,
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

    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.open = true;
      this.optType = "add";
      this.initialForm = Object.assign({}, this.form);
      this.title = "添加排刀文件";
      // 清空文件列表
      this.$nextTick(() => {
        this.$refs.itemDrillFileUpload.$refs.upload.clearFiles();
        this.$refs.diaFileUpload.$refs.upload.clearFiles();
        this.diaFileList = [];
        this.drillFileList = [];
      });
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const ItemAtpFileId = row.id || this.ids;
      getItemAtpFile(ItemAtpFileId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.diskCount = res.data.diskCount;
          this.columnsLimit = res.data.columnsLimit;
          this.rowsLimit = res.data.rowsLimit;
          this.open = true;
          this.optType = "edit";
          this.initialForm = Object.assign({}, res.data);
          this.title = "修改排刀文件";
          // 刀具文件回显
          this.diaFileList = [
            {
              name: res.data.diaFileName,
              url: res.data.diaFilePath,
            },
          ];
          // 钻带文件回显
          this.drillFileList = [
            {
              name: res.data.itemDrillFileName,
              url: res.data.itemDrillFilePath,
            },
          ];
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "downLoadATP":
          this.downLoadATP(row);
          break;
        case "handleRowknives":
          this.handleRowknives(row);
          break;
        case "handleDelete":
          this.handleDelete(row);
          break;
        default:
          break;
      }
    },
    /** 提交按钮 */
    submitForm: async function () {
      if (this.form.id != undefined) {
        //刀盘是否有变化
        if (
          this.form.diskCount != this.diskCount ||
          this.form.columnsLimit != this.columnsLimit ||
          this.form.rowsLimit != this.rowsLimit
        ) {
          const confirmRes = await this.$modal
            .confirm("刀具参数发生了变化，确定修改？")
            .catch(() => {});
          if (confirmRes != "confirm") {
            this.form.diskCount = this.diskCount;
            this.form.columnsLimit = this.columnsLimit;
            this.form.rowsLimit = this.rowsLimit;
            return this.$modal.msg("已取消修改");
          }
          updateItemAtpFile(this.form).then((res) => {
            if (res.code == 0) {
              this.$modal.msgSuccess("修改成功");
              this.open = false;
              this.getList();
            } else {
              this.$modal.notifyError(res.message);
            }
          });
        } else {
          updateItemAtpFile(this.form).then((res) => {
            if (res.code == 0) {
              this.$modal.msgSuccess("修改成功");
              this.open = false;
              this.getList();
            } else {
              this.$modal.notifyError(res.message);
            }
          });
        }
      } else {
        addItemAtpFile(this.form).then((res) => {
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
    // 删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(row.id, delItemAtpFile, this.getList);
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 下载ATP文件
    downLoadATP(row) {
      if (row.atpFilePath == null)
        return this.$modal.msgError("暂未生成ATP文件");
      this.downloadLoadingInstance = this.$loading({
        lock: true,
        text: "正在下载数据，请稍候",
        spinner: "el-icon-loading",
        background: "rgba(0, 0, 0, 0.7)",
      });
      const fileName = row.atpFilePath.substring(
        row.atpFilePath.lastIndexOf("/") + 1
      );
      downLoadATPFile(fileName)
        .then((res) => {
          const blob = new Blob([res]);
          let href = window.URL.createObjectURL(blob); //创建下载的链接
          if (window.navigator.msSaveBlob) {
            // ie 浏览器
            try {
              window.navigator.msSaveBlob(blob, fileName);
            } catch (e) {
              console.log(e);
            }
          } else {
            // 谷歌浏览器 创建a标签 添加download属性下载
            let downloadElement = document.createElement("a");
            if (typeof blob == "string") {
              downloadElement.target = "_blank";
            }
            downloadElement.href = href;
            downloadElement.download = fileName;
            document.body.appendChild(downloadElement);
            downloadElement.click(); //点击下载
            document.body.removeChild(downloadElement); //下载完成移除元素
            if (typeof blob != "string") {
              window.URL.revokeObjectURL(href); //释放掉blob对象
            }
          }
          this.downloadLoadingInstance.close();
        })
        .catch(() => {
          this.downloadLoadingInstance.close();
          this.$modal.msgError("下载失败，请联系管理员");
        });
    },
    //  打开刀具参数弹框
    async handleRowknives(row) {
      // this.$refs.myDialog.queryParams.itemAtpFileId = row.id;
      // this.$refs.myDialog.getList();
      // this.$refs.myDialog.open = true;
      // const res = await getItemAtpFile(row.id);
      // this.$refs.myDialog.form = res.data;
      localStorage.setItem("diaSettingId", row.id);
      this.$router.push({ path: "/material/atp/setting" });
    },
    //物料选择弹出框
    handleSelectProduct() {
      this.$refs.ItemSelect.showFlag = true;
      this.$refs.ItemSelect.selectedItemCode = this.form.itemCode
        ? this.form.itemCode
        : undefined;
      this.$refs.ItemSelect.title = "物料选择";
      this.$refs.ItemSelect.getList();
    },
    onItemSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "itemId", obj.id);
        this.$set(this.form, "itemTypeId", obj.itemTypeId);
        this.$set(this.form, "itemCode", obj.code);
        this.$set(this.form, "itemName", obj.name);
      }
    },

    // 钻带文件变化
    fileChangeItemDrillFilePath(file, fileList) {
      // 这是关键一句
      if (fileList.length > 0) {
        this.drillFileList = [fileList[fileList.length - 1]];
      }
    },
    // 刀具文件变化
    fileChangeDiaFilePath(file, fileList) {
      // 这是关键一句
      if (fileList.length > 0) {
        this.diaFileList = [fileList[fileList.length - 1]];
      }
    },

    // 清空文件
    onRemoveDiaFileList() {
      this.diaFileList = [];
      this.form.diaFilePath = "";
      this.form.diaFileName = "";
    },
    onRemoveDrillFileList() {
      this.drillFileList = [];
      this.form.itemDrillFilePath = "";
      this.form.itemDrillFileName = "";
    },
  },
};
</script>
