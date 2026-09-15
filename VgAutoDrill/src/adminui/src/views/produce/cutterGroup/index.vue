<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="配刀组编号" prop="cutterGroupNo">
        <el-input
          v-trim
          v-model="queryParams.cutterGroupNo"
          placeholder="请输入配刀组编号"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>

      <el-form-item label="配刀状态" prop="cutterGroupStatus">
        <el-select
          @clear="clearQueryParams('cutterGroupStatus')"
          clearable
          v-model="queryParams.cutterGroupStatus"
          placeholder="请选择"
        >
          <el-option
            v-for="item in cutterGroupStatuList"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
    </search-form>

    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-delete"
          @click="handleDelete"
          :disabled="hasPermi(['produce:cutterGroup:remove'])"
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
      :data="cutterGroupList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="配刀组编号"
        key="groupNo"
        prop="groupNo"
        min-width="180px"
        show-overflow-tooltip
        fixed="left"
        v-if="columns[0].visible"
      >
      </el-table-column>
      <el-table-column
        fixed="left"
        label="钻机编号"
        key="drillNo"
        prop="drillNo"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[1].visible"
      />
      <el-table-column
        label="钻机名称"
        key="drillName"
        prop="drillName"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[2].visible"
      />
      <el-table-column
        label="料号"
        key="itemName"
        prop="itemName"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[3].visible"
      />
      <el-table-column
        label="生产趟数"
        key="roundNum"
        prop="roundNum"
        align="center"
        show-overflow-tooltip
        v-if="columns[4].visible"
      />
      <el-table-column
        label="轴数"
        key="axisCount"
        prop="axisCount"
        align="center"
        show-overflow-tooltip
        v-if="columns[5].visible"
      />
      <el-table-column
        label="尾轮轴数"
        key="endAxisCount"
        prop="endAxisCount"
        align="center"
        show-overflow-tooltip
        v-if="columns[6].visible"
      />
      <el-table-column
        label="配刀状态"
        key="cutterGroupStatus"
        align="center"
        v-if="columns[7].visible"
        min-width="180"
      >
        <template slot-scope="scope">
          <el-tag>{{
            cutterGroupStatuList.find(
              (v) => v.value == scope.row.cutterGroupStatus
            )
              ? cutterGroupStatuList.find(
                  (v) => v.value == scope.row.cutterGroupStatus
                ).label
              : ""
          }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="锁定时间"
        min-width="180px"
        key="lockedTime"
        align="center"
        v-if="columns[8].visible"
      >
        <template slot-scope="scope">
          {{ parseTime(scope.row.lockedTime) }}
        </template>
      </el-table-column>
      <el-table-column
        label="计划要板时间"
        min-width="180px"
        key="plannedTime"
        align="center"
        v-if="columns[9].visible"
      >
        <template slot-scope="scope">
          {{ parseTime(scope.row.plannedTime) }}
        </template>
      </el-table-column>
      <el-table-column
        label="组计划生成时间"
        min-width="180px"
        key="groupDate"
        align="center"
        v-if="columns[10].visible"
      >
        <template slot-scope="scope">
          {{ parseTime(scope.row.groupDate) }}
        </template>
      </el-table-column>
      <el-table-column
        label="创建时间"
        min-width="180px"
        key="createTime"
        v-if="columns[11].visible"
        align="center"
      >
        <template slot-scope="scope">
          {{ parseTime(scope.row.createTime) }}
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
            icon="el-icon-document"
            @click="handleDetail(scope.row)"
            :disabled="hasPermi(['produce:cutterGroup:detail'])"
            >详情</el-button
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
                icon="el-icon-edit"
                :disabled="hasPermi(['produce:cutterGroup:edit'])"
                command="handleUpdate"
                >修改</el-dropdown-item
              >
              <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="hasPermi(['produce:cutterGroup:remove'])"
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

    <!-- 添加或修改配刀组计划对话框 -->
    <edit-form-dialog
      v-model="open"
      :title="title"
      :optType="optType"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form
        ref="form"
        :model="form"
        :rules="rules"
        label-width="100px"
        :disabled="optType == 'view'"
      >
        <el-row>
          <el-col :span="8">
            <el-form-item label="配刀组编号" prop="groupNo">
              <el-input
                v-model="form.groupNo"
                placeholder="请输入配刀组编号"
                disabled
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="钻机编号" prop="drillNo">
              <el-input
                v-model="form.drillNo"
                placeholder="请选择钻机"
                disabled
              >
                <el-button
                  v-debounce
                  slot="append"
                  icon="el-icon-search"
                  @click="handleSelectDevice"
                  disabled
                ></el-button>
              </el-input>

              <deviceSelect ref="deviceSelect" @onSelected="onDeviceSelected">
              </deviceSelect>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="料号" prop="status">
              <el-input
                v-model="form.drillNo"
                placeholder="请选择料号"
                disabled
              >
                <el-button
                  v-debounce
                  slot="append"
                  @click="handleSelectItem"
                  icon="el-icon-search"
                  disabled
                ></el-button
              ></el-input>
              <ItemSelect ref="ItemSelect" @onSelected="onItemSelected">
              </ItemSelect>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="生产趟数" prop="roundNum">
              <input-number
                :myNum="form.roundNum"
                @changeNum="changeNum"
                :numName="'roundNum'"
                :dis="true"
                :min="1" /></el-form-item
          ></el-col>
          <el-col :span="8">
            <el-form-item label="轴数" prop="axisCount">
              <input-number
                :myNum="form.axisCount"
                @changeNum="changeNum"
                :numName="'axisCount'"
                :dis="true"
                :min="1" /></el-form-item
          ></el-col>
          <el-col :span="8">
            <el-form-item label="尾轮轴数" prop="endAxisCount">
              <input-number
                :myNum="form.endAxisCount"
                @changeNum="changeNum"
                :numName="'endAxisCount'"
                :dis="true"
                :min="1" /></el-form-item
          ></el-col>
        </el-row>
        <el-row
          ><el-col :span="8">
            <el-form-item label="配刀状态" prop="cutterGroupStatus">
              <el-select v-model="form.cutterGroupStatus" placeholder="请选择">
                <el-option
                  v-for="item in cutterGroupStatuList"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="计划要板时间" prop="plannedTime">
              <el-date-picker
                style="width: 200px"
                clearable
                disabled
                v-model="form.plannedTime"
                type="datetime"
                placeholder="计划要板时间"
              >
              </el-date-picker>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="计划生成时间" prop="groupDate">
              <el-date-picker
                style="width: 200px"
                clearable
                disabled
                v-model="form.groupDate"
                type="datetime"
                placeholder="计划生成时间"
              >
              </el-date-picker>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
    <detail ref="cutterCroupDetail" />
  </div>
</template>

<script>
import {
  delList,
  listCutterGroup,
  updateCutterGroup,
} from "@/api/produce/cutterGroup";
import detail from "./detail.vue";
import deviceSelect from "@/components/deviceSelect";
import ItemSelect from "@/components/itemSelect";

export default {
  name: "CutterCroup",
  components: { detail, deviceSelect, ItemSelect },
  data() {
    return {
      page: "cutterCroup",
      optType: undefined,
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 配刀组计划表格数据
      cutterGroupList: [],
      // 0、未锁定1、锁定20、刀盒校验完成30、加载钻带文件完成40、加载配刀Atp文件完成
      cutterGroupStatuList: [
        {
          label: "未锁定",
          value: 0,
        },
        {
          label: "锁定",
          value: 1,
        },
        {
          label: "刀盒校验完成",
          value: 20,
        },
        {
          label: "加载钻带文件完成",
          value: 30,
        },
        {
          label: "加载配刀Atp文件完成",
          value: 40,
        },
      ],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        cutterGroupNo: undefined,
        cutterGroupStatus: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {},
      // 列信息
      columns: [
        { key: 0, label: "配刀组编号", visible: true },
        { key: 1, label: "钻机编号", visible: true },
        { key: 2, label: "钻机名称", visible: true },
        { key: 3, label: "料号", visible: true },
        { key: 4, label: "生产趟数", visible: true },
        { key: 5, label: "轴数", visible: true },
        { key: 6, label: "尾轮轴数", visible: true },
        { key: 7, label: "配刀状态", visible: true },
        { key: 8, label: "锁定时间", visible: true },
        { key: 9, label: "计划要板时间", visible: true },
        { key: 10, label: "组计划生成时间", visible: true },
        { key: 11, label: "创建时间", visible: true },
      ],
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
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
  },
  methods: {
    /** 查询配刀组计划列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listCutterGroup(this.queryParams);
      this.cutterGroupList = res.data.list;
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
      this.handleQuery();
    },
    handleSelect(obj) {
      this.$set(this.form, "itemId", obj.id);
      this.$set(this.form, "itemCode", obj.code);
    },

    /** 修改按钮操作 */
    handleUpdate(row) {
      this.form = { ...row };
      this.initialForm = Object.assign({}, { ...row });
      this.title = "修改配刀计划";
      this.open = true;
    },
    /** 提交按钮 */
    submitForm() {
      if (this.form.id != null) {
        updateCutterGroup(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
      }
    },
    // 删除
    handleDelete(row) {
      if (row.groupNo) {
        this.$modal
          .confirm(
            `确定删除<span style="color:red">配刀组编号为 ${row.groupNo}</span> 的数据项？`,
            {
              dangerouslyUseHTMLString: true, // 使用HTML片段
            }
          )
          .then((result) => {
            if (result == "confirm") {
              delList([row.id]).then((res) => {
                if (res.code == 0) {
                  this.$modal.msgSuccess("删除成功");
                  this.getList();
                } else {
                  this.$modal.notifyError(res.message);
                }
              });
            }
          })

          .catch(() => {});
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 点击详情
    handleDetail(row) {
      this.$refs.cutterCroupDetail.showFlag = true;
      this.$refs.cutterCroupDetail.title = `配刀组编号(${row.groupNo})`;
      this.$refs.cutterCroupDetail.getList(row.groupNo);
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
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

    //设备选择弹出框
    handleSelectDevice() {
      this.$refs.deviceSelect.showFlag = true;
      this.$refs.deviceSelect.showLeft = false;
      this.$refs.deviceSelect.title = "设备选择";
      // 仅查询钻机
      this.$refs.deviceSelect.selectedDeviceCode = this.form.drillNo
        ? this.form.drillNo
        : undefined;
      this.$refs.deviceSelect.queryParams.deviceKindList = [2, 3];
      this.$refs.deviceSelect.getTreeselect();
      this.$refs.deviceSelect.getList();
    },
    onDeviceSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "drillName", obj.name);
        this.$set(this.form, "drillNo", obj.code);
      }
    },
    //物料选择弹出框
    handleSelectItem() {
      this.$refs.ItemSelect.showFlag = true;
      this.$refs.ItemSelect.selectedItemCode = this.form.itemName
        ? this.form.itemName
        : undefined;
      this.$refs.ItemSelect.title = "料号选择";
      this.$refs.ItemSelect.getList();
    },
    onItemSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "itemCode", obj.code);
        this.$set(this.form, "itemId", obj.id);
        this.$set(this.form, "itemName", obj.name);
      }
    },
  },
};
</script>
