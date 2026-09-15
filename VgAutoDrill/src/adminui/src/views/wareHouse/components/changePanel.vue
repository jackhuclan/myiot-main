<template>
  <edit-form-dialog
    v-model="showFlag"
    :title="'操作板料'"
    optType="view"
    width="90%"
    :isHasCancel="true"
    @cancel="cancel"
  >
    <el-descriptions
      :column="4"
      size="medium"
      border
      style="margin-bottom: 10px"
      :contentStyle="content_style"
    >
      <el-descriptions-item label="库位" span="1">{{
        topForm.code
      }}</el-descriptions-item>
      <el-descriptions-item label="状态" span="1" v-if="setFormPage == 'AGV'">
        {{
          $status.deviceStatusOptions.find(
            (v) => v.value == topForm.deviceStatus
          )
            ? $status.deviceStatusOptions.find(
                (v) => v.value == topForm.deviceStatus
              ).label
            : ""
        }}
      </el-descriptions-item>
      <el-descriptions-item label="料仓" span="2">
        <div class="siloCode">
          {{ topForm.siloCode }}
          <el-button
            style="margin-left: 10px"
            size="small"
            v-debounce
            @click="handleSelectSiloManage"
            icon="el-icon-sort"
            type="success"
            plain
            >{{ topForm.siloCode ? "切换" : "绑定" }}</el-button
          >
        </div>
        <siloManageSelect
          ref="siloManageSelect"
          @onSelected="onsiloManageSelected"
        ></siloManageSelect>
      </el-descriptions-item>
    </el-descriptions>

    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-refresh"
          @click="hanldeSync"
          >同步</el-button
        >
      </el-col>
      <el-col :span="1.5" v-if="topForm.siloCode">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          >批量生成</el-button
        >
      </el-col>

      <el-col :span="1.5" v-if="topForm.siloCode">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-brush"
          @click="handleClear('all')"
          >批量清空</el-button
        >
      </el-col>
      <el-col :span="1.5" v-if="topForm.siloCode">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-delete"
          @click="handleDelete"
          >移除料仓</el-button
        >
      </el-col>

      <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-finished"
          @click="hanldeIssued"
          >下发</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-circle-check"
          @click="hanldeReday"
          >重新上报</el-button
        >
      </el-col>
    </el-row>

    <el-table
      v-if="queryParams.siloCode"
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
      <el-table-column label="层号" align="center" type="index" width="55" />
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
      <el-table-column
        label="物料编码"
        min-width="200"
        prop="itemCode"
      ></el-table-column>
      <el-table-column label="每叠块数" align="center" prop="pcs" />
      <el-table-column label="板长" align="center" prop="panelLength" />
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
            :disabled="!scope.row.panelCode"
            type="text"
            icon="el-icon-brush"
            @click="handleClear(scope.row)"
            >清空</el-button
          >
        </template>
      </el-table-column>
    </el-table>
    <el-empty v-else description="暂未绑定料仓" :image-size="100"></el-empty>
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
                :min="0"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item
              label="物料编码"
              prop="itemCode"
              :rules="
                form.productStatus != 1 ? rules.itemCode : [{ required: false }]
              "
            >
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
                ></el-button>
              </el-autocomplete>
              <ItemSelect
                ref="ItemSelect"
                @onSelected="onItemSelected"
              ></ItemSelect>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="销钉偏移量">
              <el-input
                v-model="form.pinOffset"
                placeholder="请输入销钉偏移量"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="批次号" prop="batchCode">
              <el-input v-model="form.batchCode" placeholder="请输入批次号" />
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
                :dis="true"
                :min="1"
                :max="1000"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="板长">
              <input-number
                :myNum="form.panelLength"
                @changeNum="changeNum"
                :numName="'panelLength'"
                :min="0"
                :dis="true"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="板宽">
              <input-number
                :myNum="form.panelWidth"
                @changeNum="changeNum"
                :dis="true"
                :numName="'panelWidth'"
                :min="0"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
  </edit-form-dialog>
</template>

