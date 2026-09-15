<template>
  <div>
    <el-form :inline="true" :model="topForm">
      <el-form-item label="料仓">
        <el-input
          disabled
          v-model="topForm.siloCode"
          placeholder="请选择要绑定的料仓"
          class="siloCode_input"
        >
          <el-button
            v-debounce
            slot="append"
            @click="handleSelectSiloManage"
            icon="el-icon-search"
          ></el-button>
        </el-input>
        <siloManageSelect
          ref="siloManageSelect"
          @onSelected="onsiloManageSelected"
        >
        </siloManageSelect>
      </el-form-item>
    </el-form>
    <div>
      <el-row :gutter="10" class="mb8">
        <el-col :span="1.5">
          <el-button
            v-debounce
            type="primary"
            plain
            icon="el-icon-refresh"
            @click="handleAdd"
            :disabled="hasPermi(['device:device:batchAdd'])"
            >批量生成</el-button
          >
        </el-col>
        <el-col :span="1.5">
          <el-button
            v-debounce
            type="danger"
            plain
            icon="el-icon-brush"
            @click="handleClear('all')"
            :disabled="hasPermi(['device:device:batchClear'])"
            >批量清空</el-button
          >
        </el-col>

        <el-col :span="1.5">
          <el-button
            v-debounce
            type="success"
            plain
            icon="el-icon-finished"
            :disabled="hasPermi(['device:device:issued'])"
            @click="hanldeIssued"
            >下发</el-button
          >
        </el-col>
      </el-row>
      <!-- 插槽--针对不同页面按钮 -->
      <slot />
    </div>

    <el-table
      border
      :max-height="maxHeight"
      :data="panelList"
      v-loading="loading"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
      :ref="page"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="层号"
        align="center"
        prop="floorNum"
        show-overflow-tooltip
      />

      <el-table-column label="板料编码" min-width="200" prop="panelCode">
        <template slot-scope="scope">
          <el-input
            :class="'panelCode' + scope.$index"
            v-model="scope.row.panelCode"
            placeholder="请输入板料编码"
            @input="changeInpput($event, scope.row)"
            @keyup.enter.native="handleSelectPanelCode($event, scope)"
          ></el-input>
        </template>
      </el-table-column>
      <el-table-column
        label="板料类型"
        min-width="110"
        align="center"
        prop="productStatus"
      >
        <template slot-scope="scope">
          <status-tag
            :options="productStatusOptions"
            :status="scope.row.productStatus * 1"
          />
        </template>
      </el-table-column>
      <el-table-column label="物料编码" min-width="200" prop="itemCode">
      </el-table-column>
      <el-table-column label="每叠块数" align="center" prop="pcs" />
      <el-table-column label="板宽" align="center" prop="panelWidth" />
      <el-table-column
        label="销钉偏移量"
        min-width="120px"
        align="center"
        prop="pinOffset"
      />
      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        min-width="140px"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <el-button
            type="text"
            icon="el-icon-brush"
            @click="handleClear(scope.row)"
            :disabled="
              hasPermi(['device:device:batchClear']) || !scope.row.panelCode
            "
            >清空</el-button
          >
        </template>
      </el-table-column>
    </el-table>
    <!-- 批量生成弹出框 -->
    <edit-form-dialog
      v-model="open"
      :title="title"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form ref="form" :model="form" label-width="100px" :rules="rules">
        <el-row>
          <el-col :span="8">
            <el-form-item label="料仓编码" prop="siloCode">
              <el-input
                v-model="form.siloCode"
                placeholder="请输入料仓编码"
                disabled
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="物料编码" prop="itemCode">
              <el-autocomplete
                v-model="form.itemCode"
                :fetch-suggestions="querySearchAsync"
                placeholder="请输入内容"
                @select="handleSelect"
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
            <el-form-item label="板料类型" prop="productStatus">
              <el-select
                @clear="clearQueryParams('productStatus')"
                v-model="form.productStatus"
                placeholder="请选择"
                clearable
              >
                <el-option
                  v-for="item in productStatusOptions"
                  :key="item.value"
                  :label="item.name"
                  :value="item.value"
                  v-optionTitle
                />
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="销钉偏移量" prop="pinOffset">
              <el-input
                v-model="form.pinOffset"
                placeholder="请输入销钉偏移量"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="板宽" prop="panelWidth">
              <el-input v-model="form.panelWidth" placeholder="请输入板宽" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="批次号" prop="batchCode">
              <el-input v-model="form.batchCode" placeholder="请输入板宽" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="每叠块数" prop="pcs">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.pcs"
                @changeNum="changeNum"
                :numName="'pcs'"
                :min="1"
                :max="1000"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="起始层" prop="beginFloorNum">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.beginFloorNum"
                @changeNum="changeNum"
                :numName="'beginFloorNum'"
                ref="beginFloorNum"
                :min="1"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="连续层数" prop="floorCount">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.floorCount"
                @changeNum="changeNum"
                :numName="'floorCount'"
                ref="floorCount"
                :min="1"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import { listItem } from "@/api/masterData/item";
