<template>
  <div class="app-container">
    <el-row :gutter="10" class="mb8" v-if="optType != 'view'">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['masterData:productCategory:addRoute'])"
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
          :disabled="hasPermi(['masterData:productCategory:removeRoute'])"
          >批量删除</el-button
        >
      </el-col>
    </el-row>
    <el-table
      border
      :ref="page"
      v-loading="loading"
      :data="routeList"
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
        label="路线编码"
        min-width="150"
        fixed="left"
        prop="routeCode"
        show-overflow-tooltip
        key="routeCode"
      ></el-table-column>
      <el-table-column
        label="路线名称"
        prop="routeName"
        min-width="150"
        show-overflow-tooltip
        key="routeName"
      />
      <el-table-column
        label="路线说明"
        prop="routeDesc"
        min-width="150"
        show-overflow-tooltip
        key="routeDesc"
      />
      <el-table-column
        label="是否启用"
        align="center"
        prop="status"
        key="status"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.status == 1">是</el-tag>
          <el-tag type="danger" v-else>否</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="优先级"
        align="center"
        prop="orderNum"
        key="orderNum"
      >
        <template slot-scope="scope">
          <span v-if="!scope.row.inpShow">{{ scope.row.orderNum }}</span>
          <input
            class="inpShow"
            v-else
            type="number"
            v-model.number="scope.row.orderNum"
          />
        </template>
      </el-table-column>
      <el-table-column
        label="备注"
        show-overflow-tooltip
        min-width="150px"
        prop="routeRemark"
        key="routeRemark"
      />

      <el-table-column
        label="操作"
        fixed="right"
        align="center"
        class-name="small-padding fixed-width"
        min-width="140px"
        v-if="optType != 'view'"
      >
        <template slot-scope="scope">
          <el-button
            v-if="!scope.row.inpShow"
            type="text"
            icon="el-icon-edit"
            @click="handleOrderNumSet(scope.row)"
            :disabled="hasPermi(['masterData:productCategory:setRoute'])"
            >设置</el-button
          >
          <el-button
            v-else
            type="text"
            icon="el-icon-edit"
            @click="handleOrderNumSave(scope.row)"
            :disabled="hasPermi(['masterData:productCategory:setRoute'])"
            >保存</el-button
          >
          <el-dropdown
            trigger="click"
            @command="(command) => handleCommand(command, scope)"
          >
            <span :class="'el-dropdown-link ' + $store.getters.size">
              <i class="el-icon-d-arrow-right el-icon--right"></i>更多
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                command="moveUp"
                icon="el-icon-top"
                :disabled="
                  hasPermi(['masterData:productCategory:moveUp']) ||
                  scope.$index === 0
                "
                >上移</el-dropdown-item
              >
              <el-dropdown-item
                command="moveDown"
                :disabled="
                  hasPermi(['masterData:productCategory:moveDown']) ||
                  getFormLength(scope.$index)
                "
                icon="el-icon-bottom"
                >下移</el-dropdown-item
              >
              <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="hasPermi(['masterData:productCategory:removeRoute'])"
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
      :autoScroll="false"
    />
    <ProductCategoryAndRouteSelect
      ref="productCategoryAndRouteSelect"
      @onSelected="onProductCategoryAndRouteSelected"
      :ids="echoIds"
    ></ProductCategoryAndRouteSelect>
  </div>
</template>

