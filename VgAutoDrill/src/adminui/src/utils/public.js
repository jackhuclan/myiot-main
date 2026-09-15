import { Notification } from "element-ui";
import modal from "@/plugins/modal";
import store from "@/store";
import { Message } from "element-ui";

export function hasPermi(value) {
  const all_permission = "*:*:*";
  const permissions = store.getters && store.getters.permissions;

  if (value && value instanceof Array && value.length > 0) {
    const permissionFlag = value;

    const hasPermissions = permissions.some((permission) => {
      return (
        all_permission === permission || permissionFlag.includes(permission)
      );
    });
    // !hasPermissions为true无权限
    return !hasPermissions;
  } else {
    throw new Error(`请设置操作权限标签值`);
  }
}

export function hasRole(value) {
  const super_admin = "admin";
  const roles = store.getters && store.getters.roles;

  if (value && value instanceof Array && value.length > 0) {
    const roleFlag = value;

    const hasRoleFlag = roles.some((role) => {
      return super_admin === role || roleFlag.includes(role);
    });

    return hasRoleFlag;
  } else {
    throw new Error(`请设置角色权限标签值"`);
  }
}

// 动态计算列宽
export function flexColumnWidth(label, prop, data) {
  // 将表头加入数组
  const arr = data.map((x) => x[prop]);
  arr.push(label);
  // 计算每列内容最大的宽度
  const maxWidth = arr.reduce((acc, item) => {
    if (item) {
      const width = getTextWidth(item);
      acc = Math.max(acc, width);
    }
    return acc;
  }, 0);
  // 返回最大宽度加上额外的内间距
  return maxWidth + 20 + "px";
}
// 计算文本宽度
function getTextWidth(text) {
  var span = document.createElement("span");
  span.innerHTML = text;
  document.body.appendChild(span);
  var width = span.offsetWidth;
  document.body.removeChild(span);
  return width;
}
/**
 * @description 所有表格共用删除
 * @param {Array} ids 选中的数据id||集合
 * @param {Array} delApi  删除API
 * @param {Array} getList  刷新数据
 * @return viod
 */
export function deleteItem(ids, delApi, getList, label) {
  if (Array.isArray(ids) && ids.length <= 0)
    return modal.msgWarning("请选择要操作的数据!");
  label = label
    ? `确定删除<span style="color:red"> ${label}</span> 的数据项？`
    : "确定删除当前操作的数据项？";
  modal
    .confirm(label, {
      dangerouslyUseHTMLString: true, // 使用HTML片段
    })
    .then((result) => {
      if (result == "confirm") {
        delApi(ids)
          .then((res) => {
            if (res.code == 0) {
              modal.msgSuccess("删除成功");
              getList();
            } else {
              modal.notifyError(res.message);
            }
          })
          .catch(() => {});
      }
    })

    .catch(() => {});
}

/**
 * @description 所有表格双击选中时表格行样式
 * @return viod
 */
export function rowStyle({ row, rowIndex }) {
  Object.defineProperty(row, "rowIndex", {
    //给每一行添加不可枚举属性rowIndex来标识当前行
    value: rowIndex,
    writable: true,
    enumerable: false,
  });
}

/**
 * @description 所有回显表格双击选中时表格行样式
 * @return viod
 */
export function echoRowStyle({ row, rowIndex }) {
  Object.defineProperty(row, "rowId", {
    //给每一行添加不可枚举属性rowIndex来标识当前行
    value: row.id,
    writable: true,
    enumerable: false,
  });
}
/**
 * @description 所有表格共用双击选中
 * @param {Object} row  选中行
 * @param {Object} that this
 * @param {Object} column 点击的列
 * @return viod
 */
export function handleDoubleClick(row, that, column) {
  if (column.label == "操作") return;
  // 获取表格对象
  let refsElTable = that.$refs[that.page];
  let findRow = that.multipleSelection.find((c) => c.rowIndex == row.rowIndex);
  //找到选中的行
  if (findRow) {
    refsElTable.toggleRowSelection(row, false); //如过重复选中，则取消选中
    return;
  }
  refsElTable.toggleRowSelection(row, true); // 实现选中行中选中事件
}
/**
 * @description 表格显隐列columns处理
 * @param {Array} columns 原数据
 * @param {String} page 页面
 * @return {Array} 处理后的数据
 */
export function getColumns(columns, page) {
  const transferColumns = sessionStorage.getItem(page + "-columns");
  if (transferColumns) {
    const arr = columns.map((v, i) => {
      return { ...v, visible: JSON.parse(transferColumns)[i].visible };
    });
    return arr;
  } else {
    return columns;
  }
}

/**
 * @description 排刀模块共用上传文件
 * @param {File} file 文件信息
 * @param {Function} api 接口
 * @return 异步
 */
export function upLoadFile(file, api) {
  if (!file) return;
  let formData = new FormData();
  formData.append("file", file);
  return api(formData);
}

/**
 * @description 判断两个数组 是否相同(顺序不同元素相同)
 */
export function isArrEqual(arr1, list, key) {
  const arr2 = list.map((v) => v[key]);
  return arr1.length === arr2.length && arr1.every((ele) => arr2.includes(ele));
}

// 有关稼动率字体颜色
export function getDutyColor(duty) {
  let color = "";
  switch (true) {
    case duty >= 90:
      // 深绿色
      color = "#006400";
      break;
    case duty >= 75 && duty < 90:
      //  绿色
      color = "#32CD32";
      break;
    case duty >= 60 && duty < 75:
      //  黄色
      color = "#FFA500";
      break;
    case duty >= 50 && duty < 60:
      //  红色
      color = "red";
      break;
    case duty < 50:
      //  暗红色
      color = "#8B0000";
      break;
  }
  return color;
}
