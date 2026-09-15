<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      :isFixed="true"
      @search="handleQuery"
      @reset="resetQuery"
      ref="queryForm"
    >
      <el-form-item label="显示历史工单" prop="isHistory">
        <el-switch v-model="queryParams.isHistory" />
      </el-form-item>
      <el-form-item label="显示已完工工单" prop="isFinish">
        <el-switch v-model="queryParams.isFinish" />
      </el-form-item>
      <el-form-item label="工单编码" prop="workOrderCode">
        <el-input
          v-trim
          v-model="queryParams.workOrderCode"
          placeholder="请输入工单编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="物料编码" prop="itemCode">
        <el-input
          v-trim
          v-model="queryParams.itemCode"
          placeholder="请输入物料编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="工艺路线" prop="routeCodeList">
        <el-select
          v-model="queryParams.routeCodeList"
          placeholder="请选择"
          multiple
          collapse-tags
          :class="
            queryParams.routeCodeList && queryParams.routeCodeList.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in routeQueryList"
            :key="item.id"
            :label="item.label"
            :value="item.code"
            v-optionTitle
          />
        </el-select>
      </el-form-item>
      <el-form-item label="开始日期" prop="startDate">
        <el-date-picker
          v-model="queryParams.startDate"
          type="date"
          placeholder="选择日期"
          style="width: 180px"
        >
        </el-date-picker>
      </el-form-item>
      <el-form-item label="查看范围" prop="taskNumber">
        <el-select v-model="queryParams.taskNumber" style="width: 150px">
          <el-option :label="'三天'" :value="3" />
          <el-option :label="'五天'" :value="5" />
          <el-option :label="'七天'" :value="7" />
        </el-select>
      </el-form-item>
    </search-form>
    <!-- <el-button
            type="danger"
            plain
            icon="el-icon-delete"
            @click="handleClose"
            v-if="$route.path == '/produce/drillTask/taskedit'"
            >关闭</el-button
          > -->
    <el-row
      v-if="visibleAndHidden"
      :gutter="10"
      ref="topRow"
      style="padding: 10px 0"
      :class="whether ? 'isRowFixed' : ''"
      :style="
        whether
          ? {
              width: '100%',
              top: $refs.queryForm.$el.offsetHeight + 'px',
              zIndex: '28 !important',
            }
          : ''
      "
    >
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-sort"
          @click="handleBatchRationalizeTask"
          :disabled="hasPermi(['produce:ticketCharts:batchRationalizeTask'])"
          >重排计划</el-button
        >
      </el-col>
    </el-row>

    <div class="wrapper" id="menuFlag">
      <div
        class="left-btn-box"
        :class="whether ? 'isBtnFixed' : ''"
        :style="
          whether
            ? {
                ...isLeftBtnFixed,
                top: task_top_height + 'px',
              }
            : ''
        "
      >
        <el-tooltip effect="dark" content="左滑单个" placement="top">
          <!-- 左边按钮单个 -->
          <el-button
            :disabled="btnLeft"
            class="btn"
            @click="scrollLeft"
            icon="el-icon-caret-left"
          ></el-button>
        </el-tooltip>
        <el-tooltip effect="dark" content="批量左滑" placement="bottom">
          <!-- 左边批量按钮 -->
          <el-button
            :disabled="btnLeft"
            class="btn"
            @click="scrollLeft(batchNum)"
            icon="el-icon-d-arrow-left"
          ></el-button>
        </el-tooltip>
      </div>
      <div class="workOrder-hidden-container" @contextmenu.prevent>
        <div
          class="fixed-date-container"
          :style="whether ? { zIndex: 27 } : ''"
        >
          <div
            ref="isSelectAll"
            :class="whether ? 'isDateFixed' : 'isSelectAll'"
            :style="
              whether ? { ...isDateFixed, top: task_top_height + 'px' } : ''
            "
          >
            <el-checkbox
              @change="handleSelectAll"
              v-model="isSelectAll"
              style="margin: 0 3px"
              v-if="visibleAndHidden"
            ></el-checkbox
            >生产工单
          </div>
          <li v-for="(date, i) in dateList" :key="i">
            <span
              class="date"
              ref="date"
              :data-date="date"
              :class="date == 'Invalid' ? 'date-after' : ''"
            >
              {{ date == "Invalid" ? "历史工单" : date }}
            </span>
          </li>
        </div>
        <div class="all-container">
          <div
            class="drill-container"
            :class="whether ? 'isFixed' : ''"
            :style="
              whether
                ? {
                    ...isFixed,
                    top: task_top_height + 'px',
                  }
                : ''
            "
            ref="drillContainer"
          >
            <div class="drill-item"><!-- 占位 --></div>
            <div
              v-for="item in workStationList"
              :key="item.id"
              class="drill-item"
            >
              <el-checkbox
                @change="handleSelect(item)"
                v-model="item.checked"
                style="margin: 0 3px"
                v-if="visibleAndHidden"
              ></el-checkbox>
              <el-tooltip
                v-if="item.routeCode"
                :content="'所属工艺路线:' + item.routeCode"
                placement="top-end"
              >
                <span>{{ item.code }}</span></el-tooltip
              >
              <span v-else>{{ item.code }}</span>
            </div>
          </div>
          <div
            v-for="(date, dateI) in dateList"
            :key="dateI"
            class="task-container"
          >
            <div class="date-container">
              <!-- 占位 -->
            </div>
            <div class="drag-container">
              <div
                ref="item"
                :data-date="date"
                :class="date == 'Invalid' ? 'workOrder-item-after' : ''"
                class="workOrder-item"
                v-for="(works, worksI) in workStationList"
                :key="works.id"
              >
                <div
                  @contextmenu.prevent="openMenu($event, workOrder)"
                  class="item"
                  :style="{
                    borderColor: workOrder.remarkColor
                      ? workOrder.remarkColor
                      : workOrderColor,
                  }"
                  v-for="workOrder in list[dateI].des[worksI].detail"
                  :key="workOrder.id"
                >
                  <div
                    class="flag flag-left"
                    :style="{
                      background: workOrder.remarkColor
                        ? workOrder.remarkColor
                        : workOrderColor,
                    }"
                  ></div>
                  <div>
                    <i
                      v-if="workOrder.isUrgent != 0"
                      class="el-icon-star-on"
                      style="margin-right: 5px; color: #f05b59"
                    />
                    <li>工单编码：{{ workOrder.workOrderCode }}</li>
                    <li>物料编码：{{ workOrder.itemCode }}</li>
                    <li>工艺路线：{{ workOrder.routeCode }}</li>
                    <li class="task_count">
                      <span>任务数量:{{ workOrder.taskCount }}</span>
                    </li>
                    <li class="task_count">
                      <span>草稿:{{ workOrder.taskDraft }}</span>
                      <span>已提交:{{ workOrder.taskCommit }}</span>
                      <span>派送中:{{ workOrder.taskSending }}</span>
                    </li>
                    <li class="task_count">
                      <span>已就位:{{ workOrder.taskBuffered }}</span>
                      <span>已开始:{{ workOrder.taskBegin }}</span>
                      <span>已完成:{{ workOrder.taskFinish }}</span>
                    </li>

                    <li>
                      开始时间：{{
                        getStartDateOrEndDate(date, workOrder.startDate)
                      }}
                    </li>
                    <li>
                      结束时间：{{
                        getStartDateOrEndDate(date, workOrder.endDate)
                      }}
                    </li>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div
        class="right-btn-box"
        :class="whether ? 'isBtnFixed' : ''"
        :style="
          whether ? { ...isRightBtnFixed, top: task_top_height + 'px' } : ''
        "
      >
        <el-tooltip effect="dark" content="右滑单个" placement="top">
          <!-- 右边按钮 -->
          <el-button
            :disabled="btnRight"
            class="btn"
            @click="scrollRight"
            icon="el-icon-caret-right"
          ></el-button>
        </el-tooltip>
        <el-tooltip effect="dark" content="批量右滑" placement="bottom">
          <!-- 右边批量按钮 -->
          <el-button
            :disabled="btnRight"
            class="btn"
            @click="scrollRight(batchNum)"
            icon="el-icon-d-arrow-right"
          ></el-button>
        </el-tooltip>
      </div>
    </div>
    <!-- 滚动后底部左滑右滑按钮 -->
    <fixedBtn
      v-if="isShowBtn"
      @scrollLeft="scrollLeft"
      @scrollRight="scrollRight"
      :btnLeft="btnLeft"
      :btnRight="btnRight"
      :batchNum="batchNum"
    />
    <template v-if="visibleAndHidden">
      <!-- 右键菜单 -->
      <contextmenu
        :workOrder="true"
        :left="left"
        :top="top"
        @handleSelectColor="handleSelectColor"
        v-if="visible"
    /></template>

    <!-- 颜色 -->
    <ColorSelect ref="ColorSelect" @onSelected="onColorSelected"> </ColorSelect>
  </div>
