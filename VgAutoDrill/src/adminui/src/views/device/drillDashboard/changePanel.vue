<template>
  <el-dialog
    v-if="panelList.length > 0 && open"
    :title="code"
    :visible.sync="open"
    width="90%"
    append-to-body
    :close-on-click-modal="false"
    v-dialogClose
    v-dialogDrag
    custom-class="drill_dialog"
  >
    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          type="primary"
          v-debounce
          plain
          icon="el-icon-refresh" 
          @click="handleSynchronousData"
          >同步</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="success"
          plain
          icon="el-icon-finished" 
          @click="hanldeIssued"
          >下发</el-button
        >
      </el-col>
    </el-row>

    <div class="wrapper" v-if="panelList.length > 0">
      <div v-for="(item, index) in panelList" :key="index">
        <el-row :gutter="10" class="mb8" v-if="panelList.length > 0">
          <el-col :span="1.5">
            <el-button
              v-debounce
              type="primary"
              plain
              icon="el-icon-plus"
              @click="handleAdd(item)" 
              >批量生成</el-button
            >
          </el-col>
          <el-col :span="1.5">
            <el-button
              v-debounce
              type="danger"
              plain
              icon="el-icon-brush"
              @click="handleDelete(item.layer)" 
              >批量清空</el-button
            >
          </el-col></el-row
        >
        <el-table border :data="item.panelDetailDtos">
          <el-table-column
            align="center"
            width="65px"
            class-name="elChgTbeClmn"
            :resizable="false"
          >
            <template slot="header">
              <div class="elHeadCon">
                <div class="headerCon1">
                  {{ types.find((v) => v.value == item.layer).label }}
                </div>
                <input
                  type="checkbox"
                  class="headerCon2"
                  :class="'selectAll' + item.layer"
                  @input="(e) => handleSelectAll(e, item)"
                />
              </div>
            </template>
            <template slot-scope="scope">
              <input
                type="checkbox"
                :class="'select' + item.layer"
                @input="(e) => handleSelect(e, item, scope.row)"
              />
            </template>
          </el-table-column>
          <el-table-column
            :label="item.layer == 0 ? '轴号' : '层号'"
            align="center"
            prop="splindleIndex"
            width="50px"
            :resizable="false"
          >
          </el-table-column>
          <el-table-column label="板料编码" min-width="160px" prop="panelCode">
            <template slot-scope="scope">
              <el-input
                size="mini"
                :class="'agv_panelCode' + scope.$index"
                v-model="scope.row.panelCode"
                placeholder="请输入板料编码"
                @keyup.enter.native="
                  handleSelectPanelCode(
                    $event,
                    scope,
                    'agv_panelCode',
                    item.productStatus
                  )
                "
              ></el-input> </template
          ></el-table-column>
          <el-table-column
            label="物料"
            prop="itemCode"
            min-width="150px"
            show-overflow-tooltip
          >
          </el-table-column>
          <el-table-column
            label="操作"
            align="center"
            fixed="right"
            class-name="small-padding fixed-width"
          >
            <template slot-scope="scope">
              <el-button
                type="text"
                icon="el-icon-brush"
                @click="handleDelete(null, scope.row)" 
                >清空</el-button
              ></template
            >
          </el-table-column>
        </el-table>
      </div>
    </div>
    <el-empty description="暂无数据" v-else></el-empty>
    <edit-form-dialog
      v-model="addOpen"
      :title="addTitle"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form ref="form" :model="addForm" label-width="100px" :rules="rules">
        <el-row>
          <el-col :span="8">
            <el-form-item label="物料编码" prop="itemCode">
              <el-autocomplete
                v-model="addForm.itemCode"
                :fetch-suggestions="querySearchAsync"
                placeholder="请输入内容"
                @select="handleSelectItem"
                popper-class="query-suggestion"
                :popper-append-to-body="false"
                :debounce="0"
              >
                <el-button
                  v-debounce
                  slot="append"
                  @click="handleSelectProduct"
                  icon="el-icon-search"
                ></el-button
              ></el-autocomplete>
              <ItemSelect ref="ItemSelect" @onSelected="onItemSelected">
              </ItemSelect>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="批次号" prop="batchCode">
              <el-input
                v-model="addForm.batchCode"
                placeholder="请输入批次号"
              />
            </el-form-item> </el-col
          ><el-col :span="8">
            <el-form-item label="销钉偏移量">
              <el-input
                v-model="addForm.pinOffset"
                placeholder="请输入销钉偏移量"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="每叠块数" prop="pcs">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="addForm.pcs"
                @changeNum="changeNum"
                :numName="'pcs'"
                :min="1"
                dis
                :max="1000"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="板长">
              <input-number
                :myNum="addForm.panelLength"
                @changeNum="changeNum"
                :numName="'panelLength'"
                dis
                :min="0"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="板宽">
              <input-number
                :myNum="addForm.panelWidth"
                @changeNum="changeNum"
                :numName="'panelWidth'"
                dis
                :min="0"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item
              :label="`起始${addForm.layer == 0 ? '轴' : '层'}`"
              prop="beginSplindleNum"
            >
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="addForm.beginSplindleNum"
                @changeNum="changeNum"
                :numName="'beginSplindleNum'"
                ref="beginSplindleNum"
                :min="1"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item
              :label="`连续${addForm.layer == 0 ? '轴' : '层'}数`"
              prop="splindleCount"
            >
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="addForm.splindleCount"
                @changeNum="changeNum"
                :numName="'splindleCount'"
                ref="splindleCount"
                :min="0"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
  </el-dialog>
