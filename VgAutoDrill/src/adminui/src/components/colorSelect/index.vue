<template>
  <select-form-dialog
    v-model="showFlag"
    v-if="showFlag"
    title="颜色选择"
    :center="true"
    @submitForm="confirmSelect"
  >
    <el-table
      border
      v-loading="loading"
      :data="colorList"
      @current-change="handleCurrent"
      @row-dblclick="handleRowDbClick"
    >
      <el-table-column width="55" align="center">
        <template v-slot="scope">
          <el-radio
            v-removeAriaHidden
            v-model="selectedColorId"
            :label="scope.row.value"
            @change="handleRowChange(scope.row)"
            >{{ "" }}</el-radio
          >
        </template>
      </el-table-column>
      <el-table-column label="编码" align="center" type="index" />

      <el-table-column label="名称" align="center" prop="name">
      </el-table-column>
      <el-table-column label="HEX" align="center" prop="value" />
      <el-table-column label="color" align="center">
        <template slot-scope="scope">
          <div
            :style="{
              margin: '0 auto',
              width: '20px',
              height: '20px',
              background: scope.row.value,
            }"
          ></div>
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
  </select-form-dialog>
</template>

<script>
export default {
  name: "ColorSelect",
  watch: {
    showFlag(val) {
      if (!val) {
        this.selectedColorId = 0;
        this.resetForm("queryForm");
      }
    },
  },
  data() {
    return {
      showFlag: false,
      selectedColorId: undefined,
      selectedRow: undefined,
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 客户表格数据
      colorList: [
        { name: "浅绿色", value: "#00ffff" },
        { name: "黄绿色", value: "#9acd32" },
        { name: "紫罗兰色", value: "#ee82ee" },
        { name: "蓟色", value: "#d8bfd8" },
        { name: "茶色", value: "#d2b48c" },
        { name: "天蓝色", value: "#87ceeb" },
        { name: "沙褐色", value: "#f4a460" },
        { name: "	洋李色", value: "#dda0dd" },
        { name: "红橙色", value: "#ff5722" },
        { name: "橙色", value: "#ffa500" },
        { name: "浅玫瑰色", value: "#ffe4e1" },
        { name: "粟色", value: "#800000" },
        { name: "亮海蓝色", value: "#20b2aa" },
        { name: "亮天蓝色", value: "#87cefa" },
        { name: "暗宝石绿", value: "#00ced1" },
        { name: "海松色", value: "#6e6b41" },
        { name: "露草色", value: "#33a3dc" },
        { name: "赤白橡色", value: "#deab8a" },
        { name: "伽罗色", value: "#7f7522" },
        { name: "黄绿色", value: "#7fff00" },
        { name: "白杏色", value: "#ffebcd" },
        { name: "粉红色", value: "#ffc0cb" },
        { name: "苍紫罗兰色", value: "#db7093" },
        { name: "中绿色", value: "#66cdaa" },
        { name: "蔷薇色", value: "#f05b72" },
        { name: "珊瑚色", value: "#f8aba6" },
        { name: "薄柿色", value: "#ca8687" },
        { name: "焦茶色", value: "#6b473c" },
        { name: "柑子色", value: "#faa755" },
        { name: "胭脂色", value: "#b3424a" },
      ],
      // 弹出层标题
      title: "",
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        name: undefined,
        code: undefined,
        status: 1,
      },
    };
  },
  methods: {
    /** 查询客户列表 */
    getList(isSearch) {
      //   this.loading = true;
      //   listClient(this.queryParams).then((res) => {
      //     this.colorList = res.data.list;
      //     this.total = res.data.total;
      this.loading = false;
      //     // 只有搜索状态下进行提示
      //     if (isSearch == "search") {
      //       this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      //     }
      //   });
    },

    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.code = this.queryParams.code?.trim();
      this.queryParams.name = this.queryParams.name?.trim();
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.resetForm("queryForm");
      this.handleQuery();
    },

    handleCurrent(row) {
      if (row) {
        this.selectedRow = row;
      }
    },
    //行双击选中
    handleRowDbClick(row) {
      if (row) {
        this.selectedRow = row;
        this.$emit("onSelected", this.selectedRow);
        this.showFlag = false;
        this.selectedColorId = undefined;
        this.selectedRow = undefined;
      }
    },
    // 单选选中数据
    handleRowChange(row) {
      if (row) {
        this.selectedRow = row;
      }
    },
    //确定选中
    confirmSelect() {
      if (this.selectedColorId == null || this.selectedColorId == 0) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRow);
      this.showFlag = false;
      this.selectedColorId = undefined;
      this.selectedRow = undefined;
    },
  },
};
</script>
