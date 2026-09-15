/**
 * v-hasPermi 操作权限处理
 * Copyright (c) 2019 ruoyi
 */

import store from "@/store";
import { Message } from "element-ui";

export default {
  inserted(el, binding, vnode) {
    const { value } = binding;
    const all_permission = "*:*:*";
    const permissions = store.getters && store.getters.permissions;

    if (value && value instanceof Array && value.length > 0) {
      const permissionFlag = value;

      const hasPermissions = permissions.some((permission) => {
        return (
          all_permission === permission || permissionFlag.includes(permission)
        );
      });
      if (!hasPermissions) {
        // 编码span
        const span = el.nodeName;
        const isSwitch = el.classList.contains("el-switch");
        // 普通button
        if (el.classList.contains("el-button")) {
          if (!el.parentNode.classList.contains("el-col")) {
            el.style.color = "#c0c4cc";
          }
          // 禁止点击事件
          el.style.pointerEvents = "none";
          el.disabled = true;
          el.classList.add("is-disabled");

          // 作为表格点击编码查看/库位看板switch按钮
        } else if (span == "SPAN" || isSwitch) {
          el.style.color = "#606266";
          el.classList.add("is-disabled");
          // 禁止点击事件
          el.style.pointerEvents = "none";
        } else if (el.classList.contains("el-dropdown-menu__item")) {
          // 按钮为更多时内部按钮禁用
          el.classList.add("is-disabled");
          // 禁止点击事件
          el.style.pointerEvents = "none";
          el.style.color = "#c0c4cc";
        }
      }
    } else {
      throw new Error(`请设置操作权限标签值`);
    }
  },
};
