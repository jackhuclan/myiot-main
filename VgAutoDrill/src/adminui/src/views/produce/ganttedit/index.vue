<template>
  <div class="app-container">
    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-select
          class="ganttTimeSelect"
          v-model="value"
          placeholder="请选择时段"
          @change="hanleChangeTime"
        >
          <el-option
            v-for="item in options"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          >
          </el-option>
        </el-select>
      </el-col>
      <el-col :span="1.5">
        <el-button
          type="info"
          plain
          icon="el-icon-sort"
          @click="toggleExpandAll"
          >展开/折叠</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          type="warning"
          plain
          icon="el-icon-refresh"
          @click="handleRefresh"
          >重新加载</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          type="danger"
          plain
          icon="el-icon-delete"
          @click="handleClose"
          >关闭</el-button
        >
      </el-col>
    </el-row>
    <div class="wrapper">
      <div class="container">
        <GanttChar
          class="left-container"
          ref="ganttChar"
          :tasks="tasks"
          :optType="optType"
          @getList="getGanttTasks"
          :isExpandAll="isExpandAll"
        ></GanttChar>
      </div>
    </div>
  </div>
</template>

<script>
import GanttChar from "@/components/ganttChar";

import { getGanttTaskList } from "@/api/produce/task";
export default {
  name: "GanttEdit",
  components: { GanttChar },
  data() {
    return {
      isExpandAll: false,
      options: [
        {
          value: 1,
          label: "1小时",
        },
        {
          value: 2,
          label: "2小时",
        },
        {
          value: 3,
          label: "3小时",
        },
        {
          value: 8,
          label: "8小时",
        },
      ],
      tasks: {
        data: [],
        links: [],
      },
      value: "8小时",
      optType: "edit",
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
      },
      form: {},
    };
  },
  created() {
    this.getGanttTasks();
  },
  methods: {
    // 获取甘特图数据
    getGanttTasks() {
      this.$nextTick(() => {
        this.$refs.ganttChar.loading = true;
      });
      getGanttTaskList(this.queryParams).then((response) => {
        this.tasks.data = response.map((v) => {
          if (v.taskStatus != 0) {
            return { ...v, readonly: true };
          } else {
            return { ...v, readonly: false };
          }
        });
        this.$refs.ganttChar.loading = false;
        this.$refs.ganttChar.reload();
      });
    },
    // 点击重新加载
    handleRefresh() {
      this.getGanttTasks();
      this.isExpandAll = false;
    },
    // 点击关闭跳转到生产排产页面
    handleClose() {
      const obj = { path: "/produce/schedule" };
      this.$tab.closeOpenPage(obj);
    },
    // 改变查看时间
    hanleChangeTime(val) {
      // 使用vuex来改变公用属性
      this.$store.commit("ganttChar/CHANGE_TIME", val);
      this.$refs.ganttChar.reload();
    },
    // 展开折叠操作
    toggleExpandAll() {
      this.isExpandAll = !this.isExpandAll;
    },
  },
  beforeDestroy() {
    this.$refs.ganttChar.closeTooltip();
    this.$refs.ganttChar.clearAllgantt();
  },
};
</script>

<style scoped>
.wrapper {
  height: calc(100vh - 180px);
}
.container {
  height: calc(100vh - 180px);
  width: 100%;
}
.left-container {
  overflow: hidden;
  position: relative;
  height: 100%;
}
.ganttTimeSelect {
  width: 100px;
}
</style>