</template>

<script>
import {
  getMOTaskList,
  remarkWorkOrderColor,
  getWorkOrder,
} from "@/api/produce/workOrder";
import { getDropSelectDatas } from "@/api/produce/route";
import { batchRationalizeTask } from "@/api/produce/drillWorkOrder.js";
// 颜色选择
import ColorSelect from "@/components/colorSelect";
import FixedBtn from "../taskCharts/components/fixedBtn.vue";
import Contextmenu from "../taskCharts/components/contextmenu.vue";
import { mapGetters } from "vuex";
export default {
  name: "TicketCharts",
  components: { ColorSelect, FixedBtn, Contextmenu },
  data() {
    return {
      // 显示搜索条件
      showSearch: true,
      loading: false,
      //定义要被拖拽对象的数组
      workStationList: [],
      dateList: [],
      list: [],
      routeQueryList: [],
      // 选中的数组
      selectList: [],
      // 批量滑动数量
      batchNum: 5,
      queryParams: {
        isHistory: true,
        isFinish: false,
        startDate: new Date().toLocaleDateString("sv-SE"),
        processCode: "drill",
        taskNumber: 7,
        workOrderCode: undefined,
        itemCode: undefined,
        routeCodeList: [],
      },
      allLength: 0,
      boxLength: 0,
      drillListEl: null,
      taskListEl: null,
      btnRight: false,
      // 左侧按钮初始化时禁用
      btnLeft: true,
      // 是否显示底部按钮
      isShowBtn: false,
      // 是否全选
      isSelectAll: false,
      // 右击菜单
      visible: false,
      top: 0,
      left: 0,
      // 要标记颜色的工单Id
      selectWorkOrder: undefined,
      //标记颜色默认色
      workOrderColor: "#ff5722",
      isFixed: {},
      isDateFixed: {},
      isLeftBtnFixed: {},
      isRightBtnFixed: {},
      whether: false,
      task_top_height: 0,
    };
  },
  computed: {
    ...mapGetters(["sidebar", "device"]),

    getStartDateOrEndDate() {
      //校验是否同年
      function checkIsSameYear(date) {
        const year = new Date(date).getFullYear();
        const currentYear = new Date().getFullYear();
        if (year == currentYear) {
          return true;
        }
        return false;
      }
      return (date, time) => {
        let str = "";
        if (date == "Invalid" && !checkIsSameYear(time)) {
          str = this.parseTime(time, "{y}-{m}-{d} {h}:{i}:{s}");
        } else if (date == "Invalid" && checkIsSameYear(time)) {
          str = this.parseTime(time, "{m}-{d} {h}:{i}:{s}");
        } else {
          str = this.parseTime(time, "{h}:{i}:{s}");
        }
        return str;
      };
    },
    // 显示隐藏
    visibleAndHidden() {
      // 是否超级管理员角色(为true)
      const hasRole = this.hasRole(["admin"]);
      // 是否仅查看
      const hasPermi = this.hasPermi(["produce:ticketCharts:view"]);
      // 超级管理员默认可显示及操作所有
      if (hasRole) return true;
      return hasPermi;
    },
  },
  activated() {
    // this.getList();
    this.getRouteList();
    window.addEventListener("scroll", this.handleScroll, true);
    window.addEventListener("resize", this.handleScroll);
  },
  deactivated() {
    window.removeEventListener("scroll", this.handleScroll, true);
    window.removeEventListener("resize", this.handleScroll, true);
    this.isShowBtn = false;
  },

  mounted() {
    window.addEventListener("scroll", this.handleScroll);
    window.addEventListener("resize", this.handleScroll);

    // 实时监听宽度变化 非窗口window变化
    const resizeObserver = new ResizeObserver((entries) => {
      this.init();
    });
    resizeObserver.observe(
      document.querySelector(".workOrder-hidden-container")
    );
    const resizeObserver1 = new ResizeObserver((entries) => {
      const topRowHeight = this.$refs.topRow?.$el?.getBoundingClientRect()
        .height
        ? this.$refs.topRow?.$el?.getBoundingClientRect().height
        : 0;
      const queryFormHeight = this.$refs.queryForm?.$el?.getBoundingClientRect()
        .height
        ? this.$refs.queryForm?.$el?.getBoundingClientRect().height
        : 0;
      this.task_top_height = queryFormHeight + topRowHeight;
    });
    resizeObserver1.observe(this.$refs.queryForm.$el);
  },
  watch: {
    visible(value) {
      if (value) {
        document.body.addEventListener("click", this.closeMenu);
      } else {
        document.body.removeEventListener("click", this.closeMenu);
      }
    },
    // 监听菜单收缩
    device: {
      handler(val) {
        if (val == "desktop") {
          if (this.sidebar.opened) {
            this.isDateFixed = {
              left: "250px",
            };
            this.isLeftBtnFixed = {
              left: "209px",
            };
          } else {
            this.isDateFixed = {
              left: "104px",
            };
            this.isLeftBtnFixed = {
              left: "62px",
            };
          }
        } else {
          this.isDateFixed = {
            left: "50px",
          };
          this.isLeftBtnFixed = {
            left: "9px",
          };
        }
      },
      immediate: true,
      deep: true,
    },
    "sidebar.opened": {
      handler(val) {
        if (this.device == "mobile") return;
        this.isFixed = {
          border: "1px solid #888",
          borderBottom: "none",
          borderLeft: "none",
        };
        this.isRightBtnFixed = {
          marginRight: "10px",
        };
        if (val) {
          this.isDateFixed = {
            left: "250px",
          };
          this.isLeftBtnFixed = {
            left: "209px",
          };
        } else {
          this.isDateFixed = {
            left: "104px",
          };
          this.isLeftBtnFixed = {
            left: "62px",
          };
        }
      },
      immediate: true,
      deep: true,
    },
  },

  methods: {
    // 改变时间样式
    changeStyle() {
      const that = this;
      setTimeout(() => {
        const items = that.$refs["item"];
        const dates = that.$refs.date;
        if (!items || !dates) return;
        this.$refs.isSelectAll.style.height =
          this.$refs.drillContainer?.getBoundingClientRect().height + "px";
        dates.forEach((v, i) => {
          let items_children = [];
          items &&
            items.forEach((item) => {
              if (v.dataset.date == item.getAttribute("data-date")) {
                items_children.push(item?.getBoundingClientRect().height);
              }
            });
          const max = [...items_children].reduce((p, v) => (p < v ? v : p));
          v.style.height = max + "px";
        });
      }, 150);
    },
    // 查询所有已审批工艺路线
    getRouteList() {
      getDropSelectDatas({
        vettingStatus: 1,
        pageNum: 1,
        pageSize: 100,
      }).then((res) => {
        this.routeQueryList = res.data?.map((v) => {
          return { ...v, label: `${v.code}(${v.name})` };
        });
      });
    },
    // 获取生产工单
    getList() {
      this.loading = true;
      this.queryParams.startDate = new Date(
        this.queryParams.startDate
      ).toLocaleDateString("sv-SE");
      getMOTaskList(this.queryParams).then((res) => {
        this.list = res.data.list && res.data.list.filter((v) => v.date);
        this.dateList = this.list.map((v) => {
          return new Date(v.date).toLocaleDateString("sv-SE").split(" ")[0];
        });
        this.workStationList =
          res.data.workStationList &&
          res.data.workStationList.map((v) => {
            return {
              ...v,
              checked: false,
            };
          });
        this.init();
        this.changeStyle();
        setTimeout(() => {
          this.loading = false;
        }, 300);
      });
    },
    // 全选
    handleSelectAll(val) {
      this.isSelectAll = val;
      if (val) {
        this.workStationList = this.workStationList.map((v) => {
          this.selectList.push(v.code);
          return { ...v, checked: true };
        });
      } else {
        this.workStationList = this.workStationList.map((v) => {
          return { ...v, checked: false };
        });
        this.selectList = [];
      }
    },
    // 反选
    handleSelect(val) {
      if (val.checked) {
        this.selectList.push(val.code);
        this.selectList = [...new Set(this.selectList)];
      } else {
        this.selectList = this.selectList.filter((v) => v !== val.code);
      }
      if (this.selectList.length == this.workStationList.length) {
        this.isSelectAll = true;
      } else {
        this.isSelectAll = false;
      }
    },
    // 左滑动逻辑
    scrollLeft(num) {
      num = typeof num == "number" ? num : 1;
      this.btnRight = false;
      if (this.allLength <= this.boxLength) {
        this.btnRight = true;
      }
      let leftMove = 0;
      if (this.taskListEl.style.left) {
        leftMove = Math.abs(parseInt(this.taskListEl.style.left));
      }
      if (leftMove + this.boxLength - 240 * num <= this.boxLength) {
        // 到最开始的时候
        this.taskListEl.style.left = "0px";
        this.btnLeft = true;
      } else {
        this.taskListEl.style.left = "-" + (leftMove - 240 * num) + "px";
        this.btnLeft = false;
      }
    },
    // 右滑动逻辑
    scrollRight(num) {
      num = typeof num == "number" ? num : 1;
      this.btnLeft = false;
      if (this.allLength <= this.boxLength) {
        this.btnLeft = true;
      }
      let leftMove = 0;
      if (this.taskListEl.style.left) {
        leftMove = Math.abs(parseInt(this.taskListEl.style.left));
      }
      if (leftMove + this.boxLength + 240 * num >= this.allLength) {
        this.taskListEl.style.left =
          "-" + (this.allLength - this.boxLength) + "px";
        this.btnRight = true;
      } else {
        this.taskListEl.style.left = "-" + (leftMove + 240 * num) + "px";
        this.btnRight = false;
      }
    },
    // 判断盒子是否出现在可视区
    isInViewPortOfOne() {
      this.visible = false;
      const offset = document
        .querySelector(".drill-header")
        .getBoundingClientRect();
      const offsetTop = offset.top;
      const offsetBottom = offset.bottom;
      // 进入可视区域
      if (offsetTop <= window.innerHeight && offsetBottom >= 0) {
        // console.log('进入可视区域');
        this.isShowBtn = false;
      } else {
        this.isShowBtn = true;
        // console.log('移出可视区域');
      }
    },

    /** 搜索按钮操作 */
    handleQuery() {
      // 到最开始的时候
      if (this.taskListEl) {
        this.taskListEl.style.left = "0px";
      }
      this.getList();
      this.isSelectAll = false;
      this.selectList = [];
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.handleQuery();
    },
    // 点击重排
    handleBatchRationalizeTask() {
      if (this.selectList.length <= 0) return this.$modal.notifyError("请选择");
      const loading = this.$loading({
        lock: true,
        text: "正在执行重排计划...",
        spinner: "el-icon-loading",
        background: "rgba(0, 0, 0, 0.7)",
      });
      batchRationalizeTask({
        startDate: this.queryParams.startDate,
        processCode: "drill",
        workStationList: this.selectList,
      }).then((res) => {
        if (res.code == 0) {
          this.getList();
          this.isSelectAll = false;
          this.selectList = [];
          this.$modal.msgSuccess("操作成功");
        } else {
          this.$modal.notifyError(res.message);
        }
        loading.close();
      });
    },
    // 关闭右键菜单、
    closeMenu() {
      this.visible = false;
    },
    // 右键打开菜单
    openMenu(event, v) {
      const menuMinWidth = 120,
        menuMinHeight = 60;
      // 宽度放不下生成新的位置
      if (event.clientX > window.innerWidth - menuMinWidth) {
        this.left = window.innerWidth - menuMinWidth;
      } else {
        this.left = event.clientX;
      }
      // 高度放不下生成新的位置
      if (event.clientY > window.innerHeight - menuMinHeight) {
        this.top = window.innerHeight - menuMinHeight;
      } else {
        this.top = event.clientY;
      }

      this.visible = true;
      getWorkOrder(v.workOrderId).then((res) => {
        if (res.code == 0) {
          this.selectWorkOrder = res.data;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 颜色选择弹出框
    handleSelectColor() {
      this.$refs.ColorSelect.showFlag = true;
      this.$refs.ColorSelect.selectedColorId = this.selectWorkOrder.remarkColor
        ? this.selectWorkOrder.remarkColor
        : undefined;
      this.$refs.ColorSelect.getList();
    },
    onColorSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.selectWorkOrder, "remarkColor", obj.value);
        remarkWorkOrderColor({
          id: this.selectWorkOrder.id,
          remarkColor: this.selectWorkOrder.remarkColor,
        }).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("标记成功");
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 初始化是否可以点击右侧
    init() {
      if (
        this.workStationList.length <= 0 ||
        !document.querySelector(".all-container")
      )
        return (this.btnRight = true), (this.btnLeft = true);
      this.allLength = this.workStationList.length * 240 + 180;
      this.boxLength = document.querySelector(
        ".workOrder-hidden-container"
      )?.offsetWidth;
      this.taskListEl = document.querySelector(".all-container");
      if (
        this.taskListEl &&
        Math.abs(parseInt(this.taskListEl.style.left)) > 0
      ) {
        if (
          Math.abs(parseInt(this.taskListEl.style.left)) +
            this.boxLength +
            240 >=
          this.allLength
        ) {
          this.taskListEl.style.left =
            "-" + (this.allLength - this.boxLength) + "px";
          this.btnRight = true;
          this.btnLeft = false;
        } else {
          this.btnRight = false;
        }
      } else {
        if (this.allLength < this.boxLength) {
          this.btnRight = true;
          this.btnLeft = true;
        } else {
          this.btnRight = false;
        }
      }
    },
    handleScroll() {
      //计算滚动条位置
      var scrollTop =
        window.pageYOffset ||
        document.documentElement.scrollTop ||
        document.body.scrollTop;
      //计算绑定div位置
      var offsetTop = document.querySelector("#menuFlag")?.offsetTop;
      //进行比较设置位置fixed
      this.whether = scrollTop > offsetTop;
      if (this.whether) {
        this.isShowBtn = true;
      } else {
        this.isShowBtn = false;
      }
    },
  },
};
</script>
<style lang="scss" scoped>
@import "../taskCharts/task.scss";
</style>
