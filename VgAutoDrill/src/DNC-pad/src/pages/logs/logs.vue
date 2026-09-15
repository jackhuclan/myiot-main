<template>
  <view>
    <view class="uni-header">
      <view class="uni-group">
        <!-- 输入框 -->
        <uni-easyinput
          style="width: 268px; margin-right: 10px"
          v-model="searchVal"
          @confirm="search"
          placeholder="请输入钻机编号"
        />
        <!-- 搜索按钮 -->
        <button type="primary" size="mini" @click="search">搜索</button>
      </view>
    </view>

    <view class="uni-container">
      <!-- 表格组件 -->
      <!--  #ifdef  H5 -->

      <uni-table :loading="loading" border stripe emptyText="没有更多数据">
        <uni-tr>
          <uni-th min-width="100" align="center">钻机编号</uni-th>
          <uni-th min-width="100" align="center">库位编号</uni-th>
          <uni-th min-width="100" align="center">物料编号</uni-th>
          <uni-th min-width="100" align="center">托盘号</uni-th>
          <uni-th width="100" align="center">操作</uni-th>
          <uni-th min-width="100" align="center">创建人</uni-th>
          <uni-th min-width="180" align="center">创建时间</uni-th>
        </uni-tr>
        <uni-tr v-for="(item, index) in tableData" :key="index">
          <uni-td align="center">{{ item.deviceCode }}</uni-td>
          <uni-td align="center">{{ item.locationCode }}</uni-td>
          <uni-td align="center">{{ item.itemCode }}</uni-td>
          <uni-td align="center">{{ item.podCode }}</uni-td>
          <uni-td align="center">
            {{ item.agvOperateName }}
          </uni-td>
          <uni-td align="center">{{ item.creator }}</uni-td>
          <uni-td align="center">{{ item.createTime }}</uni-td>
        </uni-tr>
      </uni-table>
      <!--  #endif -->
      <!--  #ifdef  APP -->
      <uni-card
        v-for="(item, index) in tableData"
        :key="index"
        sub-title="设备编码"
        :title="item.locationCode"
        :extra="item.agvOperateName"
      >
        <view>
          <text>物料编号：</text> <text>{{ item.itemCode }}</text>
        </view>
        <view>
          <text>托盘号：</text> <text>{{ item.podCode }}</text>
        </view>
        <view>
          <text>创建人：</text> <text>{{ item.creator }}</text>
        </view>
        <view>
          <text>创建时间：</text> <text>{{ item.createTime }}</text>
        </view>
      </uni-card>
      <!--  #endif -->
    </view>
  </view>
</template>

<script>
import { listLog } from "@/api/logs.js";
// 导出默认模块
export default {
  // 数据属性
  data() {
    return {
      // 搜索值
      searchVal: "",
      // 表格数据
      tableData: [],
      // 加载状态
      loading: false,
      queryForm: {
        pageNum: 1,
        pageSize: 1000,
        itemCode: undefined,
        locationCode: undefined,
        podCode: undefined,
        agvOperateType: undefined,
      },
    };
  },

  // 页面加载时的处理函数
  onLoad() {
    // 获取第一页数据
    this.getList();
  },

  // 方法
  methods: {
    // 搜索函数
    search() {
      this.getList(this.searchVal);
    },
    getList(value = "") {
      this.loading = true;
      listLog(this.queryForm).then((res) => {
        let data = res.data.list;

        if (value) {
          data = [];
          res.data.list.forEach((item) => {
            if (item.deviceCode.indexOf(value) !== -1) {
              data.push(item);
            }
          });
        }
        this.tableData = data;
        this.loading = false;
      });
    },
  },
};
</script>
