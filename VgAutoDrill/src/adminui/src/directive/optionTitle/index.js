import store from "@/store";

export default {
  bind(el, binding) {
    el.classList.add("hide");
    const ev = el.querySelector("span");
    function getActualWidthOfChars(text, options = {}) {
      const { size = 14, family = "Microsoft YaHei" } = options;
      const canvas = document.createElement("canvas");
      const ctx = canvas.getContext("2d");
      ctx.font = `${size}px ${family}`;
      const metrics = ctx.measureText(text);
      const actual =
        Math.abs(metrics.actualBoundingBoxLeft) +
        Math.abs(metrics.actualBoundingBoxRight);
      return Math.floor(Math.max(metrics.width, actual));
    }
    const size = store.getters.size;
    let num = 180;
    if (size == "mini") {
      num = 160;
    } else
      switch (size) {
        case "mini":
          num = 160;
          break;
        case "small":
          num = 165;
          break;
        case "medium":
          num = 170;
          break;
        case "default":
          num = 185;
          break;
      }
    if (getActualWidthOfChars(ev.innerHTML) > num) {
      ev.classList.add(size + "-hide");
      el.setAttribute("title", ev.innerHTML);
    } else {
      el.removeAttribute("title");
    }
  },
};
