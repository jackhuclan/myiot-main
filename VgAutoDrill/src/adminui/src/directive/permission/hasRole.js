/**
* v-hasRole 角色权限处理
* Copyright (c) 2019 ruoyi
*/

import store from '@/store'

export default {
  inserted(el, binding, vnode) {
    const { value } = binding
    const super_admin = "admin";
    const roles = store.getters && store.getters.roles

    if (value && value instanceof Array && value.length > 0) {
      const roleFlag = value

      const hasRole = roles.some(role => {
        return super_admin === role || roleFlag.includes(role)
      })

      if (!hasRole) {
        el.parentNode && el.parentNode.removeChild(el)
        // // 更多按钮下的li
        // const lis = el.querySelectorAll('.el-dropdown-menu__item');
        // // 普通button
        // const btn = el.className.includes('el-button');
        // // 编码span
        // const span = el.nodeName;
        // if (btn) {
        //   el.disabled = true;
        //   el.style.color = "#ccc";
        //   el.style.cursor = "no-drop"
        // } else if (lis.length > 0) {
        //   lis.forEach(li => {
        //     li.classList.add('is-disabled');
        //   })
        // } else if (span == "SPAN") {
        //   el.style.color = "#606266";
        //   // 禁止点击事件
        //   el.style.pointerEvents = 'none';
        // }
      }
    } else {
      throw new Error(`请设置角色权限标签值"`)
    }
  }
}
