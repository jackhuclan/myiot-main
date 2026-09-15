<template>
  <div class="app-container">
    <div class="index">
      <div class="left" :class="{ fold: openType }">
        <leftTreeSelect
          v-model="itemTypeName"
          :placeholder="'请输入分类名称'"
          :filterNode="filterNode"
          :options="itemTypeOptions"
          :defaultProps="defaultProps"
          :highlightCurrent="highlightCurrent"
          :handleNodeClick="handleNodeClick"
        ></leftTreeSelect>
      </div>
      <div class="right" :class="{ fold: openType }">
        <search-form
          v-show="showSearch"
          :form="queryParams"
          @search="handleQuery"
          @reset="resetQuery"
          :openType="openType"
        >
          <el-form-item label="排序方式" prop="queryOrderBy">
            <el-select
              @change="handleQueryOrderBy"
              @clear="clearQueryOrderBy('queryOrderBy')"
              v-model="queryParams.queryOrderBy"
              placeholder="请选择"
              style="width: 150px"
            >
              <el-option
                v-for="item in $status.queryOrderByOptions"
                :key="item.value"
                :label="item.label"
                :value="item.value"
              />
            </el-select>
          </el-form-item>

          <el-form-item label="物料编码" prop="code">
            <el-input
              v-trim
              v-model="queryParams.code"
              placeholder="请输入物料编码"
              clearable
              @keyup.enter.native="handleQuery"
            />
          </el-form-item>
          <el-form-item label="物料名称" prop="name">
            <el-input
              v-trim
              v-model="queryParams.name"
              placeholder="请输入物料名称"
              clearable
              @keyup.enter.native="handleQuery"
            />
          </el-form-item>
          <el-form-item label="物料/产品" prop="itemOrProduct">
            <el-select
              @clear="clearQueryParams('itemOrProduct')"
              v-model="queryParams.itemOrProduct"
              placeholder="请选择"
              clearable
              style="width: 150px"
            >
              <el-option :label="'物料'" :value="1" />
              <el-option :label="'产品'" :value="2" />
            </el-select>
          </el-form-item>
        </search-form>

        <el-row :gutter="10" class="mb8">
          <el-col :span="1.5">
            <el-button
              type="info"
              plain
              icon="el-icon-s-operation"
              @click="handleOpenType"
              >显/隐分类</el-button
            >
          </el-col>
          <el-col :span="1.5">
            <el-button
              v-debounce
              type="primary"
              plain
              icon="el-icon-plus"
              @click="handleAdd"
              :disabled="hasPermi(['masterData:item:add'])"
              >新增</el-button
            >
          </el-col>
          <el-col :span="1.5">
            <el-button
              v-debounce
              type="danger"
              plain
              icon="el-icon-delete"
              :disabled="hasPermi(['masterData:item:remove'])"
              @click="handleDelete"
              >批量删除</el-button
            >
          </el-col>
          <el-col :span="1.5">
            <el-button
              v-debounce
              type="info"
              plain
              icon="el-icon-upload2"
              @click="handleImport"
              :disabled="hasPermi(['masterData:item:import'])"
              >导入</el-button
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
          :data="itemList"
          @selection-change="handleSelectionChange"
          @row-dblclick="rowDblclick"
          :row-style="rowStyle"
        >
          <el-table-column type="selection" width="50" align="center" />
          <el-table-column
            label="物料编码"
            min-width="200px"
            key="code"
            fixed="left"
            show-overflow-tooltip
            v-if="columns[0].visible"
          >
            <template slot-scope="scope">
              <span
                class="click_code"
                :data-id="scope.row.id"
                v-isGetSelection="['masterData:item:view']"
                >{{ scope.row.code }}</span
              >
            </template>
          </el-table-column>
          <el-table-column
            label="物料名称"
            min-width="200px"
            key="name"
            prop="name"
            show-overflow-tooltip
            v-if="columns[1].visible"
          />
          <el-table-column
            label="层数"
            key="layerNum"
            prop="layerNum"
            show-overflow-tooltip
            align="center"
            v-if="columns[2].visible"
          />
          <el-table-column
            label="叠数"
            key="panelCount"
            prop="panelCount"
            min-width="150"
            show-overflow-tooltip
            v-if="columns[3].visible"
          />
          <el-table-column
            label="单位"
            key="unitOfMeasure"
            prop="unitOfMeasure"
            show-overflow-tooltip
            min-width="150"
            v-if="columns[4].visible"
          >
          </el-table-column>
          <el-table-column
            label="物料/产品"
            align="center"
            key="itemOrProduct"
            prop="itemOrProduct"
            v-if="columns[5].visible"
          >
            <template slot-scope="scope">
              <span>{{ scope.row.itemOrProduct == 1 ? "物料" : "产品" }}</span>
            </template>
          </el-table-column>

          <el-table-column
            label="产品大类"
            key="productCategoryName"
            prop="productCategoryName"
            show-overflow-tooltip
            min-width="150"
            v-if="columns[6].visible"
          >
          </el-table-column>
          <el-table-column
            label="工序组"
            v-if="columns[7].visible"
            key="specGroup"
            prop="specGroup"
            show-overflow-tooltip
            min-width="100px"
          />
          <el-table-column
            label="板长"
            align="center"
            key="panelLength"
            prop="panelLength"
            v-if="columns[8].visible"
          />
          <el-table-column
            label="板宽"
            align="center"
            key="panelWidth"
            prop="panelWidth"
            v-if="columns[9].visible"
          />
          <el-table-column
            label="转换前路径"
            align="center"
            key="beforeDrillFilePath"
            prop="beforeDrillFilePath"
            show-overflow-tooltip
            min-width="150px"
            v-if="columns[10].visible"
          />

          <el-table-column
            label="转换后路径"
            align="center"
            key="afterDrillFilePath"
            prop="afterDrillFilePath"
            show-overflow-tooltip
            min-width="150px"
            v-if="columns[11].visible"
          />
          <el-table-column
            label="钻带文件路径"
            key="drillFilePath"
            prop="drillFilePath"
            show-overflow-tooltip
            min-width="150"
            v-if="columns[12].visible"
          />
          <el-table-column
            label="是否启用"
            key="status"
            align="center"
            v-if="columns[13].visible"
          >
            <template slot-scope="scope">
              <el-tag v-if="scope.row.status == 1">是</el-tag>
              <el-tag v-else type="danger">否</el-tag>
            </template>
          </el-table-column>
          <el-table-column
            label="创建时间"
            key="createTime"
            prop="createTime"
            show-overflow-tooltip
            v-if="columns[14].visible"
            align="center"
            min-width="160"
          >
            <template slot-scope="scope">
              <span>{{ parseTime(scope.row.createTime) }}</span>
            </template>
          </el-table-column>
          <el-table-column
            label="最后修改时间"
            key="modifyTime"
            prop="modifyTime"
            show-overflow-tooltip
            v-if="columns[15].visible"
            align="center"
            min-width="160"
          >
            <template slot-scope="scope">
              <span>{{ parseTime(scope.row.modifyTime) }}</span>
            </template>
          </el-table-column>
          <el-table-column
            label="操作"
            align="center"
            fixed="right"
            min-width="180px"
            class-name="small-padding fixed-width"
          >
            <template slot-scope="scope">
              <el-button
                type="text"
                icon="el-icon-edit"
                @click="handleUpdate(scope.row)"
                :disabled="hasPermi(['masterData:item:edit'])"
                >修改</el-button
              >
              <el-button
                type="text"
                icon="el-icon-document-copy"
                @click="handleCopy(scope.row)"
                :disabled="hasPermi(['masterData:item:copy'])"
                >复制</el-button
              >
              <el-dropdown
                trigger="click"
                @command="(command) => handleCommand(command, scope.row)"
              >
                <span :class="'el-dropdown-link ' + $store.getters.size">
                  <i class="el-icon-d-arrow-right el-icon--right"></i>更多
                </span>
                <el-dropdown-menu slot="dropdown">
                  <!-- <el-dropdown-item
                    command="handleCopy"
                    icon="el-icon-document-copy"
                    :disabled="hasPermi(['masterData:item:copy'])"
                    >复制</el-dropdown-item
                  > -->
                  <el-dropdown-item
                    command="handleDelete"
                    icon="el-icon-delete"
                    :disabled="hasPermi(['masterData:item:remove'])"
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
      </div>
    </div>

    <!-- 添加或修改物料or产品对话框 -->
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
          <el-col :span="10">
            <el-form-item label="物料编码" prop="code">
              <div
                v-if="optType == 'copy'"
                style="
                  height: 30px;
                  margin-top: -30px;
                  display: flex;
                  align-items: center;
                  justify-content: space-between;
                "
              >
                <span>参考：{{ copyCode }}</span>
                <el-button
                  v-debounce
                  type="text"
                  icon="el-icon-document-copy"
                  @click="form.code = form.name = copyCode"
                  >复制编码</el-button
                >
              </div>
              <el-input
                v-trim
                v-model="form.code"
                placeholder="请输入物料编码"
                :disabled="
                  autoGenFlag || optType == 'edit' || optType == 'view'
                "
              />
            </el-form-item>
          </el-col>
          <el-col :span="4">
            <el-form-item label-width="80">
              <el-switch
                v-model="autoGenFlag"
                active-color="#13ce66"
                active-text="自动生成"
                @change="handleAutoGenChange(autoGenFlag)"
                :disabled="optType == 'edit' || optType == 'view'"
              >
              </el-switch>
            </el-form-item>
          </el-col>
          <el-col :span="10">
            <el-form-item label="物料名称" prop="name">
              <el-input
                v-trim
                v-model="form.name"
                placeholder="请输入物料名称"
                maxlength="255"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="物料/产品" prop="itemOrProduct">
              <el-radio-group v-removeAriaHidden v-model="form.itemOrProduct">
                <el-radio :label="1"> 物料 </el-radio>
                <el-radio :label="2"> 产品 </el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="所属分类" prop="itemTypeId">
              <treeselect
                :class="$store.getters.size + '-treeselect'"
                :disabled="optType == 'view'"
                noOptionsText="暂无数据"
                noChildrenText="暂无数据"
                noResultsText="暂无数据"
                v-model="form.itemTypeId"
                :options="itemTypeOptions"
                @open="changeTreeselectOpen"
                :normalizer="normalizer"
                :show-count="true"
                placeholder="请选择所属分类"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="单位" prop="unitOfMeasure">
              <el-select
                ref="select1"
                v-model="form.unitOfMeasure"
                placeholder="请选择单位"
              >
                <el-option
                  v-for="item in measureOptions"
                  :key="item.id"
                  :label="item.label"
                  :value="item.name"
                  v-optionTitle
                >
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>

        <el-row
          ><el-col :span="8">
            <el-form-item label="层数" prop="layerNum">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.layerNum"
                @changeNum="changeNum"
                :numName="'layerNum'"
                :dis="optType == 'view'"
                :min="1"
                :max="1000"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="叠数" prop="panelCount">
              <input-number
                :myNum="form.panelCount"
                @changeNum="changeNum"
                :numName="'panelCount'"
                :dis="optType == 'view'"
                :min="1"
                :max="1000"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="板长" prop="panelLength">
              <input-number
                :myNum="form.panelLength"
                @changeNum="changeNum"
                :numName="'panelLength'"
                :dis="optType == 'view'"
                :min="0"
              />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="8">
            <el-form-item label="板宽" prop="panelWidth">
              <input-number
                :myNum="form.panelWidth"
                @changeNum="changeNum"
                :numName="'panelWidth'"
                :dis="optType == 'view'"
                :min="0"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8" v-if="form.itemOrProduct == 2">
            <el-form-item label="产品大类" prop="productCategoryCode">
              <el-input
                v-model="form.productCategoryCode"
                placeholder="请输入产品大类"
                clearable
                @clear="clearForm"
              >
                <el-button
                  v-debounce
                  slot="append"
                  @click="handleSelectProductCategory"
                  icon="el-icon-search"
                ></el-button>
              </el-input>
              <ProductCategorySelect
                ref="productCategorySelect"
                @onSelected="onProductCategorySelected"
              >
              </ProductCategorySelect>
            </el-form-item>
          </el-col>

          <el-col :span="8">
            <el-form-item label="SpecGroup" prop="specGroup">
              <el-input
                v-model="form.specGroup"
                placeholder="请输入SpecGroup"
                :disabled="optType == 'view'"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="是否启用">
              <el-radio-group
                v-removeAriaHidden
                v-model="form.status"
                :disabled="optType == 'view'"
              >
                <el-radio :label="1">是</el-radio>
                <el-radio :label="0">否</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="24">
            <el-form-item label="转换前路径" prop="beforeDrillFilePath">
              <el-input
                :disabled="optType == 'view'"
                v-model="form.beforeDrillFilePath"
                type="textarea"
                placeholder="请输入文件路径"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="转换后路径" prop="afterDrillFilePath">
              <el-input
                :disabled="optType == 'view'"
                v-model="form.afterDrillFilePath"
                type="textarea"
                placeholder="请输入文件路径"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row v-if="optType != 'add'">
          <el-col :span="8">
            <el-form-item label="创建人">
              <el-input
                disabled
                v-model="form.creatorName"
                placeholder="创建人"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="创建时间">
              <el-date-picker
                v-model="form.createTime"
                disabled
                type="datetime"
                placeholder="创建时间"
              >
              </el-date-picker>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row v-if="optType != 'add'">
          <el-col :span="8">
            <el-form-item label="最后修改人">
              <el-input
                disabled
                v-model="form.modifierName"
                placeholder="最后修改人"
              />
            </el-form-item>
          </el-col>

          <el-col :span="12">
            <el-form-item label="最后修改时间">
              <el-date-picker
                v-model="form.modifyTime"
                disabled
                type="datetime"
                placeholder="最后修改时间"
              >
              </el-date-picker>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
    <!-- 物料产品导入 -->
    <ImportXlsx
      ref="upload"
      @getList="getList"
      :uploadUrl="uploadUrl"
      :downloadUrl="downloadUrl"
      :fileName="fileName"
    ></ImportXlsx>
  </div>