<script>
import {
  addRouteAndProductCategory,
  delRouteAndProductCategory,
  updateRouteAndProductCategory,
  delList,
  getRouteInfoList,
  getRouteAndProductCategory,
  updateDataOrderNum,
} from "@/api/produce/routeAndProductCategory";
// 选择
import ProductCategoryAndRouteSelect from "@/components/productCategoryAndRouteSelect";
export default {
  name: "ProrouteAndRoute",
  dicts: ["sys_normal_disable"],
  props: ["productCategoryId", "optType"],
  components: { ProductCategoryAndRouteSelect },
  data() {
    return {
      page: "prorouteAndRoute",
      //自动生成编码
      autoGenFlag: false,
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
      // 表格数据
      routeList: [],
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        routeId: undefined,
        productCategoryId: undefined,
      },
      isChange: 0,
    };
  },
  computed: {
    newArr: function () {
      return JSON.parse(JSON.stringify(this.routeList));
    },
  },
  watch: {
    // 监听数组是否有变化
    newArr: {
      handler: function (val, oldval) {
        this.isChange++;
        if (this.isChange > 1) {
          if (JSON.stringify(val) != JSON.stringify(oldval)) {
            this.$emit("father_watch");
          }
        }
      },
      deep: true,
    },
  },
  methods: {
    /** 根据产品大类查询工艺路线列表 */
    async getList() {
      this.loading = true;
      const res = await getRouteInfoList({
        ...this.queryParams,
        productCategoryId: this.productCategoryId,
      });
      this.routeList = res.data.list.map((v) => {
        return { ...v, inpShow: false };
      });
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

    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.id,
          delRouteAndProductCategory,
          this.getList,
          "路线编码为" + row.routeCode
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 优先级设置
    handleOrderNumSet(row) {
      row.inpShow = true;
    },
    // 优先级保存
    handleOrderNumSave(row) {
      const id = row.id || this.ids;
      updateRouteAndProductCategory({
        id,
        orderNum: row.orderNum,
        status: 1,
      }).then((res) => {
        if (res.code == 0) {
          this.getList();
          this.$modal.msgSuccess("设置成功");
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 上移
    moveUp({ row, $index }) {
      const item = this.routeList.find((v, i) => i == $index - 1);
      updateDataOrderNum({ oldDataID: row.id, replaceDataID: item.id }).then(
        (res) => {
          if (res.code == 0) {
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        }
      );
    },
    // 下移
    moveDown({ row, $index }) {
      const item = this.routeList.find((v, i) => i == $index + 1);
      updateDataOrderNum({ oldDataID: row.id, replaceDataID: item.id }).then(
        (res) => {
          if (res.code == 0) {
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        }
      );
    },
    // 控制下移按钮的显示于隐藏
    getFormLength(index) {
      if (index === this.routeList.length - 1) return true;
      else return false;
    },
    // 更多操作
    handleCommand(command, scope) {
      switch (command) {
        case "handleOrderNumSet":
          this.handleOrderNumSet(scope.row);
          break;
        case "handleOrderNumSave":
          this.handleOrderNumSave(scope.row);
          break;
        case "moveDown":
          this.moveDown(scope);
          break;
        case "moveUp":
          this.moveUp(scope);
          break;
        case "handleDelete":
          this.handleDelete(scope.row);
          break;
        default:
          break;
      }
    },

    /** 新增按钮操作 */
    handleAdd() {
      // 需要回显的数据
      this.echoIds = [];
      getRouteInfoList({
        pageNum: 1,
        pageSize: 10000,
        productCategoryId: this.productCategoryId,
      }).then((res) => {
        res.data.list?.forEach((v) => {
          this.echoIds.push(v.routeId);
        });
        this.$refs.productCategoryAndRouteSelect.queryParams.pageNum = 1;
        this.$refs.productCategoryAndRouteSelect.showFlag = true;
        this.$refs.productCategoryAndRouteSelect.showData = this.echoIds;
        this.$refs.productCategoryAndRouteSelect.getList();
      });
    },
    // 路线选择框
    onProductCategoryAndRouteSelected(rows) {
      const flag_List = this.routeList.map((v) => v.routeId);
      if (JSON.stringify(rows) == JSON.stringify(flag_List)) {
        return (this.$refs.productCategoryAndRouteSelect.showFlag = false);
      }
      let productCategoryIds = [];
      let orderNums = [];
      rows.forEach((v, i) => {
        productCategoryIds.push(this.productCategoryId);
        orderNums.push(i + 1);
      });

      addRouteAndProductCategory({
        productCategoryIds,
        routeIds: rows,
        orderNums,
      })
        .then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("操作成功");
            this.queryParams.pageNum = 1;
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
<style>
.inpShow {
  border: 1px #5e9fff solid;
  border-radius: 2px;
  width: 80%;
  outline: none;
}
</style>
