<template>
  <edit-form-dialog
    v-model="open"
    title="生成检验记录"
    :isFormModified="isFormModified"
    @submitForm="submitForm"
  >
    <el-form ref="form" :model="form" :rules="rules" label-width="100px">
      <el-row>
        <el-col :span="8">
          <el-form-item label="任务编码" prop="taskCode">
            <el-input
              v-model="form.taskCode"
              disabled
              placeholder="请输入任务编码"
            >
            </el-input>
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="工单编码" prop="workOrderCode">
            <el-input
              v-model="form.workOrderCode"
              disabled
              placeholder="请输入工单编码"
            >
            </el-input>
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="工单名称" prop="workOrderName">
            <el-input
              v-model="form.workOrderName"
              disabled
              placeholder="请输入工单名称"
            >
            </el-input>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="8">
          <el-form-item label="检验人" prop="userName">
            <el-input v-model="form.userName" placeholder="请选择检验人">
              <el-button
                v-debounce
                slot="append"
                @click="handleUser3Select"
                icon="el-icon-search"
              ></el-button>
            </el-input>
          </el-form-item>
          <UserSingleSelect
            ref="user3Select"
            @onSelected="onUser3Selected"
          ></UserSingleSelect>
        </el-col>
        <el-col :span="8">
          <el-form-item label="是否合格">
            <el-radio-group v-removeAriaHidden v-model="form.isCheckOk">
              <el-radio :label="'1'">合格</el-radio>
              <el-radio :label="'0'">不合格</el-radio>
            </el-radio-group>
          </el-form-item>
        </el-col>
      </el-row>
      <el-form-item label="备注" prop="remark">
        <el-input
          type="textarea"
          v-model="form.remark"
          placeholder="请输入备注"
          :autosize="{ minRows: 2, maxRows: 10 }"
          :resize="optType == 'view' ? 'none' : ''"
        ></el-input>
      </el-form-item>
    </el-form>
  </edit-form-dialog>
</template>

<script>
import UserSingleSelect from "@/components/userSelect";
import { addCheckRecords } from "@/api/produce/task";
export default {
  data() {
    return {
      optType:'',
      open: false,
      form: {},
      initialForm: {},
      rules: {
        feedBackType: [
          { required: true, message: "报工类型不能为空", trigger: "change" },
        ],
        taskCode: [
          { required: true, message: "请选择生产任务", trigger: "blur" },
        ],
      },
    };
  },
  components: { UserSingleSelect },
  watch: {
    "form.userName": {
      handler(val) {
        if (val == "") {
          this.form.userName = undefined;
        }
      },
      deep: true,
    },
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
  },
  methods: {
    //点击审核人选择按钮
    handleUser3Select() {
      this.$refs.user3Select.showFlag = true;
      this.$refs.user3Select.selectedId = this.form.userName
        ? this.form.userName
        : undefined;
      this.$refs.user3Select.getList();
      this.$refs.user3Select.getTreeselect();
    },
    //审核人选择返回
    onUser3Selected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "userName", obj.realName);
      }
    },
    /** 提交检验记录按钮 */
    submitForm() {
      addCheckRecords(this.form).then((res) => {
        if (res.code == 0) {
          this.open = false;
          this.$modal.msgSuccess("操作成功");
          this.$emit("parent_getList");
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
  },
};
</script>