</template>

<script>
import {
  listItem,
  getItem,
  delItem,
  addItem,
  updateItem,
  treeselect,
  pCTreeselect,
  wHTreeselect,
  delList,
} from "@/api/masterData/item";
import { getDropSelectDatas } from "@/api/masterData/unitMeasure";
// 产品大类选择
import ProductCategorySelect from "@/components/productCategorySelect/radio.vue";

export default {
  name: "Item",
  components: { ProductCategorySelect },

  data() {
    // 自定义校验规则
    const itemTypeIdRule = (rule, value, callback) => {
      if (this.form.itemTypeId == 1) {
        callback(new Error("根分类不可选，请重新选择"));
      } else if (!this.form.itemTypeId) {
        callback(new Error("请选择所属分类"));
      } else {
        callback();
      }
    };
    const panelCountRule = (rule, value, callback) => {
      if (this.form.panelCount <= 0) {
        callback(new Error("叠数不能为0"));
      } else {
        callback();
      }
    };
    const layerNumRule = (rule, value, callback) => {
      if (this.form.layerNum <= 0) {
        callback(new Error("层数不能为0"));
      } else {
        callback();
      }
    };
    const checkBeforeDrillFilePath = (rule, value, callback) => {
      //查找"_convduo"
      let patt = new RegExp("_convduo", "i");
      // 是否包含
      let result = patt.test(value);
      if (result) {
        return callback(new Error("当前路径不允许包含'_convduo'字段"));
      }
      callback();
    };
    return {
      page: "item",
      autoGenFlag: false,
      // 分类属性结构显隐
      openType: true,
      enCode: "",
      optType: "",
      // 复制时提示
      copyCode: "",
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 物料or产品表格数据
      itemList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      seeDetailsOpen: false,
      measureOptions: [],
      // 树形结构选项
      highlightCurrent: true,
      itemTypeStatus: undefined,
      // 物料产品分类
      itemTypeOptions: [],
      // 库房树形
      wareHouseOptions: [],
      defaultProps: {
        children: "children",
        label: "label",
      },
      // 树形结构双向绑定名称
      itemTypeName: "",
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        name: undefined,
        code: undefined,
        itemOrProduct: undefined,
        itemTypeId: undefined,
        queryOrderBy: 4,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        name: [
          {
            required: true,
            message: "物料or产品名称不能为空",
            trigger: "change",
          },
        ],
        code: [
          {
            required: true,
            message: "物料or产品编码不能为空",
            trigger: "change",
          },
        ],
        status: [
          {
            required: true,
            message: "物料or产品状态不能为空",
            trigger: "blur",
          },
        ],
        unitOfMeasure: [
          {
            required: true,
            message: "单位不能为空",
            trigger: "change",
          },
        ],
        itemTypeId: [
          {
            required: true,
            validator: itemTypeIdRule,
            trigger: "change",
          },
        ],
        itemOrProduct: [
          {
            required: true,
            message: "请选择类别",
            trigger: "blur",
          },
        ],
        productCategoryCode: [
          {
            required: true,
            message: "请选择产品大类",
            trigger: "change",
          },
        ],
        panelCount: [
          {
            required: true,
            validator: panelCountRule,
            trigger: "change",
          },
        ],
        layerNum: [
          {
            required: true,
            validator: layerNumRule,
            trigger: "change",
          },
        ],
        // 转换前路径校验
        beforeDrillFilePath: [
          {
            required: false,
            validator: checkBeforeDrillFilePath,
            trigger: "change",
          },
        ],
      },
      // 物料产品导入参数
      // 导入的url
      uploadUrl: "/v1/Item/UploadList",
      // 下载的url
      downloadUrl: "/v1/Item/DownLoad",
      // 导入的模板下载名
      fileName: "物料模板.xlsx",
      // 列信息
      columns: [
        { key: 0, label: "物料编码", visible: true },
        { key: 1, label: "物料名称", visible: true },
        { key: 2, label: "层数", visible: true },
        { key: 3, label: "叠数", visible: true },
        { key: 4, label: "单位", visible: true },
        { key: 5, label: "物料/产品", visible: true },
        { key: 6, label: "产品大类", visible: true },
        { key: 7, label: "工序组", visible: true },
        { key: 8, label: "板长", visible: true },
        { key: 9, label: "板宽", visible: true },
        { key: 10, label: "转换前路径", visible: true },
        { key: 11, label: "转换后路径", visible: true },
        { key: 12, label: "钻带文件路径", visible: true },
        { key: 13, label: "是否启用", visible: true },
        { key: 14, label: "创建时间", visible: true },
        { key: 15, label: "最后修改时间", visible: true },
      ],
    };
  },

  activated() {
    this.queryParams.itemTypeId = undefined;
    this.getList();
    this.getTreeselect();
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
    /** 查询物料or产品列表 */
    async getList(isSearch) {
      this.queryParams.queryOrderBy =
        this.$cache.local.get("itemQueryOrderBy") != undefined &&
        this.$cache.local.get("itemQueryOrderBy") != "0"
          ? this.$cache.local.get("itemQueryOrderBy") * 1
          : 4;
      this.loading = true;
      const res = await listItem(this.queryParams);
      this.itemList = res.data.list;
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
    /** 查询下拉树结构 */
    async getTreeselect() {
      // 左侧下拉
      const res = await treeselect();
      this.itemTypeOptions = res.data;
    },

    /** 转换类型数据结构 */
    normalizer(node) {
      if (node.children && !node.children.length) {
        delete node.children;
      }
      return {
        id: node.id,
        label: node.label,
        isDisabled: node.status == 0,
        children: node.children,
      };
    },
    // 获取单位
    async getUnitMeasure() {
      // 单位
      const measureOptionsRes = await getDropSelectDatas({
        pageNum: 1,
        pageSize: 100,
      });

      this.measureOptions = measureOptionsRes.data;
      // 库房下拉
      const wareHouseOptionsRes = await wHTreeselect();
      this.wareHouseOptions = wareHouseOptionsRes.data.list;
    },
    // 获取库房
    async getWHTreeselect() {
      const wareHouseOptionsRes = await wHTreeselect();
      this.wareHouseOptions = wareHouseOptionsRes.data.list;
    },
    // 筛选节点
    filterNode(value, data) {
      if (!value) return true;
      return data.label.indexOf(value) !== -1;
    },
    // 左侧树形结构节点单击事件
    handleNodeClick(data) {
      this.highlightCurrent = true;
      this.itemTypeStatus = data.status;
      if (data.label != "全部") {
        this.queryParams.itemTypeId = data.id;
      } else {
        this.queryParams.itemTypeId = undefined;
      }

      this.handleQuery();
    },

    // 节点产品大类单击事件
    handleProductCategoryChanged(data) {
      this.form.productCategoryId = data.id;
      this.form.productCategoryName = data.label;
      this.form.productCategoryCode = data.code;
    },
    // 显隐分类
    handleOpenType() {
      this.openType = !this.openType;
    },
    // 表单重置
    reset() {
      this.form = {
        name: "",
        code: "",
        itemOrProduct: 2, //新增时分类默认选中产品
        unitOfMeasure: "",
        itemTypeId: 1, //新增时默认是全部
        productCategoryName: "",
        warehouseName: "",
        productCategoryCode: "",
        dispenseMachines: 1,
        panelCount: 1,
        layerNum: 1,
        beforeDrillFilePath: "",
        afterDrillFilePath: "",
        status: 1,
        panelLength: 0,
        panelWidth: 0,
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
      this.itemTypeName = "";
      this.queryParams.itemTypeId = undefined;
      this.highlightCurrent = false;
      this.$cache.local.remove("itemQueryOrderBy");
      this.resetForm("queryForm");
      this.handleQuery();
    },
    // 点击排序下拉框x号重置
    clearQueryOrderBy(val) {
      this.$cache.local.remove("itemQueryOrderBy");
    },
    // 监听排序方式选择
    handleQueryOrderBy(val) {
      this.$cache.local.set("itemQueryOrderBy", val);
    },
    // 查询明细按钮操作
    handleView(id) {
      this.reset();
      getItem(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "查看(" + res.data.code + ")";
          this.optType = "view";
          this.autoGenFlag = false;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 新增按钮操作 */
    handleAdd() {
      if (this.itemTypeStatus != null && this.itemTypeStatus == 0)
        return this.$modal.msgError("当前分类已停用");
      this.reset();
      this.getUnitMeasure();
      this.getWHTreeselect();
      this.open = true;
      if (this.queryParams.itemTypeId) {
        this.form.itemTypeId = this.queryParams.itemTypeId;
      }
      this.initialForm = Object.assign({}, this.form);
      this.title = "添加物料or产品";
      this.optType = "add";
      this.autoGenFlag = false;
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      this.getUnitMeasure();
      this.getWHTreeselect();
      const id = row.id || this.ids;
      getItem(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.enCode = res.data.code;
          this.initialForm = Object.assign({}, res.data);
          this.open = true ;
          this.title = `修改(${res.data.code})物料`;
          this.optType = "edit";
          this.autoGenFlag = false;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 复制按钮操作 */
    handleCopy(row) {
      this.reset();
      this.optType = "copy";
      getItem(row.id).then((res) => {
        if (res.code == 0) {
          this.copyCode = res.data.code;
          this.form = { ...res.data, code: "", id: undefined };
          this.title = `复制(${this.copyCode})物料`;
          this.open = true;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleCopy":
          this.handleCopy(row);
          break;
        case "handleDelete":
          this.handleDelete(row);
          break;
        default:
          break;
      }
    },
    // 点击产品大类清空按钮
    clearForm() {
      this.form.productCategoryId = undefined;
      this.form.productCategoryCode = undefined;
      this.form.productCategoryName = undefined;
    },
    /** 提交按钮 */
    submitForm: function () {
      if (this.form.id != undefined) {
        updateItem(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addItem(this.form).then((res) => {
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
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(row.id, delItem, this.getList, "物料编码为" + row.code);
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    /** 导入按钮操作 */
    handleImport() {
      this.$refs.upload.title = "物料产品导入";
      this.$refs.upload.open = true;
    },
    // 产品大类选择弹出框
    handleSelectProductCategory() {
      this.$refs.productCategorySelect.showFlag = true;
      this.$refs.productCategorySelect.selectedProductCategoryId = this.form
        .productCategoryId
        ? this.form.productCategoryId
        : undefined;
      this.$refs.productCategorySelect.getList();
    },
    // 产品大类选择框
    onProductCategorySelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "productCategoryName", obj.name);
        this.$set(this.form, "productCategoryCode", obj.code);
        this.$set(this.form, "productCategoryId", obj.id);
        this.$set(
          this.form,
          "dispenseMachines",
          obj.dispenseMachines != null ? obj.dispenseMachines : 0
        );
      }
    },
    // 自动生成编码
    handleAutoGenChange(autoGenFlag) {
      if (autoGenFlag) {
        this.getEncode({
          rulesCode: "ITEM_CODE",
          buildCount: 1,
        }).then((response) => {
          const code = response.data[0];
          this.form.code = code;
          this.form.name = code;
        });
      } else {
        if (this.optType == "edit") {
          this.form.code = this.enCode;
          this.form.name = this.enName;
          return;
        }
        this.form.code = "";
        this.form.name = "";
      }
    },
    changeTreeselectOpen() {
      const doms = document.querySelectorAll(".el-dialog .el-select");
      doms.forEach((v, i) => {
        this.$refs[`select` + (i + 1)]?.blur();
      });
    },
  },
};
</script>
