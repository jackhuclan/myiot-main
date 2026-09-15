import Vue from "vue";
import Cookies from "js-cookie";
import Element from "element-ui";
import horizontalScroll from "el-table-horizontal-scroll";

import dayjs from "dayjs";
import "dayjs/locale/zh-cn"; // 导入中文语言包
import weekOfYear from "dayjs/plugin/weekOfYear";
import weekday from "dayjs/plugin/weekday";

import "./assets/styles/element-variables.scss";
import jsonView from "vue-json-views";
//引入bin-code-editor相关插件和样式
import CodeEditor from "bin-code-editor";
import "bin-code-editor/lib/styles/index.css";
//import './assets/font_icon/iconfont.css' //导入字体图标
// 首先我们使用npm install echarts --save 下载echarts对应相关包名
// 然后我们在mian.js里面导入echarts import * as echarts from 'echarts'
import * as echarts from "echarts"; //
import "@/assets/styles/index.scss"; // global css
import "@/assets/styles/ruoyi.scss"; // ruoyi css
import App from "./App";
import store from "./store";
import router from "./router";
import mixins from "./mixins";
import directive from "./directive"; // directive
import plugins from "./plugins"; // plugins
import status from "./status";
import { download, upload, exportExcel, getEncode } from "@/utils/request";

// // 树形结构下拉框
import Treeselect from "@riophae/vue-treeselect";
import "@riophae/vue-treeselect/dist/vue-treeselect.css";
// 封装的左侧树形筛选结构
import LeftTreeSelect from "@/components/leftTreeSelect";
// 封装的查询表单
import SearchForm from "@/components/SearchForm";
// 封装的状态Tag
import StatusTag from "@/components/statusTag";
// 封装的查看库存弹框
import ItemSearchDialog from "@/components/itemSearchDialog";
// 表格列封装组件
import TableColumn from "@/components/TableColumn";
import "./assets/icons"; // icon
import "./permission"; // permission control
import { getDicts } from "@/api/system/dict/data";
import { getConfigKey } from "@/api/system/config";
import {
  parseTime,
  resetForm,
  addDateRange,
  selectDictLabel,
  selectDictLabels,
  handleTree,
  utcToLocal,
  getNextDate,
  timeDifference,
} from "@/utils/ruoyi";
// 分页组件
import Pagination from "@/components/Pagination";
// 自定义表格工具组件
import RightToolbar from "@/components/RightToolbar";
// 富文本组件
import Editor from "@/components/Editor";
// 文件上传组件
import FileUpload from "@/components/FileUpload";
// 图片上传组件
import ImageUpload from "@/components/ImageUpload";
// 图片预览组件
import ImagePreview from "@/components/ImagePreview";
// 字典标签组件
import DictTag from "@/components/DictTag";
// 头部标签组件
import VueMeta from "vue-meta";
// 字典数据组件
import DictData from "@/components/DictData";
// 导入组件
import ImportXlsx from "@/components/importXlsx";
// 公共删除方法 公共双击选中事件 获取表格显隐列columns数据方法 排刀模块公共上传
import {
  flexColumnWidth,
  deleteItem,
  handleDoubleClick,
  rowStyle,
  echoRowStyle,
  getColumns,
  upLoadFile,
  isArrEqual,
  hasPermi,
  hasRole,
  getDutyColor,
} from "@/utils/public";
// 自封装计数器
import InputNumber from "@/components/inputNumber";
// 自封装排刀页面上传文件组件
import UpLoadFile from "@/components/upLoadFile";
// 自封装弹出表单新增修改对话框dialog组件
import EditFormDialog from "@/components/EditFormDialog";
// 自封装弹出选择数据对话框dialog组件
import SelectFormDialog from "@/components/SelectFormDialog";
// 自定义tooltip
import tooltip from "@/components/tooltip";
// 呼吸灯
import BreathingLight from "@/components/BreathingLight";
dayjs.extend(weekOfYear);
dayjs.extend(weekday);
dayjs.locale("zh-cn"); // 设置语言为中文
Vue.mixin(mixins);
// 全局状态挂载
Vue.prototype.$status = status;
// 全局方法挂载
Vue.prototype.getDicts = getDicts;
Vue.prototype.getConfigKey = getConfigKey;
Vue.prototype.parseTime = parseTime;
Vue.prototype.timeDifference = timeDifference;

Vue.prototype.getNextDate = getNextDate;

Vue.prototype.getColumns = getColumns;
// 全局挂载自适应列宽方法
Vue.prototype.flexColumnWidth = flexColumnWidth;
// 全局挂载删除放法
Vue.prototype.deleteItem = deleteItem;
// 全局挂载双击选中
Vue.prototype.handleDoubleClick = handleDoubleClick;
Vue.prototype.rowStyle = rowStyle;
Vue.prototype.echoRowStyle = echoRowStyle;
// 国际时间转换本地
Vue.prototype.utcToLocal = utcToLocal;
Vue.prototype.resetForm = resetForm;
Vue.prototype.addDateRange = addDateRange;
Vue.prototype.selectDictLabel = selectDictLabel;
Vue.prototype.selectDictLabels = selectDictLabels;
//自动生成编码
Vue.prototype.getEncode = getEncode;
// 下载文件
Vue.prototype.download = download;
// 导入文件
Vue.prototype.upload = upload;
// 导出文件
Vue.prototype.exportExcel = exportExcel;
Vue.prototype.handleTree = handleTree;
Vue.prototype.$echarts = echarts; //把echarts全局挂载原型（prototype）上
//排刀模块的公共上传方法
Vue.prototype.upLoadFile = upLoadFile;
// 判断两个数组 是否相同(顺序不同元素相同)
Vue.prototype.isArrEqual = isArrEqual;
Vue.prototype.hasPermi = hasPermi;
Vue.prototype.hasRole = hasRole;

// 稼动率颜色
Vue.prototype.getDutyColor = getDutyColor;

// 全局组件挂载
Vue.component("StatusTag", StatusTag);
Vue.component("TableColumn", TableColumn);
Vue.component("ItemSearchDialog", ItemSearchDialog);
Vue.component("EditFormDialog", EditFormDialog);
Vue.component("SelectFormDialog", SelectFormDialog);
Vue.component("DictTag", DictTag);
Vue.component("Pagination", Pagination);
Vue.component("RightToolbar", RightToolbar);
Vue.component("Editor", Editor);
Vue.component("FileUpload", FileUpload);
Vue.component("ImageUpload", ImageUpload);
Vue.component("ImagePreview", ImagePreview);
Vue.component("Treeselect", Treeselect);
Vue.component("leftTreeSelect", LeftTreeSelect);
Vue.component("ImportXlsx", ImportXlsx);
Vue.component("input-number", InputNumber);
Vue.component("SearchForm", SearchForm);
Vue.component("BreathingLight", BreathingLight);
// 排刀模块的公共上传文件
Vue.component("UpLoadFile", UpLoadFile);
Vue.component("tooltip", tooltip);
Vue.component("jsonView", jsonView);

//vue使用这个插件
Vue.use(CodeEditor);
Vue.use(directive);
Vue.use(plugins);
Vue.use(VueMeta);
DictData.install();
Vue.use(horizontalScroll);
/**
 * If you don't want to use mock-server
 * you want to use MockJs for mock api
 * you can execute: mockXHR()
 *
 * Currently MockJs will be used in the production environment,
 * please remove it before going online! ! !
 */

Vue.use(Element, {
  size: Cookies.get("size") || "medium", // set element-ui default size
});

Vue.config.productionTip = false;

new Vue({
  el: "#app",
  router,
  store,
  render: (h) => h(App),
});