// 物料产品选择
import ItemSelect from "@/components/itemSelect";
import { getCentralRackPanels, updateRack } from "@/api/wareHouse/rack";
import {
  addOrUpdateSiloDetails,
  bindSingle,
  unBindPanel,
  movePanelToOtherSilo,
} from "@/api/wareHouse/silo";
import countDown from "@/components/countDown"; //引入路径，可更改
import siloManageSelect from "@/components/siloManageSelect";
import { allotsPanelData } from "@/api/device/schedulement";

export default {
  components: { countDown, siloManageSelect, ItemSelect },
  name: "SetForm",
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
    const checkProductStatus = async (rule, value, callback) => {
      if (!value || value == "-1") {
        return callback(new Error("请选择板料类型"));
      }
      callback();
    };
    return {
      page: "changePanel",
      showFlag: false,
      dialogTitle: "",
      topForm: {},
      open: false,
      form: {},
      initialForm: {},
      title: "",
      loading: false,
      maxHeight: 0,
      panelList: [],
      // 判断数据有没有变化
      initialPanelList: [],
      // 是否下发
      issued: false,
      // 表格选中
      multipleSelection: [],
      ids: [],
      // 查询参数
      queryParams: {
        siloCode: undefined,
      },
      // 表单验证
      rules: {
        panelWidth: [
          { required: true, validator: validatePanelWidth, trigger: "change" },
        ],
        pinOffset: [
          { required: true, validator: validatePanelWidth, trigger: "change" },
        ],
        productStatus: [
          { required: true, validator: checkProductStatus, trigger: "change" },
        ],
        itemCode: [
          { required: true, validator: checkItemCode, trigger: "change" },
        ],
      },
      // 层数
      productStatusOptions: this.$status.productStatusOptions.map((v) => {
        return {
          ...v,
          name: v.label + (v.value != -1 ? "(" + v.value + ")" : ""),
        };
      }),
    };
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
    isListModified() {
      const list = JSON.parse(JSON.stringify(this.panelList)).map((v) => {
        if (v.isInput) {
          v.panelCode = v.inputValue;
        }
        delete v.isInput;
        return { ...v };
      });
      return JSON.stringify(list) !== JSON.stringify(this.initialPanelList);
    },
  },
  watch: {
    "form.itemCode": {
      handler(val) {
        if (!val) {
          this.form.itemId = undefined;
          this.form.itemName = "";
        }
      },
    },
  },
  methods: {
    // 获取板料数据
    getList(val) {
      if (val == "issued") {
        this.issued = false;
      }
      this.loading = true;

      getCentralRackPanels(val.code || this.topForm.code).then((res) => {
        if (res.code == 0) {
          const list = res.data.map((v) => {
            return {
              ...v,
              floorNum: v.floorNum + 1,
              inputValue: v.panelCode,
            };
          });
          this.panelList = list;
          this.initialPanelList = Object.assign(
            [],
            res.data.map((v) => {
              return {
                ...v,
                floorNum: v.floorNum + 1,
                inputValue: v.panelCode,
              };
            })
          );
        } else {
          this.panelList = [];
        }
        this.loading = false;
        this.maxHeight =
          window.innerHeight - 400 < 400 ? 400 : window.innerHeight - 400;
      });
    },
    //监听row-dblclick事件，实现选中
    rowDblclick(row, column, event) {
      this.handleDoubleClick(row, this, column);
    },
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.multipleSelection = selection;
      this.ids = selection.map((item) => item.floorNum);
    },
    // 表单重置
    reset() {
      this.form = {
        beginFloorNum: 1,
        floorCount: 1,
        siloCode: "",
        pcs: 1,
        panelWidth: "620",
        pinOffset: "0",
        productStatus: -1,
        panelCode: "",
        itemCode: "",
        itemName: "",
      };
      this.resetForm("form");
    },
    //料仓弹出框
    handleSelectSiloManage() {
      this.$refs.siloManageSelect.showFlag = true;
      this.$refs.siloManageSelect.selectedSiloId = this.topForm.siloCode
        ? this.topForm.siloCode
        : undefined;
      this.$refs.siloManageSelect.getList();
    },
    onsiloManageSelected(obj) {
      if (obj != undefined && obj != null) {
        updateRack({ ...this.topForm, siloCode: obj.code }).then((res) => {
          if (res.code == 0) {
            this.$set(this.topForm, "siloCode", obj.code);
            this.$set(this.queryParams, "siloCode", obj.code);
            this.$modal.msgSuccess("绑定成功");
            this.$emit("getRackList");
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 新增
    handleAdd() {
      this.reset();
      this.open = true;
      this.form.siloCode = this.queryParams.siloCode;
      this.title = "批量生成(总层数" + this.panelList.length + ")";
      this.initialForm = Object.assign({}, this.form);
      this.$nextTick(() => {
        this.$refs.beginFloorNum.currentMax = this.panelList.length;
        this.$refs.beginFloorNum.maxDisabled = false;
        this.$refs.floorCount.currentMax =
          this.panelList.length + 1 - this.form.beginFloorNum;
        this.$refs.floorCount.maxDisabled = false;
      });
    },
    // 操作输入物料编码
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
              this.$modal.notifyError(res.message);
            }
          });
        }, 500);
      }
    },
    handleSelect(obj) {
      this.$set(this.form, "itemId", obj.id);
      this.$set(this.form, "itemCode", obj.code);
    },
    //物料选择弹出框
    handleSelectProduct() {
      this.$refs.ItemSelect.showFlag = true;
      this.$refs.ItemSelect.selectedItemCode = this.form.itemCode
        ? this.form.itemCode
        : undefined;
      this.$refs.ItemSelect.title = "物料选择";
      this.$refs.ItemSelect.getList();
    },
    onItemSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "itemCode", obj.code);
        this.$set(this.form, "itemId", obj.id);
        this.$set(this.form, "itemName", obj.name);
      }
    },
    // 点击计数器
    changeNum(params) {
      this.form[params.str] = params.value;
      const count = this.panelList.length + 1 - this.form.beginFloorNum;
      this.$refs.floorCount.currentMax = count;
      this.$refs.floorCount.maxDisabled = false;
      if (this.form.floorCount >= count) {
        this.$forceUpdate();
        this.form.floorCount = count;
        this.$set(this.form, "floorCount", count);
      }
    },

    // 批量生成提交
    submitForm() {
      this.form.pinOffset = this.form.pinOffset * 1;
      this.form.panelWidth = this.form.panelWidth * 1;
      addOrUpdateSiloDetails({
        ...this.form,
        // 首字母大写
        siloCode:
          this.panelList[0]?.siloCode ||
          this.topForm.siloCode.replace(
            this.topForm.siloCode[0],
            this.topForm.siloCode[0].toUpperCase()
          ),
        pcs: this.form.pcs ? this.form.pcs + "" : "0",
      }).then((res) => {
        if (res.code == 0) {
          this.panelList = res.data;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
      this.open = false;
    },

    // 板料弹框关闭时
    cancel() {
      if (!this.isListModified && !this.issued) return (this.showFlag = false);
      this.$modal
        .confirm(
          `数据发生<span  style='color:red'>变化</span>,未下发${"<span  style='color:red'>将不产生改变</span>"},是否下发？`,
          {
            dangerouslyUseHTMLString: true, // 使用HTML片段
            confirmButtonText: "下发",
            // 是否将取消（点击取消按钮）与关闭（点击关闭按钮或遮罩层、按下 ESC 键）进行区分
            distinguishCancelAndClose: true,
          }
        )
        .then((res) => {
          const data = {
            rackCode: this.topForm.code,
            deviceCode: this.topForm.relateDeviceCode,
          };
          allotsPanelData(data).then((res) => {
            if (res.code != 0) {
              this.$modal.notifyError(res.message);
            } else {
              this.issued = true;
              this.$modal.msgSuccess("下发成功");
              this.getList("issued");
              setTimeout(() => {
                this.showFlag = false;
              }, 500);
            }
          });
        })
        .catch((action) => {
          // 放弃保存并离开页面
          if (action === "cancel") return (this.showFlag = false);
        });
    },
    // 只输入不回车
    changeInpput(e, row) {
      row.isInput = true;
    }, // 移动板料
    movePanel(event, label, row) {
      this.$modal
        .confirm(label, {
          dangerouslyUseHTMLString: true, // 使用HTML片段
          // 是否将取消（点击取消按钮）与关闭（点击关闭按钮或遮罩层、按下 ESC 键）进行区分
          distinguishCancelAndClose: true,
        })
        .then((result) => {
          movePanelToOtherSilo({
            targetSiloCode: this.topForm.siloCode.replace(
              this.topForm.siloCode[0],
              this.topForm.siloCode[0].toUpperCase()
            ),
            targetFloor: row.floorNum - 1,
            panelCode: row.panelCode,
          }).then((res) => {
            this.panelList = res.data;
          });
          if (nextDom) {
            nextDom.querySelector("input").focus(); // 自动获取焦点
          } else {
            event.target.blur(); //失去焦点
          }
        })
        .catch((action) => {
          // 放弃保存并离开页面
          if (action === "cancel") return (row.panelCode = "");
        });
    },
    // 板料回车
    handleSelectPanelCode(event, scope) {
      delete scope.row.isInput;
      let label = "";
      // 非空判断
      if (
        scope.row.panelCode == null ||
        !/\S/.test(scope.row.panelCode) ||
        scope.row.panelCode == scope.row.inputValue
      )
        return;
      const nextDom = document.querySelector(".panelCode" + (scope.$index + 1));
      bindSingle({
        // 首字母大写
        siloCode: this.topForm.siloCode.replace(
          this.topForm.siloCode[0],
          this.topForm.siloCode[0].toUpperCase()
        ),
        floorNum: scope.row.floorNum,
        isRaw: true,
        panelCode: scope.row.panelCode,
      }).then((res) => {
        if (res.code == 0) {
          if (nextDom) {
            nextDom.querySelector("input").focus(); // 自动获取焦点
          } else {
            event.target.blur(); //失去焦点
          }
          this.$modal.msgSuccess("操作成功");
        } else {
          if (res.message.includes("已存在于料仓")) {
            label = `<span style="color:red">${scope.row.panelCode}</span>---${res.message}是否移动至该料仓？`;
            this.movePanel(event, label, scope.row);
          } else if (res.message.includes("已存在本料仓")) {
            label = `<span style="color:red">${scope.row.panelCode}</span>---${res.message}是否移动至该层？`;
            this.movePanel(event, label, scope.row);
          } else {
            this.$modal.notifyError(scope.row.panelCode + "---" + res.message);
            scope.row.panelCode = "";
          }

          event.target.focus(); // 自动获取焦点
          event.target.select();
        }
      });
    },
    // 点击清空
    handleClear(row) {
      if (row == "all" && this.ids.length <= 0)
        return this.$modal.msgWarning("请选择要操作的数据");
      let label =
        row == "all"
          ? "确定清空当前操作的数据项？"
          : `确定清空 <span style="color:red">层号为 ${row.floorNum}</span> 的数据项？`;
      let unBindSiloDetails =
        row == "all"
          ? this.multipleSelection.map((v) => {
              return { floorNum: v.floorNum, siloCode: v.siloCode };
            })
          : [{ floorNum: row.floorNum, siloCode: row.siloCode }];

      this.$modal
        .confirm(label, {
          dangerouslyUseHTMLString: true, // 使用HTML片段
        })
        .then((result) => {
          if (result == "confirm") {
            if (row == "all") {
              this.panelList = this.panelList.map((v) => {
                if (this.ids.includes(v.floorNum)) {
                  return {
                    siloCode: v.siloCode,
                    itemCode: "",
                    panelCode: "",
                    floorNum: v.floorNum,
                    productStatus: 1,
                    pcs: "0",
                    panelWidth: 0,
                    pinOffset: 0,
                    siloStatus: 0,
                    id: 0,
                    status: 1,
                  };
                }
                return v;
              });
            } else {
              this.panelList = this.panelList.map((v) => {
                if (v.floorNum == row.floorNum) {
                  return {
                    siloCode: v.siloCode,
                    itemCode: "",
                    panelCode: "",
                    floorNum: v.floorNum,
                    productStatus: 1,
                    pcs: "0",
                    panelWidth: 0,
                    pinOffset: 0,
                    siloStatus: 0,
                    id: 0,
                    status: 1,
                  };
                }
                return v;
              });
            }

            unBindPanel({ unBindSiloDetails }).then((res) => {
              if (res.code == 0) {
                this.$modal.msgSuccess("清空成功");
              } else {
                this.$modal.notifyError(res.message);
              }
            });
          }
        })
        .catch(() => {});
    },
    // 下发
    hanldeIssued() {
      const data = {
        rackCode: this.topForm.code,
        deviceCode: this.topForm.relateDeviceCode,
      };
      allotsPanelData(data).then((res) => {
        if (res.code != 0) {
          this.$modal.notifyError(res.message);
        } else {
          this.issued = true;
          this.$modal.msgSuccess("下发成功");
          this.getList("issued");
        }
      });
    },
  },
};
</script>

<style lang="scss" scoped>
::v-deep .siloCode_input .el-input__inner {
  font-size: 18px !important;
  color: #6d6b6b !important;
}
::v-deep .siloCode_input .el-input__inner::placeholder {
  font-size: 16px !important;
}
</style>