</template>

<script>
// 物料产品选择
import ItemSelect from "@/components/itemSelect";
import { listItem } from "@/api/masterData/item";
import {
  getPanelList,
  batchAddOrUpdateData,
  updateDrillPanelDetail,
  clearData,
  loadPanelDetailData,
} from "@/api/device/device";
import {
  synchronousPanelData,
  allotsPanelData,
} from "@/api/device/schedulement";
export default {
  props: ["code"],
  components: { ItemSelect },
  data() {
    const validatePanelWidth = (rule, value, callback) => {
      const str = value + "";
      if (isNaN(Number(str)) || str == undefined || str.trim() === "") {
        //当输入不是数字的时候，Number后返回的值是NaN;然后用isNaN判断。
        callback(new Error("请输入数字"));
      }
      callback();
    };
    const checkItemCode = async (rule, value, callback) => {
      const res = await listItem({
        pageNum: 1,
        pageSize: 1000,
        name: undefined,
        code: value,
        // 只允许选择产品物料
        itemOrProduct: "2",
        status: 1,
      });
      if (!value) {
        return callback(new Error("产品编码不能为空"));
      } else if (res.data.list.length == 0) {
        return callback(new Error("产品编码不存在,请重新输入或选择"));
      }
      callback();
    };
    const checkSplindleCount = (rule, value, callback) => {
      if (this.addForm.splindleCount <= 0) {
        return callback(new Error("数量不能小于等于0"));
      }
      callback();
    };
    return {
      open: false,
      panelList: [],
      multipleSelection: [],
      splindleIndexs: [],
      types: [
        {
          label: "生料仓",
          value: 0,
        },
        {
          label: "钻机",
          value: 1,
        },
        {
          label: "熟料仓",
          value: 2,
        },
      ],
      type: null,
      //   批量生成
      addOpen: false,
      addTitle: "",
      addForm: {},
      initialForm: {},
      rules: {
        pinOffset: [
          { required: true, validator: validatePanelWidth, trigger: "change" },
        ],
        itemCode: [
          { required: true, validator: checkItemCode, trigger: "change" },
        ],
        splindleCount: [
          { required: true, validator: checkSplindleCount, trigger: "change" },
        ],
      },
    };
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.addForm) !== JSON.stringify(this.initialForm);
    },
  },

  watch: {
    "addForm.itemCode": {
      handler(val) {
        if (!val) {
          this.addForm.pcs = 0;
          this.addForm.panelWidth = 0;
          this.addForm.panelLength = 0;
        }
      },
    },
    open(val) {
      if (!val) {
        this.multipleSelection = [];
        this.$emit("changeSetInterval");
        // this.getList(this.code);
        document
          .querySelectorAll(".wrapper input[type=checkbox]")
          .forEach((v) => {
            v.checked = false;
          });
      }
    },
    multipleSelection(val) {
      this.splindleIndexs = val.map((v) => v.splindleIndex);
    },
    panelList(val) {
      if (val.length <= 0 && !this.open) {
        this.$modal
          .confirm("暂无数据,请加载初始数据！")
          .then((result) => {
            if (result == "confirm") {
              loadPanelDetailData({ deviceCode: this.code })
                .then((res) => {
                  if (res.code == 0) {
                    setTimeout(() => {
                      this.getList(this.code);
                    }, 500);
                  } else {
                    this.$modal.notifyError(res.message);
                  }
                })
                .catch(() => {});
            }
          })

          .catch(() => {});
      } else {
        this.open = true;
      }
    },
  },
  methods: {
    getList(deviceCode) {
      getPanelList({ deviceCode }).then((res) => {
        this.panelList = res.data;
      });
      document
        .querySelectorAll(".wrapper input[type=checkbox]")
        .forEach((v) => {
          v.checked = false;
        });
    },
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.multipleSelection = selection;
      this.splindleIndexs = selection.map((item) => item.id);
    },
    // 多选框选中数据
    handleSelectAll(e, val) {
      const selectAll = e.target.checked;
      if (selectAll) {
        this.multipleSelection = val.panelDetailDtos.map((item) => {
          return {
            ...item,
            mark: val.layer,
          };
        });
      } else {
        const id_List = val.panelDetailDtos.map((v) => v.id);
        this.multipleSelection = this.multipleSelection.filter(
          (item) => !id_List.includes(item.id)
        );
      }
      this.type = val.layer;

      const doms = document.querySelectorAll(".wrapper input[type=checkbox]");
      doms.forEach((v) => {
        if (
          !v.classList.contains("select" + val.layer) &&
          !v.classList.contains("headerCon2")
        ) {
          v.checked = false;
        } else v.checked = selectAll;
      });
      document.querySelectorAll(".headerCon2 ").forEach((v) => {
        if (!v.classList.contains("selectAll" + val.layer)) {
          v.checked = false;
        }
      });
    },
    // 反选
    handleSelect(e, item, row) {
      const selected = e.target.checked;

      if (selected) {
        this.multipleSelection.push({ ...row, mark: item.layer });
        this.multipleSelection = this.multipleSelection.filter(
          (v) => v.mark == item.layer
        );
      } else {
        this.multipleSelection = this.multipleSelection.filter(
          (v) => v.id != row.id
        );
      }
      this.type = item.layer;
      const dom = document.querySelector(".selectAll" + item.layer);
      document
        .querySelectorAll(".wrapper input[type=checkbox]")
        .forEach((v) => {
          if (!v.classList.contains("select" + item.layer)) {
            v.checked = false;
          }
        });
      const length = this.multipleSelection.filter(
        (v) => v.mark == item.layer
      ).length;
      if (length == item.panelDetailDtos.length) {
        dom.checked = true;
      } else {
        dom.checked = false;
      }
    },
    // 表单重置
    reset() {
      this.addForm = {
        beginSplindleNum: 1,
        splindleCount: 0,
        pcs: 0,
        panelLength: 0,
        panelWidth: 0,
        pinOffset: "0",
        itemCode: "",
        productStatus: 0,
        layer: 0,
        batchCode: "",
      };
      this.resetForm("form");
    },
    handleAdd(val) {
      this.reset();
      let length = this.panelList.find((v) => v.layer == val.layer)
        ?.panelDetailDtos.length;
      const label = this.types.find((v) => v.value == val.layer)?.label;

      this.addForm.layer = val.layer;
      this.addOpen = true;
      this.addTitle = `生成${label}板料(总${
        val == 0 ? "轴" : "层"
      }数${length})`;
      this.initialForm = Object.assign({}, this.addForm);
      this.$nextTick(() => {
        this.$refs.beginSplindleNum.currentMax = length;
        this.$refs.beginSplindleNum.maxDisabled = false;
        this.$refs.splindleCount.currentMax =
          length + 1 - this.addForm.beginSplindleNum;
        this.$refs.splindleCount.maxDisabled = false;
      });
    },
    handleDelete(type, row) {
      const label = this.types.find((v) => v.value == this.type)?.label;
      let layer = null,
        confirmText = null;
      if (!row && this.splindleIndexs.length <= 0)
        return this.$modal.msgWarning("请选择要操作的数据！");
      if (!row && type != this.type)
        return this.$modal.msgError(
          "操作错误，当前选中为" + label + "下数据！"
        );
      if (row) {
        layer = row.layer;
        confirmText = `确定清空 <span style="color:red">${
          layer == 0 ? "轴号" : "层号"
        }为 ${row.splindleIndex}</span> 的数据项？`;
      } else {
        layer = this.multipleSelection[0]?.layer;
        confirmText = "确定清空当前操作的数据项？";
      }
      const splindleIndexs = row?.splindleIndex
        ? [row?.splindleIndex]
        : this.splindleIndexs;
      const obj = {
        deviceCode: this.code,
        layer,
        splindleIndexs,
      };
      this.$modal
        .confirm(confirmText, {
          dangerouslyUseHTMLString: true, // 使用HTML片段
        })
        .then((result) => {
          if (result == "confirm") {
            clearData(obj).then((res) => {
              if (res.code == 0) {
                this.$modal.msgSuccess("清空成功");
                this.getList(this.code);
              } else {
                this.$notify({
                  type: "error",
                  title: "提示",
                  message: res.message,
                });
              }
            });
          }
        })
        .catch(() => {});
    },
    querySearchAsync(queryString, cb) {
      if (queryString === "" || !queryString) {
        let arr = [];
        cb(arr);
      } else {
        clearTimeout(this.timeout);
        this.timeout = setTimeout(() => {
          listItem({
            pageNum: 1,
            pageSize: 1000,
            name: undefined,
            code: queryString,
            // 只允许选择产品物料
            itemOrProduct: "2",
            status: 1,
          }).then((res) => {
            if (res.code === 0) {
              let arr = res.data.list.map((v) => {
                return {
                  value: v.code,
                  ...v,
                };
              });
              const showSuggestion =
                document.querySelector(".query-suggestion");
              cb(arr);
              if (arr.length > 0) {
                showSuggestion.style.display = "block";
              }
            } else {
              this.$notify({
                type: "error",
                title: "提示",
                message: res.message,
              });
            }
          });
        }, 500);
      }
    },
    handleSelectItem(obj) {
      this.$set(this.addForm, "pcs", obj.panelCount);
      this.$set(this.addForm, "panelLength", obj.panelLength);
      this.$set(this.addForm, "panelWidth", obj.panelWidth);
      this.$set(this.addForm, "itemCode", obj.code);
    },
    //物料选择弹出框
    handleSelectProduct() {
      this.$refs.ItemSelect.showFlag = true;
      this.$refs.ItemSelect.selectedItemCode = this.addForm.itemCode
        ? this.addForm.itemCode
        : undefined;
      this.$refs.ItemSelect.title = "物料选择";
      this.$refs.ItemSelect.getList();
    },
    onItemSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.addForm, "pcs", obj.panelCount);
        this.$set(this.addForm, "panelLength", obj.panelLength);
        this.$set(this.addForm, "panelWidth", obj.panelWidth);
        this.$set(this.addForm, "itemCode", obj.code);
      }
    },
    // 点击计数器
    changeNum(params) {
      this.addForm[params.str] = params.value;
      const length = this.panelList.find((v) => v.layer == this.addForm.layer)
        ?.panelDetailDtos.length;
      const count = length + 1 - this.addForm.beginSplindleNum;
      this.$refs.splindleCount.currentMax = count;
      this.$refs.splindleCount.maxDisabled = false;
      if (this.addForm.splindleCount >= count) {
        this.$forceUpdate();
        this.addForm.splindleCount = count;
        this.$set(this.addForm, "splindleCount", count);
      }
    },
    // 批量生成提交
    submitForm() {
      this.addForm.pinOffset = this.addForm.pinOffset * 1;
      batchAddOrUpdateData({
        ...this.addForm,
        deviceCode: this.code,
      }).then((res) => {
        if (res.code == 0) {
          this.$modal.msgSuccess("操作成功");
          this.addOpen = false;
          this.getList(this.code);
        } else {
          this.$notify({
            type: "error",
            title: "提示",
            message: res.message,
          });
        }
      });
    },

    // 板料回车
    handleSelectPanelCode(event, scope, className, productStatus) {
      const nextDom = document.querySelector(
        "." + className + (scope.$index + 1)
      );
      const obj = { ...scope.row };
      updateDrillPanelDetail(obj).then((res) => {
        if (res.code == 0) {
          if (nextDom) {
            nextDom.querySelector("input").focus(); // 自动获取焦点
          } else {
            event.target.blur(); //失去焦点
          }
          this.$modal.msgSuccess("操作成功");
          this.getList(this.code);
        } else {
          this.$notify({
            type: "error",
            title: "提示",
            message: res.message,
          });
          event.target.focus(); // 自动获取焦点
          event.target.select();
        }
      });
    },
    // 同步
    handleSynchronousData() {
      synchronousPanelData({
        deviceCode: this.code,
      }).then((res) => {
        if (res.code != 0) return this.$modal.notifyError(res.message);
        this.getList(this.code);
        this.$modal.msgSuccess("同步成功");
      });
    },
    // 下发
    hanldeIssued() {
      const layers = this.panelList.map((v) => v.layer);
      allotsPanelData({
        deviceCode: this.code,
        layers,
      }).then((res) => {
        if (res.code != 0) return this.$modal.notifyError(res.message);
        this.getList(this.code);
        this.$modal.msgSuccess("下发成功");
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.wrapper {
  display: flex;
  overflow-x: auto;
  overflow-y: hidden;
  padding: 0 2px;
  > div {
    flex: 1;
    &:not(&:first-child) {
      padding-left: 10px;
    }
  }
}
::v-deep .drill_dialog .el-dialog__body {
  padding: 0px 20px 10px 20px !important;
}
/* 如果单元格的padding */
::v-deep .el-table__header .elChgTbeClmn .cell {
  padding: 0px !important;
}
::v-deep .elChgTbeClmn {
  padding: 0px !important;
}
.elHeadCon {
  position: relative;
  width: 100%;
  height: 40px;
  box-sizing: border-box;
  line-height: 60px;
  background: linear-gradient(
    to bottom right,
    transparent 0%,
    transparent calc(50% - 1px),
    #dfe6ec 50%,
    transparent calc(50% + 1px),
    transparent 100%
  );
}

.headerCon1 {
  position: absolute;
  left: 2px;
  bottom: 0;
  color: #d4515e;
}
.headerCon2 {
  position: absolute;
  left: 50%;
  transform: translateX(-50%);
  bottom: 1px;
}
::v-deep .el-radio {
  display: block !important;
  margin: 5px 0 5px 7px !important;
}
</style>