<script>
import { listItem } from "@/api/masterData/item";
// 物料产品选择
import ItemSelect from "@/components/itemSelect";
import { getCentralRackPanels, unBind, updateRack } from "@/api/wareHouse/rack";
import {
  addOrUpdateSiloDetails,
  isBindSingle,
  bindSingle,
  unBindPanel,
  movePanelToOtherSilo,
  setManual,
  setReady,
  listSiloDetails,
  agvBindSilo,
  getLocationDetail,
} from "@/api/wareHouse/silo";
import siloManageSelect from "@/components/siloManageSelect";
import { allotsPanelData } from "@/api/device/schedulement";
import { allotsDeviceCommand } from "@/api/device/device";

export default {
  components: { siloManageSelect, ItemSelect },
  name: "SetForm",
  props: ["setFormPage"],
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
    // 连续层数
    const checkFloorCount = (rule, value, callback) => {
      if (this.form.floorCount <= 0) {
        return callback(new Error("连续层数不能小于等于0"));
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
      content_style: {
        // 居中
        height: "30px",
        "font-size": "16px",
        // 排列第二行
        "word-break": "break-all",
      },
      deviceStatus: "ONLINE",
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
        pinOffset: [
          { required: true, validator: validatePanelWidth, trigger: "change" },
        ],
        productStatus: [
          {
            required: true,
            validator: checkProductStatus,
            trigger: "change",
          },
        ],
        itemCode: [
          { required: true, validator: checkItemCode, trigger: "change" },
        ],
        floorCount: [
          { required: true, validator: checkFloorCount, trigger: "change" },
        ],
      },
      // 层数
      productStatusOptions: this.$status.productStatusOptions.map((v) => {
        return {
          ...v,
          name: v.label + (v.value != -1 ? "(" + v.value + ")" : ""),
        };
      }),
      detailTimer: null,
      // 原来的料仓
      oldSiloCode: undefined,
    };
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
    isListModified() {
      // 料仓变化
      let changeSilo = false;
      if (this.oldSiloCode != undefined && !this.topForm.siloCode) {
        changeSilo = true;
      } else {
        //料仓有改变
        changeSilo = this.oldSiloCode == this.topForm.siloCode;
      }

      const list = JSON.parse(JSON.stringify(this.panelList)).map((v) => {
        if (v.isInput) {
          v.panelCode = v.inputValue;
        }
        delete v.isInput;
        return { ...v };
      });

      return (
        // 为true时列表无变化
        JSON.stringify(list) == JSON.stringify(this.initialPanelList) &&
        // 为true时 移除料仓 无需提示
        changeSilo
      );
    },
  },
  watch: {
    "form.itemCode": {
      handler(val) {
        if (!val) {
          this.form.itemId = undefined;
          this.form.itemName = "";
          this.form.pcs = 0;
          this.form.panelLength = 0;
          this.form.panelWidth = 0;
        }
      },
    },
    showFlag: {
      handler(val) {
        if (val) {
          if (this.setFormPage == "AGV") {
            this.$emit("closeTimer");
          }
          this.getDevice(3000);
        } else {
          clearInterval(this.detailTimer);
          this.detailTimer = null;
          if (this.setFormPage == "AGV") {
            this.$emit("openTimer");
          } else {
            this.$emit("getRackList");
          }
        }
      },
      immediate: true,
      deep: true,
    },
  },
  methods: {
    // 获取状态
    getDevice(val) {
      if (this.detailTimer) {
        clearInterval(this.detailTimer);
        this.detailTimer = null;
      }
      this.detailTimer = setInterval(() => {
        setTimeout(() => {
          this.deviceStatus = "READY";
          // this.getDetail(this.detailId); //调用接口的方法
        }, 0);
      }, val);
    },
    // 获取料仓关联的板料
    getPanel() {
      getLocationDetail(this.topForm.code).then((res) => {
        const list = res.data.map((v) => {
          return {
            ...v,
            inputValue: v.panelCode,
          };
        });
        this.panelList = list;
        this.initialPanelList = Object.assign(
          [],
          res.data.map((v) => {
            return {
              ...v,
              inputValue: v.panelCode,
            };
          })
        );
      });
    },
    // 获取板料数据
    getList(val) {
      if (val == "issued") {
        this.issued = false;
      }
      this.loading = true;

      getCentralRackPanels(this.topForm.code).then((res) => {
        if (res.code != 1) {
          const list = res?.data.map((v) => {
            return {
              ...v,
              inputValue: v.panelCode,
            };
          });
          this.panelList = list;
          this.initialPanelList = Object.assign(
            [],
            res.data.map((v) => {
              return {
                ...v,
                inputValue: v.panelCode,
              };
            })
          );
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
        floorCount: 0,
        siloCode: "",
        pcs: 0,
        panelLength: 0,
        panelWidth: 0,
        pinOffset: "0",
        productStatus: undefined,
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
      let api = this.setFormPage == "AGV" ? agvBindSilo : updateRack;
      let data =
        this.setFormPage == "AGV"
          ? { deviceCode: this.topForm.code, siloCode: obj.code }
          : { ...this.topForm, siloCode: obj.code };
      if (obj != undefined && obj != null) {
        api(data).then((res) => {
          if (res.code == 0) {
            this.$set(this.topForm, "siloCode", obj.code);
            this.$set(this.queryParams, "siloCode", obj.code);
            this.$modal.msgSuccess("绑定成功");
            this.getPanel();
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
      this.title =
        "批量生成--(料仓编码" +
        this.queryParams.siloCode +
        ")--(总层数" +
        this.panelList.length +
        ")";
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
      this.$set(this.form, "itemName", obj.name);
      this.$set(this.form, "itemCode", obj.code);
      this.$set(this.form, "pcs", obj.panelCount);
      this.$set(this.form, "panelLength", obj.panelLength);
      this.$set(this.form, "panelWidth", obj.panelWidth);
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
        this.$set(this.form, "itemId", obj.id);
        this.$set(this.form, "itemName", obj.name);
        this.$set(this.form, "itemCode", obj.code);
        this.$set(this.form, "pcs", obj.panelCount);
        this.$set(this.form, "panelLength", obj.panelLength);
        this.$set(this.form, "panelWidth", obj.panelWidth);
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
      if (this.form.productStatus == 1) {
        let unBindSiloDetails = [];
        // floorNum=数据真实floorNum值
        let floorNum = this.form.beginFloorNum - 1;
        for (let i = 0; i < this.form.floorCount; i++) {
          unBindSiloDetails.push({
            floorNum: floorNum++,
            locationCode: this.topForm.code,
          });
        }
        this.panelList = this.panelList.map((v) => {
          if (unBindSiloDetails.map((i) => i.floorNum).includes(v.floorNum)) {
            return {
              siloCode: v.siloCode,
              itemCode: "",
              panelCode: "",
              floorNum: v.floorNum,
              productStatus: 1,
              pcs: "0",
              panelWidth: 0,
              panelLength: 0,
              pinOffset: 0,
              siloStatus: 0,
              id: 0,
              status: 1,
            };
          }
          return v;
        });
        unBindPanel({ unBindSiloDetails }).then((res) => {});
      } else {
        addOrUpdateSiloDetails({
          ...this.form,
          siloCode: this.topForm.siloCode.replace(
            this.topForm.siloCode[0],
            this.topForm.siloCode[0].toUpperCase()
          ),
          locationCode: this.topForm.code,
          pcs: this.form.pcs ? this.form.pcs + "" : "0",
        }).then((res) => {
          if (res.code == 0) {
            this.panelList = res.data;
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }

      this.open = false;
    },

    // 板料弹框关闭时
    cancel() {
      if (this.isListModified) return (this.showFlag = false);
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
    },

    // 板料回车
    handleSelectPanelCode(event, scope) {
      delete scope.row.isInput;
      // 存储未改变前的panelCode
      const noChageCode = JSON.parse(
        JSON.stringify(scope.row.inputValue ? scope.row.inputValue : "")
      );
      // 非空判断
      if (
        scope.row.panelCode == null ||
        !/\S/.test(scope.row.panelCode) ||
        scope.row.panelCode == scope.row.inputValue
      )
        return;
      const nextDom = document.querySelector(".panelCode" + (scope.$index + 1));
      isBindSingle({
        floorNum: scope.row.floorNum,
        isRaw: true,
        panelCode: scope.row.panelCode,
        locationCode: this.topForm.code,
      }).then((res) => {
        if (res.code == 0) {
          if (nextDom) {
            nextDom.querySelector("input").focus(); // 自动获取焦点
          } else {
            event.target.blur(); //失去焦点
          }
          this.panelList = res.data?.locationDetails.map((v) => {
            return {
              ...v,
              inputValue: v.panelCode,
            };
          });
          this.$modal.msgSuccess("操作成功");
        } else {
          let { alertLevel } = res.data;
          if (alertLevel == 1) {
            this.$modal
              .confirm(res.message + "是否继续操作？", {
                // 是否将取消（点击取消按钮）与关闭（点击关闭按钮或遮罩层、按下 ESC 键）进行区分
                distinguishCancelAndClose: true,
              })
              .then((result) => {
                bindSingle({
                  locationCode: this.topForm.code,
                  floorNum: scope.row.floorNum,
                  isRaw: true,
                  panelCode: scope.row.panelCode,
                }).then((res) => {
                  this.panelList = res.data?.locationDetails.map((v) => {
                    return {
                      ...v,
                      inputValue: v.panelCode,
                    };
                  });
                });
                if (nextDom) {
                  nextDom.querySelector("input").focus(); // 自动获取焦点
                } else {
                  event.target.blur(); //失去焦点
                }
              })
              .catch((action) => {
                // 放弃保存并离开页面
                scope.row.panelCode = noChageCode;
              });
          } else {
            this.$modal.notifyError(`(${scope.row.panelCode})${res.message}`);
            // 放弃保存并离开页面
            scope.row.panelCode = noChageCode;
          }
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
          : `确定清空 <span style="color:red">层号为 ${
              row.floorNum + 1
            }</span> 的数据项？`;
      let unBindSiloDetails =
        row == "all"
          ? this.multipleSelection.map((v) => {
              return { floorNum: v.floorNum, locationCode: this.topForm.code };
            })
          : [{ floorNum: row.floorNum, locationCode: this.topForm.code }];

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
                    panelLength: 0,
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
                    panelLength: 0,
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
    // 同步
    async hanldeSync() {
      const res = await this.$modal
        .confirm("确定执行同步操作？")
        .catch(() => {});
      if (res) return this.getList("issued");
    },
    // 下发
    async hanldeIssued() {
      const res = await this.$modal
        .confirm("确定执行下发操作？")
        .catch(() => {});
      if (!res) return;
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
          this.initialPanelList = Object.assign(this.panelList);
          this.oldSiloCode = this.topForm.siloCode;
        }
      });
    },
    // 设置手动/就绪
    async handleSet(label) {
      let code = this.topForm.siloCode;
      const api = label == "手动" ? setManual : setReady;
      const res = await this.$modal
        .confirm("确定将当前料仓" + code + "设置" + label + "？")
        .catch(() => {});
      if (res) {
        api({ siloCode: code }).then((result) => {
          if (result.code == 0) {
            this.$modal.msgSuccess("设置" + label + "成功");
            this.getList("issued");
          } else {
            this.$modal.notifyError(result.message);
          }
        });
      }
    },
    // 点击解绑
    handleDelete(item) {
      let api = this.setFormPage == "AGV" ? agvBindSilo : unBind;
      let data =
        this.setFormPage == "AGV"
          ? { deviceCode: this.topForm.code }
          : { rackCode: this.topForm.code };
      this.$modal
        .confirm(
          `确定解绑 <span style="color:red"> ${this.topForm.code}</span> 关联的料仓?`,
          {
            dangerouslyUseHTMLString: true, // 使用HTML片段
          }
        )
        .then((result) => {
          if (result == "confirm") {
            api(data).then((res) => {
              if (res.code == 0) {
                this.$modal.msgSuccess("解绑成功");
                this.panelList = [];
                this.initialPanelList = Object.assign([]);

                this.topForm.siloCode = "";
              } else {
                this.$modal.notifyError(res.message);
              }
            });
          }
        })
        .catch(() => {});
    },
    // 重新上报
    async hanldeReday() {
      const res = await this.$modal
        .confirm("确定执行重新上报操作？")
        .catch(() => {});
      if (!res) return;
      allotsDeviceCommand({
        locationCode: this.topForm.code,
        command: "ResetStatusCommand",
      }).then((res) => {
        if (res.code != 0) {
          this.$modal.notifyError(res.message);
        } else {
          this.$modal.msgSuccess("操作成功");
          this.getList("issued");
        }
      });
    },
  },
};
</script>

<style lang="scss" scoped>
::v-deep .siloCode {
  display: flex;
  align-items: center;
  .el-button {
    .el-icon-sort {
      transform: rotate(90deg) !important;
    }
  }
}
</style>
