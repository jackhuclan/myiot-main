import modal from "@/plugins/modal";

export function mergeList(mergearr, workStationList) {
  let list = mergearr.map((v) => {
    const children = v.children.map((child) => {
      let dataInfo = {};
      let newList = [];
      child.arr.forEach((item, index) => {
        let { startTime } = item;
        const time = startTime.split(" ")[1].split(":")[0];
        if (!dataInfo[time]) {
          dataInfo[time] = {
            time: time + ":00",
            child: [],
          };
        }
        dataInfo[time].child.push(item);
      });
      newList = Object.values(dataInfo).sort(
        (a, b) => a.time.split(":")[0] * 1 - b.time.split(":")[0] * 1
      ); // list 转换成功的数据
      if (Object.values(dataInfo).length > 0) {
        const startTime = newList[0].time.split(":")[0] * 1 - 1;
        const endTime = newList[newList.length - 1].time.split(":")[0] * 1 + 1;
        if (startTime >= 0) {
          newList.unshift({
            time:
              "00:00--" +
              (startTime < 10 ? "0" + startTime : startTime) +
              ":00",
            child: [],
          });
        }
        if (endTime <= 23) {
          newList.push({
            time:
              (endTime < 10 && endTime < 24 ? "0" + endTime : endTime) +
              ":00--23:00",
            child: [],
          });
        }
      } else {
        newList.push({
          time: "00:00--23:00",
          child: [],
        });
      }
      return { ...child, length: newList.length, arr: newList };
    });
    const times = children
      .reduce((p, v) => (p.length < v.length ? v : p))
      .arr.map((v) => v.time);
    return {
      ...v,
      times,
      children: children.map((v) => {
        if (v.arr.length < times.length) {
          return {
            ...v,
            arr: times.map((v2, i) => {
              return {
                time: v2,
                child:
                  v.arr[i] && v.arr[i].child.length > 0 ? v.arr[i].child : [],
              };
            }),
          };
        }
        return v;
      }),
    };
  });
  return list;
}

// 判断任务是否跨越日期和机器
export function isAllEqual(arr) {
  if (arr.length > 0) {
    return !arr.some((val) => {
      // 如果全是历史任务只进行机器判断
      if (val.history) {
        return val.workStationCode !== arr[0].workStationCode;
      }
      return (
        new Date(val.startTime).toLocaleDateString() !==
          new Date(arr[0].startTime).toLocaleDateString() ||
        val.workStationCode !== arr[0].workStationCode ||
        val.history != arr[0].history
      );
    });
  } else {
    return true;
  }
}
// 数组套对象去重
export function removeDuplicate(list) {
  return list.reduce((acc, curr) => {
    const index = acc.findIndex((item) => item.id === curr.id);
    if (index < 0) {
      acc.push(curr);
    }
    return acc;
  }, []);
}
// 复制信息
export function copyText(node) {
  if (node.target.parentNode.nodeName != "LI") return;
  // 当前点击元素获取的值
  const value = node.target.innerHTML;
  node.target.style.background = "#1890ff";
  let oInput = document.createElement("input");
  oInput.setAttribute("value", value);
  document.body.appendChild(oInput);
  oInput.select();
  // 该API已被弃用
  document.execCommand("copy");
  navigator.clipboard?.writeText(value);
  document.body.removeChild(oInput);
  setTimeout(() => {
    modal.msgSuccess("复制成功 " + value);
    node.target.style.background = "none";
  }, 800);
}
