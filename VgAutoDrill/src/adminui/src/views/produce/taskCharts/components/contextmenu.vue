<!-- 任务分布右键菜单 -->
<template>
  <ul
    :style="{
      left: left + 'px',
      top: top + 'px',
    }"
    class="menuselect"
  >
    <!-- 阻止事件冒泡 -->
    <template v-if="workOrder">
      <div>
        <li>
          <el-button
            v-debounce
            plain
            @click="handleSelectColor"
            :disabled="hasPermi(['produce:ticketCharts:remarkcolor'])"
          >
            <svg-icon icon-class="selectcolor" style="margin-right: 5px" />
            标记颜色</el-button
          >
        </li>
      </div>
    </template>
    <template v-else>
      <div>
        <li @click.stop>
          <el-popover placement="right" width="130" trigger="hover">
            <el-checkbox-group v-model="list" @change="handleChange">
              <el-checkbox
                class="dropdown-checkbox"
                label="任务编码"
              ></el-checkbox>
              <el-checkbox
                class="dropdown-checkbox"
                label="工单编码"
              ></el-checkbox>
              <el-checkbox
                class="dropdown-checkbox"
                label="物料编码"
              ></el-checkbox>
              <el-checkbox
                class="dropdown-checkbox"
                label="钻机编码"
              ></el-checkbox>
              <el-checkbox
                class="dropdown-checkbox"
                label="开始时间"
              ></el-checkbox>
              <el-checkbox
                class="dropdown-checkbox"
                label="生产叠数"
              ></el-checkbox>
              <el-checkbox class="dropdown-checkbox" label="状态"></el-checkbox>
              <el-checkbox
                class="dropdown-checkbox"
                label="预计完成时长"
              ></el-checkbox>
              <el-checkbox
                class="dropdown-checkbox"
                label="组计划编号"
              ></el-checkbox>
            </el-checkbox-group>
            <el-button style="margin-left: 3px" slot="reference"
              ><i class="el-icon-info"></i>
              选择展示字段
            </el-button>
          </el-popover>
        </li>
        <li>
          <el-button @click="handleMenuSelect" v-debounce plain>
            <svg-icon icon-class="checkall" class="checkall" />
            全部选中</el-button
          >
        </li>
        <li>
          <el-button @click="handleUnCheckAll" v-debounce plain
            ><svg-icon icon-class="uncheck" class="uncheck" />
            取消全部</el-button
          >
        </li>
        <li>
          <el-button
            v-debounce
            plain
            icon="el-icon-check"
            @click="handleCommitAll"
            :disabled="hasPermi(['produce:taskCharts:commit'])"
            >批量提交</el-button
          >
        </li>
        <li>
          <el-button
            v-debounce
            plain
            icon="el-icon-refresh-left"
            @click="handleRevokeAll"
            :disabled="hasPermi(['produce:taskCharts:revoke'])"
          >
            批量撤销
          </el-button>
        </li>
      </div>

      <!-- <li>
        <el-button
          v-debounce
          plain
          icon="el-icon-share"
          @click="handleTransferTask"
          :disabled="hasPermi(['produce:tasks:transferTask'])"
          >批量转发</el-button
        >
      </li> -->
    </template>
  </ul>
</template>

<script>
export default {
  name: "contextmenu",
  props: {
    workOrder: {
      type: Boolean,
      default: false,
    },
    left: {
      type: Number,
      default: 0,
    },
    top: {
      type: Number,
      default: 0,
    },
    value: {},
  },
  watch: {
    value: {
      handler(val) {
        this.list = val;
      },
    },
  },
  mounted() {
    this.list = this.value;
  },
  data() {
    return {
      list: [],
    };
  },
  methods: {
    handleChange(val) {
      this.$emit("input", val);
    },
    handleMenuSelect() {
      this.$emit("handleMenuSelect", true);
    },
    handleUnCheckAll() {
      this.$emit("handleUnCheckAll");
    },
    handleCommitAll() {
      this.$emit("handleCommitAll");
    },
    handleRevokeAll() {
      this.$emit("handleRevokeAll");
    },
    handleSelectColor() {
      this.$emit("handleSelectColor");
    },
    // 点击批量转发
    handleTransferTask() {
      this.$emit("handleTransferTask");
    },
  },
};
</script>

<style lang="scss" scoped>
// 右键菜单
.menuselect {
  margin: 0;
  background: #fff;
  z-index: 3888;
  position: fixed;
  list-style-type: none;
  padding: 5px 0;
  border-radius: 4px;
  font-size: 12px;
  font-weight: 400;
  color: #333;
  -webkit-box-shadow: 2px 2px 3px 0 rgba(0, 0, 0, 0.3);
  box-shadow: 2px 2px 3px 0 rgba(0, 0, 0, 0.3);
  li {
    margin: 0;
    display: flex;
    align-items: center;
    cursor: pointer;
    .uncheckall {
      margin-right: 5px;
    }
    .uncheck {
      margin-right: 6px;
    }
    .checkall {
      margin-right: 2px;
      width: 15px;
      height: 15px;
    }
    .el-button {
      padding: 7px 16px;
      width: 100% !important;
      border: none !important;
      background: transparent !important;
    }
  }
  li:hover {
    background: #eee;
  }
}
</style>
