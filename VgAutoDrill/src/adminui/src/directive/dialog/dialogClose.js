/**
* v-dialogClose 防止复制弹框中的数据意外关闭弹框
*/
export default {
    inserted(el, binding, vnode) {
        let classDialogmodel = null;
        let that = vnode.context;//获取到this
        // 监测弹框鼠标事件
        el.addEventListener('mousedown', (e) => {
            // 如果为true，则表示点击发生在遮罩层
            classDialogmodel =
                !!e.target.classList.contains("el-dialog__wrapper")
        })
        el.addEventListener('mouseup', (e) => {
            if (
                !!e.target.classList.contains("el-dialog__wrapper") &&
                classDialogmodel
            ) {
                // 点击遮罩层并不是复制拖拽文字到遮罩层关闭遮罩层
                if (that.cancel) return that.cancel();
                that.open = false
            }
            classDialogmodel = false;

        })
    },
};
