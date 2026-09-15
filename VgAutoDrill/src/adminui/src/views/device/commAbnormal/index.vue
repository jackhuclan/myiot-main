<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="消息来源" prop="messageId">
        <el-input
          v-trim
          v-model="queryParams.messageId"
          placeholder="请输入消息来源"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
    </search-form>

    <el-row :gutter="10" class="mb8">
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
      :data="deviceServiceInvocationsList"
    >
      <el-table-column
        label="id"
        key="id"
        prop="id"
        align="center"
        min-width="80"
        v-if="columns[0].visible"
      >
      </el-table-column>
      <el-table-column
        label="消息来源"
        key="messageId"
        prop="messageId"
        min-width="180"
        show-overflow-tooltip
        v-if="columns[1].visible"
      />
      <el-table-column
        label="主叫设备"
        key="routingKey"
        prop="routingKey"
        min-width="180"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />
      <el-table-column
        label="请求"
        key="requestTopic"
        prop="requestTopic"
        min-width="180"
        show-overflow-tooltip
        v-if="columns[3].visible"
      >
      </el-table-column>
      <el-table-column
        label="响应"
        key="responseTopic"
        prop="responseTopic"
        min-width="180"
        show-overflow-tooltip
        v-if="columns[4].visible"
      >
      </el-table-column>
      <el-table-column
        label="尝试次数"
        key="retries"
        prop="retries"
        align="center"
        show-overflow-tooltip
        v-if="columns[5].visible"
      />

      <el-table-column
        label="是否超时"
        align="center"
        key="isTimeout"
        prop="isTimeout"
        v-if="columns[6].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isTimeout">是</el-tag>
          <el-tag v-else type="danger">否</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="原因"
        key="reason"
        prop="reason"
        min-width="180"
        show-overflow-tooltip
        v-if="columns[7].visible"
      />
      <el-table-column
        label="首次调用时间"
        key="firstInvocationTimestamp"
        prop="firstInvocationTimestamp"
        min-width="180"
        show-overflow-tooltip
        v-if="columns[8].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.firstInvocationTimestamp) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="末次调用时间"
        key="lastInvocationTimestamp"
        prop="lastInvocationTimestamp"
        min-width="180"
        show-overflow-tooltip
        v-if="columns[9].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.lastInvocationTimestamp) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="是否紧急"
        align="center"
        key="isUrgent"
        prop="isUrgent"
        v-if="columns[10].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isUrgent">是</el-tag>
          <el-tag v-else type="danger">否</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="是否处理"
        align="center"
        key="isDealed"
        prop="isDealed"
        v-if="columns[11].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isDealed">是</el-tag>
          <el-tag v-else type="danger">否</el-tag>
        </template>
      </el-table-column>

      <el-table-column
        label="创建时间"
        align="center"
        key="createTime"
        prop="createTime"
        min-width="180"
        v-if="columns[12].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.createTime) }}</span>
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
</template>

<script>
import { listDeviceServiceInvocations } from "@/api/device/commAbnormal";
export default {
  name: "CommAbnormal",
  data() {
    return {
      page: "commAbnormal",
      loading: false,
      showSearch: true,
      deviceServiceInvocationsList: [],
      total: 0,
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        messageId: undefined,
        invocationStatus: [],
        requestTopic: undefined,
        responseTopic: undefined,
      },
      // 列信息，
      columns: [
        { key: 0, label: "id", visible: true },
        { key: 1, label: "消息来源", visible: true },
        { key: 2, label: "主叫设备", visible: true },
        { key: 3, label: "请求", visible: true },
        { key: 4, label: "响应", visible: true },
        { key: 5, label: "尝试次数", visible: true },
        { key: 6, label: "是否超时", visible: true },
        { key: 7, label: "原因", visible: true },
        { key: 8, label: "首次调用时间", visible: true },
        { key: 9, label: "末次调用时间", visible: true },
        { key: 10, label: "是否紧急", visible: true },
        { key: 11, label: "是否已处理", visible: true },
        { key: 12, label: "创建时间", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  methods: {
    getList() {
      this.loading = true;
      listDeviceServiceInvocations(this.queryParams).then((res) => {
        this.deviceServiceInvocationsList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
      });
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
  },
};
</script>

<style lang="scss" scoped></style>
