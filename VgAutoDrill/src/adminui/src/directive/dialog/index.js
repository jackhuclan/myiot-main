/**
 * 拖拽移动
 * @param  {elementObjct} bar 鼠标点击控制拖拽的元素
 * @param {elementObjct}  target 移动的元素
 * @param {function}  callback 移动后的回调
 */
export function startDrag(bar, target, callback) {
    var params = {
        top: 0,
        left: 0,
        currentX: 0,
        currentY: 0,
        flag: false,
        cWidth: 0,
        cHeight: 0,
        tWidth: 0,
        tHeight: 0
    };

    bar.onmousedown = function (event) {
        // 按下时初始化params
        var e = event ? event : window.event;
        params = {
            top: target.offsetTop,
            left: target.offsetLeft,
            currentX: e.clientX,
            currentY: e.clientY,
            flag: true,
            cWidth: document.body.clientWidth,
            cHeight: document.body.clientHeight,
            tWidth: target.offsetWidth,
            tHeight: target.offsetHeight
        };
        if (!event) {
            // 防止IE文字选中
            bar.onselectstart = function () {
                return false;
            }
        }

        document.onmousemove = function (event) {
            // 防止文字选中
            window.getSelection ? window.getSelection().removeAllRanges() : document.selection.empty();

            var e = event ? event : window.event;
            if (params.flag) {
                var nowX = e.clientX;
                var nowY = e.clientY;
                // 差异距离
                var disX = nowX - params.currentX;
                var disY = nowY - params.currentY;
                // 最终移动位置
                var zLeft = 0;
                var zTop = 0;

                zLeft = parseInt(params.left) + disX;
                // // 限制X轴范围
                // if (zLeft <= -parseInt(params.tWidth / 2)) {
                //     zLeft = -parseInt(params.tWidth / 2);
                // }
                // if (zLeft >= params.cWidth - parseInt(params.tWidth * 0.5)) {
                //     zLeft = params.cWidth - parseInt(params.tWidth * 0.5);
                // }

                zTop = parseInt(params.top) + disY;
                // // 限制Y轴范围
                // if (zTop <= 0) {
                //     zTop = 0;
                // }
                // if (zTop >= params.cHeight - parseInt(params.tHeight * 0.5)) {
                //     zTop = params.cHeight - parseInt(params.tHeight * 0.5);
                // }

                // 执行移动
                target.style.left = zLeft + 'px';
                target.style.top = zTop + 'px';
            }

            if (typeof callback == "function") {
                callback(zLeft, zTop);
            }
        }

        document.onmouseup = function () {
            params.flag = false;
            document.onmousemove = null;
            document.onmouseup = null;
        };
    };
}