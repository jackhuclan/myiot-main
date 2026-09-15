<template>
  <div class="app-container">
    <el-tabs :tab-position="'left'" style="height: calc(100vh - 110px)">
      <el-tab-pane :label="'未读消息(2)'">
        <div class="msgContent">
          <p>通知内容</p>
          <p class="time">
            <span>通知时间</span><el-button size="mini">标记已读</el-button>
          </p>
        </div>
        <div class="msgContent">
          <p>通知内容</p>
          <p class="time">
            <span>通知时间</span><el-button size="mini">标记已读</el-button>
          </p>
        </div>
        <div class="msgContent">
          <p>通知内容</p>
          <p class="time">
            <span>通知时间</span><el-button size="mini">标记已读</el-button>
          </p>
        </div>
        <div class="msgContent">
          <p>通知内容</p>
          <p class="time">
            <span>通知时间</span><el-button size="mini">标记已读</el-button>
          </p>
        </div>
      </el-tab-pane>
      <el-tab-pane :label="'已读消息(2)'">已读消息</el-tab-pane>
    </el-tabs>
  </div>
</template>

<script>
export default {};
</script>
<style scoped lang="scss">
.msgContent {
  display: flex;
  align-items: center;
  width: 100%;
  justify-content: space-between;
  border-bottom: 1px #ccc solid;
  padding: 0 20px;
  .time {
    width: 300px;
    span {
      margin-right: 20px;
    }
  }
}
</style>
