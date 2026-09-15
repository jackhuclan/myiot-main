import store from "@/store";
let opened = false;
let value = undefined;
let device = false;
let width = null;
let pageSearch = false;
// 某些页面固定一致开启吸顶（任务分布、工单分布）
let isFixed = false;
export default {
  bind(el) {
    el.classList.add("default_affix");
  },
  update(el, binding, vnode) {
    opened = store.getters.sidebar.opened;
    device = store.getters.device;
    const sidebar = document.querySelector(".sidebar-container");
    width = window.getComputedStyle(sidebar).width;
    pageSearch = store.getters.fixedSearch;
    value = binding.value;
    isFixed = vnode.context.isFixed;
  },
  inserted(el) {
    let whether = false;
    el._affix_scroll_handle = () => {
      //计算滚动条位置
      const scrollTop =
        window.pageYOffset ||
        document.documentElement.scrollTop ||
        document.body.scrollTop;
      //进行比较设置位置fixed
      whether = scrollTop > 50;
      let scrollHeight = document.documentElement.scrollHeight;
      let clientHeight = document.documentElement.clientHeight;
      if ((!pageSearch || el.style.display == "none") && !isFixed) return;
      if (scrollTop == 0) {
        el.classList.remove("fixed_affix");
      }

      if (
        scrollHeight - parseInt(window.getComputedStyle(el).height) <=
        clientHeight
      )
        return;
      if (whether) {
        if (device == "mobile") {
          el.style.width = "100vw";
        } else {
          opened
            ? (el.style.width = `calc(100% - ${
                value == false ? "400px" : "200px"
              })`)
            : (el.style.width = `calc(100% - ${
                value == false ? "254px" : "54px"
              })`);
        }
        el.classList.add("fixed_affix");
      } else {
        el.classList.remove("fixed_affix");
        el.style.width = `100%`;
      }
    };
    el._affix_resize_handle = () => {
      if (device == "mobile") {
        el.style.width = "100vw";
      } else {
        el.style.width = `calc(100% - ${width}-${
          value == false ? "200px" : "0"
        })`;
      }
    };
    window.addEventListener("scroll", el._affix_scroll_handle);
    window.addEventListener("resize", el._affix_resize_handle);
  },
  unbind(el) {
    el._affix_scroll_handle &&
      window.removeEventListener("scroll", el._affix_scroll_handle);
    el._affix_resize_handle &&
      window.removeEventListener("resize", el._affix_resize_handle);
  },
};
