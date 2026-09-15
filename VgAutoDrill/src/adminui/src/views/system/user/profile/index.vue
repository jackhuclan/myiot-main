<template>
  <div class="app-container">
    <el-row :gutter="20">
      <el-col :span="7" :xs="24">
        <el-card class="box-card">
          <div slot="header" class="clearfix">
            <span>个人信息</span>
          </div>
          <div>
            <!-- 头像上传 -->
            <!-- <div class="text-center">
              <userAvatar :user="user" />
            </div> -->
            <ul class="list-group list-group-striped">
              <li class="list-group-item">
                <div><svg-icon icon-class="user" /><span>账号名称</span></div>
                <div class="pull-right">{{ user.userName }}</div>
              </li>
              <li class="list-group-item">
                <div><svg-icon icon-class="phone" /><span>手机号码</span></div>
                <div class="pull-right">{{ user.mobile }}</div>
              </li>
              <li class="list-group-item">
                <div><svg-icon icon-class="email" /><span>用户邮箱</span></div>
                <div class="pull-right">{{ user.email }}</div>
              </li>
              <li class="list-group-item">
                <div><svg-icon icon-class="tree" /><span>所属部门</span></div>
                <div class="pull-right">{{ user.departmentName }}</div>
              </li>
              <!-- <li class="list-group-item">
              <div>
                <svg-icon icon-class="peoples" /><span>所属角色</span>
              </div>
                <div class="pull-right">{{ roleGroup }}</div>
              </li> -->
              <li class="list-group-item">
                <div><svg-icon icon-class="date" /><span>创建日期</span></div>
                <div class="pull-right">{{ parseTime(user.createTime) }}</div>
              </li>
            </ul>
          </div>
        </el-card>
      </el-col>
      <el-col :span="17" :xs="24">
        <el-card>
          <div slot="header" class="clearfix">
            <span>基本资料</span>
          </div>
          <el-tabs v-model="activeTab">
            <el-tab-pane label="基本资料" name="userinfo">
              <userInfo :user="user" />
            </el-tab-pane>
            <!-- user.id为1时是admin账户超级管理员 密码不可修改 -->
            <el-tab-pane label="修改密码" name="resetPwd" v-if="user.id != 1">
              <resetPwd :user="user" :userId="user.id" />
            </el-tab-pane>
          </el-tabs>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import userAvatar from "./userAvatar";
import userInfo from "./userInfo";
import resetPwd from "./resetPwd";
import { getUserProfile } from "@/api/system/user";

export default {
  name: "Profile",
  components: { userAvatar, userInfo, resetPwd },
  data() {
    return {
      user: {},
      roleGroup: {},
      postGroup: {},
      activeTab: "userinfo",
    };
  },
  created() {
    this.getUser();
  },
  methods: {
    getUser() {
      getUserProfile().then((response) => {
        this.user = response.data.user;
        this.roleGroup = response.roleGroup;
        this.postGroup = response.postGroup;
      });
    },
  },
};
</script>
