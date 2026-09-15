export default {
  inserted(el, binding, vnode) { 
    let classDialogmodel = null;
    let that = vnode.context; //获取到this
    // 监测弹框鼠标事件
    el.addEventListener("mousedown", (e) => {
      const flag = e.target.getAttribute("name") == "content";
      // 如果为true，则表示点击发生在遮罩层
      classDialogmodel = !!flag;
    });
    el.addEventListener("mouseup", (e) => {
      const flag = e.target.getAttribute("name") == "content";
      if (!!flag && classDialogmodel) {
        // 点击遮罩层并不是复制拖拽文字到遮罩层关闭遮罩层
        if (that.cancel) return that.cancel();
        that.$refs.popup.close();
      }
      classDialogmodel = false;
    });
  },
};
