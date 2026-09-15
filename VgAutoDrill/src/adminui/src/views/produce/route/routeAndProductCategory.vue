<template>
  <div class="app-container">
    <el-row :gutter="10" v-if="optType != 'view'" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['produce:routeAndProductCategory:add'])"
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
          :disabled="hasPermi(['produce:routeAndProductCategory:remove'])"
          >批量删除</el-button
        >
      </el-col>
    </el-row>

    <el-table
      border
      :ref="page"
      v-loading="loading"
      :data="routeAndProductCategoryList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column
        type="selection"
        width="55"
        align="center"
        v-if="optType != 'view'"
      />
      <el-table-column
        label="产品大类编码"
        prop="productCategoryCode"
        show-overflow-tooltip
      />
      <el-table-column
        label="产品大类名称"
        prop="productCategoryName"
        show-overflow-tooltip
      />
      <el-table-column label="创建时间" align="center" prop="createTime">
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.createTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        v-if="optType !== 'view'"
        label="操作"
        align="center"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <el-button
            v-debounce
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['produce:routeAndProductCategory:remove'])"
            >删除</el-button
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
    <ProductCategorySelect
      ref="productCategorySelect"
      @onSelected="onProductCategorySelected"
    ></ProductCategorySelect>
  </div>
</template>

<script>
import {
  listRouteAndProductCategory,
  getRouteAndProductCategory,
  delRouteAndProductCategory,
  addRouteAndProductCategory,
  updateRouteAndProductCategory,
  delList,
} from "@/api/produce/routeAndProductCategory";
// 产品大类选择
import ProductCategorySelect from "@/components/productCategorySelect/checkbox.vue";

export default {
  name: "RouteAndProduct", 
  components: {
    ProductCategorySelect,
  },
  props: ["optType", "routeId"],
  data() {
    return {
      page: "routeAndProduct",
      type: undefined,
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 需要回显的数据
      echoIds: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 工艺关联产品大类表格数据
      routeAndProductCategoryList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        routeId: undefined,
        productCategoryId: undefined,
      },
      // 表单参数
      form: {},
      // 表单校验
      rules: {
        routeId: [
          { required: true, message: "工艺路线不能为空", trigger: "blur" },
        ],
        productCategoryName: [
          { required: true, message: "产品大类不能为空", trigger: "blur" },
        ],
      },
      isChange: false,
    };
  },
  created() {
    this.getList();
  },
  methods: {
    /** 查询关联产品列表 */
    async getList() {
      this.loading = true;
      this.queryParams.routeId = this.routeId;

      const res = await listRouteAndProductCategory(this.queryParams);
      this.routeAndProductCategoryList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
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
        productCategoryId: undefined,
      };
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList();
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.resetForm("queryForm");
      this.handleQuery();
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      //需要回显的数据
      this.echoIds = [];

      listRouteAndProductCategory({
        pageNum: 1,
        pageSize: 10000,
        routeId: this.routeId,
      }).then((res) => {
        res.data.list?.forEach((v) => {
          this.echoIds.push(v.productCategoryId);
        });
        this.$refs.productCategorySelect.queryParams.pageNum = 1;
        this.$refs.productCategorySelect.showFlag = true;
        this.$refs.productCategorySelect.showData = this.echoIds;
        this.$refs.productCategorySelect.getList();
      });
    },
    // 删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.id,
          delRouteAndProductCategory,
          this.getList,
          "产品大类编码为" + row.productCategoryCode
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },

    // 产品大类选择框
    onProductCategorySelected(rows) {
      if (
        this.isArrEqual(
          rows,
          this.routeAndProductCategoryList,
          "productCategoryId"
        )
      ) {
        return (this.$refs.productCategorySelect.showFlag = false);
      }
      let routeIds = [];
      rows.forEach((v) => {
        routeIds.push(this.routeId);
      });

      addRouteAndProductCategory({
        productCategoryIds: rows,
        routeIds,
        fromRoute: true,
      })
        .then((res) => {
          if (res.code == 0) {
            this.queryParams.pageNum = 1;
            this.$modal.msgSuccess("操作成功");
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        })
        .catch(() => {});
    },
  },
};
</script>
<style lang="scss" scoped>
.el-table {
  margin-top: 10px;
}
</style>
