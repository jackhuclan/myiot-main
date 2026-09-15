/**
 * 解决<el-radio-group ></el-radio-group>element官方组件bug(官方暂未解决)
 * 使用 <el-radio-group  v-removeAriaHidden></<el-radio-group>
 */
const RemoveAriaHidden = {
  bind(el, binding) {
    let ariaEls = el.querySelectorAll(".el-radio__original");
    ariaEls.forEach((item) => {
      item.removeAttribute("aria-hidden");
    });
  },
};
 
export default RemoveAriaHidden;
