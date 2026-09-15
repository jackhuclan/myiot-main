<template>
  <edit-form-dialog
    v-model="showFlag"
    :title="title"
    optType="view"
    @submitForm="submitForm"
  >
    <el-table border v-loading="loading" :data="detailList">
      <el-table-column label="任务编码" align="center" prop="taskCode" />

      <el-table-column label="lot编号" align="center" prop="itemCode" />
      <el-table-column
        label="每趟实际钻板数"
        width="150px"
        align="center"
        prop="panelNum"
      />
      <el-table-column label="是否首次" align="center" prop="isFirstCutter">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isFirstCutter">是</el-tag>
          <el-tag type="danger" v-else>否</el-tag>
        </template></el-table-column
      >
    </el-table>
  </edit-form-dialog>
</template>

<script>
import { getCutterGroup } from "@/api/produce/cutterGroup";
export default {
  name: "CutterGroupDetail",

  data() {
    return {
      showFlag: false,
      // 遮罩层
      loading: true,
      // 总条数
      total: 0,
      // 表格数据
      detailList: [],
      // 弹出层标题
      title: "",
    };
  },
  methods: {
    /** 查询客户列表 */
    getList(groupNo) {
      this.loading = true;
      getCutterGroup(groupNo).then((res) => {
        this.detailList = res.data;
        this.loading = false;
      });
    },

    //确定选中
    submitForm() {},
  },
};
</script>
